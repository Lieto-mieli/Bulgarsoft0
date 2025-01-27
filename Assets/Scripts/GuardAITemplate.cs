using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class GuardAITemplate : MonoBehaviour
{
    GameObject selector;
    new Camera camera;
    AttackTargetLists targetLists;
    bool selected = false;
    public float moveSpeed;
    public float hitPoints;
    public float attackDamage;
    public float attackRange;
    public float attackCooldown;
    public float attackEndlag;
    bool ignoreTargets;
    Vector3 curPos;
    Vector2 targetPos;
    float cooldown;
    float endlag;
    GameObject autoTarget;
    GameObject manualTarget;
    float bestSoFar;
    SpriteRenderer spriteRender;

    // Start is called before the first frame update
    void Start()
    {
        selector = GameObject.FindWithTag("Selector");
        camera = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
        targetLists = GameObject.FindWithTag("TargetLists").GetComponent<AttackTargetLists>();
        curPos.z = curPos.y;
        targetPos = curPos;
        targetLists.playerTargets.Add(gameObject);
        spriteRender = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        cooldown -= Time.deltaTime;
        endlag -= Time.deltaTime;
        //debug(
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
        else if(cooldown > 0)
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
        if (endlag <= 0)
        {
            if (manualTarget != null)
            {
                if (Vector2.Distance(manualTarget.transform.position, transform.position) <= attackRange)
                {
                    manualTarget = null;
                }
            }
            bestSoFar = 9999;
            autoTarget = null;
            foreach (GameObject target in targetLists.enemyTargets)
            {
                if (target != null && Vector2.Distance(target.transform.position, transform.position) <= attackRange && Vector2.Distance(target.transform.position, transform.position) <= bestSoFar)
                {
                    bestSoFar = Vector2.Distance(target.transform.position, transform.position);
                    autoTarget = target;
                }
            }
            selected = selector.GetComponent<Selector>().currentlySelected.Contains(this.gameObject);
            if (selected)
            {
                //Instantiate(outline, transform.position, new Quaternion());
                if (Input.GetMouseButtonDown(1))
                {
                    MoveToPosition();
                }
            }
            if (Vector2.Distance((Vector2)curPos, targetPos) < 0.01f && cooldown <= 0 || !ignoreTargets && cooldown <= 0)
            {
                ignoreTargets = false;
                if (manualTarget != null)
                {
                    AttackTarget(manualTarget);
                }
                else if (autoTarget != null)
                {
                    AttackTarget(autoTarget);
                }
            }
            else
            {
                curPos = new Vector2(transform.position.x, transform.position.y);
                curPos.z = curPos.y;
                transform.position = Vector2.MoveTowards(curPos, targetPos, moveSpeed * Time.deltaTime);
            }
        }
    }
    void OnMouseDown()
    {
        if (!selected)
        {
            selector.GetComponent<Selector>().addObject(this.gameObject);
        }
        else
        {
            selector.GetComponent<Selector>().currentlySelected.Remove(this.gameObject);
        }
    }
    public void MoveToPosition()
    {
        ignoreTargets = true;
        if (selector.GetComponent<Selector>().currentlySelected.Count == 1) 
        {
            targetPos = camera.ScreenToWorldPoint(Input.mousePosition);
        }
        else
        {
            int array = selector.GetComponent<Selector>().currentlySelected.IndexOf(gameObject);
            int Count = Convert.ToInt16(selector.GetComponent<Selector>().currentlySelected.Count);
            int sqrtInt = Convert.ToInt16(math.sqrt(Count));
            float overflow = math.sqrt(Count) - sqrtInt;
            int totalSize;
            int collumns = 1;
            int rows = 1;
            if (overflow > 0)
            { 
                totalSize = sqrtInt + 1; 
                if (overflow < 0.5)
                {
                    collumns = sqrtInt;
                    rows = sqrtInt+1;
                }
                else
                {
                    collumns = sqrtInt+1;
                    rows = sqrtInt+1;
                }
            }
            else 
            { 
                totalSize = sqrtInt;
                collumns = sqrtInt;
                rows = sqrtInt;
            }
            int row = Convert.ToInt16(array / collumns)*collumns;
            int collumn = Convert.ToInt16((array / rows)-0.000001);
            float maxSizeX = MathF.Max((4 * math.log(collumns)-1)/1.5f,0);
            float maxSizeY = MathF.Max((4 * math.log(rows)-1)/1.5f,0);
            Debug.Log($"({collumns}, {rows}, {collumn}, {row}, {maxSizeX}, {maxSizeY})");
            targetPos = new Vector2(camera.ScreenToWorldPoint(Input.mousePosition).x+((maxSizeX * (collumn / MathF.Max(collumns - 1, 1))) - maxSizeX * 0.5f), camera.ScreenToWorldPoint(Input.mousePosition).y + (maxSizeY * (row / MathF.Max(rows-1,1))) - maxSizeY * 0.5f);
        }
    }
    public void AttackTarget(GameObject target)
    {
        Debug.Log($"{gameObject.name} Attacks {target.name}");
        cooldown = attackCooldown;
        target.GetComponent<EnemyAITemplate>().hitPoints -= attackDamage;
        endlag = attackEndlag;
    }
}
