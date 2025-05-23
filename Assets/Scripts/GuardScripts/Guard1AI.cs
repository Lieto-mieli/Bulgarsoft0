using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Guard1AI : GuardAITemplate
{
    int magSize;
    public GameObject projectile;
    public AudioClip gunshotClip; // Assign in Inspector or load in code
    private AudioSource audioSource;
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
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;

    }
    public override void AttackTarget(GameObject target) //ranged hy�kk�ys yritys
    {// kalle alkaa
        //Debug.Log(magSize);
        if (magSize <= 0) //jos tyhj� mag
        {
            //reload
            magSize = 10;
            Debug.Log("RELOADING");
            base.cooldown = base.attackCooldown; //inheritatun cooldownin resettaus
            base.endlag = base.attackEndlag; // sama mutta endlag 
            Debug.Log("lattaa"); //endlag meinaa ettei voi teh� mit���n ja kooldown on ettei voi hy�k�t�
        }
        else if (magSize >= 1) //jos t�ys mag
        {
            Debug.Log("mikesi toimi)");
            magSize--;
            //Debug.Log($"{gameObject.name} Attacks {target.name}");
            base.cooldown = base.attackCooldown;
            GameObject tempBullet = Instantiate(projectile, new Vector3(transform.position.x, transform.position.y, 1), Quaternion.identity); //bullet spawnaus
            audioSource.PlayOneShot(gunshotClip);
            tempBullet.GetComponent<Bullet>().TarPos = base.autoTarget.transform.position; //bullet targettaus mik��n ei toimi
            //target.GetComponent<GuardAITemplate>().hitPoints -= attackDamage;
            base.endlag = base.attackEndlag;
        }
    }

    public override void AttackMove() //yritys manuaali target hy�kk�yksest� DEFUNCT
    {
        GameObject tempBullet = new GameObject(); //tyhj�?
        manualTarget = tempBullet;
        tempBullet.transform.position = camera.ScreenToWorldPoint(Input.mousePosition); //ammu kursorin suuntaan
    } //kalle loppuu
    
}
