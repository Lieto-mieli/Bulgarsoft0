using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Guard1AI : GuardAITemplate
{
    void Start()
    {
        //Guard1Stats
        moveSpeed = 2;
        hitPoints = 10;
        attackDamage = 2;
        attackRange = 2;
        attackCooldown = 1.2f;
        attackEndlag = 0.2f;
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
