using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ice : MonoBehaviour
{
    // speed the player will increase by
    private float speedIncrease = 3f;
    private float newDrag = 2f;
    private float oldDrag;

    // variable to hold the players stats
    private PlayerStats stats;

    // check for collision
    private void OnTriggerEnter2D(Collider2D collided)
    {
        // check to see if the collided object is a player
        if (collided.CompareTag("Player"))
        {
            Slide(collided);
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
    void Slide(Collider2D player)
    {
        // get the stats from the player
        stats = player.GetComponent<PlayerStats>();

        oldDrag = stats.getDrag();

        // set the new speeds for the player
        stats.setSpeed(stats.getSpeed() + speedIncrease);
        stats.setMaxSpeed(stats.getMaxSpeed() + speedIncrease);
        stats.setDrag(newDrag);
    }

    // reset the players speed
    void ResetSpeed()
    {
        // set the speeds back
        stats.setSpeed(stats.getSpeed() - speedIncrease);
        stats.setMaxSpeed(stats.getMaxSpeed() - speedIncrease);
        stats.setDrag(7);
    }
}
