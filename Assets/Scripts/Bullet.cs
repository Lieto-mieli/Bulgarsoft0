using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Bullet : MonoBehaviour
{
    /// <summary>
    /// kalle kalle
    /// </summary>
    public Vector2 TarPos;
    Vector3 curPos;
    float velocity = 12.0f;
    void Update()
    {
        curPos = transform.position; //luodin sijainnin m‰‰ritys vektoreilla
        transform.position = Vector2.MoveTowards(curPos ,TarPos, velocity * Time.deltaTime); //liiku kohdetta kohti
        //Debug.Log(refScripti.targetPos + " " + TarPos);
        Collider2D hitCollider = Physics2D.OverlapCircle(curPos, 0.1f); //easy collision detector
        if(hitCollider != null && Vector2.Distance((Vector2)curPos, TarPos) < 1)  
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
        if(velocity == 0)
        {
            Destroy(gameObject);    
        }
    }
}
