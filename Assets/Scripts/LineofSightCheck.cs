using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Principal;
using UnityEngine;

public class LineofSightCheck : MonoBehaviour
{
    private LineRenderer line;
    private PolygonCollider2D polyCollider;
    private List<string> tagsToCheck;
    private bool tagCheck;

    // Start is called before the first frame update
    void Start()
    {
        line = GetComponent<LineRenderer>();
        polyCollider = GetComponent<PolygonCollider2D>();
        NewLineCheck(line.GetPosition(0), line.GetPosition(1), new List<string> { "Terrain" });
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(Physics2D.IsTouching(polyCollider, GameObject.FindGameObjectWithTag("Terrain").GetComponent<Collider2D>()));
    }
    public bool NewLineCheck(Vector2 startPos, Vector2 endPos, List<string> checkedTags)
    {
        tagCheck = false;
        tagsToCheck = checkedTags;
        line.SetPositions(new Vector3[] { startPos, endPos });
        Vector3[] temp = new Vector3[2];
        line.GetPositions(temp);
        List<Vector2> temp2 = CalculateColliderPoints(temp);
        polyCollider.SetPath(0, temp2.ConvertAll(p => (Vector2)transform.InverseTransformPoint(p)));
        Physics2D.simulationMode = SimulationMode2D.Script;
        Physics2D.Simulate(0.02f);
        Physics2D.simulationMode = SimulationMode2D.Update;
        foreach (string tag in tagsToCheck)
        {
            //Debug.Log(tag);
            foreach (GameObject taggedObject in GameObject.FindGameObjectsWithTag(tag))
            {
                //Debug.Log(taggedObject.name);
                if (polyCollider.IsTouching(taggedObject.GetComponent<Collider2D>()))
                {
                    tagCheck = true;
                }
            }
        }
        return tagCheck;
    }
    public List<Vector2> CalculateColliderPoints(Vector3[] linePoints)
    {
        Vector3[] positions = linePoints;
        float width = line.startWidth;
        float m =  (positions[1].y - positions[0].y) / (positions[1].x - positions[0].x);
        float xDelta = (width / 2) * (m / Mathf.Pow(m * m + 1, 0.5f));
        float yDelta = (width / 2) * (1 / Mathf.Pow(1 * m + m, 0.5f));
        Vector3[] offsets = new Vector3[2];
        offsets[0] = new Vector3(-xDelta, yDelta);
        offsets[1] = new Vector3(xDelta, -yDelta);
        List<Vector2> colliderPoints = new List<Vector2>
        {
            positions[0] + offsets[0],
            positions[1] + offsets[0],
            positions[1] + offsets[1],
            positions[0] + offsets[1],
        };
        return colliderPoints;
    }
}
