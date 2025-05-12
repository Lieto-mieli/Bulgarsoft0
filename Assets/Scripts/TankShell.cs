using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class TankShell : MonoBehaviour
{
    /// <summary>
    /// kalle kalle
    /// </summary>
    public Vector3 TarPos;
    public Fent TowerRef;
    public Vector3 curPos;
    float velocity = 24.0f;

    void Start()
    {
        TowerRef = GameObject.Find("Fent").GetComponent<Fent>();
        TarPos.x = TowerRef.personalCurPos.x;
        TarPos.y = TowerRef.personalCurPos.y;
    }

    void Update()
    {
        curPos = transform.position; //panoksen sijainnin maaritys vektoreilla
        transform.position = Vector2.MoveTowards(curPos ,TarPos, velocity * Time.deltaTime); //liiku kohdetta kohti
        Collider2D hitCollider = Physics2D.OverlapCircle(curPos, 0.3f); //easy collision detector
        if(hitCollider != null && Vector2.Distance((Vector2)curPos, TarPos) < 1)  
        {
            if (hitCollider.CompareTag("Guard"))//osuuko gardiin
            {
                hitCollider.gameObject.GetComponent<GuardAITemplate>().hitPoints -= 15; //reduce hp
                Destroy(gameObject);
            }
        }
        if(velocity == 0)
        {
            Destroy(gameObject);    
        }
    }
}
