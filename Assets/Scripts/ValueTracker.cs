using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.SearchService;
using UnityEngine;
using Unity.Mathematics;
using System.IO;

public class ValueTracker : MonoBehaviour
{
    public TextMeshProUGUI cashCounter;
    public GameObject gameplaySum;
    public GameObject buymenuSum;
    public int playerCash;
    public int waveNum;
    public List<GameObject> playerUnits;
    public WaveManager waveManager;
    void Start()
    {
        waveNum = 1;
        playerCash = 100;
    }
    void Update()
    {
        if (cashCounter.IsActive())
        {
            cashCounter.text = $"{playerCash} $";
        }
    }
    public void StartWave()
    {
        gameplaySum.SetActive(true);
        buymenuSum.SetActive(false);
        waveManager.NewWave();
        foreach (GameObject unit in playerUnits)
        {
            unit.transform.position = new Vector2(13, 5);
        }
    }
    public void EndWave()
    {
        playerCash += waveNum * 150;
        gameplaySum.SetActive(false);
        buymenuSum.SetActive(true);
        foreach (GameObject unit in playerUnits)
        {
            unit.GetComponent<GuardAITemplate>().hitPoints = Mathf.Min(unit.GetComponent<GuardAITemplate>().maxHp, unit.GetComponent<GuardAITemplate>().hitPoints += (unit.GetComponent<GuardAITemplate>().maxHp * 0.2f));
        }
        waveNum++;
    }
}
