using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MortarGuardAI : GuardAITemplate
{
    public float AoESize;
    public GameObject explosionParent;
    public List<ParticleSystem> explosion;
    // Start is called before the first frame update
    void Start()
    {
        //MortarGuardStats
        moveSpeed = UnitStatsList.unitStats[4][0];
        hitPoints = UnitStatsList.unitStats[4][1];
        maxHp = UnitStatsList.unitStats[4][1];
        attackDamage = UnitStatsList.unitStats[4][2];
        attackRange = UnitStatsList.unitStats[4][3];
        attackCooldown = UnitStatsList.unitStats[4][4];
        attackEndlag = UnitStatsList.unitStats[4][5];
        size = UnitStatsList.unitStats[4][8];

        AoESize = 2.5f;
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
    public override void AttackTarget(GameObject target)
    {
        base.cooldown = base.attackCooldown / atkSpeedMult;
        GameObject temp = Instantiate(explosionParent, target.transform.position, new Quaternion());
        temp.GetComponent<ParticleSystem>().Play();
        //temp.GetComponentsInChildren<ParticleSystem>(explosion);
        //foreach (ParticleSystem par in explosion)
        //{
        //    par.Play();
        //}
        Collider2D[] results = Physics2D.OverlapCircleAll(target.transform.position, AoESize);
        foreach (Collider2D c in results)
        {
            if (c.gameObject.CompareTag("Guard"))
            {
                c.gameObject.GetComponent<GuardAITemplate>().hitPoints -= attackDamage/2;
            }
            if (c.gameObject.CompareTag("Enemy"))
            {
                c.gameObject.GetComponent<EnemyAITemplate>().hitPoints -= attackDamage;
            }
            if(c.gameObject.CompareTag("Boss")) //enemy tagi paskoo nii jos pitaa boss tagin ja lisaa listiin nii toimii
            {
                c.gameObject.GetComponent<A7VMain>().hitPoints -= attackDamage;
            }
        }
        base.endlag = base.attackEndlag / atkSpeedMult;
    }
    public override void AttackMove()
    {
        GameObject temp = new GameObject();
        manualTarget = temp;
        temp.transform.position = camera.ScreenToWorldPoint(Input.mousePosition);
    }
}
