using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{

    // functions for switching between scenes
    public void LoadMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    public void LoadGenerateFromFile()
    {
        SceneManager.LoadScene("LevelLoader");
    }

    public void LoadGenerateMarkov()
    {
        SceneManager.LoadScene("LevelGenerator");
    }

    public void LoadCreateTrainingData()
    {
        SceneManager.LoadScene("TrainingDataTool");
    }
}
