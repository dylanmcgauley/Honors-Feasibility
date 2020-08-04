using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamMove : MonoBehaviour
{
    // public can variables
    public GameObject player;
    public Vector2 offset;

    // private cam variables
    private Vector2 followThreshold;
    private Vector2 follow;
    private float xDiff, yDiff;
    private Vector3 newPos;
    private Rigidbody2D rb;
    private float speed = 2f;

    private void Start()
    {
        // get the players rigid body
        rb = player.GetComponent<Rigidbody2D>();
        // calculate the follow threshold
        followThreshold = GetThreshold();
    }

    void FixedUpdate()
    {
        // set the follow object
        follow = player.transform.position;

        // calculate the x and y distances 
        xDiff = Vector2.Distance(Vector2.right * transform.position.x, Vector2.right * follow.x);
        yDiff = Vector2.Distance(Vector2.up * transform.position.y, Vector2.up * follow.y);

        newPos = transform.position; // reset position

        if (Mathf.Abs(xDiff) >= followThreshold.x) newPos.x = follow.x;
        if (Mathf.Abs(yDiff) >= followThreshold.y) newPos.y = follow.y;

        float camSpeed = rb.velocity.magnitude > speed ? rb.velocity.magnitude : speed;

        // move the camera
        transform.position = Vector3.MoveTowards(transform.position, newPos, camSpeed * Time.deltaTime);
    }

    // Debug box for threshold
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Vector2 border = GetThreshold();
        Gizmos.DrawWireCube(transform.position, new Vector3(border.x * 2, border.y * 2, 1));
    }

    // Calculate the threshold distance before the camera can move 
    private Vector3 GetThreshold()
    {
        // get the cameras aspect ratio
        Rect camAspect = Camera.main.pixelRect;

        Vector2 r = new Vector2(Camera.main.orthographicSize * camAspect.width / camAspect.height, Camera.main.orthographicSize);
        r.x -= offset.x;
        r.y -= offset.y;
        return r;
    }
}
