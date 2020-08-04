using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scorex2 : MonoBehaviour
{
    // set multiplier and duration
    public int multiplier = 2;
    private float multiplierDuration = 20;

    // for holding the players stats
    private PlayerStats stats;

    // check for collision
    private void OnTriggerEnter2D(Collider2D collided)
    {
        // check to see if the collided object is a player
        if (collided.CompareTag("Player"))
        {
            StartCoroutine(ApplyMultiplier(collided));
        }
    }

    private IEnumerator ApplyMultiplier(Collider2D player)
    {
        // get the players stats
        stats = player.GetComponent<PlayerStats>();

        // apply the new multiplier
        stats.setScoreMultiplier(multiplier * stats.getScoreMultiplier());

        // remove the powerup from the screen
        gameObject.GetComponent<SpriteRenderer>().enabled = false;
        gameObject.GetComponent<BoxCollider2D>().enabled = false;

        // wait for the multiplier duration to end
        yield return new WaitForSeconds(multiplierDuration);

        // reset the players multiplier
        stats.setScoreMultiplier(stats.getScoreMultiplier() / multiplier);
        gameObject.SetActive(false);
    }
}
