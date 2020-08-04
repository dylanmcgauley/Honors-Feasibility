using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Tilemaps;
using TMPro;

public class PaintMap : MonoBehaviour
{
    public Tilemap map;

    // list of block types
    public Tile[] blocks = new Tile[23];
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
    public Tile borderTile;

    private Tile currentTile;

    Vector3Int clickPos;

    // create a reader to read the text file
    StreamReader reader = new StreamReader("Assets/Level/TrainingDataNew.txt");

    // Start is called before the first frame update
    void Start()
    {
        for (int x = 0; x < blocks.Length; x++)
        {
            blocks[x] = Resources.Load("Tiles/Block" + x.ToString()) as Tile;
        }

        currentTile = blocks[0];

        for (int x = -1; x < 16; x++)
        {
            map.SetTile(new Vector3Int(-1, x, 1), blocks[19]); // border tile
            map.SetTile(new Vector3Int(100, x, 1), blocks[19]); // border tile
        }

        for (int x = 0; x < 100; x++)
        {
            map.SetTile(new Vector3Int(x, -1, 1), blocks[19]); // border tile
            map.SetTile(new Vector3Int(x, 15, 1), blocks[19]); // border tile
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(1))
        {
            clickPos = map.WorldToCell(Camera.main.ScreenToWorldPoint(Input.mousePosition));
            map.SetTile(clickPos, currentTile);
        }
        // if escape is pressed go back to the main menu
        if (Input.GetKeyDown(KeyCode.Escape)) gameObject.GetComponent<SceneLoader>().LoadMenu();
    }

    public void SaveToText()
    {
        // open or create a file to store the created level data in
        FileStream file = File.Open("Assets/Level/NewLevel.txt", FileMode.OpenOrCreate);
        file.Close();
        StreamWriter writer = new StreamWriter("Assets/Level/NewLevel.txt", true);

        // loop through the draw area and write the level data to the text file
        for (int x = 0; x < 100; x++)
        {
            for (int y = 0; y < 14; y++)
            {
                if (map.GetTile(new Vector3Int(x, y, 0)) == blocks[0]) writer.Write('#');
                else if (map.GetTile(new Vector3Int(x, y, 0)) == blocks[1]) writer.Write('x');
                else if (map.GetTile(new Vector3Int(x, y, 0)) == blocks[2]) writer.Write('p');
                else if (map.GetTile(new Vector3Int(x, y, 0)) == blocks[3]) writer.Write('Q');
                else if (map.GetTile(new Vector3Int(x, y, 0)) == blocks[4]) writer.Write('U');
                else if (map.GetTile(new Vector3Int(x, y, 0)) == blocks[5]) writer.Write('0');
                else if (map.GetTile(new Vector3Int(x, y, 0)) == blocks[6]) writer.Write('l');
                else if (map.GetTile(new Vector3Int(x, y, 0)) == blocks[7]) writer.Write('_');
                else if (map.GetTile(new Vector3Int(x, y, 0)) == blocks[8]) writer.Write('N');
                else if (map.GetTile(new Vector3Int(x, y, 0)) == blocks[9]) writer.Write('m');
                else if (map.GetTile(new Vector3Int(x, y, 0)) == blocks[10]) writer.Write('.');
                else if (map.GetTile(new Vector3Int(x, y, 0)) == blocks[11]) writer.Write('I');
                else if (map.GetTile(new Vector3Int(x, y, 0)) == blocks[12]) writer.Write('A');
                else if (map.GetTile(new Vector3Int(x, y, 0)) == blocks[13]) writer.Write('S');
                else if (map.GetTile(new Vector3Int(x, y, 0)) == blocks[14]) writer.Write('|');
                else if (map.GetTile(new Vector3Int(x, y, 0)) == blocks[15]) writer.Write('-');
                else if (map.GetTile(new Vector3Int(x, y, 0)) == blocks[16]) writer.Write('?');
                else if (map.GetTile(new Vector3Int(x, y, 0)) == blocks[17]) writer.Write('^');
                else if (map.GetTile(new Vector3Int(x, y, 0)) == blocks[18]) writer.Write('>');
                else if (map.GetTile(new Vector3Int(x, y, 0)) == blocks[19]) writer.Write('v');
                else if (map.GetTile(new Vector3Int(x, y, 0)) == blocks[20]) writer.Write('E');
                else if (map.GetTile(new Vector3Int(x, y, 0)) == blocks[21]) writer.Write('<');
                else if (map.GetTile(new Vector3Int(x, y, 0)) == blocks[22]) writer.Write('8');
                else if (!map.HasTile(new Vector3Int(x, y, 0))) writer.Write('x');
            }
            // write a new line
            writer.Write('\n');
        }
        // close the file
        writer.Close();
    }

    public void ChangeTile(int val)
    {
        currentTile = blocks[val];
    }

}
