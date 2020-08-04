using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ApplySettings : MonoBehaviour
{
    // gameobjects needed
    public GameObject trainingFile;
    public GameObject mcts;
    public GameObject order;
    public Text nText;

    private void Start()
    {
        // set the default values
        PlayerPrefs.SetString("GenerationMethod", "NN");
        PlayerPrefs.SetString("TrainingFile", "Small");
        PlayerPrefs.SetString("UseMCTS", "Yes");
        PlayerPrefs.SetFloat("Order", 16);
        nText.text = "14";
    }

    // set the game to use the neural net levels
    public void UseNN()
    {
        PlayerPrefs.SetString("GenerationMethod", "NN");
        trainingFile.SetActive(false);
        mcts.SetActive(false);
        order.SetActive(false);
    }

    // set the game to use the markov generated levels
    public void UseMarkov()
    {
        PlayerPrefs.SetString("GenerationMethod", "Markov");
        trainingFile.SetActive(true);
        mcts.SetActive(true);
        order.SetActive(true);
    }

    // set the markov model to be built with the small training file
    public void SetSmallFile()
    {
        PlayerPrefs.SetString("TrainingFile", "Small");
    }

    // set the markov model to be built with the large training file
    public void SetLargeFile()
    {
        PlayerPrefs.SetString("TrainingFile", "Large");
    }

    // set the markov to use the MCTS
    public void SetUseMCTS()
    {
        if (PlayerPrefs.GetString("UseMCTS") == "Yes") PlayerPrefs.SetString("UseMCTS", "No");
        else if (PlayerPrefs.GetString("UseMCTS") == "No") PlayerPrefs.SetString("UseMCTS", "Yes");
    }

    // set the order of n
    public void SetN(float n)
    {
        PlayerPrefs.SetFloat("Order", n);
        nText.text = n.ToString();
    }
}
