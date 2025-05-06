using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.UIElements.Experimental;
using static UnityEngine.GraphicsBuffer;

public class GuardAITemplate : MonoBehaviour
{
    public GuardAITemplate()
    {
    }
    public GuardAITemplate(float moveSpeed, float hitPoints, float attackDamage, float attackRange, float attackCooldown, float attackEndlag)
    {
        this.moveSpeed = moveSpeed;
        this.hitPoints = hitPoints;
        this.attackDamage = attackDamage;
        this.attackRange = attackRange;
        this.attackCooldown = attackCooldown;
        this.attackEndlag = attackEndlag;
    }
    public GameObject selector;
    public new Camera camera;
    public AttackTargetLists targetLists;
    public ValueTracker valueTracker;
    public bool selected = false;
    public float moveSpeed;
    public float hitPoints;
    public float maxHp;
    public float attackDamage;
    public float attackRange;
    public float attackCooldown;
    public float attackEndlag;
    public float size;
    public bool ignoreTargets;
    public Vector3 curPos;
    public Vector2 targetPos;
    public float cooldown;
    public float endlag;
    public GameObject autoTarget;
    public GameObject manualTarget;
    float bestSoFar;
    public SpriteRenderer spriteRender;
    public Pathfinder pathfinder;
    List<Vector2> shortcutPath;
    public GuardState currentState;
    public float atkSpeedMult = 1;
    public Vector3 KustomEulerAngles;
    public enum GuardState
    {
        Passive,
        MovingToPosition,
        AttackingTarget,
    }
    // Start is called before the first frame update
    void Start()
    {
        KustomEulerAngles = transform.eulerAngles;
        selector = GameObject.FindWithTag("Selector");
        camera = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
        targetLists = GameObject.FindWithTag("TargetLists").GetComponent<AttackTargetLists>();
        valueTracker = GameObject.FindWithTag("ValueTracker").GetComponent<ValueTracker>();
        targetPos = curPos;
        targetLists.playerTargets.Add(gameObject);
        spriteRender = GetComponent<SpriteRenderer>();
        pathfinder = GameObject.FindWithTag("Pathfinder").GetComponent<Pathfinder>();
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.x > 17.63)
        {
            //KustomEulerAngles.y = 180f;
            transform.eulerAngles = new Vector3(0, 180f, 0);
            //Debug.Log("ali "+transform.position.x);
        }
        else
        {
            transform.eulerAngles = new Vector3(0, 0f, 0);
            //Debug.Log("yli");
            //KustomEulerAngles.y = 0f;
        }

