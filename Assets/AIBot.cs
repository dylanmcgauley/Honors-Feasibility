using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Algorithms;

public class AIBot : MonoBehaviour
{
    // public variables
    public Tilemap map;
    private Pathfinder pathFinder;

    private Vector2 destination;

    private float maxPosError = 1;

    private int currentNode = -1;

    // list to store all the nodes in the bots path
    private List<Vector2Int> botPath = new List<Vector2Int>();

    private void Start()
    {
        pathFinder = new Pathfinder(map);
    }

    private void Update()
    {
        // if screen is clicked move to clicked position
        if (Input.GetKeyDown(KeyCode.Mouse0)) TileSelected(map.WorldToCell(Camera.main.ScreenToWorldPoint(Input.mousePosition)));
    }

    // get the selected tile from the user and make sure it is on the ground
    private void TileSelected(Vector3Int tilePos)
    {
        int searchLimit = 15;
        int tries = 0;

        // while not on the ground move down untill a ground tile is found
        while (!map.HasTile(new Vector3Int(tilePos.x, tilePos.y, 1)))
        {
            if (tries < searchLimit)
            {
                tilePos.y--;
                tries++;
            }
            else return;
        }

        // move to block above the ground tile
        MoveToTarget(new Vector2Int(tilePos.x, tilePos.y + 1));
    }

    // check to see if the bot is on the ground
    private bool onGround (Vector2Int pos)
    {
        // if the block bellow the bot is a ground tile return true else return false
        if (map.HasTile(new Vector3Int(pos.x, pos.y - 1, 1))) return true;

        return false;
    }

    // Find a path and move to the target position
    private void MoveToTarget(Vector2Int target)
    {
        // get the tile the bot is stood on for the start position
        Vector3Int startPos = map.WorldToCell(transform.position);

        // clear the list of nodes
        botPath.Clear();

        if (pathFinder.FindNewPath(new Vector2Int(startPos.x, startPos.y), target))
        {
            currentNode = 1;
            botPath = pathFinder.GetPathNodes();
        }
        else currentNode = -1;
    }

    // check to see the Bot has reached the next nodes X position
    private bool NodeXReached(Vector2 pathPos, Vector2 prevPos, Vector2 currentPos)
    {
        if ((prevPos.x <= currentPos.x && pathPos.x >= currentPos.x)
            || (prevPos.x >= currentPos.x && pathPos.x <= currentPos.x)
            || Mathf.Abs(pathPos.x - currentPos.x) <= maxPosError) return true;
        else return false;
    }

    // check to see if the Bot has reached the next nodes Y position
    private bool NodeYReached(Vector2 pathPos, Vector2 prevPos, Vector2 currentPos)
    {
        if ((prevPos.y <= currentPos.y && pathPos.y >= currentPos.y)
            || (prevPos.y >= currentPos.y && pathPos.y <= currentPos.y)
            || (Mathf.Abs(pathPos.y - currentPos.y) <= maxPosError)) return true;
        else return false;
    }

    // retrieve all of the path data needed for the bot
    private void GetPathData (out Vector2 prevPos, out Vector2 currentPos, out Vector2 nextPos, out bool nextPosGround, out bool xReached, out bool yReached)
    {
        // set previous position to the last node reached
        prevPos = new Vector2(botPath[currentNode - 1].x, botPath[currentNode - 1].y);

        // set the current position to the current target node
        currentPos = new Vector2(botPath[currentNode].x, botPath[currentNode].y);

        // set the next position to the current target node
        nextPos = currentPos;

        // if the path continues set the next node to be the 
        if (botPath.Count > currentNode + 1) nextPos = new Vector2(botPath[currentNode + 1].x, botPath[currentNode + 1].y);
        // set the next position to the current target node
        else nextPos = currentPos;

        // check to see if the next node is on the ground
        if (map.HasTile(new Vector3Int(botPath[currentNode].x, botPath[currentNode].y - 1, 1))) nextPosGround = true;
        else nextPosGround = false;

        // where the bot is in the map
        Vector2 botPosition = transform.position;

        // check to see if the X or Y position of the target node has been reached
        xReached = NodeXReached(botPosition, prevPos, currentPos);
        yReached = NodeYReached(botPosition, prevPos, currentPos);
    }

    // Draw the path to the screen
    void OnDrawGizmos()
    {
        if (pathFinder != null)
        pathFinder.DrawPath();
    }
}
