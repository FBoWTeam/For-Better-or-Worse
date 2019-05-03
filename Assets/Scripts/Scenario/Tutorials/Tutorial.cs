using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class Tutorial : MonoBehaviour
{
    public bool isCinematicTutorial;

    [HideInInspector]
    public GameObject tutorialCanvas;

    [Header("Text & Image")]
    public Sprite tutorialImage;
	public string tutorialTitleKey;
	public string tutorialTextKey;
	public Sprite tutorialButton1;
	public string tutorialTextButton1Key;
	public Sprite tutorialButton2;
	public string tutorialTextButton2Key;

	public Sprite normalReadyButton;
	public Sprite pressedReadyButton;


    

    private void Start()
    {
        tutorialCanvas = GameManager.gameManager.tutorials;
    }

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
			tutorialCanvas = GameManager.gameManager.tutorials;
			StartCoroutine(TutorialCoroutine());
        }
    }


    IEnumerator TutorialCoroutine()
    {
		Debug.Log("bonjour");
        GameManager.gameManager.isPaused = true;
        bool readyPlayer1 = false;
        bool readyPlayer2 = false;

        tutorialCanvas.SetActive(true);

        tutorialCanvas.transform.Find("tutorialImage").GetComponent<Image>().sprite = tutorialImage;
        tutorialCanvas.transform.Find("tutorialTitle").GetComponent<TextMeshProUGUI>().text = I18n.Translate(tutorialTitleKey);
        tutorialCanvas.transform.Find("tutorialText").GetComponent<TextMeshProUGUI>().text = I18n.Translate(tutorialTextKey);
		tutorialCanvas.transform.Find("tutorialButton1").GetComponent<Image>().sprite = tutorialButton1;
		tutorialCanvas.transform.Find("tutorialTextButton1").GetComponent<TextMeshProUGUI>().text = I18n.Translate(tutorialTextButton1Key);
		tutorialCanvas.transform.Find("tutorialButton2").GetComponent<Image>().sprite = tutorialButton2;
		tutorialCanvas.transform.Find("tutorialTextButton2").GetComponent<TextMeshProUGUI>().text = I18n.Translate(tutorialTextButton2Key);

		tutorialCanvas.transform.Find("tutorialReadyP1").GetComponent<Image>().sprite = normalReadyButton;
		tutorialCanvas.transform.Find("tutorialReadyP2").GetComponent<Image>().sprite = normalReadyButton;

		while (readyPlayer1 == false || readyPlayer2 == false)
        {
            if (Input.GetKey(KeyCode.Joystick1Button0) || Input.GetKey(KeyCode.Space))
            {
                readyPlayer1 = true;
				tutorialCanvas.transform.Find("tutorialReadyP1").GetComponent<Image>().sprite = pressedReadyButton;
			}
            if (Input.GetKey(KeyCode.Joystick2Button0) || Input.GetKey(KeyCode.Keypad0))
            {
                readyPlayer2 = true;
				tutorialCanvas.transform.Find("tutorialReadyP2").GetComponent<Image>().sprite = pressedReadyButton;
			}
            yield return new WaitForEndOfFrame();
        }

        tutorialCanvas.SetActive(false);
        GameManager.gameManager.isPaused = false;
		GameManager.gameManager.orb.GetComponent<OrbController>().FreezeOrb(0.5f);

        Destroy(gameObject);
    }

}
