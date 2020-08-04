using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    // how much health to give the player 
    private int boost = 1;
    private float delay = 0.3f;

    // holds the players stats
    private PlayerStats stats;

    // check for collision
    private void OnTriggerEnter2D(Collider2D collided)
    {
        // check to see if the collided object is a player
        if (collided.CompareTag("Player"))
        {
            StartCoroutine(GiveHealth(collided));
        }
    }

    IEnumerator GiveHealth(Collider2D player)
    {
        // get stats from the player
        stats = player.GetComponent<PlayerStats>();

        if (stats.getHealth() + boost <= stats.getMaxHealth())
        {
            // apply the health boost
            stats.setHealth(stats.getHealth() + boost);
        }

        yield return new WaitForSeconds(delay);

        // remove the game object
        gameObject.SetActive(false);
    }
}
