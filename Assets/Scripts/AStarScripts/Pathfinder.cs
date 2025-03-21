using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Pathfinder : MonoBehaviour
{

    //[SerializeField] private PathfindingDebugStepVisual pathfindingDebugStepVisual;
    //[SerializeField] private PathfindingVisual pathfindingVisual;
    private Pathfinding pathfinding;
    private new Camera camera;
    [SerializeField]private LineofSightCheck LoSCheck;
    List<PathNode> path;
    //bool gaming = false;
    //float delay = 0;
    public float speed;
    //Vector3 curPos;
    public GameObject greenBox;
    public GameObject redBox;
    List<Vector2> shortcutPath;

    private void Start()
    {
        camera = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
        pathfinding = new Pathfinding(20, 10, new GameObject[] {greenBox,redBox});
        //pathfindingDebugStepVisual.Setup(pathfinding.GetGrid());
        //pathfindingVisual.SetGrid(pathfinding.GetGrid());
    }

    private void Update()
    {
        //delay -= Time.deltaTime;

        //if (gaming && delay < 0)
        //{
        //    delay = speed;
        //    path.Remove(path[0]);
        //    transform.position = new Vector2(path[0].x, path[0].y);
        //    if (path.Count < 2) { gaming = false; }
        //}

        //if (gaming)
        //{
        //    curPos = new Vector2(transform.position.x, transform.position.y);
        //    curPos.z = curPos.y;
        //    transform.position = Vector2.MoveTowards(curPos, shortcutPath[0], 4 * Time.deltaTime);
        //    if (Vector2.Distance((Vector2)curPos, shortcutPath[0]) < 0.02f)
        //    {
        //        shortcutPath.Remove(shortcutPath[0]);
        //        if (shortcutPath.Count < 1) { gaming = false; }
        //    }
        //}

        //DEBUGTOOL(
        if (Input.GetKeyDown(KeyCode.W))
        {
            Vector3 mouseWorldPosition = camera.ScreenToWorldPoint(Input.mousePosition);
            mouseWorldPosition += new Vector3(0.5f, 0.5f, 0);
            pathfinding.GetGrid().GetXY(mouseWorldPosition, out int x, out int y);
            pathfinding.GetNode(x, y).SetIsWalkable(!pathfinding.GetNode(x, y).isWalkable);
            pathfinding.UpdateVisualDebug(pathfinding.GetGrid());
        }
        //DEBUGTOOL)
    }
    public List<Vector2> ShortcutPath(List<PathNode> input, Vector2 origin, Vector2 destination, float size)
    {
        //bool removeFirst = true;
        List<Vector2> output = new List<Vector2>() {origin};
        int inputArray = 0;
        Vector2 curChoice = Vector2.zero;
        while (inputArray < input.Count)
        {
            if (LoSCheck.NewLineCheck(output[output.Count - 1], new Vector2(input[inputArray].x, input[inputArray].y), new List<string>{ "Terrain" }, size))
            {
                if (inputArray == 0)
                {
                }
                else if (curChoice == Vector2.zero)
                {
                    output.Add(new Vector2(input[inputArray].x, input[inputArray].y));
                }
                else
                {
                    output.Add(curChoice);
                }
                curChoice = Vector2.zero;
            }
            else
            {
                curChoice = new Vector2(input[inputArray].x, input[inputArray].y);
            }
                inputArray++;
        }
        output.Add(destination);
        output.Remove(origin);
        return output;
    }
    public List<Vector2> Pathfind(Vector2 position, Vector2 destination, float size)
    {
        Vector2 trueOrigin = new Vector2(Convert.ToInt32(position.x), Convert.ToInt32(position.y));
        Vector2 mouseWorldPosition = destination;
        mouseWorldPosition += new Vector2(0.5f, 0.5f);
        pathfinding.GetGrid().GetXY(mouseWorldPosition, out int x, out int y);
        path = pathfinding.FindPath(Convert.ToInt32(position.x), Convert.ToInt32(position.y), x, y);
        if (path != null)
        {
            //for (int i = 0; i < path.Count - 1; i++)
            //{
            //    Debug.DrawLine(new Vector3(path[i].x, path[i].y), new Vector3(path[i + 1].x, path[i + 1].y), Color.green, 5f, false);
            //}
            mouseWorldPosition += new Vector2(-0.5f, -0.5f);
            shortcutPath = ShortcutPath(path, trueOrigin, mouseWorldPosition, size);
            //foreach(Vector2 pos in shortcutPath)
            //{
            //    Debug.Log($"({pos.x}, {pos.y})");
            //}
            return shortcutPath;
        }
        return null;
    }
}