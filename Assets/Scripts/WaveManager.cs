using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    float spawnDelay;
    public float waveIntensity;
    public int waveMagnitude;
    public GameObject Enemy1;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (spawnDelay < 0 && waveMagnitude > 0) 
        { 
            spawnDelay = (10/waveIntensity)*Random.Range(0.8f, 1.2f);
            SpawnEnemy();
        }
        spawnDelay -= Time.deltaTime;
    }
    public void SpawnEnemy()
    {
        if (true)
        {
            Vector2 temp = new Vector2(1, Random.Range(2, 8));
            Instantiate(Enemy1,temp,new Quaternion());
        }
    }
}
