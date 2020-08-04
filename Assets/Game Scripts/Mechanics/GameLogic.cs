using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;

public class GameLogic : MonoBehaviour
{
    // player variables
    private GameObject player;
    private GameObject fPlayer;
    private PlayerStats stats;
    private GameObject cam;
    private GameObject camFollower;
    public Tilemap map;
    private Vector2Int checkpoint; // checkpoint for the player
    bool camMode = false;

    private int fallScoreLoss = 10;
    private int fallHealthLoss = 1;

    // game variables
    private bool gameOver = false;
    private int checkpointLength = 35;
    private int scoreSave;

    // UI elements
    public Text scoreText;
    public Text endScore;
    public Text multiplierText;
    public GameObject gameOverScreen;

    // Start is called before the first frame update
    void Start()
    {
        // get player data
        player = GameObject.Find("Player");
        camFollower = GameObject.Find("CM vcam1");
        fPlayer = GameObject.Find("FakePlayer");
        stats = player.GetComponent<PlayerStats>();
        stats.setAlive(true);
        checkpoint = new Vector2Int(map.WorldToCell(player.transform.position).x, map.WorldToCell(player.transform.position).y);
        // get the camera object
        cam = GameObject.Find("Main Camera");
    }

    // Update is called once per frame
    void Update()
    {
        // carry out player checks
        if (!gameOver) PlayerCheck();
        else GameOver();

        // if escape is pressed go back to the main menu
        if (Input.GetKeyDown(KeyCode.Escape)) gameObject.GetComponent<SceneLoader>().LoadMenu();

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (!camMode)
            {
                fPlayer.transform.position = player.transform.position;
                camFollower.GetComponent<Cinemachine.CinemachineVirtualCamera>().m_Follow = fPlayer.transform;
                camMode = true;
            }
            else
            {
                camFollower.GetComponent<Cinemachine.CinemachineVirtualCamera>().m_Follow = player.transform;
                camMode = false;
            }
        }
    }

    // carry out all of the player checks
    void PlayerCheck()
    {
        // check if the player has reached a checkpoint
        if (map.WorldToCell(player.transform.position).x > (checkpoint.x + checkpointLength))
        {
            Vector2Int currentTile = new Vector2Int(map.WorldToCell(player.transform.position).x, 14);

            // check for a tile on that point
            while (!map.HasTile(new Vector3Int(currentTile.x, currentTile.y, 1)))
            {
                currentTile.y--;

                if (currentTile.y == 0)
                {
                    currentTile.x--;
                    currentTile.y = 14;
                }
            }
            // set the new checkpoint
            checkpoint = currentTile;
        }

        // check to see if the player has fallen off the level
        if (player.transform.position.y < 0)
        {
            // return player to last checkpoint and adjust players health
            player.transform.position = map.CellToWorld(new Vector3Int(checkpoint.x, checkpoint.y + 1, 1));
            player.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
            stats.setHealth(stats.getHealth() - fallHealthLoss);
            stats.setScore(stats.getScore() - fallScoreLoss);
        }

        // ensure the player cannot go to far to the left
        if (player.transform.position.x < 0) player.transform.position = new Vector3(0, player.transform.position.y, player.transform.position.z);

        // check the player is alive
        if (stats.getHealth() == 0)
        {
            gameOver = true;
            scoreSave = stats.getScore();
        }

        // update UI elements
        scoreText.text = stats.getScore().ToString();
        multiplierText.text = "x" + stats.getScoreMultiplier();
    }

    void GameOver()
    {
        // set the player to be dead
        endScore.text = "Final Score\n" + scoreSave; 
        stats.Die();
        checkpoint = new Vector2Int(map.WorldToCell(player.transform.position).x, map.WorldToCell(player.transform.position).y);
        stats.gameObject.SetActive(false);
        gameOverScreen.SetActive(true);
        endScore.gameObject.SetActive(true);

        // restart the game
        if (Input.GetKeyDown(KeyCode.Space))
        {
            gameOver = false;
            stats.gameObject.SetActive(true);
            gameOverScreen.SetActive(false);
            endScore.gameObject.SetActive(false);
        }
    }
}
