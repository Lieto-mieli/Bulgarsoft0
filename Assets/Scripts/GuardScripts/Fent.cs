using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Fent : GuardAITemplate
{
    // Start is called before the first frame update
    static float ability1Cooldown = 45;
    static float ability1Duration = 15;
    public float curAbility1Cooldown;
    float curAbility1Duration;
    List<GameObject> ability1Targets;
    bool ability1Active;
    void Start()
    {
        ability1Active = false;
        moveSpeed = UnitStatsList.unitStats[2][0];
        hitPoints = UnitStatsList.unitStats[2][1];
        maxHp = UnitStatsList.unitStats[2][1];
        size = UnitStatsList.unitStats[2][8];
        selected = false;
        selector = GameObject.FindWithTag("Selector");
        targetLists = GameObject.FindWithTag("TargetLists").GetComponent<AttackTargetLists>();
        targetLists.playerTargets.Add(gameObject);
        Vector3 tempPos = transform.position;
        tempPos.z = tempPos.y;
        transform.position = tempPos;
    }
    // Update is called once per frame
    void Update()
    {
        curAbility1Cooldown -= Time.deltaTime;
        curAbility1Duration -= Time.deltaTime;
        //Debug.Log(selector.name);
        selected = selector.GetComponent<Selector>().buildingSelected == this.gameObject;
        if (hitPoints <= 0)
        {
            GameOver();
        }
        if (curAbility1Duration <= 0)
        {
            ability1Active = false;
        } 
        if (ability1Targets!=null)
        {
            if (!ability1Active && ability1Targets.Count != 0)
            {
                foreach (GameObject guard in ability1Targets)
                {
                    guard.GetComponent<GuardAITemplate>().atkSpeedMult = 1.0f;
                }
                ability1Targets.Clear();
            }
            else if (ability1Targets.Count != 0)
            {
                foreach (GameObject guard in ability1Targets)
                {
                    if (guard == null)
                    {
                        ability1Targets.Remove(guard);
                    }
                    else if (guard.GetComponent<GuardAITemplate>().atkSpeedMult != 1.3f)
                    {
                        guard.GetComponent<GuardAITemplate>().atkSpeedMult = 1.3f;
                    }
                }
            }
        }
        Collider2D[] results = Physics2D.OverlapBoxAll(transform.position, new Vector2(size, size), 0);
        foreach (Collider2D c in results)
        {
            //Debug.Log("guardshittis");
            if (c.gameObject.CompareTag("Guard") && c.gameObject != this.gameObject)
            {
                //curPos = new Vector2(transform.position.x, transform.position.y);
                //transform.position = Vector3.MoveTowards((Vector2)curPos, (Vector2)c.transform.position, MathF.Min(-0.5f+Vector2.Distance((Vector2)curPos, c.transform.position),0) * Time.deltaTime);
                if (moveSpeed != 0) { c.gameObject.GetComponent<GuardAITemplate>().PushAway(transform.position, size); }
                else if (moveSpeed == 0) { c.gameObject.GetComponent<GuardAITemplate>().PushAway(transform.position, size * 3); }
            }
            if (c.gameObject.CompareTag("Enemy"))
            {
                //curPos = new Vector2(transform.position.x, transform.position.y);
                //transform.position = Vector3.MoveTowards((Vector2)curPos, (Vector2)c.transform.position, MathF.Min(-0.5f+Vector2.Distance((Vector2)curPos, c.transform.position),0) * Time.deltaTime);
                if (moveSpeed != 0) { c.gameObject.GetComponent<EnemyAITemplate>().PushAway(transform.position, size * 3); }
                else if (moveSpeed == 0) { c.gameObject.GetComponent<EnemyAITemplate>().PushAway(transform.position, size * 3); }
            }
        }
    }
    void OnMouseDown()
    {
        //Debug.Log("mods, kill this guy.");
        //if (!selected)
        //{
        //    selector.GetComponent<Selector>().SelectBuilding(this.gameObject);
        //}
        //else
        //{
        //    selector.GetComponent<Selector>().DeselectBuilding();
        //}
    }
    public void GameOver()
    {
        //this will be a fail state
    }
    public override void MoveToPosition()
    {
        //Cannot move
    }
    public override void AttackTarget(GameObject target)
    {
        //Cannot attack
    }
    public void CallToArms() // give all guards within 10 units a 1.3x multiplier in attack frequency for 15 seconds, with a cooldown of 45
    {
        ability1Targets = new List<GameObject>();
        if (curAbility1Cooldown <= 0)
        {
            curAbility1Cooldown = ability1Cooldown;
            foreach (GameObject guard in GameObject.FindGameObjectsWithTag("Guard"))
            {
                if (Vector2.Distance(guard.transform.position, this.transform.position) <= 10)
                {
                    ability1Targets.Add(guard);
                }
            }
            foreach(GameObject guard in ability1Targets)
            {
                guard.GetComponent<GuardAITemplate>().atkSpeedMult = 1.3f;
            }
            ability1Active = true;
            curAbility1Duration = ability1Duration;
        }
    }
}
