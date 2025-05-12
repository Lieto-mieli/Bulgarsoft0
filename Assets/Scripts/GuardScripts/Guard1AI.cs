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
    public override void AttackTarget(GameObject target) //ranged hyökkays yritys
    {// kalle alkaa
        //Debug.Log(magSize);
        if (magSize <= 0) //jos tyhja mag
        {
            //reload
            magSize = 10;
            Debug.Log("RELOADING");
            base.cooldown = base.attackCooldown; //inheritatun cooldownin resettaus
            base.endlag = base.attackEndlag; // sama mutta endlag 
            Debug.Log("lattaa"); //endlag meinaa ettei voi tehö mitööön ja kooldown on ettei voi hyökata
        }
        else if (magSize >= 1) //jos tays mag
        {
            Debug.Log("mikesi toimi)");
            magSize--;
            //Debug.Log($"{gameObject.name} Attacks {target.name}");
            base.cooldown = base.attackCooldown;
            GameObject tempBullet = Instantiate(projectile, new Vector3(transform.position.x, transform.position.y, 1), Quaternion.identity); //bullet spawnaus
            tempBullet.GetComponent<Bullet>().TarPos = base.autoTarget.transform.position; //bullet targettaus mikaan ei toimi
            //target.GetComponent<GuardAITemplate>().hitPoints -= attackDamage;
            base.endlag = base.attackEndlag;
        }
    }

    public override void AttackMove() //yritys manuaali target hyökkayksesta DEFUNCT
    {
        GameObject tempBullet = new GameObject(); //tyhja?
        manualTarget = tempBullet;
        tempBullet.transform.position = camera.ScreenToWorldPoint(Input.mousePosition); //ammu kursorin suuntaan
    } //kalle loppuu
}
