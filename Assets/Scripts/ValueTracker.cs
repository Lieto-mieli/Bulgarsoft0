using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Unity.Mathematics;
using UnityEditor.SearchService;
using UnityEngine.SceneManagement;

public class ValueTracker : MonoBehaviour
{
    public WindowControl windowControl;
    public TextMeshProUGUI cashCounter;
    public GameObject gameplaySum;
    public GameObject buymenuSum;
    public int playerCash;
    public int waveNum;
    public List<GameObject> playerUnits;
    public WaveManager waveManager;
    public bool preWave;
    public float preWaveTimer;
    public GameObject preWaveScreen;
    public TextMeshProUGUI preWaveDayCount;
    public GameObject postWaveScreen;
    public TextMeshProUGUI postWaveDayCount;
    public TextMeshProUGUI postWaveCashBefore;
    public TextMeshProUGUI postWaveCashGain;
    public TextMeshProUGUI postWaveCashAfter;
    public TextMeshProUGUI postWaveGuardsLost;
    public TextMeshProUGUI postWaveEnemiesKilled;
    public int guardsLost;
    public int enemiesKilled;
    void Start()
    {
        if (PlayerPrefs.HasKey("WindowType"))
        {
            windowControl.ChangeType(PlayerPrefs.GetInt("WindowType"));
        }
        waveNum = 1;
        playerCash = 0;
        PreWave();
    }
    void Update()
    {
        if (cashCounter.IsActive())
        {
            cashCounter.text = $"{playerCash} $";
        }
        if (preWave)
        {
            preWaveTimer -= Time.deltaTime;
            if (preWaveTimer < 0)
            {
                StartWave();
                preWave = false;
            }
        }
        //c
        if (playerUnits.Count == 0)
        {
            SceneManager.LoadScene("EndMenu");
        }
        //c
    }
    public void PreWave()
    {
        Color temp;
        buymenuSum.SetActive(false);
        preWave = true;
        preWaveTimer = 1.2f;
        preWaveDayCount.text = $"Day {waveNum}";
        temp = new Color(1, (227f - (waveNum * 2)) / 255f, (204f - (waveNum * 2)) / 255f, 1);
        preWaveDayCount.color = temp;
        preWaveScreen.SetActive(true);
    }
    public void StartWave()
    {
        preWaveScreen.SetActive(false);
        gameplaySum.SetActive(true);
        waveManager.NewWave();
        //foreach (GameObject unit in playerUnits)
        //{
        //    unit.transform.position = new Vector2(UnityEngine.Random.Range(12.5f, 13.5f), UnityEngine.Random.Range(4.5f, 5.5f));
        //}
    }
    public void PostWave()
    {
        Color temp;
        float cashBefore = playerCash;
        //monetary changes to playe cash happen here!!
        playerCash += waveNum * 150;

        gameplaySum.SetActive(false);
        postWaveDayCount.text = $"Day {waveNum} Completed";
        postWaveCashBefore.text = $"{cashBefore} $";
        temp = ChooseTextColor(cashBefore,true);
        postWaveCashBefore.color = temp;
        postWaveCashGain.text = $"+{waveNum * 150} $";
        //  These two are currently useless, but may be useful at somepoint
        //temp = ChooseTextColor(waveNum * 150, true);
        //postWaveCashGain.color = temp;
        postWaveCashAfter.text = $"{playerCash} $";
        temp = ChooseTextColor(playerCash - cashBefore, true);
        postWaveCashAfter.color = temp;
        postWaveGuardsLost.text = $"{guardsLost}x";
        temp = ChooseTextColor(guardsLost, false);
        postWaveGuardsLost.color = temp;
        postWaveEnemiesKilled.text = $"{enemiesKilled}x";
        postWaveEnemiesKilled.color = new Color(125f / 255f, 200f / 255f, 125f / 255f, 1); ;
        postWaveScreen.SetActive(true);
    }
    public void EndWave()
    {
        postWaveScreen.SetActive(false);
        buymenuSum.SetActive(true);
        foreach (GameObject unit in playerUnits)
        {
            GuardAITemplate u = unit.GetComponent<GuardAITemplate>();
            u.hitPoints = Mathf.Min(u.maxHp, u.hitPoints += (u.maxHp * 0.2f));
            u.currentState = GuardAITemplate.GuardState.Passive;
            u.ignoreTargets = false;
        }
        waveNum++;
        guardsLost = 0;
        enemiesKilled = 0;
    }
    private Color ChooseTextColor(float value, bool expectedValue)
    {
        Color color;
        if(expectedValue)
        {
            if (value == 0)
            {
                color = new Color(175f / 255f, 175f / 255f, 175f / 255f, 1);
            }
            else if (value > 0)
            {
                color = new Color(125f / 255f, 1, 125f / 255f, 1);
            }
            else
            {
                color = new Color(1, 125f / 255f, 125f / 255f, 1);
            }
        }
        else
        {
            if (value == 0)
            {
                color = new Color(175f / 255f, 175f / 255f, 175f / 255f, 1);
            }
            else if (value < 0)
            {
                color = new Color(125f / 255f, 1, 125f / 255f, 1);
            }
            else
            {
                color = new Color(1, 125f / 255f, 125f / 255f, 1);
            }
        }
        return color;
    }

}
