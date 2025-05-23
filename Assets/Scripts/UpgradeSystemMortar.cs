using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//C
public class UpgradeSystemMortar : MonoBehaviour
{
    public GuardAITemplate guardToUpgrade;
    public Button upgradeAttackButton;
    public Button upgradeReloadSpeed;
    public Button upgradeHealth;

    int currentPoints = UpgradeSystem.Instance.upgradePoints;

    void Start()
    {
        upgradeAttackButton.onClick.AddListener(UpgradeAttackDamage);
        upgradeReloadSpeed.onClick.AddListener(UpgradeReloadSpeed);
        upgradeHealth.onClick.AddListener(UpgradeHealth);
    }

    void UpgradeAttackDamage()
    {
        if (UpgradeSystem.Instance.upgradePoints > 0)
        {
            guardToUpgrade.attackDamage += 5f;
            UpgradeSystem.Instance.upgradePoints--;
            Debug.Log("Attack Damage upgraded to: " + guardToUpgrade.attackDamage);
        }
        else
        {
            Debug.Log("Not enough upgrade points!");
        }
    }

    void UpgradeReloadSpeed()
    {
        if (UpgradeSystem.Instance.upgradePoints > 0)
        {
            guardToUpgrade.attackCooldown += 5f;
            UpgradeSystem.Instance.upgradePoints--;
            Debug.Log("Reload Speed upgraded to: " + guardToUpgrade.attackCooldown);
        }
        else
        {
            Debug.Log("Not enough upgrade points!");
        }
    }
    void UpgradeHealth()
    {
        if (UpgradeSystem.Instance.upgradePoints > 0)
        {
            guardToUpgrade.hitPoints += 5f;
            UpgradeSystem.Instance.upgradePoints--;
            Debug.Log("Health upgraded to: " + guardToUpgrade.hitPoints);
        }
        else
        {
            Debug.Log("Not enough upgrade points!");
        }
    }
}
