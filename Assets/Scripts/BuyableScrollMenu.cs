using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System;
public class BuyableScrollMenu : MonoBehaviour
{
    public string curSelected;
    private static List<string> unitList = new List<string>() { 
        "guard1" ,
        "tower"
    }; //this will change depending on player unlocks
    private int nroOfUnits;
    public List<GameObject> selectButtonList = new List<GameObject>();
    public GameObject exampleButton;
    // moveSpeed, hitPoints, attackDamage, attackRange, attackCooldown, attackEndlag, defence
    private float[] statMaximums = new float[] { 3f, 100f, 15f, 8f, 5f, 1f, 3f };
    public GameObject infoDisplay;
    public ValueTracker valueTracker;
    public List<GameObject> units;
    public Canvas canvas;
    void Start()
    {
        nroOfUnits = unitList.Count;
        GetComponent<RectTransform>().sizeDelta.Set(GetComponent<RectTransform>().sizeDelta.x, nroOfUnits * 160);
        exampleButton.SetActive(true);
        int i = 0;
        foreach (string unit in unitList) 
        { 
            int id = UnitStatsList.IDList.IndexOf(unit);
            selectButtonList.Add(Instantiate(exampleButton));
            GameObject temp = selectButtonList[selectButtonList.Count - 1];
            temp.transform.SetParent(this.transform, false);
            float canvasScale = canvas.gameObject.transform.lossyScale.y;
            temp.transform.position = new Vector2(temp.transform.position.x, ((temp.transform.position.y-(160*i))));
            temp.name = unit;
            temp.transform.Find("Name").GetComponent<TextMeshProUGUI>().text = unit;
            temp.transform.Find("Damage/Slider").GetComponent<Slider>().value = UnitStatsList.unitStats[id][2] / statMaximums[2];
            temp.transform.Find("AttackSpeed/Slider").GetComponent<Slider>().value = 1-(((UnitStatsList.unitStats[id][4] / statMaximums[4]) + (UnitStatsList.unitStats[id][5] / statMaximums[5]))/2);
            temp.transform.Find("Health/Slider").GetComponent<Slider>().value = UnitStatsList.unitStats[id][1] / statMaximums[1];
            temp.transform.Find("Defence/Slider").GetComponent<Slider>().value = UnitStatsList.unitStats[id][6] / statMaximums[6];
            temp.transform.Find("MovementSpeed/Slider").GetComponent<Slider>().value = UnitStatsList.unitStats[id][0] / statMaximums[0];
            temp.transform.Find("Range/Slider").GetComponent<Slider>().value = UnitStatsList.unitStats[id][3] / statMaximums[3];
            temp.transform.Find("Image").GetComponent<Image>().sprite = Resources.Load<Sprite>(UnitStatsList.unitImages[id]);
            temp.transform.Find("Cost").GetComponent<TextMeshProUGUI>().text = $"Cost: {UnitStatsList.unitStats[id][7]}";
            i++;
        }
        exampleButton.SetActive(false);
    }
    public void BuyUnit()
    { 
        if (UnitStatsList.unitStats[UnitStatsList.IDList.IndexOf(curSelected)][7] <= valueTracker.playerCash)
        {
            valueTracker.playerCash -= (int)UnitStatsList.unitStats[UnitStatsList.IDList.IndexOf(curSelected)][7];
            valueTracker.playerUnits.Add(Instantiate(units[UnitStatsList.IDList.IndexOf(curSelected)]));
            valueTracker.playerUnits.Last().transform.SetParent(valueTracker.gameplaySum.transform,true);
            // cash register sound here would be nice?
        }
        else
        {
            // play error sound here?
        }
    }
}