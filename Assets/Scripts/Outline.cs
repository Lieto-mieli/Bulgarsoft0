using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Outline : MonoBehaviour
{
    //int virus = 0;
    Vector3 curPos;
    public GameObject guardToFollow;
    public GameObject selectList;
    // Start is called before the first frame update
    void Start()
    {
        selectList = GameObject.FindWithTag("Selector");
    }

    // Update is called once per frame
    void Update()
    {
        if (!selectList.GetComponent<Selector>().currentlySelected.Contains(guardToFollow)||selectList.GetComponent<Selector>().currentlySelected.Count == 0)
        {
            Destroy(gameObject);
        }
        curPos = guardToFollow.transform.position;
        curPos.z += 0.01f;
        transform.position = curPos;
        //if (virus == 1)
        //{
        //    virus = 0;
        //}
        //else
        //{
        //    Destroy(gameObject);
        //}
    }
}
