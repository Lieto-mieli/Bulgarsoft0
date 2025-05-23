using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

//C
public class UpgradePointDisplay : MonoBehaviour
{
    //public Text upgradeText; // Reference to a UI Text component
    public TMP_Text upgradeText;
    public UpgradeSystem upgradeSystem;
    int currentPoints;

    private void Start()
    {
        currentPoints = UpgradeSystem.Instance.upgradePoints;
    }

    void Update()
    {
        if (UpgradeSystem.Instance.upgradePoints > 0)
        {
            upgradeText.text = "Upgrade Points: " + UpgradeSystem.Instance.upgradePoints;
        }
        else
        {
            upgradeText.text = "No upgrade points left!";
        }
    }
}
