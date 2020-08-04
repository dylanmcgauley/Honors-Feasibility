using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Algorithms;

public class Pathfinder
{
    private PathSearch pathFinder;
    private Tilemap map;
    private byte[,] grid;

    // list to store all the nodes in the path
    private List<Vector2Int> pathNodes = new List<Vector2Int>();

    public Pathfinder(Tilemap tilemap)
    {
        // tile map to be used
        map = tilemap;

        // grid to save the map data to 
        grid = new byte[Mathf.NextPowerOfTwo(map.size.x), Mathf.NextPowerOfTwo(map.size.y)];
    }

    // start the pathfinder with the built grid and map
    private void InitializePathfinder()
    {
        FillGrid();

        if (grid.Length == 0) return;

        // give the pathfinder a grid to fill and a tilemap to use
        pathFinder = new PathSearch(grid, map);

        pathFinder.Formula = HeuristicFormula.Manhattan;
        //if false then diagonal movement will be prohibited
        pathFinder.Diagonals = false;
        //if true then diagonal movement will have higher cost
        pathFinder.HeavyDiagonals = false;
        //estimate of path length
        pathFinder.HeuristicEstimate = 6;
        pathFinder.PunishChangeDirection = false;
        pathFinder.TieBreaker = false;
        pathFinder.SearchLimit = 1000000;
        pathFinder.DebugProgress = false;
        pathFinder.DebugFoundPath = false;
    }

    // build the grid the algorithm will use
    private void FillGrid()
    {
        if (map.size.x > 0 && map.size.y > 0)
        {
            // grid to save the map data to 
            grid = new byte[Mathf.NextPowerOfTwo(map.size.x), Mathf.NextPowerOfTwo(map.size.y)];

            for (int x = 5; x < map.size.x - 5; x++)
            {
                for (int y = 0; y < map.size.y + 1; y++)
                {
                    if (map.HasTile(new Vector3Int(x, y, 1)))
                    {
                        grid[x, y] = 0;
                    }
                    else grid[x, y] = 1;
                }
            }
        }
    }

    // function to generate a new path through the level
    public bool FindNewPath(Vector2Int startPos, Vector2Int endPos)
    {
        // initialize the pathifinder with the current map
        InitializePathfinder();

        // ensure there is a grid
        if (grid.GetLength(0) == 0 || grid.GetLength(1) == 0) return false;

        // get a new path from the pathfinder
        var newPath = pathFinder.FindPath(startPos, endPos, 1, 1, 5);

        // clear the old path
        pathNodes.Clear();

        // check that a new path exists and has nodes
        if (newPath != null && newPath.Count > 1)
        {
            // fill the path list in reverse order so they are in correct order
            for (int x = newPath.Count - 1; x >= 0; x--) pathNodes.Add(newPath[x]);

            return true; // path was found
        }
        else return false; // path was not found
    }

    // get the list of path nodes to use
    public List<Vector2Int> GetPathNodes()
    {
        return pathNodes;
    }

    public void DrawPath()
    {
        //draw the path
        if (pathNodes != null && pathNodes.Count > 0)
        {
            var start = pathNodes[0];

            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(new Vector3(start.x, start.y, 1.0f), 0.25f);

            for (var i = 1; i < pathNodes.Count; ++i)
            {
                var end = pathNodes[i];
                Gizmos.color = Color.blue;
                Gizmos.DrawSphere(new Vector3(end.x, end.y, 1.0f), 0.25f);
                Gizmos.color = Color.red;
                Gizmos.DrawLine(new Vector3(start.x, start.y, 0.5f),
                                new Vector3(end.x, end.y, 0.5f));
                start = end;
            }
        }
    }
}
