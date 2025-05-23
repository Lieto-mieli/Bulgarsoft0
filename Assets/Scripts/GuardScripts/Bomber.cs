using UnityEngine;

public class Bomber : MonoBehaviour
{
    
    public float moveSpeed = 10f;
    public float delOutOfBounds = 40f;
    public GameObject bomb;
    public float dropDistance = 9f;
    public float bombCooldown = 2f;

    private float lastBombTime = -999f;
    private GameObject targetEnemy;

    public AttackTargetLists targetLists;
    
    void Start()
    {
        // Get the target list and register this bomber as a player target
        targetLists = GameObject.FindWithTag("TargetLists").GetComponent<AttackTargetLists>();
        targetLists.playerTargets.Add(gameObject);
    }

    void Update()
    {
        DeleteOutOfBounds();
        DropBombs();
    }

    void DeleteOutOfBounds()
    {
        // moves the bomber plane to the right
        transform.Translate(Vector2.right * moveSpeed * Time.deltaTime);

        // if bomber plane is out of bounds, destroy the bomber.
        if (transform.position.x > delOutOfBounds)
        {
            Destroy(gameObject);
        }
    }

    void DropBombs()
    {
        // Loop through all enemies in the target list
        foreach (GameObject enemy in targetLists.enemyTargets)
        {
            if (enemy == null) continue;

            // Calculate horizontal and vertical distance to enemy
            float xDistance = Mathf.Abs(enemy.transform.position.x - transform.position.x);
            float yDistance = Mathf.Abs(enemy.transform.position.y - transform.position.y);

            if (xDistance < 1f && yDistance <= dropDistance && Time.time - lastBombTime > bombCooldown)
            {
                Debug.Log("Dropping bomb on: " + enemy.name);

                // Create a new bomb at bomber's position
                GameObject newBomb = Instantiate(bomb, transform.position, Quaternion.identity);

                // Set the bombs target Y position to the enemys Y
                Bomb bomb1 = newBomb.GetComponent<Bomb>();
                if (bomb1 != null)
                {
                    bomb1.SetTargetY(enemy.transform.position.y);
                }

                // Update the last drop time and exit loop
                lastBombTime = Time.time;
                break; // Only drop one bomb per update
            }
        }
    }
}
