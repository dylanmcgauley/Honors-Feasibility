using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class ScanGraph : MonoBehaviour
{
    // Path to update
    private AstarPath graph;

    private void Start()
    {
        // find the graph object in the scene
        graph = GameObject.Find("A* Grid").gameObject.GetComponent<AstarPath>();

        InvokeRepeating("UpdateGraph", 0f, 10f);
    }

    // update the graph every 2 seconds
    public void UpdateGraph()
    {
        graph.Scan();
    }
}
