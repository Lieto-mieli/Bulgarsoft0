using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class EnemyAiTempRanged : EnemyAITemplate
{

    public GameObject projectile;
    enum EnemyState
    {
        Passive,
        MovingToPosition,
        AttackingTarget,
    }
    //Instantiate(projectile, new Vector3(transform.position.x, transform.position.y, 1), Quaternion.identity);
    public override void AttackTarget(GameObject target)
    {
        //Debug.Log($"{gameObject.name} Attacks {target.name}");
        base.cooldown = base.attackCooldown;
        GameObject tempBullet = Instantiate(projectile, new Vector3(transform.position.x, transform.position.y, 1), Quaternion.identity);
        tempBullet.GetComponent<Bullet>().TarPos = base.autoTarget.transform.position;
        //target.GetComponent<GuardAITemplate>().hitPoints -= attackDamage;
        base.endlag = base.attackEndlag;
    }
}
