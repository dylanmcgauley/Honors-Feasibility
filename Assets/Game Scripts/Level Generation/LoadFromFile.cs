using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using TMPro;

public class LoadFromFile : MonoBehaviour
{
    // string variables for path and holding the level data
    private string level = ""; // currently loaded level
    private Vector3Int currentPos;
    private int currentLevel = 0;
    private int rowCount = 0;
    private Tile tempTile;
    private ScanGraph scan;
    private bool useMCTS = false;

    // Tile maps
    public Tilemap groundMap;
    public Tilemap trapMap;
    public Tilemap powerupMap;

    private GameObject checkText;

    // Array of block types
    private Tile[] blocks = new Tile[24];

    // Special Tile Prefabs
    private GameObject shooterPrefab;
    private GameObject energyPrefab;
    private GameObject icePrefab;
    private GameObject explosivePrefab;
    private GameObject healthPrefab;
    private GameObject breakableRPrefab;
    private GameObject breakableNRPrefab;
    private GameObject mudPrefab;
    private GameObject pointsCPrefab;
    private GameObject pointsRPrefab;
    private GameObject scorex2Prefab;
    private GameObject scorex3Prefab;
    private GameObject slidePrefab;
    private GameObject spikeyPrefab;
    private GameObject springPrefab;
    private GameObject birdPrefab;

    private PlayerStats stats;

    bool generateMarkov = false;
    bool generateFile = false;

    // required scripts for generation
    private SimpleMarkov markov = new SimpleMarkov();
    private TestMap mapTest = new TestMap();

    StreamReader reader = new StreamReader("Assets/Level/NNTest.txt");
    private string trainingLevel = "";
    private string nnLevel = "";

    // Start is called before the first frame update
    void Start()
    {
        // load the blocks
        for (int x = 0; x < blocks.Length; x++)
        {
            blocks[x] = Resources.Load<Tile>("Tiles/Block" + x);
        }

        reader = new StreamReader("Assets/Level/NNTest.txt");
        trainingLevel = reader.ReadToEnd();

        stats = GameObject.Find("Player").GetComponent<PlayerStats>();

        // load tile prefabs
        shooterPrefab = Resources.Load("Prefabs/Shooter") as GameObject;
        energyPrefab = Resources.Load("Prefabs/Energy") as GameObject;
        icePrefab = Resources.Load("Prefabs/Ice") as GameObject;
        explosivePrefab = Resources.Load("Prefabs/Explosive") as GameObject;
        healthPrefab = Resources.Load("Prefabs/Health") as GameObject;
        breakableRPrefab = Resources.Load("Prefabs/Breakable(R)") as GameObject;
        breakableNRPrefab = Resources.Load("Prefabs/Breakable(NR)") as GameObject;
        mudPrefab = Resources.Load("Prefabs/Mud") as GameObject;
        pointsCPrefab = Resources.Load("Prefabs/Points(Common)") as GameObject;
        pointsRPrefab = Resources.Load("Prefabs/Points(Rare)") as GameObject;
        scorex2Prefab = Resources.Load("Prefabs/Scorex2") as GameObject;
        scorex3Prefab = Resources.Load("Prefabs/Scorex3") as GameObject;
        slidePrefab = Resources.Load("Prefabs/SlideUp") as GameObject;
        spikeyPrefab = Resources.Load("Prefabs/Spikey") as GameObject;
        springPrefab = Resources.Load("Prefabs/Spring") as GameObject;
        birdPrefab = Resources.Load("Prefabs/Bird") as GameObject;

        // get the map testing script with the tilemap on it
        mapTest = GameObject.Find("LevelManager").GetComponent<TestMap>();
        scan = GameObject.Find("A* Grid").GetComponent<ScanGraph>();

        // get the playability text
        checkText = GameObject.Find("/Main Camera/Canvas/Playability");

        // set tile gameobjects to prefabs
        blocks[2].gameObject = energyPrefab;
        blocks[8].gameObject = shooterPrefab;
        blocks[3].gameObject = scorex2Prefab;
        blocks[4].gameObject = explosivePrefab;
        blocks[5].gameObject = slidePrefab;
        blocks[6].gameObject = pointsCPrefab;
        blocks[7].gameObject = breakableNRPrefab;
        blocks[9].gameObject = birdPrefab;
        blocks[10].gameObject = icePrefab;
        blocks[11].gameObject = healthPrefab;
        //blocks[12].gameObject = Prefab;
        blocks[13].gameObject = pointsRPrefab;
        blocks[15].gameObject = springPrefab;
        blocks[16].gameObject = breakableRPrefab;
        blocks[20].gameObject = scorex3Prefab;
        blocks[21].gameObject = mudPrefab;
        blocks[22].gameObject = spikeyPrefab;

        // make the tiles dissapear to be replaced by prefabs
        blocks[2].sprite = null;
        blocks[8].sprite = null;
        blocks[3].sprite = null;
        blocks[4].sprite = null;
        blocks[5].sprite = null;
        blocks[6].sprite = null;
        blocks[7].sprite = null;
        blocks[9].sprite = null;
        blocks[10].sprite = null;
        blocks[11].sprite = null;
        //blocks[12].sprite = null;
        blocks[13].sprite = null;
        blocks[15].sprite = null;
        blocks[16].sprite = null;
        blocks[20].sprite = null;
        blocks[21].sprite = null;
        blocks[22].sprite = null;

        // check what playmode the game is in
        if (PlayerPrefs.GetString("GenerationMethod") == "Test")
        {
            // Testing
            GameObject.Find("/Main Camera/Canvas/Generate(MCTS)").gameObject.SetActive(false);
            level = trainingLevel;
            GenerateFromString(level);
            AddEnd();
        }
        else if (PlayerPrefs.GetString("GenerationMethod") == "Markov")
        {
            generateMarkov = true;
            // check to see if the markov model is using MCTS
            if (PlayerPrefs.GetString("UseMCTS") == "Yes") useMCTS = true;
            else useMCTS = false;

            // build the markov chain
            markov.BuildMarkov();
        }
        else if (PlayerPrefs.GetString("GenerationMethod") == "NN")
        {
            int randLevel = Random.Range(0, 12);
            GameObject.Find("/Main Camera/Canvas/Generate(MCTS)").gameObject.SetActive(false);
            reader = new StreamReader("Assets/Level/NNLevel" + randLevel.ToString() + ".txt");
            nnLevel = reader.ReadToEnd();
            GenerateFromString(nnLevel);
            AddEnd();
        }
    }

