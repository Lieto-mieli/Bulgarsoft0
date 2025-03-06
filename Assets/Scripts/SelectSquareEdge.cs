using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectSquareEdge : MonoBehaviour
{
    public enum edge
    {
        N,
        E,
        S,
        W,
    }
    public edge which;
    public GameObject square;
    public RectTransform rectTransform;
    public Transform trans;
    public Vector2 temp;
    void Start()
    {
        rectTransform = square.GetComponent<RectTransform>();
        trans = GetComponent<Transform>();
    }

    // Update is called once per frame
    //void Update()
    //{
    //    if (which == edge.N)
    //    {
    //        temp = trans.localScale;
    //        temp.x = rectTransform.sizeDelta.x+2;
    //        temp.y = 1;
    //        trans.localScale = temp;
    //    }
    //    if (which == edge.E)
    //    {
    //        temp = trans.localScale;
    //        temp.y = rectTransform.sizeDelta.y+2;
    //        temp.x = 1;
    //        trans.localScale = temp;
    //    }
    //    if (which == edge.S)
    //    {
    //        temp = trans.localScale;
    //        temp.x = -rectTransform.sizeDelta.x-2;
    //        temp.y = 1;
    //        trans.localScale = temp;
    //    }
    //    if (which == edge.W)
    //    {
    //        temp = trans.localScale;
    //        temp.y = -rectTransform.sizeDelta.y-2;
    //        temp.x = 1;
    //        trans.localScale = temp;
    //    }
    //}
}
