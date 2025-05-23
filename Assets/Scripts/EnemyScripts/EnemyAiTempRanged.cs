using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class EnemyAiTempRanged : EnemyAITemplate
{
    //kalle loi lieto 80% kalle 20%
    public Vector3 CurPos;
    //bool doNotMove = false;
    int magSize;
    public GameObject projectile;
    enum EnemyState
    {
        Passive,
        MovingToPosition,
        AttackingTarget,
    }
    void Start() //kalle alkaa(?)
    {

        moveSpeed = UnitStatsList.unitStats[3][0];
        hitPoints = UnitStatsList.unitStats[3][1];
        maxHp = UnitStatsList.unitStats[3][1];
        attackDamage = UnitStatsList.unitStats[3][2];
        attackRange = UnitStatsList.unitStats[3][3];
        attackCooldown = UnitStatsList.unitStats[3][4];
        attackEndlag = UnitStatsList.unitStats[3][5];
        size = UnitStatsList.unitStats[3][8];
        valueTracker = GameObject.FindWithTag("ValueTracker").GetComponent<ValueTracker>();
        pathfinder = GameObject.FindWithTag("Pathfinder").GetComponent<Pathfinder>();
        targetLists = GameObject.FindWithTag("TargetLists").GetComponent<AttackTargetLists>();
        targetLists.enemyTargets.Add(gameObject);
        spriteRender = GetComponent<SpriteRenderer>();
        magSize = 10;
    }

    private void Update()
    {
        CurPos = transform.position;
    }
    //Instantiate(projectile, new Vector3(transform.position.x, transform.position.y, 1), Quaternion.identity);
    public override void AttackTarget(GameObject target) //kalle alkaa
    {
        //Debug.Log(magSize);
        if (magSize <= 0) //sama ranged attack kun guardilla
        {
            //doNotMove = true;
            //reload
            magSize = 10;
            Debug.Log("RELOADING");
            base.cooldown = base.attackCooldown;
            base.endlag = base.attackEndlag;
        }
        else if (magSize >= 1)
        {
            //doNotMove= false;
            magSize --;
            //Debug.Log($"{gameObject.name} Attacks {target.name}");
            base.cooldown = base.attackCooldown;
            GameObject tempBullet = Instantiate(projectile, new Vector3(transform.position.x, transform.position.y, 1), Quaternion.identity);
            tempBullet.GetComponent<Bullet>().TarPos = base.autoTarget.transform.position;
            //target.GetComponent<GuardAITemplate>().hitPoints -= attackDamage;
            base.endlag = base.attackEndlag;
        }
    } 

    /*private void Update()
    {
        if (doNotMove == true) //lataa
        {
            moveSpeed = 0;
        }
        else if (doNotMove == false) //ei lataa
        {
            moveSpeed = UnitStatsList.unitStats[3][0];
        }
    }*/ //kalle loppuu tarkotus oli että vihu ei liiku ku lataa
}
