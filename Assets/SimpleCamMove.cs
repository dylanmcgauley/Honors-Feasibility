using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleCamMove : MonoBehaviour
{
    public bool drawingLevel = false;

    // Update is called once per frame
    void Update()
    {
        if (!drawingLevel)
        {
            // simple camera side scroll
            transform.Translate(new Vector3(0.01f, 0, 0));
        }
        else
        {
            // allow the player to move the camera when painting training data
            if (Input.GetKeyDown(KeyCode.A)) transform.Translate(new Vector3(-5f, 0, 0));
            else if (Input.GetKeyDown(KeyCode.D)) transform.Translate(new Vector3(5f, 0, 0));
        }
    }
}
