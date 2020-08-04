using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableGround : MonoBehaviour
{
    // adjustable variables
    public int pointBoost = 20;
    public float respawnTime = 3f;
    public float destroyDelay = 1f;
    public float destroyDelay2 = 0.3f;
    public bool respawns = false;
    public bool pointsBlock = false;

    private PlayerStats stats;

    // check for collision
    private void OnTriggerEnter2D(Collider2D collided)
    {
        // check to see if the collided object is a player
        if (collided.CompareTag("Player"))
        {
            // if its a points block add points else break the block
            if (pointsBlock) StartCoroutine(ApplyPoints(collided));
            else StartCoroutine(BreakBlock());
        }
    }

    IEnumerator ApplyPoints(Collider2D player)
    {
        // get the stats from the player
        stats = player.GetComponent<PlayerStats>();

        // add score to the player
        stats.setScore(stats.getScore() + (pointBoost * stats.getScoreMultiplier()));

        // delay before the object gets destroyed
        yield return new WaitForSeconds(destroyDelay2);

        // destroy the block
        gameObject.SetActive(false);
    }

    IEnumerator BreakBlock()
    {
        // delay before the object gets destroyed
        yield return new WaitForSeconds(destroyDelay);
        
        // if the object respawns wait for the delay then reactivate it
        if (respawns)
        {
            // disable the gameobject
            gameObject.GetComponent<BoxCollider2D>().enabled = false;
            gameObject.GetComponent<SpriteRenderer>().enabled = false;
            gameObject.layer = 2;

            yield return new WaitForSeconds(respawnTime);

            // reable the game object
            gameObject.GetComponent<BoxCollider2D>().enabled = true;
            gameObject.GetComponent<SpriteRenderer>().enabled = true;
            gameObject.layer = 9;
        }
        else
        {
            // destroy the block when touched
            gameObject.SetActive(false);
        }
    }
}
