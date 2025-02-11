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
    RectTransform rectTransform;
    Transform trans;
    Vector2 temp;
    void Start()
    {
        rectTransform = square.GetComponent<RectTransform>();
        trans = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (which == edge.N)
        {
            temp = trans.localScale;
            temp.x = rectTransform.sizeDelta.x+2;
            trans.localScale = temp;
        }
        if (which == edge.E)
        {
            temp = trans.localScale;
            temp.y = rectTransform.sizeDelta.y+2;
            trans.localScale = temp;
        }
        if (which == edge.S)
        {
            temp = trans.localScale;
            temp.x = -rectTransform.sizeDelta.x-2;
            trans.localScale = temp;
        }
        if (which == edge.W)
        {
            temp = trans.localScale;
            temp.y = -rectTransform.sizeDelta.y-2;
            trans.localScale = temp;
        }
    }
}
