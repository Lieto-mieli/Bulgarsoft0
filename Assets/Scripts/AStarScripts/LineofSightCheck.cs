using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Principal;
using UnityEngine;

public class LineofSightCheck : MonoBehaviour
{
    private LineRenderer line;
    //private PolygonCollider2D polyCollider;
    private List<string> tagsToCheck;
    private bool tagCheck;
    private

    // Start is called before the first frame update
    void Start()
    {
        line = GetComponent<LineRenderer>();
        //polyCollider = GetComponent<PolygonCollider2D>();
       // NewLineCheck(line.GetPosition(0), line.GetPosition(1), new List<string> { "Terrain" });
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(Physics2D.IsTouching(polyCollider, GameObject.FindGameObjectWithTag("Terrain").GetComponent<Collider2D>()));
    }
    public bool NewLineCheck(Vector2 startPos, Vector2 endPos, List<string> checkedTags, float size) //checks if there are walls/terrain inbetween two points on the map
    {
        tagCheck = false;
        tagsToCheck = checkedTags;
        line.SetPositions(new Vector3[] { startPos, endPos });
        Vector3[] temp = new Vector3[2];
        line.GetPositions(temp);
        List<Vector2> colliderPoints = CalculateColliderPoints(temp, size);
        //polyCollider.SetPath(0, colliderPoints.ConvertAll(p => (Vector2)transform.InverseTransformPoint(p)));
        //Physics2D.simulationMode = SimulationMode2D.Script;
        //Physics2D.Simulate(0.02f);
        //Physics2D.simulationMode = SimulationMode2D.Update;
        foreach (string tag in tagsToCheck)
        {
            //Debug.Log(tag);
            //foreach (GameObject taggedObject in GameObject.FindGameObjectsWithTag(tag))
            //{
            //    //Debug.Log(taggedObject.name);
            //    if (polyCollider.IsTouching(taggedObject.GetComponent<Collider2D>()))
            //    {
            //        tagCheck = true;
            //    }
            //}
            if(RaycastCheck(new List<Vector2> { colliderPoints[0], colliderPoints[1], colliderPoints[2], colliderPoints[3] }, tag))
            {
                tagCheck = true;
            }
        }
        //Debug.Log(tagCheck);
        return tagCheck;
    }
    public List<Vector2> CalculateColliderPoints(Vector3[] linePoints, float size) //calculate where to draw raycast checks, currently set to width 0
    {
        size = 0; //it barely functions but it does so i wont change it yet
        Vector3[] positions = linePoints;
        //foreach (Vector2 position in positions)
        //{
        //    Debug.Log($"{position.x}, {position.y}");
        //}
        Vector3 direction = positions[1] - positions[0];
        Vector3 right = new Vector3(direction.y, -direction.x, 0).normalized;
        Vector3 left = new Vector3(-direction.y, direction.x, 0).normalized;
        float width = size;
        List<Vector2> raycastPoints = new List<Vector2>
        {
            positions[0] + left * width,
            positions[1] + left * width,
            positions[1] + right * width,
            positions[0] + right * width,
        };
        //foreach (Vector2 position in raycastPoints)
        //{
        //    Debug.Log($"{position.x}, {position.y}");
        //}
        return raycastPoints;
        //float m =  (positions[1].y - positions[0].y) / (positions[1].x - positions[0].x);
        //float xDelta = (width / 2) * (m / Mathf.Pow(m * m + 1, 0.5f));
        //float yDelta = (width / 2) * (1 / Mathf.Pow(1 * m + m, 0.5f));
        //Vector3[] offsets = new Vector3[2];
        //offsets[0] = new Vector3(-xDelta, yDelta);
        //offsets[1] = new Vector3(xDelta, -yDelta);
        //foreach (Vector2 position in offsets)
        //{
        //    Debug.Log($"{position.x}, {position.y}");
        //}

        //List<Vector2> colliderPoints = new List<Vector2>
        //{
        //    positions[0] + offsets[0],
        //    positions[1] + offsets[0],
        //    positions[1] + offsets[1],
        //    positions[0] + offsets[1],
        //};
        //foreach (Vector2 position in colliderPoints)
        //{
        //    Debug.Log($"{position.x}, {position.y}");
        //}
        //return colliderPoints;
    }
    public bool RaycastCheck(List<Vector2> points, string tagToCheck) //check if raycast check hits an object with tagToCheck tag, could shit the bed if theres more than 99 hits but im sure that wont happen :)
    {
        bool returnValue = false;
        RaycastHit2D[] hit0 = new RaycastHit2D[99];
        RaycastHit2D[] hit1 = new RaycastHit2D[99]; ;
        Vector2 direction0 = points[1] - points[0];
        Vector2 direction1 = points[2] - points[3];
        ContactFilter2D filt = new ContactFilter2D();

        float distance = Vector2.Distance(points[0], points[1]);
        Physics2D.Raycast(points[0], direction0, filt, hit0, distance) ;
        Debug.DrawLine(points[0], points[0] + direction0.normalized * distance, Color.magenta, 1f);
        Physics2D.Raycast(points[3], direction1, filt, hit1, distance);
        Debug.DrawLine(points[3], points[3] + direction1.normalized * distance, Color.magenta, 1f);
        foreach(RaycastHit2D hit in hit0)
        {
            if (hit.collider != null)
            {
                if (hit.collider.gameObject.CompareTag(tagToCheck)) { returnValue = true; }
            }
        }
        foreach (RaycastHit2D hit in hit1)
        {
            if (hit.collider != null)
            {
                if (hit.collider.gameObject.CompareTag(tagToCheck)) { returnValue = true; }
            }
        }
        return returnValue;
    }
}
