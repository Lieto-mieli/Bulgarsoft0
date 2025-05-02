using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyAITemplate : MonoBehaviour
{
    public float moveSpeed;
    public float hitPoints;
    public float maxHp;
    public float attackDamage;
    public float attackRange;
    public float attackCooldown;
    public float attackEndlag;
    public float size;
    public AttackTargetLists targetLists;
    public ValueTracker valueTracker;
    bool ignoreTargets;
    Vector3 curPos;
    public Vector2 targetPos;
    public float cooldown;
    public float endlag;
    public SpriteRenderer spriteRender;
    public Pathfinder pathfinder;
    List<Vector2> shortcutPath;
    EnemyState currentState;
    public GameObject autoTarget;
    float bestSoFar;
    enum EnemyState
    {
        Passive,
        MovingToPosition,
        AttackingTarget,
    }
    // Start is called before the first frame update
    void Start()
    {
        valueTracker = GameObject.FindWithTag("ValueTracker").GetComponent<ValueTracker>();
        pathfinder = GameObject.FindWithTag("Pathfinder").GetComponent<Pathfinder>();
        targetLists = GameObject.FindWithTag("TargetLists").GetComponent<AttackTargetLists>();
        targetLists.enemyTargets.Add(gameObject);
        spriteRender = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (hitPoints <= 0) //if hitpoints go to/below 0, die
        {
            targetLists.enemyTargets.Remove(gameObject);
            valueTracker.enemiesKilled++;
            Destroy(gameObject);
        }
        //debug( this sets the enemies hue based on what state they are in, should be replaced with animations once they exist
        if (endlag > 0)
        {
            Color tempColor = new Color
            {
                r = 1,
                g = 0.5f,
                b = 0.5f,
                a = 1
            };
            spriteRender.color = tempColor;
        }
        else if (cooldown > 0)
        {
            Color tempColor = new Color
            {
                r = 1,
                g = 1,
                b = 0.5f,
                a = 1
            };
            spriteRender.color = tempColor;
        }
        else
        {
            Color tempColor = new Color
            {
                r = 1,
                g = 1,
                b = 1,
                a = 1
            };
            spriteRender.color = tempColor;
        }
        //debug)
        endlag -= Time.deltaTime;
        cooldown -= Time.deltaTime;
        if (endlag <= 0) // enemy has to be out of their attacks endlag to do anything other than die
        {
            bestSoFar = 9999;
            autoTarget = null;
            foreach (GameObject target in targetLists.playerTargets) //chooses the closest target that is in attack range
            {
                if (target != null && Vector2.Distance(target.transform.position, transform.position) <= attackRange && Vector2.Distance(target.transform.position, transform.position) <= bestSoFar)
                {
                    bestSoFar = Vector2.Distance(target.transform.position, transform.position);
                    autoTarget = target;
                    targetPos = autoTarget.transform.position;
                }
            }
            //Instantiate(outline, transform.position, new Quaternion());
            if (targetLists.playerTargets.Count > 0 && autoTarget == null) //if there are targets, but none of them are within attackRange distance, choose the closest target
            {
                foreach (GameObject target in targetLists.playerTargets)
                {
                    if (target != null && Vector2.Distance(target.transform.position, transform.position) <= bestSoFar)
                    {
                        bestSoFar = Vector2.Distance(target.transform.position, transform.position);
                        autoTarget = target;
                        targetPos = autoTarget.transform.position;
                    }
                }
            }
            if (autoTarget != null&&shortcutPath==null)//this is not perfect and should be redone at some point
            {
                MoveToPosition(targetPos);
            }
            if (Vector2.Distance((Vector2)curPos, targetPos) < attackRange && cooldown <= 0 )
            {
                if (autoTarget != null)
                {
                    AttackTarget(autoTarget);
                    currentState = EnemyState.AttackingTarget;
                }
            }
            else
            {
                if (currentState == EnemyState.MovingToPosition && Vector2.Distance((Vector2)curPos, targetPos) > attackRange)
                {
                    //Debug.Log("moving");
                    curPos = new Vector2(transform.position.x, transform.position.y);
                    transform.position = Vector2.MoveTowards(curPos, shortcutPath[0], moveSpeed * Time.deltaTime);
                    Debug.Log(Vector2.Distance((Vector2)curPos, shortcutPath[0]));
                    Debug.Log(shortcutPath.Count);
                    if (Vector2.Distance((Vector2)curPos, shortcutPath[0]) < 0.02f)
                    {
                        shortcutPath.Remove(shortcutPath[0]);
                        if (shortcutPath.Count < 1) 
                        { 
                            currentState = EnemyState.Passive;
                            shortcutPath = null;
                        }
                    }
                    Vector3 tempPos = transform.position;
                    tempPos.z = tempPos.y;
                    transform.position = tempPos;
                }
            }
        }
        Collider2D[] results = Physics2D.OverlapCircleAll(transform.position, size);//checks for nearby enemies and guards and pushes them away from this enemy
        foreach (Collider2D c in results)
        {
            //Debug.Log("enemyshittis");
            if (c.gameObject.CompareTag("Enemy") && c.gameObject != this.gameObject)
            {
                //curPos = new Vector2(transform.position.x, transform.position.y);
                //transform.position = Vector3.MoveTowards((Vector2)curPos, (Vector2)c.transform.position, MathF.Min(-0.5f+Vector2.Distance((Vector2)curPos, c.transform.position),0) * Time.deltaTime);
                if (moveSpeed != 0) { c.gameObject.GetComponent<EnemyAITemplate>().PushAway(transform.position, size); }
                else if (moveSpeed == 0) { c.gameObject.GetComponent<EnemyAITemplate>().PushAway(transform.position, size * 3); }
            }
            if (c.gameObject.CompareTag("Guard"))
            {
                //curPos = new Vector2(transform.position.x, transform.position.y);
                //transform.position = Vector3.MoveTowards((Vector2)curPos, (Vector2)c.transform.position, MathF.Min(-0.5f+Vector2.Distance((Vector2)curPos, c.transform.position),0) * Time.deltaTime);
                if (moveSpeed != 0) { c.gameObject.GetComponent<GuardAITemplate>().PushAway(transform.position, size * 3); }
                else if (moveSpeed == 0) { c.gameObject.GetComponent<GuardAITemplate>().PushAway(transform.position, size * 3); }
            }
        }
    }
    public void PushAway(Vector2 awayPos, float pushForce) // called by other enemies and guards that want to push this enemy away, only works if this enemy is not currently going somewhere
    {
        if (moveSpeed > 0 && currentState != EnemyState.MovingToPosition)
        {
            Vector2 curPos = new Vector2(transform.position.x, transform.position.y);
            transform.position = Vector3.MoveTowards(curPos, awayPos, MathF.Min((-pushForce * 1.41f) + Vector2.Distance(curPos, awayPos), 0) * Time.deltaTime);
        }
    }
    public void MoveToPosition(Vector2 pos) //set enemy movement path to pos from position
    {
        //Debug.Log("moveStarted");
        if (true)
        {
            List<Vector2> path = pathfinder.Pathfind(transform.position, pos, size);
            if (path != null)
            {
                targetPos = pos;
                path.Add(targetPos);
                shortcutPath = path;
                //foreach(Vector2 pathNode in path)
                //{
                //    Debug.Log(pathNode.x+","+pathNode.y);
                //}
                currentState = EnemyState.MovingToPosition;
            }
        }
    }
    public virtual void AttackTarget(GameObject target) //get the GuardAITemplate script of the target and reduce its hp
    {
        //Debug.Log($"{gameObject.name} Attacks {target.name}");
        cooldown = attackCooldown;
        target.GetComponent<GuardAITemplate>().hitPoints -= attackDamage;
        endlag = attackEndlag;
    }
}
