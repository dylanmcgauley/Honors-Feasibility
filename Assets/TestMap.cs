using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;

public class TestMap : MonoBehaviour
{
    // pathfinder and tilemap to use
    private Pathfinder pathFinder;
    public Tilemap map;

    private GameObject playablityText;

    // list to store all the nodes in the bots path
    private List<Vector2Int> pathNodes = new List<Vector2Int>();

    // vectors to hold start and end locations of the map
    private Vector2Int mapStart;
    private Vector2Int mapEnd;

    private bool draw = false;

    // Start is called before the first frame update
    void Start()
    {
        // initialize the pathfinder
        pathFinder = new Pathfinder(map);

        // find the UI text element
        playablityText = GameObject.Find("Main Camera/Canvas/Playability");
    }

    // Update is called once per frame
    void Update()
    {
        // if screen is clicked check map playability
        if (Input.GetKeyDown(KeyCode.Mouse1)) CheckMap();
    }

    // quick check for map playability
    public bool QuickCheck()
    {
        draw = false;

        for (int x = 5; x > 0; x--)
        {
            // search for the starting tile of the level
            for (int y = map.size.y; y > 0; y--)
            {
                if (map.HasTile(new Vector3Int(x, y, 1)))
                {
                    mapStart = new Vector2Int(x, y + 1);
                    goto StartFound;
                }
            }
        }

        StartFound:

        // search for the last tile of the map
        for (int x = map.size.x - 6; x > 5; x--)
        {
            for (int y = map.size.y; y > 0; y--)
            {
                if (map.HasTile(new Vector3Int(x, y, 1)))
                {
                    //map.SetTile(new Vector3Int(x, y + 2, 1), endTile);
                    mapEnd = new Vector2Int(x, y + 1);
                    goto EndFound;
                }
            }
        }

        EndFound:

        // return wether or not a path is found
        if (pathFinder.FindNewPath(mapStart, mapEnd))
        {
            return true;
        }
        else return false;
    }

    private void CheckMap()
    {
        draw = false;

        for (int x = 5; x > 0; x--)
        {
            // search for the starting tile of the level
            for (int y = map.size.y; y > 0; y--)
            {
                if (map.HasTile(new Vector3Int(x, y, 1)))
                {
                    mapStart = new Vector2Int(x, y + 1);
                    goto StartFound;
                }
            }
        }

        StartFound:

        // search for the last tile of the map
        for (int x = map.size.x - 6; x > 5; x--)
        {
            for (int y = map.size.y; y > 0; y--)
            {
                if (map.HasTile(new Vector3Int(x, y, 1)))
                {
                    //map.SetTile(new Vector3Int(x, y + 2, 1), endTile);
                    mapEnd = new Vector2Int(x, y + 1);
                    goto EndFound;
                }
            }
        }

        EndFound:

        // set the playability text to appear
        playablityText.SetActive(true);
        playablityText.GetComponent<Text>().text = "Unplayable";

        // if a path is found display to screen
        if (pathFinder.FindNewPath(mapStart, mapEnd))
        {
            playablityText.GetComponent<Text>().text = "Playable";
            draw = true;
        }
    }

    // draw the path to the screen
    void OnDrawGizmos()
    {
        if (draw)
        pathFinder.DrawPath();
    }
}
