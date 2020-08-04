using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiveEnergy : MonoBehaviour
{
    // energy to be given to the player
    public int energyBoost = 30;

    // stats from the player
    private PlayerStats stats;

    // check for collision
    private void OnTriggerEnter2D(Collider2D collided)
    {
        // check to see if the collided object is a player
        if (collided.CompareTag("Player"))
        {
            ApplyEnergy(collided);
        }
    }

    // give the player energy
    private void ApplyEnergy(Collider2D player)
    {
        // get the stats from the player
        stats = player.GetComponent<PlayerStats>();

        // add energy to the player and destroy the game object
        if (stats.getEnergy() <= stats.getMaxEnergy() - 10)
        {
            stats.setEnergy(stats.getEnergy() + 10);
        }

        gameObject.SetActive(false);
    }
}
