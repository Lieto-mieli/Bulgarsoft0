using SuperTiled2Unity;
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
    public SuperMap map;
    private int characterToSpawn;
    private GameObject chosenEnemy;
    public SuperCustomProperties superCustomProperties;
    private List<Transform> walkableTiles = new List<Transform>();
    //C re made 
    void Start()
    {
        CacheWalkableTiles(); // Populate walkable tiles once
    }

    void Update()
    {
        if (waveMagnitude <= 0 && targetLists.enemyTargets.Count == 0)
        {
            valueTracker.PostWave();
        }

        if (spawnDelay < 0 && waveMagnitude > 0)
        {
            spawnDelay = (10 / waveIntensity) * Random.Range(0.8f, 1.2f);
            SpawnEnemy();
            waveMagnitude -= 1;
        }

        spawnDelay -= Time.deltaTime;
    }

    void CacheWalkableTiles()
    {
        SuperCustomProperties[] allTiles = FindObjectsOfType<SuperCustomProperties>();
        foreach (var tileProps in allTiles)
        {
            if (tileProps.HasProperty("passable") && tileProps.GetInt("passable") == 1)
            {
                walkableTiles.Add(tileProps.transform);
            }
        }

        if (walkableTiles.Count == 0)
        {
            Debug.LogWarning("No walkable tiles found! Enemies won't spawn.");
        }
    }

    public void SpawnEnemy()
    {
        if (walkableTiles.Count == 0)
        {
            Debug.LogWarning("Spawn failed: No walkable tiles available.");
            return;
        }

        characterToSpawn = Random.Range(1, 3);
        Debug.Log(characterToSpawn);

        switch (characterToSpawn)
        {
            case 1: chosenEnemy = Enemy1; break;
            case 2: chosenEnemy = Enemy2; break;
        }

        Transform spawnTile = walkableTiles[Random.Range(0, walkableTiles.Count)];
        GameObject enemy = Instantiate(chosenEnemy, spawnTile.position, Quaternion.identity);
        enemy.transform.SetParent(valueTracker.gameplaySum.transform);
    }

    public void NewWave()
    {
        waveIntensity = Mathf.Pow(valueTracker.waveNum, 1.35f);
        waveMagnitude = (int)(Mathf.Pow(valueTracker.waveNum, 1.35f) * 2);
    }
}
