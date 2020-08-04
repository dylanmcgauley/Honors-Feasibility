using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class BirdLogic : MonoBehaviour
{
    private GameObject player;
    private AIDestinationSetter destination;
    private bool destinationSet = false;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        destination = gameObject.GetComponent<AIDestinationSetter>();
    }

    // Update is called once per frame
    void Update()
    {
        // check if a destination has already been set
        if (!destinationSet)
        {
            // if bird is within range of player set it as the destination
            if (transform.position.x - player.gameObject.transform.position.x > -10 && transform.position.x - player.gameObject.transform.position.x < 10)
            {
                SetDestination();
                destinationSet = true;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            CollisionCheck(collision);
        }
    }

    private void SetDestination()
    {
        destination.target = player.transform;
    }

    // check collisons for the player
    private void CollisionCheck(Collider2D player)
    {
        PlayerStats stats = player.GetComponent<PlayerStats>();

        // player killed the bird
        if (player.transform.position.y > transform.position.y)
        {
            stats.setScore(stats.getScore() + 50);
        }
        // bird killed player
        else
        {
            stats.setHealth(stats.getHealth() - 1);
        }

        gameObject.SetActive(false);
    }
}