    // Generates a level in the editor using an inputted text file
    void GenerateFromString(string level)
    {
        currentPos = new Vector3Int(0, 0, 1);

        groundMap.ClearAllTiles();
        powerupMap.ClearAllTiles();
        trapMap.ClearAllTiles();

        for (int x = 0; x < level.Length; x++)
        {

            // check to see what block the file is outputting 
            // set the block then update the current position
            if (currentPos.y == 14)
            {
                rowCount = 0;
                currentPos.x++;
                currentPos.y = 0;
            }

            if (level[x] == '#')
            {
                groundMap.SetTile(currentPos, blocks[0]);
                currentPos.y++;
                rowCount++;
            }
            else if (level[x] == 'x')
            {
                //groundMap.SetTile(currentPos, blocks[1]);
                currentPos.y++;
            }
            else if (level[x] == 'p')
            {
                if (currentPos.x > 14) powerupMap.SetTile(currentPos, blocks[2]);
                currentPos.y++;
            }
            else if (level[x] == 'Q')
            {
                if (currentPos.x > 14) powerupMap.SetTile(currentPos, blocks[3]);
                currentPos.y++;
            }
            else if (level[x] == 'U')
            {
                if (currentPos.x > 14) trapMap.SetTile(currentPos, blocks[4]);
                currentPos.y++;
            }
            else if (level[x] == '0')
            {
                groundMap.SetTile(currentPos, blocks[5]);
                currentPos.y++;
            }
            else if (level[x] == 'l')
            {
                powerupMap.SetTile(currentPos, blocks[6]);
                currentPos.y++;
            }
            else if (level[x] == '_')
            {
                trapMap.SetTile(currentPos, blocks[7]);
                currentPos.y++;
            }
            else if (level[x] == 'N')
            {
                if (currentPos.x > 14) trapMap.SetTile(currentPos, blocks[8]);
                currentPos.y++;
            }
            else if (level[x] == 'm')
            {
                if (currentPos.x > 14) trapMap.SetTile(currentPos, blocks[9]);
                currentPos.y++;
            }
            else if (level[x] == '.')
            {
                groundMap.SetTile(currentPos, blocks[10]);
                currentPos.y++;
            }
            else if (level[x] == 'I')
            {
                powerupMap.SetTile(currentPos, blocks[11]);
                currentPos.y++;
            }
            else if (level[x] == 'A')
            {
                groundMap.SetTile(currentPos, blocks[12]);
                currentPos.y++;
            }
            else if (level[x] == 'S')
            {
                groundMap.SetTile(currentPos, blocks[17]);
                currentPos.y++;
            }
            else if (level[x] == '|')
            {
                groundMap.SetTile(currentPos, blocks[14]);
                currentPos.y++;
            }
            else if (level[x] == '-')
            {
                groundMap.SetTile(currentPos, blocks[15]);
                currentPos.y++;
            }
            else if (level[x] == '?')
            {
                trapMap.SetTile(currentPos, blocks[16]);
                currentPos.y++;
            }
            else if (level[x] == '^')
            {
                powerupMap.SetTile(currentPos, blocks[13]);
                currentPos.y++;
            }
            else if (level[x] == '>')
            {
                groundMap.SetTile(currentPos, blocks[18]);
                currentPos.y++;
            }
            else if (level[x] == 'v')
            {
                groundMap.SetTile(currentPos, blocks[19]);
                currentPos.y++;
            }
            else if (level[x] == 'E')
            {
                if (currentPos.x > 14) powerupMap.SetTile(currentPos, blocks[20]);
                currentPos.y++;
            }
            else if (level[x] == '<')
            {
                groundMap.SetTile(currentPos, blocks[21]);
                currentPos.y++;
            }
            else if (level[x] == '8')
            {
                trapMap.SetTile(currentPos, blocks[22]);
                currentPos.y++;
            }
        }

        // make sure there are no gaps 
        for (int x = 0; x < currentPos.x; x++)
        {
            for (int y = 0; y < 14; y++)
            {
                if (!groundMap.HasTile(new Vector3Int(x, y, 1)))
                {
                    groundMap.SetTile(currentPos, blocks[1]);
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        // generate a new level using the markov chain
        if (generateMarkov)
        {
            groundMap.ClearAllTiles();
            powerupMap.ClearAllTiles();
            trapMap.ClearAllTiles();
            checkText.SetActive(false);
            stats.Die();
            markov.SetTreeReturned(false);

            // fill the level with data generated through the markov chain
            if (useMCTS)
            {
                while (!mapTest.QuickCheck())
                {
                    GenerateFromString(markov.GenerateMarkov());
                    markov.EvaluateTree();
                    AddEnd();
                }
            }
            else
            {
                // use without MCTS
                GenerateFromString(markov.GenerateMarkov());
                AddEnd();
            }
            generateMarkov = false;
        }

        // refresh the map if the player dies
        if (stats.getHealth() == 0)
        {
            groundMap.ClearAllTiles();
            powerupMap.ClearAllTiles();
            trapMap.ClearAllTiles();

            if (PlayerPrefs.GetString("GenerationMethod") == "NN") GenerateFromString(nnLevel);
            else if (PlayerPrefs.GetString("GenerationMethod") == "Markov") generateMarkov = true;
            else if (PlayerPrefs.GetString("GenerationMethod") == "Test") generateFile = true;
        }

        // generate a new level using a inputted file
        if (generateFile)
        {
            // fill the string with the level data
            level = trainingLevel;
            GenerateFromString(level);
            AddEnd();
            generateFile = false;
        }

    }

    // add a staircase to end the level
    private void AddEnd()
    {
        int temp = 1;
        currentPos.y = 0;
        for (int x = 0; x < 10; x++)
        {
            for (int y = 0; y < temp; y++)
            {
                groundMap.SetTile(currentPos, blocks[19]);
                currentPos.y++;
            }
            currentPos.y = 0;
            currentPos.x++;
            temp++;
        }
    }

    public void MarkovButton()
    {
        if (!generateMarkov) generateMarkov = true;
    }

    public void FileButton()
    {
        if (!generateFile) generateFile = true;
    }

    public void CreateChain()
    {
        markov.BuildMarkov();
    }
}
