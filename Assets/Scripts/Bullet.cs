using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Vector2 TarPos;
    Vector3 curPos;
    float velocity = 6.0f;
    // Start is called before the first frame update

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        curPos = transform.position;
        transform.position = Vector2.MoveTowards(curPos ,TarPos, velocity * Time.deltaTime);
        //Debug.Log(refScripti.targetPos + " " + TarPos);
        Collider2D hitCollider = Physics2D.OverlapCircle(curPos, 0.1f);
        if(hitCollider != null)
        {
            if (hitCollider.CompareTag("Guard"))
            {
                hitCollider.gameObject.GetComponent<GuardAITemplate>().hitPoints -= 1;
                Destroy(gameObject);
            }
        }
    }
}
