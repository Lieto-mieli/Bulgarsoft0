using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Fent : GuardAITemplate
{
    // Start is called before the first frame update
    void Start()
    {
        hitPoints = 100;
        selected = false;
        selector = GameObject.FindWithTag("Selector");
        targetLists = GameObject.FindWithTag("TargetLists").GetComponent<AttackTargetLists>();
        targetLists.playerTargets.Add(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(selector.name);
        selected = selector.GetComponent<Selector>().buildingSelected == this.gameObject;
        if (hitPoints <= 0)
        {
            GameOver();
        }
    }
    void OnMouseDown()
    {
        //Debug.Log("mods, kill this guy.");
        if (!selected)
        {
            selector.GetComponent<Selector>().SelectBuilding(this.gameObject);
        }
        else
        {
            selector.GetComponent<Selector>().DeselectBuilding();
        }
    }
    public void GameOver()
    {

    }
    new public void MoveToPosition()
    {
        //Cannot move
    }
    new public void AttackTarget(GameObject target)
    {
        //Cannot attack
    }
}
