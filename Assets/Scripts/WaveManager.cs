using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    float spawnDelay;
    public float waveIntensity;
    public int waveMagnitude;
    public GameObject Enemy1;
    public GameObject Enemy2;
    public ValueTracker valueTracker;
    public AttackTargetLists targetLists;
    private int characterToSpawn;
    private GameObject chosenEnemy;
    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        if (waveMagnitude <= 0 && targetLists.enemyTargets.Count == 0)
        {
            valueTracker.PostWave();
        }
        if (spawnDelay < 0 && waveMagnitude > 0) 
        { 
            spawnDelay = (10/waveIntensity)*Random.Range(0.8f, 1.2f);
            SpawnEnemy();
            waveMagnitude -= 1;
        }
        spawnDelay -= Time.deltaTime;
    }
    public void SpawnEnemy()
    {
        characterToSpawn = Random.Range(1, 3);
        Debug.Log(characterToSpawn);
        switch(characterToSpawn)
        {
            case 1: chosenEnemy = Enemy1;
                break;
            case 2: chosenEnemy = Enemy2;
                break;
        }
        Debug.Log(chosenEnemy);
        if (true)
        {
            Vector2 temp = new Vector2(1, Random.Range(2, 8));
            Instantiate(chosenEnemy, temp, new Quaternion()).transform.SetParent(valueTracker.gameplaySum.transform);
        }
    }
    public void NewWave()
    {
        waveIntensity = Mathf.Pow(valueTracker.waveNum, 1.3f);
        waveMagnitude = (int)(Mathf.Pow(valueTracker.waveNum, 1.3f)*2);
    }
}
