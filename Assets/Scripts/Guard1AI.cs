using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Guard1AI : GuardAITemplate
{
    void Start()
    {
        //Guard1Stats
        moveSpeed = UnitStatsList.unitStats[0][0];
        hitPoints = UnitStatsList.unitStats[0][1];
        maxHp = UnitStatsList.unitStats[0][1];
        attackDamage = UnitStatsList.unitStats[0][2];
        attackRange = UnitStatsList.unitStats[0][3];
        attackCooldown = UnitStatsList.unitStats[0][4];
        attackEndlag = UnitStatsList.unitStats[0][5];
        //;
        selector = GameObject.FindWithTag("Selector");
        camera = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
        targetLists = GameObject.FindWithTag("TargetLists").GetComponent<AttackTargetLists>();
        curPos.z = curPos.y;
        targetPos = curPos;
        targetLists.playerTargets.Add(gameObject);
        spriteRender = GetComponent<SpriteRenderer>();
        pathfinder = GameObject.FindWithTag("Pathfinder").GetComponent<Pathfinder>();
    }
}
