using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using UnityEngine;
using UnityEngine.Tilemaps;

class NGram
{
    public string value;
    public int count = 0;
    public List<char> possibleNext = new List<char>();
}

public class SimpleMarkov : MonoBehaviour
{
    // string variables for path and holding the level data
    private string filePath = "Assets/Level/TrainingData.txt";
    private string level = "";
    //string newLevel = "";
    int levelLength = 310;
    public int n = 15;
    Random rand = new Random();
    List<NGram> ngrams = new List<NGram>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void BuildMarkov()
    {
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
                newGram.possibleNext.Add(level[x + n]);
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
                        ngrams[y].possibleNext.Add(level[x + n]);
                        break;
                    }

                    if (y == ngrams.Count - 1 && ngrams[y].value != gram && x < level.Length - n)
                    {
                        NGram newGram = new NGram();
                        newGram.value = gram;
                        newGram.count++;
                        newGram.possibleNext.Add(level[x + n]);
                        ngrams.Add(newGram);
                        newGram = null;
                        break;
                    }
                }
            }
        }
    }

    public string GenerateMarkov()
    {
        string currentGram = level.Substring(0, n);
        string output = currentGram;

        for (int i = 0; i < levelLength; i++)
        {
            for (int x = 0; x < ngrams.Count; x++)
            {
                if (ngrams[x].value == currentGram)
                {
                    List<char> possibleChars = ngrams[x].possibleNext;
                    if (ngrams[x].possibleNext.Count == 0) break;
                    string nextChar = possibleChars[Random.Range(0, possibleChars.Count)].ToString();
                    output += nextChar;
                    currentGram = output.Substring(output.Length - n, n);
                }
            }
        }

        return output;
    }

    // Update is called once per frame
    void Update()
    {
   
    }
}
