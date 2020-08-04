using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MCTreeNode : MonoBehaviour
{
    private float uct;
    private float placementScore;
    private float currentBest;
    private int visits;
    private int desiredSTiles = 4;
    public bool endGeneration;

    private float c = 2* (1 / Mathf.Sqrt(2));
    private double epsilon = 1e-6;

    public SimpleMarkov markovModel;
    public MCTreeNode parentNode;
    public List<MCTreeNode> possibleSlices;
    public MCTreeNode[] childNodes;

    // Start is called before the first frame update
    void Start()
    {
        placementScore = 0;
        visits = 0;
        possibleSlices = new List<MCTreeNode>();
        parentNode = null;
        endGeneration = false;

        //GetPossibleSlices(markovModel.GetNGrams());
        childNodes = new MCTreeNode[possibleSlices.Count];
    }

    // Tree node initializer for new nodes
    public MCTreeNode(string tile, MCTreeNode parent)
    {
        placementScore = 0;
        visits = 0;
        possibleSlices = new List<MCTreeNode>(parent.possibleSlices);
        parentNode = parent;

        //GetPossibleSlices(markovModel.GetNGrams());
        childNodes = new MCTreeNode[possibleSlices.Count];
    }

    // Tree node initializer for new nodes
    public MCTreeNode(string tile)
    {
        placementScore = 0;
        visits = 0;

        //GetPossibleSlices(markovModel.GetNGrams());
        //childNodes = new MCTreeNode[possibleSlices.Count];
    }

    // Get a list of the possible slices from the markov model
    void GetPossibleSlices(List<NGram> slices)
    {
        foreach (NGram slice in slices)
        {
            // Add a new node for each of the possible tiles
            possibleSlices.Add(new MCTreeNode(slice.value, this));
        }
    }

    void ExpandTree()
    {
        if (possibleSlices.Count > 0 && !endGeneration)
        {
            childNodes = new MCTreeNode[possibleSlices.Count];

            for (int x = 0; x < possibleSlices.Count; x++)
            {
                //childNodes[x] = new MCTreeNode();
            }
        }
    }

    // Find the best child out of the available choices (Selection Stage)
    MCTreeNode SelectBestChild()
    {
        MCTreeNode best = null;
        currentBest = 0;

        // loop through each child and calculate their uct value
        foreach(MCTreeNode child in childNodes)
        {
            uct = (child.placementScore) / (child.visits + (float)epsilon) + (c * Mathf.Sqrt(2* Mathf.Log(visits + 1) / (child.visits + (float)epsilon)));

            // if the value is greater than the current best value set new best
            if (uct > currentBest)
            {
                currentBest = uct;
                best = child;
            }
        }
        // return the best child
        return best;
    }

    // Update the nodes stats (visits and score)
    void UpdateNode(float score)
    {
        // update the parent
        if (parentNode) UpdateNode(score);

        visits++;
        placementScore += score;
    }
}
