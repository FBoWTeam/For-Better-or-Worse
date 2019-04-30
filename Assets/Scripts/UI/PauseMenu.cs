using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;


public class PauseMenu : MonoBehaviour
{
    bool pauseMenuActive = false;

    [Header("Main Components")]
    public GameObject mainMenu;
    public GameObject guide;
    public GameObject mainMenuFirstSelected;
    public EventSystem eS;

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
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Joystick1Button7))
        {
            if (!pauseMenuActive)
            {
                print("ACTIVE");
                Time.timeScale = 0;
                pauseMenuActive = true;
                GameManager.gameManager.isPaused = true;
                GameManager.gameManager.UIManager.gameObject.SetActive(false);
                mainMenu.SetActive(true);
                eS.SetSelectedGameObject(mainMenuFirstSelected);
            }
            else if (pauseMenuActive)
            {
                if (guide.activeSelf)
                {
                    mappingPanel.SetActive(false);
                    powerPanel.SetActive(false);
                }

                Resume();
            }
        }

        if (pauseMenuActive && guide.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.Backspace) || Input.GetKeyDown(KeyCode.Joystick1Button1))
            {
                mappingPanel.SetActive(false);
                powerPanel.SetActive(false);
                guide.SetActive(false);
                mainMenu.SetActive(true);
                eS.SetSelectedGameObject(mainMenuFirstSelected);
            }

            if ((Input.GetAxis("HorizontalP1") > 0.5f) && !powerPanel.activeSelf)
            {
                mappingPanel.SetActive(false);
                powerPanel.SetActive(true);
            }
            else if ((Input.GetAxis("HorizontalP1") < 0.5f) && !mappingPanel.activeSelf)
            {
                powerPanel.SetActive(false);
                mappingPanel.SetActive(true);
            }
        }
    }

    public void Resume()
    {
        mainMenu.SetActive(false);
        guide.SetActive(false);
        GameManager.gameManager.UIManager.gameObject.SetActive(true);
        GameManager.gameManager.isPaused = false;
        pauseMenuActive = false;
        Time.timeScale = 1;
    }

    public void OpenGuide()
    {
        mainMenu.SetActive(false);
        guide.SetActive(true);
        mappingPanel.SetActive(true);
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
