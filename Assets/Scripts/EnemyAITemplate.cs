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
    public float attackDelay;
    public GameObject fent;
    AttackTargetLists targetLists;
    Vector3 curPos;
    float delay;
    // Start is called before the first frame update
    void Start()
    {
        fent = GameObject.FindWithTag("Fent");
        targetLists = GameObject.FindWithTag("TargetLists").GetComponent<AttackTargetLists>();
        targetLists.enemyTargets.Add(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if (delay < 0)
        {
            if (Vector2.Distance(transform.position, fent.transform.position) < attackRange)
            {
                AttackTarget();
                delay = attackDelay;
            }
            else
            {
                MoveForward();
            }
        }

        delay -= Time.deltaTime;
        if (hitPoints <= 0)
        {
            targetLists.enemyTargets.Remove(gameObject);
            Destroy(gameObject);
        }
    }
    public void MoveForward()
    {
        curPos = new Vector3(transform.position.x, transform.position.y);
        curPos.x += moveSpeed * Time.deltaTime;
        curPos.z = curPos.y;
        transform.position = curPos;
    }
    public void AttackTarget()
    {
        fent.GetComponent<Fent>().health -= 1;
    }
}
