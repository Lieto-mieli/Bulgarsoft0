using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Bullet : MonoBehaviour
{
    /// <summary>
    /// kalle kalle
    /// </summary>
    public Vector2 TarPos; //apuna targettauksen laskuun vaan nykyaa
    public Vector2 spawnPos;
    public Vector2 curPos;
    public Vector2 fixedTrajectory; //uus targettaus
    float velocity = 12.0f;
    private void Start()
    {
        spawnPos = transform.position; //spawnpos on valttamaton jotta luoti lentaa oikeaan suuntaan
    }
    void Update()
    {
        curPos = transform.position; //vain collison detection kayttaa
        Collider2D hitCollider = Physics2D.OverlapCircle(curPos, 0.1f); //easy collision detect    
        fixedTrajectory = (TarPos - spawnPos).normalized; //stackoverflow sano et toimii tolla normalizedilla
        transform.position += (Vector3)(fixedTrajectory * velocity * Time.deltaTime); //toimiva bulletin liike ilman etta pysahtyy tarposissa
        //transform.position = Vector2.Lerp(curPos, TarPos, velocity * Time.deltaTime); //Legacy matkustaminen, stoppas aina tposissa
        if (hitCollider != null && Vector2.Distance((Vector2)curPos, TarPos) < 1)  
        {
            if (hitCollider.CompareTag("Guard"))//osuuko gardiin
            {
                hitCollider.gameObject.GetComponent<GuardAITemplate>().hitPoints -= 1; //reduce hp
                Destroy(gameObject);
            }
            else if (hitCollider.CompareTag("Enemy")) //osuuko vihuun
            {
                hitCollider.gameObject.GetComponent<EnemyAITemplate>().hitPoints -= 1;
                Destroy(gameObject);
            }
            /*else if ((Vector2)transform.position == TarPos && hitCollider == null)
            {
                Destroy(gameObject);
            }*/
        }
    }
}