        if (hitPoints <= 0) //if hitpoints go to/below 0, die
        {
            targetLists.playerTargets.Remove(gameObject);
            valueTracker.playerUnits.Remove(gameObject);
            if (selected)
            {
                selector.GetComponent<Selector>().currentlySelected.Remove(this.gameObject);
            }
            valueTracker.guardsLost++;
            Destroy(gameObject);
        }
        cooldown -= Time.deltaTime;
        endlag -= Time.deltaTime;
        //debug( this sets the guards hue based on what state they are in, should be replaced with animations once they exist
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
        if (endlag <= 0) // guard has to be out of their attacks endlag to do anything, other than die and exist of course
        {
            //if (manualTarget != null)
            //{
            //    if (Vector2.Distance(manualTarget.transform.position, transform.position) <= attackRange)
            //    {
            //        manualTarget = null;
            //    }
            //}
            bestSoFar = 9999;
            autoTarget = null;
            foreach (GameObject target in targetLists.enemyTargets) //chooses the closest target that is in attack range
            {
                if (target != null && Vector2.Distance(target.transform.position, transform.position) <= attackRange && Vector2.Distance(target.transform.position, transform.position) <= bestSoFar)
                {
                    bestSoFar = Vector2.Distance(target.transform.position, transform.position);
                    autoTarget = target;
                }
            }
            selected = selector.GetComponent<Selector>().currentlySelected.Contains(this.gameObject);
            if (selected) //if guard is selected, listen for player inputs
            {
                //Instantiate(outline, transform.position, new Quaternion());
                if (Input.GetMouseButtonDown(1))
                {
                    if (selector.GetComponent<Selector>().AMove)
                    {
                        manualTarget = null;
                        AttackMove();
                    }
                    else
                    {
                        manualTarget = null;
                        MoveToPosition();
                    }
                    //Debug.Log("move");
                }
            }
            //if the guard should attack a target and hasnt been given a moveToPosition command, do so.
            if (manualTarget != null && !ignoreTargets && cooldown <= 0)
            {
                AttackTarget(manualTarget);
            }
            else if (autoTarget != null && !ignoreTargets && cooldown <= 0)
            {
                AttackTarget(autoTarget);
            }
            else // if no attack is made, and the guard should move, do so.
            {
                if (currentState == GuardState.MovingToPosition && endlag <= 0)
                {
                    //Debug.Log("moving");
                    curPos = new Vector2(transform.position.x, transform.position.y);
                    transform.position = Vector2.MoveTowards(curPos, shortcutPath[0], moveSpeed * Time.deltaTime);
                    if (Vector2.Distance((Vector2)curPos, shortcutPath[0]) < 0.02f)
                    {
                        shortcutPath.Remove(shortcutPath[0]);
                        if (shortcutPath.Count < 1) 
                        { 
                            currentState = GuardState.Passive;
                            ignoreTargets = false;
                        }
                    }
                    Vector3 tempPos = transform.position;
                    tempPos.z = tempPos.y;
                    transform.position = tempPos;
                }
            }
        }
        Collider2D[] results = Physics2D.OverlapCircleAll(transform.position, size); //checks for nearby guards and enemies and pushes them away from this guard
        //Debug.Log($"this:{results[0].name}, {transform.position}");
        foreach(Collider2D c in results)
        {
            //Debug.Log("guardshittis");
            if (c.gameObject.CompareTag("Guard") && c.gameObject != this.gameObject)
            {
                //curPos = new Vector2(transform.position.x, transform.position.y);
                //transform.position = Vector3.MoveTowards((Vector2)curPos, (Vector2)c.transform.position, MathF.Min(-0.5f+Vector2.Distance((Vector2)curPos, c.transform.position),0) * Time.deltaTime);
                if (moveSpeed != 0) { c.gameObject.GetComponent<GuardAITemplate>().PushAway(transform.position, size); }
                else if (moveSpeed == 0) { c.gameObject.GetComponent<GuardAITemplate>().PushAway(transform.position, size*3); }
            }
            if (c.gameObject.CompareTag("Enemy"))
            {
                //curPos = new Vector2(transform.position.x, transform.position.y);
                //transform.position = Vector3.MoveTowards((Vector2)curPos, (Vector2)c.transform.position, MathF.Min(-0.5f+Vector2.Distance((Vector2)curPos, c.transform.position),0) * Time.deltaTime);
                if (moveSpeed != 0) { c.gameObject.GetComponent<EnemyAITemplate>().PushAway(transform.position, size * 3); }
                else if (moveSpeed == 0) { c.gameObject.GetComponent<EnemyAITemplate>().PushAway(transform.position, size * 3); }
            }
        }
    }
    public void PushAway(Vector2 awayPos, float pushForce) // called by other guards and enemies that want to push this guard away, only works if this guard is not currently going somewhere
    {
        //Debug.Log("ebin");
        if (moveSpeed > 0 && currentState != GuardState.MovingToPosition)
        {
            Vector2 curPos = new Vector2(transform.position.x, transform.position.y);
            transform.position = Vector3.MoveTowards(curPos, awayPos, MathF.Min((-pushForce*1.41f) + Vector2.Distance(curPos, awayPos), 0) * Time.deltaTime);
        }
    }
    //void OnMouseDown()
    //{
    //    if (!selected)
    //    {
    //        selector.GetComponent<Selector>().AddObject(this.gameObject);
    //    }
    //    else
    //    {
    //        selector.GetComponent<Selector>().RemoveObject(this.gameObject);
    //    }
    //}
    public virtual void MoveToPosition() //set guard movement path to where the player is pointing and ignore enemies until reaching destination
    {
        //Debug.Log("moveStarted");
        ignoreTargets = true;
        //if (selector.GetComponent<Selector>().currentlySelected.Count == 1) 
        //{
            List<Vector2> path = pathfinder.Pathfind(transform.position, camera.ScreenToWorldPoint(Input.mousePosition),size);
            if (path != null)
            {
                targetPos = camera.ScreenToWorldPoint(Input.mousePosition); 
                path.Add(targetPos);
                shortcutPath = path;
                currentState = GuardState.MovingToPosition;
            }
        //}
        //else
        //{
            //int array = selector.GetComponent<Selector>().currentlySelected.IndexOf(gameObject)+1;
            //int Count = Convert.ToInt16(selector.GetComponent<Selector>().currentlySelected.Count);
            //int sqrtInt = Convert.ToInt16(math.sqrt(Count));
            //float overflow = math.sqrt(Count) - sqrtInt;
            //int totalSize;
            //int collumns = 1;
            //int rows = 1;
            //if (overflow > 0)
            //{ 
            //    totalSize = sqrtInt + 1; 
            //    if (overflow < 0.5)
            //    {
            //        collumns = sqrtInt;
            //        rows = sqrtInt+1;
            //    }
            //    else
            //    {
            //        collumns = sqrtInt+1;
            //        rows = sqrtInt+1;
            //    }
            //}
            //else 
            //{ 
            //    totalSize = sqrtInt;
            //    collumns = sqrtInt;
            //    rows = sqrtInt;
            //}
            //int row = Convert.ToInt16(array / collumns);
            //int collumn = Convert.ToInt16((array / rows)-0.000001);
            //float maxSizeX = MathF.Max((4 * math.log(collumns)-1)/1.5f,0);
            //float maxSizeY = MathF.Max((4 * math.log(rows)-1)/1.5f,0);
            //Debug.Log($"({collumns}, {rows}, {collumn}, {row}, {maxSizeX}, {maxSizeY})");
            //List<Vector2> path = pathfinder.Pathfind(transform.position, camera.ScreenToWorldPoint(Input.mousePosition));
            //if (path != null)
            //{
                //targetPos = new Vector2(path[path.Count - 1].x + ((maxSizeX * (collumn / MathF.Max(collumns - 1, 1))) - maxSizeX * 0.5f), path[path.Count - 1].y + (maxSizeY * (row / MathF.Max(rows - 1, 1))) - maxSizeY * 0.5f);
                //path.Add(targetPos);
            //    shortcutPath = path;
            //    currentState = GuardState.MovingToPosition;
            //}
        //}
    }
    public virtual void AttackTarget(GameObject target)//get the EnemyAITemplate script of the target and reduce its hp
    {
        //Debug.Log($"{gameObject.name} Attacks {target.name}");
        cooldown = attackCooldown / atkSpeedMult;
        target.GetComponent<EnemyAITemplate>().hitPoints -= attackDamage;
        endlag = attackEndlag / atkSpeedMult;
    }
    public virtual void AttackMove() // similar to moveToPosition but does not ignore attack targets while doing so.
    {
        ignoreTargets = false;
        List<Vector2> path = pathfinder.Pathfind(transform.position, camera.ScreenToWorldPoint(Input.mousePosition), size);
        if (path != null)
        {
            targetPos = camera.ScreenToWorldPoint(Input.mousePosition);
            path.Add(targetPos);
            shortcutPath = path;
            currentState = GuardState.MovingToPosition;
        }
    }
}
