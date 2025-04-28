using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Guard1AI : GuardAITemplate
{
    int magSize;
    public GameObject projectile;
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
        size = UnitStatsList.unitStats[0][8];
        //;
        selector = GameObject.FindWithTag("Selector");
        camera = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
        targetLists = GameObject.FindWithTag("TargetLists").GetComponent<AttackTargetLists>();
        valueTracker = GameObject.FindWithTag("ValueTracker").GetComponent<ValueTracker>();
        curPos.z = curPos.y;
        targetPos = curPos;
        targetLists.playerTargets.Add(gameObject);
        spriteRender = GetComponent<SpriteRenderer>();
        pathfinder = GameObject.FindWithTag("Pathfinder").GetComponent<Pathfinder>();
    }

    public override void AttackTarget(GameObject target) //kalle alkaa
    {
        //Debug.Log(magSize);
        if (magSize <= 0)
        {
            //reload
            magSize = 10;
            Debug.Log("RELOADING");
            base.cooldown = base.attackCooldown;
            base.endlag = base.attackEndlag;
            Debug.Log("lattaa");
        }
        else if (magSize >= 1)
        {
            Debug.Log("mikesi toimi)");
            magSize--;
            //Debug.Log($"{gameObject.name} Attacks {target.name}");
            base.cooldown = base.attackCooldown;
            GameObject tempBullet = Instantiate(projectile, new Vector3(transform.position.x, transform.position.y, 1), Quaternion.identity);
            tempBullet.GetComponent<Bullet>().TarPos = base.autoTarget.transform.position;
            //target.GetComponent<GuardAITemplate>().hitPoints -= attackDamage;
            base.endlag = base.attackEndlag;
        }
    }

    public override void AttackMove()
    {
        GameObject tempBullet = new GameObject();
        manualTarget = tempBullet;
        tempBullet.transform.position = camera.ScreenToWorldPoint(Input.mousePosition);
    } //kalle loppuu
}
