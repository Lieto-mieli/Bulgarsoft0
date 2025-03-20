using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.SearchService;
using UnityEngine;

public class ValueTracker : MonoBehaviour
{
    public TextMeshProUGUI cashCounter;
    public GameObject gameplaySum;
    public GameObject buymenuSum;
    public int playerCash;
    public int waveNum;
    public List<GameObject> playerUnits;
    private void Update()
    {
        if (cashCounter.IsActive())
        {
            cashCounter.text = $"{playerCash} $";
        }
    }
    public void StartWave()
    {
        waveNum++;
        gameplaySum.SetActive(true);
        buymenuSum.SetActive(false);
        foreach (GameObject unit in playerUnits)
        {
            
        }
    }
    public void EndWave()
    {
        playerCash += waveNum * 200;
        gameplaySum.SetActive(false);
        buymenuSum.SetActive(true);
        foreach (GameObject unit in playerUnits)
        {
            unit.GetComponent<GuardAITemplate>().hitPoints = Mathf.Min(unit.GetComponent<GuardAITemplate>().maxHp, unit.GetComponent<GuardAITemplate>().hitPoints += (unit.GetComponent<GuardAITemplate>().maxHp * 0.2f));
        }
    }
}
