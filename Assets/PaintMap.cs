using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PaintMap : MonoBehaviour
{
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
    public Tile borderTile;

    private Tile currentTile;

    Vector3Int clickPos;
    private string level;

    // Start is called before the first frame update
    void Start()
    {
        currentTile = block1;

        for (int x = -1; x < 16; x++)
        {
            map.SetTile(new Vector3Int(-1, x, 1), borderTile);
            map.SetTile(new Vector3Int(100, x, 1), borderTile);
        }

        for (int x = 0; x < 100; x++)
        {
            map.SetTile(new Vector3Int(x, -1, 1), borderTile);
            map.SetTile(new Vector3Int(x, 15, 1), borderTile);
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            clickPos = map.WorldToCell(Camera.main.ScreenToWorldPoint(Input.mousePosition));
            map.SetTile(clickPos, block1);
        }
    }

    public void SaveToText()
    {
        FileStream file = File.Open("Assets/Level/NewLevel.txt", FileMode.OpenOrCreate);
        file.Close();
        StreamWriter writer = new StreamWriter("Assets/Level/NewLevel.txt", true);
        // clear the level variable
        level = "";

        for (int x = 0; x < 100; x++)
        {
            for (int y = 0; y < 14; y++)
            {
                if (map.GetTile(new Vector3Int(x, y, 0)) == block1) writer.Write('=');
                else if (map.GetTile(new Vector3Int(x, y, 0)) == block2) writer.Write('.');
                else if (map.GetTile(new Vector3Int(x, y, 0)) == block3) writer.Write('g');
                else if (map.GetTile(new Vector3Int(x, y, 0)) == block4) writer.Write('A');
                else if (map.GetTile(new Vector3Int(x, y, 0)) == block5) writer.Write('8');
                else if (map.GetTile(new Vector3Int(x, y, 0)) == block6) writer.Write('p');
                else if (map.GetTile(new Vector3Int(x, y, 0)) == block7) writer.Write('b');
                else if (map.GetTile(new Vector3Int(x, y, 0)) == block8) writer.Write('?');
                else if (map.GetTile(new Vector3Int(x, y, 0)) == block9) writer.Write('H');
                else if (map.GetTile(new Vector3Int(x, y, 0)) == block10) writer.Write('k');
                else if (map.GetTile(new Vector3Int(x, y, 0)) == block11) writer.Write('|');
                else if (map.GetTile(new Vector3Int(x, y, 0)) == block12) writer.Write('!');
                else if (map.GetTile(new Vector3Int(x, y, 0)) == block13) writer.Write('0');
                else if (map.GetTile(new Vector3Int(x, y, 0)) == block14) writer.Write('K');
                else if (map.GetTile(new Vector3Int(x, y, 0)) == block15) writer.Write('I');
                else if (map.GetTile(new Vector3Int(x, y, 0)) == block16) writer.Write('x');
                else if (map.GetTile(new Vector3Int(x, y, 0)) == block17) writer.Write('-');
                else if (map.GetTile(new Vector3Int(x, y, 0)) == block18) writer.Write('>');
                else if (map.GetTile(new Vector3Int(x, y, 0)) == block19) writer.Write('<');
                else if (map.GetTile(new Vector3Int(x, y, 0)) == block20) writer.Write('^');
                else if (map.GetTile(new Vector3Int(x, y, 0)) == block21) writer.Write('F');
                else if (map.GetTile(new Vector3Int(x, y, 0)) == block22) writer.Write('v');
                else if (map.GetTile(new Vector3Int(x, y, 0)) == block23) writer.Write('_');
                else if (!map.HasTile(new Vector3Int(x, y, 0))) writer.Write('.');
            }
            writer.Write('\n');
        }
        writer.Close();
    }
}
