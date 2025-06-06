﻿using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Tilemaps;
using SuperTiled2Unity;
using System.IO;
using System;
using static UnityEditor.PlayerSettings;

public class Pathfinding {

    private const int MOVE_STRAIGHT_COST = 10;
    private const int MOVE_DIAGONAL_COST = 14;

    public static Pathfinding Instance { get; private set; }

    private Grid<PathNode> grid;
    private List<PathNode> openList;
    private List<PathNode> closedList;
    private GameObject[] debugBoxes;
    private List<GameObject> currentDebug;
    //public void Start()
    //{
    //    grid.LoadMapGrid(maps[0]);
    //}
    public Pathfinding(int width, int height, GameObject tilemap) 
    {
        Instance = this;
        grid = new Grid<PathNode>(width, height, 1f, Vector3.zero, (Grid<PathNode> g, int x, int y) => new PathNode(g, x, y), tilemap);
    }
    public Pathfinding(int width, int height, GameObject[] debugBoxes, GameObject tilemap)
    {
        Instance = this;
        this.debugBoxes = debugBoxes;
        grid = new Grid<PathNode>(width, height, 1f, Vector3.zero, (Grid<PathNode> g, int x, int y) => new PathNode(g, x, y), tilemap);
        SetPassables(grid, tilemap);
        currentDebug = new List<GameObject>();
        UpdateVisualDebug(grid);
    }
    public void SetPassables(Grid<PathNode> grid, GameObject tilemap) //this sets which tiles cant be walked through based on the tilemap provided
    {
        SuperCustomProperties[] tiles = tilemap.GetComponentsInChildren<SuperCustomProperties>();
        float count = 0;
        foreach (SuperCustomProperties tile in tiles)
        {
            CustomProperty temp = null;
            //Debug.Log(count);
            //Debug.Log(tile.transform.position.x + "," + tile.transform.position.y);
            tile.m_Properties.TryGetProperty("passable", out temp);
            if (temp != null)
            {
                int y = (int)(count / 60);
                int x = (int)(count - (y * 60));
                //Debug.Log(x + "," + y);
                if (temp.m_Value == "0")
                {
                    float pos = count;
                    grid.GetGridObject(x, y).SetIsWalkable(false);
                    tile.gameObject.tag = "Terrain";
                    tile.gameObject.AddComponent<BoxCollider2D>();
                    //Vector2 tempCol = tile.GetComponent<BoxCollider2D>().size;
                    //tile.GetComponent<BoxCollider2D>().size.Set(0.64f, 0.64f);
                }
                else if (temp.m_Value == "1")
                {
                    //do nothing lol
                }
            }
            count++;
        }
    }
    public void UpdateVisualDebug(Grid<PathNode> grid) // this sets up coloured squares that indicate if a tile can be walked through, for debug purposes
    {
        if (currentDebug != null)
        {
            foreach (GameObject box in currentDebug)
            {
                GameObject.Destroy(box);
            }
            currentDebug.Clear();
        }
        for (int x = 0; x < grid.gridArray.GetLength(0); x++)
        {
            for (int y = 0; y < grid.gridArray.GetLength(1); y++)
            {
                if (grid.gridArray[x, y].isWalkable)
                {
                        currentDebug.Add(UnityEngine.Object.Instantiate(debugBoxes[0], new Vector3(x * grid.cellSize, (y * grid.cellSize), 99), new Quaternion()));
                    
                }
                else if (!grid.gridArray[x, y].isWalkable)
                {
                        currentDebug.Add(UnityEngine.Object.Instantiate(debugBoxes[1], new Vector3(x * grid.cellSize, (y * grid.cellSize), 99), new Quaternion()));
                }
            }
        }
    }
    public Grid<PathNode> GetGrid() 
    {
        return grid;
    }

    public List<Vector3> FindPath(Vector3 startWorldPosition, Vector3 endWorldPosition) {
        grid.GetXY(startWorldPosition, out int startX, out int startY);
        grid.GetXY(endWorldPosition, out int endX, out int endY);

        List<PathNode> path = FindPath(startX, startY, endX, endY);
        if (path == null) {
            return null;
        } else {
            List<Vector3> vectorPath = new List<Vector3>();
            foreach (PathNode pathNode in path) {
                vectorPath.Add(new Vector3(pathNode.x, pathNode.y) * grid.GetCellSize() + Vector3.one * grid.GetCellSize() * .5f);
            }
            return vectorPath;
        }
    }

