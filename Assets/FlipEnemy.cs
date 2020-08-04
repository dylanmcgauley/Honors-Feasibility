using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class FlipEnemy : MonoBehaviour
{
    // ais path
    private AIPath path;
    private Vector3 enemyScale;

    // Start is called before the first frame update
    void Start()
    {
        // get the path from the enemy
        path = gameObject.GetComponent<AIPath>();
        enemyScale = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        // flip the sprite to face the correct way
        if (path.desiredVelocity.x >= 0.02f) transform.localScale = new Vector3(-enemyScale.x, enemyScale.y, 1f);
        else transform.localScale = new Vector3(enemyScale.x, enemyScale.y, 1f);
    }
}
