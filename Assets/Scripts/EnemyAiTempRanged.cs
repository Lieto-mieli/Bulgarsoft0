using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class EnemyAiTempRanged : EnemyAITemplate
{
    int magSize;
    public GameObject projectile;
    enum EnemyState
    {
        Passive,
        MovingToPosition,
        AttackingTarget,
    }
    void Start()
    {
        moveSpeed = UnitStatsList.unitStats[3][0];
        hitPoints = UnitStatsList.unitStats[3][1];
        maxHp = UnitStatsList.unitStats[3][1];
        attackDamage = UnitStatsList.unitStats[3][2];
        attackRange = UnitStatsList.unitStats[3][3];
        attackCooldown = UnitStatsList.unitStats[3][4];
        attackEndlag = UnitStatsList.unitStats[3][5];
        size = UnitStatsList.unitStats[3][8];
        pathfinder = GameObject.FindWithTag("Pathfinder").GetComponent<Pathfinder>();
        targetLists = GameObject.FindWithTag("TargetLists").GetComponent<AttackTargetLists>();
        targetLists.enemyTargets.Add(gameObject);
        magSize = 10;
    }
    //Instantiate(projectile, new Vector3(transform.position.x, transform.position.y, 1), Quaternion.identity);
    public override void AttackTarget(GameObject target)
    {
        //Debug.Log(magSize);
        if (magSize <= 0)
        {
            //reload
            magSize = 10;
            Debug.Log("RELOADING");
        }
        else if (magSize >= 1)
        {
            magSize --;
            //Debug.Log($"{gameObject.name} Attacks {target.name}");
            base.cooldown = base.attackCooldown;
            GameObject tempBullet = Instantiate(projectile, new Vector3(transform.position.x, transform.position.y, 1), Quaternion.identity);
            tempBullet.GetComponent<Bullet>().TarPos = base.autoTarget.transform.position;
            //target.GetComponent<GuardAITemplate>().hitPoints -= attackDamage;
            base.endlag = base.attackEndlag;
        }
    }
}
