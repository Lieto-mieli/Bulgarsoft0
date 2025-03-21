using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    float spawnDelay;
    public float waveIntensity;
    public int waveMagnitude;
    public GameObject Enemy1;
    public ValueTracker valueTracker;
    public AttackTargetLists targetLists;
    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        if (waveMagnitude <= 0 && targetLists.enemyTargets.Count == 0)
        {
            valueTracker.EndWave();
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
        if (true)
        {
            Vector2 temp = new Vector2(1, Random.Range(2, 8));
            Instantiate(Enemy1, temp, new Quaternion()).transform.parent = valueTracker.gameplaySum.transform;
        }
    }
    public void NewWave()
    {
        waveIntensity = Mathf.Pow(valueTracker.waveNum, 1.3f);
        waveMagnitude = (int)(Mathf.Pow(valueTracker.waveNum, 1.3f)*2);
    }
}
