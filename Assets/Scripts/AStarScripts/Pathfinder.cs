using SuperTiled2Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Pathfinder : MonoBehaviour
{
    private Pathfinding pathfinding;
    private new Camera camera;
    [SerializeField]private LineofSightCheck LoSCheck;
    List<PathNode> path;
    public float speed;
    public GameObject greenBox;
    public GameObject redBox;
    List<Vector2> shortcutPath;

    public GameObject maps;
    public Grid<PathNode> LoadMapGrid(GameObject map)
    {
        SuperMap sMap = map.GetComponent<SuperMap>();
        pathfinding = new Pathfinding(sMap.m_Width, sMap.m_Height, new GameObject[] { greenBox, redBox }, map);
        return pathfinding.GetGrid();
    }
    private void Start()
    {
        camera = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
        LoadMapGrid(maps.GetComponent<CurrentMap>().map);
    }
    private void Update()
    {
        //DEBUGTOOL(
        //if (Input.GetKeyDown(KeyCode.W))
        //{
        //    Vector3 mouseWorldPosition = camera.ScreenToWorldPoint(Input.mousePosition);
        //    mouseWorldPosition += new Vector3(0.5f, 0.5f, 0);
        //    pathfinding.GetGrid().GetXY(mouseWorldPosition, out int x, out int y);
        //    pathfinding.GetNode(x, y).SetIsWalkable(!pathfinding.GetNode(x, y).isWalkable);
        //    pathfinding.UpdateVisualDebug(pathfinding.GetGrid());
        //}
        //DEBUGTOOL)
    }
    public List<Vector2> ShortcutPath(List<PathNode> input, Vector2 origin, Vector2 destination, float size) // attempts get the shortest path when foregoing tile-based movement
    {
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
    public List<Vector2> Pathfind(Vector2 position, Vector2 destination, float size) // called by actors in the scene, which lets them pathfind
    {
        Vector2 trueOrigin = new Vector2(Convert.ToInt32(position.x), Convert.ToInt32(position.y));
        Vector2 mouseWorldPosition = destination;
        mouseWorldPosition += new Vector2(0.5f, 0.5f);
        pathfinding.GetGrid().GetXY(mouseWorldPosition, out int x, out int y);
        path = pathfinding.FindPath(Convert.ToInt32(position.x), Convert.ToInt32(position.y), x, y);
        //foreach (PathNode pathNode in path)
        //{
        //    pathNode.y = -pathNode.y;
        //    Debug.Log(pathNode.x + "," + pathNode.y);
        //}
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