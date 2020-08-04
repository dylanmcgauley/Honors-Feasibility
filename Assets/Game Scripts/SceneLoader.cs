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
        SceneManager.LoadScene("LevelGenerator");
        PlayerPrefs.SetString("GenerationMethod", "Test");
    }

    public void LoadGame()
    {
        SceneManager.LoadScene("LevelGenerator");
    }

    public void LoadCreateTrainingData()
    {
        SceneManager.LoadScene("TrainingDataTool");
    }

    public void LoadOptionsMenu()
    {
        SceneManager.LoadScene("Options");
    }
}
