using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sharp : MonoBehaviour
{
    // how much health the player will loose
    private int healthLoss = 1;

    // damage cooldown
    private float cooldown = 1;

    // hold the players stats
    private PlayerStats stats;

    // check for collision
    private void OnTriggerStay2D(Collider2D collided)
    {
        // check to see if the collided object is a player
        if (collided.CompareTag("Player"))
        {
            StartCoroutine(Hurt(collided));
        }
    }

    IEnumerator Hurt(Collider2D player)
    {
        // get the stats from the player
        stats = player.GetComponent<PlayerStats>();

        // damage the player
        stats.setHealth(stats.getHealth() - healthLoss);

        // wait for the cooldown before running again
        yield return new WaitForSeconds(cooldown);
    }
}
