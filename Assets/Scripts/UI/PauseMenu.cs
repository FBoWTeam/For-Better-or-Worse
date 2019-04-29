using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;


public class PauseMenu : MonoBehaviour
{
    public static bool gameIsPaused = false;

    [Header("Main Components")]
    public GameObject mainMenu;
    public GameObject guide;

    [Header("Difficulty")]
    public TextMeshProUGUI difficultyText;
    public TextMeshProUGUI difficultyExplicationText;

    [Header("Mapping")]
    public Image mappingImage;
    public Sprite mappingVF;
    public Sprite mappingVA;

    private void Awake()
    {
        if (GameData.english)
        {
            mappingImage.sprite = mappingVA;
        }
        else
        {
            mappingImage.sprite = mappingVF;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Joystick1Button7) || Input.GetKeyDown(KeyCode.Joystick2Button7))
        {
            if (gameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }

        if (guide.activeSelf)
        {

        }
    }

    public void Resume()
    {
        Time.timeScale = 1f;
        gameIsPaused = false;
    }

    void Pause()
    {
        Time.timeScale = 0f;
        gameIsPaused = true;
    }

    public void LoadMenu()
    {
        //Debug.Log("Menu");
        SceneManager.LoadScene(2);
    }
}
