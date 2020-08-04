using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spring : MonoBehaviour
{
    private float springForce = 2;

    // check for collision
    private void OnTriggerEnter2D(Collider2D collided)
    {
        // check to see if the collided object is a player
        if (collided.CompareTag("Player"))
        {
            // apply a force to act as a spring
            collided.gameObject.GetComponent<Rigidbody2D>().AddForce(Vector2.up * springForce, ForceMode2D.Impulse);
        }
    }
}
