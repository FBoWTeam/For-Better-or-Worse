using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class Tutorial : MonoBehaviour
{
    public bool isCinematicTutorial;

    [Header("Canvas")]
    public GameObject tutorialCanvas;

    [Header("Text & Image")]
    public Sprite tutorialImage;
    public Sprite tutorialButton;
    public string tutorialTitleKey;
    public string tutorialTextKey;


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(TutorialCoroutine());
        }
    }

    private void OnEnable()
    {
        if (isCinematicTutorial)
        {
            StartCoroutine(TutorialCoroutine());
        }
    }


    IEnumerator TutorialCoroutine()
    {
        GameManager.gameManager.isPaused = true;
        bool readyPlayer1 = false;
        bool readyPlayer2 = false;

        tutorialCanvas.SetActive(true);

        tutorialCanvas.transform.Find("tutorialImage").GetComponent<Image>().sprite = tutorialImage;
        tutorialCanvas.transform.Find("tutorialButton").GetComponent<Image>().sprite = tutorialButton;
        tutorialCanvas.transform.Find("tutorialTitle").GetComponent<TextMeshProUGUI>().text = I18n.Translate(tutorialTitleKey);
        tutorialCanvas.transform.Find("tutorialText").GetComponent<TextMeshProUGUI>().text = I18n.Translate(tutorialTextKey);


        while (readyPlayer1 == false || readyPlayer2 == false)
        {
            if (Input.GetKey(KeyCode.Joystick1Button0))
            {
                readyPlayer1 = true;
                tutorialCanvas.transform.Find("readyIconP1").gameObject.SetActive(true);
            }
            if (Input.GetKey(KeyCode.Joystick2Button0))
            {
                readyPlayer2 = true;
                tutorialCanvas.transform.Find("readyIconP2").gameObject.SetActive(true);
            }
            yield return new WaitForEndOfFrame();
        }

        tutorialCanvas.transform.Find("readyIconP1").gameObject.SetActive(false);
        tutorialCanvas.transform.Find("readyIconP2").gameObject.SetActive(false);
        tutorialCanvas.SetActive(false);
        GameManager.gameManager.isPaused = true;
    }

}
