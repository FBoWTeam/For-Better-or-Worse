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

    [Header("Main Components")]
    public GameObject mappingPanel;
    public GameObject powerPanel;

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

        CheckDifficulty();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Joystick1Button7) || Input.GetKeyDown(KeyCode.Joystick2Button7))
        {
            if (GameManager.gameManager.isPaused)
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
        GameManager.gameManager.isPaused = false;
    }

    void Pause()
    {
        Time.timeScale = 0f;
        GameManager.gameManager.isPaused = true;
    }

    public void OpenGuide()
    {
        mainMenu.SetActive(false);
        guide.SetActive(true);
    }

    public void ChangeDifficulty()
    {
        GameData.worseModeActivated = (GameData.worseModeActivated) ? false : true;

        CheckDifficulty();
    }

    public void CheckDifficulty()
    {
        if (GameData.worseModeActivated)
        {
            difficultyText.SetText(I18n.Translate("menu.pause.bouton6"));
            difficultyExplicationText.SetText(I18n.Translate("menu.niveaux.difficulte5"));
        }
        else
        {
            difficultyText.SetText(I18n.Translate("menu.pause.bouton5"));
            difficultyExplicationText.SetText(I18n.Translate("menu.niveaux.difficulte3"));
        }
    }

    public void OpenMappingPanel()
    {
        powerPanel.SetActive(false);
        mappingPanel.SetActive(true);
    }

    public void OpenPowerPanel()
    {
        mappingPanel.SetActive(false);
        powerPanel.SetActive(true);
    }
    public void LoadMenu()
    {
        //Debug.Log("Menu");
        SceneManager.LoadScene(2);
    }
}
