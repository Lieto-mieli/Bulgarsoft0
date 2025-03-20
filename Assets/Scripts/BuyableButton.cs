using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuyableButton : MonoBehaviour
{
    public GameObject scrollMenuScriptObject;
    // Start is called before the first frame update
    public void LoadDesc()
    {
        scrollMenuScriptObject.GetComponent<BuyableScrollMenu>().curSelected = this.name;
        scrollMenuScriptObject.GetComponent<BuyableScrollMenu>().infoDisplay.transform.Find("Description").GetComponent<TextMeshProUGUI>().text = 
            UnitStatsList.unitDescriptions[UnitStatsList.IDList.IndexOf(this.name)];
        if (UnitStatsList.unitStats[UnitStatsList.IDList.IndexOf(this.name)][0] == 0)
        {
            scrollMenuScriptObject.GetComponent<BuyableScrollMenu>().infoDisplay.transform.Find("Buy/Text").GetComponent<TextMeshProUGUI>().text =
            $"Build for {UnitStatsList.unitStats[UnitStatsList.IDList.IndexOf(this.name)][7]}$";
        }
        else
        {
            scrollMenuScriptObject.GetComponent<BuyableScrollMenu>().infoDisplay.transform.Find("Buy/Text").GetComponent<TextMeshProUGUI>().text =
            $"Train for {UnitStatsList.unitStats[UnitStatsList.IDList.IndexOf(this.name)][7]}$";
        }
        scrollMenuScriptObject.GetComponent<BuyableScrollMenu>().curSelected = this.name;
    }
}
