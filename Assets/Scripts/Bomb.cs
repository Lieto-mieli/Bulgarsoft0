using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Bomb : MonoBehaviour
{
    

    public float speed = 5f;
    public GameObject explosionParent;
    public List<ParticleSystem> explosion;
    private float targetY;
    private bool targetYSet = false;
    private WaitForSeconds bombTimer;
    private bool startedTimer = false;
    public float damage;

    public float AoESize = 2.5f;

    public void SetTargetY(float y)
    {
        targetY = y;
        targetYSet = true;
    }

    public void Start()
    {
        damage = UnitStatsList.unitStats[5][2];
    }
    void Update()
    {
        // Do nothing if the target Y position hasn't been set yet.
        if (!targetYSet)
        {
            return;
        }

        // Move bomb down toward the target Y position.
        if (transform.position.y > targetY)
        {
            transform.position += Vector3.down * speed * Time.deltaTime;

            // Start the bomb timer coroutine
            StartCoroutine(BombTimer());
            startedTimer = true;
        }


    }

    // Coroutine to wait, then explode
    IEnumerator BombTimer()
    {
        yield return new WaitForSeconds(3f);
        Debug.Log("BOOM");

        // Create the explosion effect
        GameObject temp = Instantiate(explosionParent, transform.position, new Quaternion());
        temp.GetComponent<ParticleSystem>().Play();

        // Damage all enemies and guards
        Collider2D[] results = Physics2D.OverlapCircleAll(transform.position, AoESize);
        foreach (Collider2D c in results)
        {
            if (c.gameObject.CompareTag("Guard"))
            {
                c.gameObject.GetComponent<GuardAITemplate>().hitPoints -= damage / 2;
            }
            if (c.gameObject.CompareTag("Enemy"))
            {
                c.gameObject.GetComponent<EnemyAITemplate>().hitPoints -= damage;
            }
        }


        Destroy(gameObject); // destroy the bomb
    }
}
