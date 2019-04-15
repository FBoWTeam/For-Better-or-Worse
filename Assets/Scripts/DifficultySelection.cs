using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DifficultySelection : MonoBehaviour
{
    public int sceneToLoad;

    public void SetHardMode(int sceneToLoad)
    {
        PlayerPrefs.SetInt("Mode", 1);
        SceneManager.LoadScene(sceneToLoad);
    }

    public void SetEasyMode(int sceneToLoad)
    {
        PlayerPrefs.SetInt("Mode", 0);
        SceneManager.LoadScene(sceneToLoad);
    }

}
