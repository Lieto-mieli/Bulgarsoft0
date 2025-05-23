using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


//C
public class UpgradeSystem : MonoBehaviour
{
    public static UpgradeSystem Instance;
    public GuardAITemplate guardToUpgrade;
    public Button upgradeAttackButton;
    public Button upgradeReloadSpeed;
    public Button upgradeHealth;
    public int upgradePoints = 3;

    void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        upgradeAttackButton.onClick.AddListener(UpgradeAttackDamage);
    }

    void UpgradeAttackDamage()
    {
        if (upgradePoints > 0)
        {
            guardToUpgrade.attackDamage += 5f;
            upgradePoints--;
            Debug.Log("Attack Damage upgraded to: " + guardToUpgrade.attackDamage);
        }
        else
        {
            Debug.Log("Not enough upgrade points!");
        }
    }

    void UpgradeReloadSpeed()
    {
        if (upgradePoints > 0)
        {
            guardToUpgrade.attackCooldown += 5f;
            upgradePoints--;
            Debug.Log("Reload Speed upgraded to: " + guardToUpgrade.attackCooldown);
        }
        else
        {
            Debug.Log("Not enough upgrade points!");
        }
    }
    void UpgradeHealth()
    {
        if (upgradePoints > 0)
        {
            guardToUpgrade.hitPoints += 5f;
            upgradePoints--;
            Debug.Log("Health upgraded to: " + guardToUpgrade.hitPoints);
        }
        else
        {
            Debug.Log("Not enough upgrade points!");
        }
    }

}