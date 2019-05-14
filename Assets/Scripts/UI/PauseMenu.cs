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

    public SoundEmitter soundEmitter;

    private void Start()
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
        if (GameManager.gameManager.canActivatePauseMenu)
        {
            if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Joystick1Button7))
            {
                if (!pauseMenuActive)
                {
                    soundEmitter.PlaySound(0);
                    Time.timeScale = 0.0f;
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
                    soundEmitter.PlaySound(0);
                    mappingPanel.SetActive(false);
                    powerPanel.SetActive(false);
                    guide.SetActive(false);
                    mainMenu.SetActive(true);
                    eS.SetSelectedGameObject(mainMenuFirstSelected);
                }

                if (((Input.GetAxis("HorizontalP1") >= 0.5f) && !powerPanel.activeSelf) || Input.GetKeyDown(KeyCode.D))
                {
                    //print("Right");
                    soundEmitter.PlaySound(0);
                    mappingPanel.SetActive(false);
                    powerPanel.SetActive(true);
                }
                else if (((Input.GetAxis("HorizontalP1") <= -0.5f) && !mappingPanel.activeSelf) || Input.GetKeyDown(KeyCode.Q))
                {
                    //sprint("Left");
                    soundEmitter.PlaySound(0);
                    powerPanel.SetActive(false);
                    mappingPanel.SetActive(true);
                }
            }
        }
    }

    public void Resume()
    {
        soundEmitter.PlaySound(0);
        mainMenu.SetActive(false);
        guide.SetActive(false);
        GameManager.gameManager.UIManager.gameObject.SetActive(true);
        GameManager.gameManager.isPaused = false;
		pauseMenuActive = false;
        Time.timeScale = 1.0f;
    }

    public void OpenGuide()
    {
        soundEmitter.PlaySound(0);
        mainMenu.SetActive(false);
        guide.SetActive(true);
        mappingPanel.SetActive(true);
    }

    public void ChangeDifficulty()
    {
        soundEmitter.PlaySound(0);
        GameData.worseModeActivated = (GameData.worseModeActivated) ? false : true;
        GameManager.gameManager.orb.GetComponent<OrbController>().canHitPlayer = GameData.worseModeActivated;
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
        soundEmitter.PlaySound(0);
        powerPanel.SetActive(false);
        mappingPanel.SetActive(true);
    }

    public void OpenPowerPanel()
    {
        soundEmitter.PlaySound(0);
        mappingPanel.SetActive(false);
        powerPanel.SetActive(true);
    }
    public void LoadMenu()
    {
        soundEmitter.PlaySound(0);
        StartCoroutine(LoadMenuCoroutine());
    }

    IEnumerator LoadMenuCoroutine()
    {
        print("LOAD");
        mainMenu.SetActive(false);
        StartCoroutine(GameManager.gameManager.FadeCoroutine("FadeIn"));
        yield return new WaitUntil(() => GameManager.gameManager.isPaused == true);
        Time.timeScale = 1;
        SceneManager.LoadScene(2);
    }
}
