using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
  
    private PlayerStats stats; // holds the players stat values
    private Rigidbody2D rb; // the players rigid body
    private Animator animator;

    private float gravityMultiplier = 4f; // multiplys the fall acceleration
    private float playerGravity = 1f; // the gravity for the rigid body
    private float jumpPower = 14; // player jump force 
    private float jumpCooldown = 0.2f; // jump cooldown
    private float boostCooldown = 2f;
    private float jumpTimer;
    private float animationTimer;
    private bool grounded = false; // bool to tell if player is in the air
    private bool faceRight = true; // check the direction of the player
    private bool jumping = false;

    public float floorLength = 0.9f;
    public LayerMask floorLayer;

    private Vector2 playerScale; // the players local scale
    private Vector2 playerDirection; // direction the player is moving
    private Vector3 rayOffset; // jump ray offset for the feet position

    private void Start()
    {
        // get the stats from the player
        stats = gameObject.GetComponent<PlayerStats>();
        playerScale = gameObject.transform.localScale;
        rb = gameObject.GetComponent<Rigidbody2D>();
        rayOffset = new Vector3(0.3f, 0, 0);
        animator = gameObject.GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        // update all of the players movements
        if (stats.getAlive()) UpdateMovement();
    }

    void UpdateMovement()
    {
        animationTimer += Time.deltaTime;
        jumpTimer += Time.deltaTime;

        // get the input from the player on their direction
        playerDirection = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        // set the animator values
        animator.SetFloat("Speed", Mathf.Abs(playerDirection.x));
        animator.SetBool("Jump", jumping);

        // check with Raycast to see if player is on the ground
        grounded = Physics2D.Raycast(gameObject.transform.position + rayOffset, Vector2.down, floorLength, floorLayer) || Physics2D.Raycast(gameObject.transform.position - rayOffset, Vector2.down, floorLength, floorLayer);

        // check if player is jumping
        if (Input.GetButtonDown("Jump"))
        {
            // check if player is jumping
            if (jumpTimer > jumpCooldown && grounded) Jump();
        }

        // check the direction of the player and flip if neccessary
        if (playerDirection.x < 0.0f && faceRight) PlayerFlip();
        else if (playerDirection.x > 0.0f && !faceRight) PlayerFlip();

        // move the player with the desired speed
        rb.AddForce(Vector2.right * playerDirection * stats.getSpeed());

        // dont let the velocity go over the max speed
        if (Mathf.Abs(rb.velocity.x) > stats.getMaxSpeed()) rb.velocity = new Vector2(Mathf.Sign(rb.velocity.x) * stats.getMaxSpeed(), rb.velocity.y);

        // apply linear drag to the player
        ApplyDrag();
    }

    // apply linear drag to the running character
    void ApplyDrag()
    {
        // if past movement threshold apply drag
        if (grounded)
        {
            if (Mathf.Abs(playerDirection.x) < 0.5f) rb.drag = stats.getDrag();
            else rb.drag = 0;

            // on ground so set gravity scale to 0
            rb.gravityScale = 0;

            if (animationTimer > 0.4) jumping = false;
        }
        else
        {
            // apply air drag on the player
            rb.drag = 0.2f * stats.getDrag();
            rb.gravityScale = playerGravity; // reset gravity scale to 1

            // apply gravity multiplier
            if (rb.velocity.y < 0) rb.gravityScale = playerGravity * gravityMultiplier;
            else if (rb.velocity.y > 0 && !Input.GetButton("Jump")) rb.gravityScale = playerGravity * (gravityMultiplier / 2);
        }
    }

    // flip the players direction
    void PlayerFlip()
    {
        // flip the player by flipping the x scale
        faceRight = !faceRight;
        playerScale.x *= -1;
        transform.localScale = playerScale;
    }

    // make the player jump
    void Jump()
    {
        // apply an upward force to make the player jump
        rb.velocity = new Vector2(rb.velocity.x, 0); // reset the y velocity
        rb.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
        jumpTimer = 0;
        jumping = true;
        animationTimer = 0;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position + rayOffset, transform.position + rayOffset + Vector3.down * floorLength);
        Gizmos.DrawLine(transform.position - rayOffset, transform.position - rayOffset + Vector3.down * floorLength);
    }
}
