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
    public Camera mainCamera; // Reference to the main camera for line-of-sight checks

    //C re made 

    // HÄTÄTILANNETTA VARTEN ÄLÄ POISTA
    //void Start()
    //{
    //    CacheWalkableTiles(); // Populate walkable tiles once
    //}

    //void Update()
    //{
    //    if (waveMagnitude <= 0 && targetLists.enemyTargets.Count == 0)
    //    {
    //        valueTracker.PostWave();
    //    }

    //    if (spawnDelay < 0 && waveMagnitude > 0)
    //    {
    //        spawnDelay = (10 / waveIntensity) * Random.Range(0.8f, 1.2f);
    //        SpawnEnemy();
    //        waveMagnitude -= 1;
    //    }

    //    spawnDelay -= Time.deltaTime;
    //}

    //void CacheWalkableTiles()
    //{
    //    SuperCustomProperties[] allTiles = FindObjectsOfType<SuperCustomProperties>();
    //    foreach (var tileProps in allTiles)
    //    {
    //        if (tileProps.HasProperty("passable") && tileProps.GetInt("passable") == 1)
    //        {
    //            walkableTiles.Add(tileProps.transform);
    //        }
    //    }

    //    if (walkableTiles.Count == 0)
    //    {
    //        Debug.LogWarning("No walkable tiles found! Enemies won't spawn.");
    //    }
    //}

    //public void SpawnEnemy()
    //{
    //    if (walkableTiles.Count == 0)
    //    {
    //        Debug.LogWarning("Spawn failed: No walkable tiles available.");
    //        return;
    //    }

    //    characterToSpawn = Random.Range(1, 3);
    //    Debug.Log(characterToSpawn);

    //    switch (characterToSpawn)
    //    {
    //        case 1: chosenEnemy = Enemy1; break;
    //        case 2: chosenEnemy = Enemy2; break;
    //    }

    //    Transform spawnTile = walkableTiles[Random.Range(0, walkableTiles.Count)];
    //    GameObject enemy = Instantiate(chosenEnemy, spawnTile.position, Quaternion.identity);
    //    enemy.transform.SetParent(valueTracker.gameplaySum.transform);
    //}

    //public void NewWave()
    //{
    //    waveIntensity = Mathf.Pow(valueTracker.waveNum, 1.35f);
    //    waveMagnitude = (int)(Mathf.Pow(valueTracker.waveNum, 1.35f) * 2)
    //}

    //C RE RE Remade
    void Start() // Unity method called once when the script starts
    {
        mainCamera = Camera.main; // Assigns the main camera from the scene
        CacheWalkableTiles(); // Populates the list of walkable tiles
    }

    void Update() // Unity method called once per frame
    {
        // If all enemies have been spawned and there are no remaining enemy targets
        if (waveMagnitude <= 0 && targetLists.enemyTargets.Count == 0)
        {
            valueTracker.PostWave(); // Signal end of the wave to ValueTracker
        }

        // If it's time to spawn and we still have enemies left to spawn
        if (spawnDelay < 0 && waveMagnitude > 0)
        {
            spawnDelay = (10 / waveIntensity) * Random.Range(0.8f, 1.2f); // Reset spawn delay with slight randomness
            SpawnEnemy(); // Call the method to spawn an enemy
            waveMagnitude -= 1; // Decrease the number of enemies remaining to spawn
        }

        spawnDelay -= Time.deltaTime; // Reduce spawn delay over time
    }

    void CacheWalkableTiles() // Populates walkableTiles list based on map data
    {
        SuperCustomProperties[] allTiles = FindObjectsOfType<SuperCustomProperties>(); // Get all tile objects with custom properties
        foreach (var tileProps in allTiles) // Loop through each tile
        {
            if (tileProps.HasProperty("passable") && tileProps.GetInt("passable") == 1) // Check if tile is marked as passable
            {
                walkableTiles.Add(tileProps.transform); // Add the tile's position to walkable list
            }
        }

        if (walkableTiles.Count == 0) // If no walkable tiles found
        {
            Debug.LogWarning("No walkable tiles found! Enemies won't spawn."); // Log a warning to help debug
        }
    }

    public void SpawnEnemy() // Spawns an enemy at a random valid tile
    {
        if (walkableTiles.Count == 0) // If no tiles are available
        {
            Debug.LogWarning("Spawn failed: No walkable tiles available."); // Warn and return
            return; // Exit the method early
        }

        // Create a new list of tiles not visible to the camera
        List<Transform> hiddenTiles = new List<Transform>();
        foreach (var tile in walkableTiles) // Loop through walkable tiles
        {
            if (!IsInCameraView(tile.position)) // Check if tile is not in camera view
            {
                hiddenTiles.Add(tile); // Add hidden tile to the list
            }
        }

        if (hiddenTiles.Count == 0) // If no hidden tiles found
        {
            Debug.LogWarning("No hidden tiles available, defaulting to random walkable tile."); // Warn and fallback to all tiles
            hiddenTiles = walkableTiles; // Use all walkable tiles instead
        }

        characterToSpawn = Random.Range(1, 3); // Randomly select which enemy to spawn (1 or 2)

        switch (characterToSpawn) // Choose enemy prefab based on selected number
        {
            case 1: chosenEnemy = Enemy1; break; // Choose Enemy1
            case 2: chosenEnemy = Enemy2; break; // Choose Enemy2
        }

        Transform spawnTile = hiddenTiles[Random.Range(0, hiddenTiles.Count)]; // Choose a random hidden tile to spawn on
        GameObject enemy = Instantiate(chosenEnemy, spawnTile.position, Quaternion.identity); // Spawn the enemy at the tile's position
        enemy.transform.SetParent(valueTracker.gameplaySum.transform); // Parent the enemy under gameplay container for organization
    }

    public void NewWave() // Called to start a new wave
    {
        waveIntensity = Mathf.Pow(valueTracker.waveNum, 1.35f); // Set wave intensity based on wave number
        waveMagnitude = (int)(Mathf.Pow(valueTracker.waveNum, 1.35f) * 2); // Set how many enemies to spawn this wave
    }

    bool IsInCameraView(Vector3 position) // Checks if a position is visible to the camera
    {
        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(mainCamera); // Get the camera's view frustum planes
        Bounds bounds = new Bounds(position, Vector3.one); // Create a bounding box at the position
        return GeometryUtility.TestPlanesAABB(planes, bounds); // Return whether the box is inside the view frustum
    }
}
