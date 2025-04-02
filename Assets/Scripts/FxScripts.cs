using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FxScripts : MonoBehaviour   
{
    float timer;
    // Start is called before the first frame update
    void Start()
    {
        timer = 5f;
    }

    // Update is called once per frame
    void Update()
    {
        timer-= Time.deltaTime;
        if (timer < 0)
        {
            Destroy(gameObject);
        }
    }
}
