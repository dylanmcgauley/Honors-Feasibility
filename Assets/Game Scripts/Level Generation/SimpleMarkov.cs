using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using UnityEngine;
using UnityEngine.Tilemaps;

// Small class to define an Ngram
public class NGram
{
    public string value;
    public int count = 0;
    public List<string> possibleNext = new List<string>();
}

public class SimpleMarkov : MonoBehaviour
{
    string filePath;
    private string level = "";
    private int levelLength = 6000;
    private int levelHeight = 14;
    private int n = 14;
    Random rand = new Random();
    private List<NGram> ngrams = new List<NGram>();
    private List<MCTreeNode> treenodes = new List<MCTreeNode>();
    private int currentGram = 0;
    private bool treeReturnedLevel = false;
    private TestMap test = new TestMap();

    public void BuildMarkov()
    {
        // get the map testing script with the tilemap on it
        test = GameObject.Find("LevelManager").GetComponent<TestMap>();

        // string variables for path and holding the level data
        if (PlayerPrefs.GetString("TrainingFile") == "Large")
        {
            filePath = "Assets/Level/TrainingDataNew.txt";
        }
        else if (PlayerPrefs.GetString("TrainingFile") == "Small")
        {
            filePath = "Assets/Level/TrainingDataSmall.txt";
        }

        // get the order of n from the player prefs
        n = (int)PlayerPrefs.GetFloat("Order");

        // create a reader to read the text file
        StreamReader reader = new StreamReader(filePath);

        // clear the variables incase chain is being rebuilt
        level = "";
        ngrams.Clear();

        // fill the string with the level data
        level = reader.ReadToEnd();

        // loop through to find each gram in the text file and store each one in a list
        for (int x = 0; x <= level.Length - n; x++)
        {
            string gram = level.Substring(x, n);

            // if its the first value set the gram and add it to the list of ngrams
            if (x == 0)
            {
                NGram newGram = new NGram();
                newGram.value = gram;
                newGram.count++;
                newGram.possibleNext.Add(level.Substring(x + n, n));
                ngrams.Add(newGram);
                newGram = null;
            }
            // else loop through each ngram in the list to see if it already exists and add on to the count if it does
            else
            {
                for (int y = 0; y < ngrams.Count; y++)
                {
                    if (ngrams[y].value == gram && x < level.Length - n)
                    {
                        ngrams[y].count++;
                        ngrams[y].possibleNext.Add(level.Substring(x + n, n));
                        break;
                    }

                    if (y == ngrams.Count - 1 && ngrams[y].value != gram && x < level.Length - n)
                    {
                        NGram newGram = new NGram();
                        newGram.value = gram;
                        newGram.count++;
                        newGram.possibleNext.Add(level.Substring(x + n, n));
                        ngrams.Add(newGram);
                        newGram = null;
                        break;
                    }
                }
            }
        }
    }

    // Generates a new level using the markov model
    public string GenerateMarkov()
    {
        // set the current gram and create an empty string to store the level output
        string currentGram = level.Substring(0, n);
        string output = currentGram;

        // loop through for the level width and height (in tiles)
        for (int i = 0; i < levelLength / n; i++)
        {
                // loop through for each of the discovered ngrams
                for (int x = 0; x < ngrams.Count; x++)
                {
                    // if a match is found for the current gram select a random next tile from the list of possibilites
                    // to add on to the output string and set the new gram accordingly
                    if (ngrams[x].value == currentGram)
                    {
                        List<string> possibleChars = ngrams[x].possibleNext;
                        if (ngrams[x].possibleNext.Count == 0) break;
                        string nextChar = possibleChars[Random.Range(0, possibleChars.Count)].ToString();
                        output += nextChar;
                        currentGram = output.Substring(output.Length - n, n);
                        break;
                    }
                }
        }

        // return the generated level
        return output;
    }

    // check to see if the MCTS has returned a valid level
    public void EvaluateTree()
    {
        // initialize the MCTS with the root string then carry out search
        MCTreeNode rootNode = new MCTreeNode("##xxxxxxxxxxxx");
        if (test.QuickCheck()) treeReturnedLevel = true;
        else treeReturnedLevel = false;
    }

    public bool GetTreeReturned()
    {
        return treeReturnedLevel;
    }

    public void SetTreeReturned(bool flag)
    {
        treeReturnedLevel = flag;
    }
}
