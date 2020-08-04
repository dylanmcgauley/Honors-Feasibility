using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{
    // private variables
    private int score = 0; // players score
    private int scoreMultiplier = 1; // score multiplier for pickups
    private int health = 4; // players health
    private int maxHealth = 4;
    private float speed = 13f; // player movement speed
    private float maxSpeed = 12f; // max speed the player can achieve
    private int energy = 0; // sprint energy the player has 
    private int maxEnergy = 100; // max sprint energy the player can gather
    public float drag = 7f; // drag the player experiences when running
    private bool alive = false; // players death state

    private Rigidbody2D rb; // the players rigid body
    public Slider healthSlider;
    public Slider energySlider;

    private Vector3 startPos;

    private void Start()
    {
        startPos = transform.position;
    }

    private void Update()
    {
        // set UI elements
        healthSlider.value = health;
        energySlider.value = energy;
    }

    // reset stats and position
    public void Die()
    {
        energy = 0;
        health = 4;
        score = 0;
        scoreMultiplier = 1;

        transform.position = startPos;
    }

    // getters
    public int getScore()
    {
        return score;
    }

    public int getHealth()
    {
        return health;
    }

    public float getSpeed()
    {
        return speed;
    }

    public float getMaxSpeed()
    {
        return maxSpeed;
    }

    public float getDrag()
    {
        return drag;
    }

    public int getEnergy()
    {
        return energy;
    }

    public int getMaxEnergy()
    {
        return maxEnergy;
    }

    public int getMaxHealth()
    {
        return maxHealth;
    }

    public int getScoreMultiplier()
    {
        return scoreMultiplier;
    }

    public bool getAlive()
    {
        return alive;
    }

    // setters
    public void setScore(int newScore)
    {
        score = newScore;
    }

    public void setHealth(int newHealth)
    {
        health = newHealth;
    }

    public void setSpeed(float newSpeed)
    {
        speed = newSpeed;
    }

    public void setMaxSpeed(float newSpeed)
    {
        maxSpeed = newSpeed;
    }

    public void setDrag(float newDrag)
    {
        drag = newDrag;
    }

    public void setAlive(bool flag)
    {
        alive = flag;
    }

    public void setEnergy(int newEnergy)
    {
        energy = newEnergy;
    }

    public void setMaxEnergy(int newMaxEnergy)
    {
        maxEnergy = newMaxEnergy;
    }

    public void setScoreMultiplier(int newMultiplier)
    {
        scoreMultiplier = newMultiplier;
    }
}
