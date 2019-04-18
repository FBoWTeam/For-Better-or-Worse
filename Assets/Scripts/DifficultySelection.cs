using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DifficultySelection : MonoBehaviour
{
    public void SetDifficultyMode(bool worseActivated)
    {
		GameData.worseModeActivated = worseActivated;
        SceneManager.LoadScene(GameData.nextSceneToLoad);
    }

}
