using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mud : MonoBehaviour
{
    // new speed and max speed
    public float speed = 5f;
    public float maxSpeed = 4f;

    // saves for the past speeds
    private float oldSpeed;
    private float oldMax;

    // variable to hold the players stats
    private PlayerStats stats;

    // check for collision
    private void OnTriggerEnter2D(Collider2D collided)
    {
        // check to see if the collided object is a player
        if (collided.CompareTag("Player"))
        {
            ReduceSpeed(collided);
        }
    }

    // check when collision is over
    private void OnTriggerExit2D(Collider2D collided)
    {
        if (collided.CompareTag("Player"))
        {
            ResetSpeed();
        }
    }

    // reduce the players movement speed
    void ReduceSpeed(Collider2D player)
    {
        // get the stats from the player
        stats = player.GetComponent<PlayerStats>();

        // save the old values
        oldSpeed = stats.getSpeed();
        oldMax = stats.getMaxSpeed();

        // set the new speeds for the player
        stats.setSpeed(speed);
        stats.setMaxSpeed(maxSpeed);
    }
    
    // reset the players speed
    void ResetSpeed()
    {
        // set the speeds back
        stats.setSpeed(oldSpeed);
        stats.setMaxSpeed(oldMax);
    }
}
