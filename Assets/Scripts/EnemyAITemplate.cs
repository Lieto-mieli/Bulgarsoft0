using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyAITemplate : MonoBehaviour
{
    public float moveSpeed;
    public float hitPoints;
    public float attackDamage;
    public float attackRange;
    public float attackCooldown;
    public float attackEndlag;
    AttackTargetLists targetLists;
    bool ignoreTargets;
    Vector3 curPos;
    Vector2 targetPos;
    float cooldown;
    float endlag;
    Pathfinder pathfinder;
    List<Vector2> shortcutPath;
    EnemyState currentState;
    GameObject autoTarget;
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
        pathfinder = GameObject.FindWithTag("Pathfinder").GetComponent<Pathfinder>();
        targetLists = GameObject.FindWithTag("TargetLists").GetComponent<AttackTargetLists>();
        targetLists.enemyTargets.Add(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if (hitPoints <= 0)
        {
            targetLists.enemyTargets.Remove(gameObject);
            Destroy(gameObject);
        }
        endlag -= Time.deltaTime;
        cooldown -= Time.deltaTime;
        if (endlag <= 0)
        {
            bestSoFar = 9999;
            autoTarget = null;
            foreach (GameObject target in targetLists.playerTargets)
            {
                if (target != null && Vector2.Distance(target.transform.position, transform.position) <= attackRange && Vector2.Distance(target.transform.position, transform.position) <= bestSoFar)
                {
                    bestSoFar = Vector2.Distance(target.transform.position, transform.position);
                    autoTarget = target;
                    targetPos = autoTarget.transform.position;
                }
            }
            //Instantiate(outline, transform.position, new Quaternion());
            if (targetLists.playerTargets.Count > 0 && autoTarget == null)
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
            if (autoTarget != null)
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
                if (currentState == EnemyState.MovingToPosition)
                {
                    //Debug.Log("moving");
                    curPos = new Vector2(transform.position.x, transform.position.y);
                    curPos.z = curPos.y;
                    transform.position = Vector2.MoveTowards(curPos, shortcutPath[0], moveSpeed * Time.deltaTime);
                    if (Vector2.Distance((Vector2)curPos, shortcutPath[0]) < 0.02f)
                    {
                        shortcutPath.Remove(shortcutPath[0]);
                        if (shortcutPath.Count < 1) { currentState = EnemyState.Passive; }
                    }
                }
            }
        }
    }
    public void MoveToPosition(Vector2 pos)
    {
        //Debug.Log("moveStarted");
        if (true)
        {
            List<Vector2> path = pathfinder.Pathfind(transform.position, pos);
            if (path != null)
            {
                targetPos = pos;
                path.Add(targetPos);
                shortcutPath = path;
                currentState = EnemyState.MovingToPosition;
            }
        }
    }
    public void AttackTarget(GameObject target)
    {
        //Debug.Log($"{gameObject.name} Attacks {target.name}");
        cooldown = attackCooldown;
        target.GetComponent<GuardAITemplate>().hitPoints -= attackDamage;
        endlag = attackEndlag;
    }
}
