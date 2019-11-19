using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using UnityEngine;
using UnityEngine.Tilemaps;

public class LoadFromFile : MonoBehaviour
{
    // string variables for path and holding the level data
    private string level = "";
    private Vector3Int lastPos;
    private Vector3Int currentPos;
    private int rowCount = 0;
    public Tilemap map;

    // list of block types
    public Tile block1;
    public Tile block2;
    public Tile block3;
    public Tile block4;
    public Tile block5;
    public Tile block6;
    public Tile block7;
    public Tile block8;
    public Tile block9;
    public Tile block10;
    public Tile block11;
    public Tile block12;
    public Tile block13;
    public Tile block14;
    public Tile block15;
    public Tile block16;
    public Tile block17;
    public Tile block18;
    public Tile block19;
    public Tile block20;
    public Tile block21;
    public Tile block22;
    public Tile block23;

    bool generateMarkov = false;
    bool generateFile = false;


    SimpleMarkov markov = new SimpleMarkov();

    // create a reader to read the text file
    StreamReader reader = new StreamReader("Assets/Level/TrainingData.txt");

    // Start is called before the first frame update
    void Start()
    {
        //Debug.Log(reader.ReadToEnd());
    }

    // Generates a level in the editor using an inputted text file
    void GenerateFromString(string level)
    {
        lastPos = new Vector3Int(0, 0, 1);
        currentPos = new Vector3Int(0, 0, 1);

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

            if (level[x] == '=')
                {
                    map.SetTile(currentPos, block1);
                    currentPos.y++;
                    rowCount++;
                }
                else if (level[x] == '.')
                {
                    map.SetTile(currentPos, block2);
                    currentPos.y++;
                }
                else if (level[x] == 'g')
                {
                    map.SetTile(currentPos, block3);
                    currentPos.y++;
                }
                else if (level[x] == 'A')
                {
                    map.SetTile(currentPos, block4);
                    currentPos.y++;
                }
                else if (level[x] == '8')
                {
                    map.SetTile(currentPos, block5);
                    currentPos.y++;
                }
                else if (level[x] == 'p')
                {
                    map.SetTile(currentPos, block6);
                    currentPos.y++;
                }
                else if (level[x] == 'b')
                {
                    map.SetTile(currentPos, block7);
                    currentPos.y++;
                }
                else if (level[x] == '?')
                {
                    map.SetTile(currentPos, block8);
                    currentPos.y++;
                }
                else if (level[x] == 'H')
                {
                    map.SetTile(currentPos, block9);
                    currentPos.y++;
                }
                else if (level[x] == 'k')
                {
                    map.SetTile(currentPos, block10);
                    currentPos.y++;
                }
                else if (level[x] == '|')
                {
                    map.SetTile(currentPos, block11);
                    currentPos.y++;
                }
                else if (level[x] == '!')
                {
                    map.SetTile(currentPos, block12);
                    currentPos.y++;
                }
                else if (level[x] == '0')
                {
                    map.SetTile(currentPos, block13);
                    currentPos.y++;
                }
                else if (level[x] == 'K')
                {
                    map.SetTile(currentPos, block14);
                    currentPos.y++;
                }
                else if (level[x] == 'I')
                {
                    map.SetTile(currentPos, block15);
                    currentPos.y++;
                }
                // change from purple
                else if (level[x] == 'x')
                {
                    map.SetTile(currentPos, block16);
                    currentPos.y++;
                }
                else if (level[x] == '-')
                {
                    map.SetTile(currentPos, block17);
                    currentPos.y++;
                }
                else if (level[x] == '>')
                {
                    map.SetTile(currentPos, block18);
                    currentPos.y++;
                }
                else if (level[x] == '<')
                {
                map.SetTile(currentPos, block19);
                currentPos.y++;
                }
                else if (level[x] == '^')
                {
                map.SetTile(currentPos, block20);
                currentPos.y++;
                }
                else if (level[x] == 'F')
                {
                map.SetTile(currentPos, block21);
                currentPos.y++;
                }
                else if (level[x] == 'v')
                {
                map.SetTile(currentPos, block22);
                currentPos.y++;
                }
                else if (level[x] == '_')
                {
                map.SetTile(currentPos, block23);
                currentPos.y++;
                }
            //else if (level[x] == '\n')
            //{
            //    currentPos.x++;
            //    currentPos.y = 0;
            //}
        }

            // make sure there are no gaps 
        for (int x = 0; x < currentPos.x; x++)
        {
            for (int y = 0; y < 14; y++)
            {
                if (!map.HasTile(new Vector3Int(x, y, 1)))
                {
                    map.SetTile(currentPos, block2);
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (generateMarkov)
        {
            // fill the level with data generated through the markov chain
            GenerateFromString(markov.GenerateMarkov());
            generateMarkov = false;
        }

        if (generateFile)
        {
            // fill the string with the level data
            level = reader.ReadToEnd();
            GenerateFromString(level);
            generateFile = false;
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

    public void SetN(float n)
    {
        markov.n = (int)n;
    }
}