    public List<PathNode> FindPath(int startX, int startY, int endX, int endY) //finds the shortest path between two tiles via a-star algorithm
    {
        PathNode startNode = grid.GetGridObject(startX, startY);
        PathNode endNode = grid.GetGridObject(endX, endY);
        if (startNode == null || endNode == null) {
            // Invalid Path
            return null;
        }
        openList = new List<PathNode> { startNode };
        closedList = new List<PathNode>();
        for (int x = 0; x < grid.GetWidth(); x++) 
        {
            for (int y = 0; y < grid.GetHeight(); y++) 
            {
                PathNode pathNode = grid.GetGridObject(x, y);
                pathNode.gCost = 99999999;
                pathNode.CalculateFCost();
                pathNode.cameFromNode = null;
            }
        }
        startNode.gCost = 0;
        startNode.hCost = CalculateDistanceCost(startNode, endNode);
        startNode.CalculateFCost();
        while (openList.Count > 0) 
        {
            PathNode currentNode = GetLowestFCostNode(openList);
            if (currentNode == endNode) 
            {
                return CalculatePath(endNode);
            }
            openList.Remove(currentNode);
            closedList.Add(currentNode);

            foreach (PathNode neighbourNode in GetNeighbourList(currentNode)) 
            {
                if (closedList.Contains(neighbourNode)) continue;
                if (!neighbourNode.isWalkable) 
                {
                    closedList.Add(neighbourNode);
                    continue;
                }
                int tentativeGCost = currentNode.gCost + CalculateDistanceCost(currentNode, neighbourNode);
                if (tentativeGCost < neighbourNode.gCost) 
                {
                    neighbourNode.cameFromNode = currentNode;
                    neighbourNode.gCost = tentativeGCost;
                    neighbourNode.hCost = CalculateDistanceCost(neighbourNode, endNode);
                    neighbourNode.CalculateFCost();
                    if (!openList.Contains(neighbourNode)) 
                    {
                        openList.Add(neighbourNode);
                    }
                }
                //PathfindingDebugStepVisual.Instance.TakeSnapshot(grid, currentNode, openList, closedList);
            }
        }

        // Out of nodes on the openList
        return null;
    }

    private List<PathNode> GetNeighbourList(PathNode currentNode) //returns tiles that are next to the given tile
    {
        List<PathNode> neighbourList = new List<PathNode>();

        if (currentNode.x - 1 >= 0) 
        {
            // Left
            neighbourList.Add(GetNode(currentNode.x - 1, currentNode.y));
            // Left Down
            if (currentNode.y - 1 >= 0) neighbourList.Add(GetNode(currentNode.x - 1, currentNode.y - 1));
            // Left Up
            if (currentNode.y + 1 < grid.GetHeight()) neighbourList.Add(GetNode(currentNode.x - 1, currentNode.y + 1));
        }
        if (currentNode.x + 1 < grid.GetWidth()) 
        {
            // Right
            neighbourList.Add(GetNode(currentNode.x + 1, currentNode.y));
            // Right Down
            if (currentNode.y - 1 >= 0) neighbourList.Add(GetNode(currentNode.x + 1, currentNode.y - 1));
            // Right Up
            if (currentNode.y + 1 < grid.GetHeight()) neighbourList.Add(GetNode(currentNode.x + 1, currentNode.y + 1));
        }
        // Down
        if (currentNode.y - 1 >= 0) neighbourList.Add(GetNode(currentNode.x, currentNode.y - 1));
        // Up
        if (currentNode.y + 1 < grid.GetHeight()) neighbourList.Add(GetNode(currentNode.x, currentNode.y + 1));

        return neighbourList;
    }

    public PathNode GetNode(int x, int y) 
    {
        return grid.GetGridObject(x, y);
    }

    private List<PathNode> CalculatePath(PathNode endNode) 
    {
        List<PathNode> path = new List<PathNode>();
        path.Add(endNode);
        PathNode currentNode = endNode;
        while (currentNode.cameFromNode != null) {
            path.Add(currentNode.cameFromNode);
            currentNode = currentNode.cameFromNode;
        }
        path.Reverse();
        return path;
    }

    private int CalculateDistanceCost(PathNode a, PathNode b) 
    {
        int xDistance = Mathf.Abs(a.x - b.x);
        int yDistance = Mathf.Abs(a.y - b.y);
        int remaining = Mathf.Abs(xDistance - yDistance);
        return MOVE_DIAGONAL_COST * Mathf.Min(xDistance, yDistance) + MOVE_STRAIGHT_COST * remaining;
    }

    private PathNode GetLowestFCostNode(List<PathNode> pathNodeList) 
    {
        PathNode lowestFCostNode = pathNodeList[0];
        for (int i = 1; i < pathNodeList.Count; i++) {
            if (pathNodeList[i].fCost < lowestFCostNode.fCost) {
                lowestFCostNode = pathNodeList[i];
            }
        }
        return lowestFCostNode;
    }

}
