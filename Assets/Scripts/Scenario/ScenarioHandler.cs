using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class ScenarioHandler : MonoBehaviour
{
	public List<Dialog> dialogs;

	[HideInInspector]
	public PlayableDirector director;
	DialogSystem dialogSystem;
	int actualDialog;
	Dialog dialogToDisplay;

	GameObject skipCanvas;

	public void Initialize()
	{
		director = GetComponent<PlayableDirector>();

		skipCanvas = transform.GetChild(0).gameObject;
		skipCanvas.SetActive(false);

		dialogSystem = GameObject.Find("DialogSystem").GetComponent<DialogSystem>();
		dialogSystem.gameObject.SetActive(false);
		actualDialog = 0;
        
        director.Play();
		director.stopped += WhenEnded;

		StartCoroutine(SkipIntroListener());
;
	}

	public void LaunchNextDialog()
	{
		dialogSystem.gameObject.SetActive(true);
		director.Pause();
		StartCoroutine(dialogSystem.StartDialog(dialogs[actualDialog++], director));
	}

	private void WhenEnded(PlayableDirector obj)
	{
		GameObject.Find("BlackBands").SetActive(false);
		GameManager.gameManager.damageTakenP1 = 0;
		GameManager.gameManager.damageTakenP2 = 0;
		GameManager.gameManager.UIManager.gameObject.SetActive(true);
		GameManager.gameManager.UIManager.UpdateHealthBar();
        GameManager.gameManager.player1.GetComponent<PlayerController>().active = true;
        GameManager.gameManager.player2.GetComponent<PlayerController>().active = true;
        GameManager.gameManager.player1.GetComponent<OrbHitter>().active = true;
        GameManager.gameManager.player2.GetComponent<OrbHitter>().active = true;
        GameManager.gameManager.orb.GetComponent<OrbController>().canHitPlayer = GameData.worseModeActivated;
        GameManager.gameManager.isCutScene = false;

		if (!GameData.introSkiped)
		{
			Destroy(this.gameObject);
		}
	}

	IEnumerator SkipIntroListener()
	{
		bool introSkiped = false;
		while(!introSkiped)
		{
			if(Input.GetKey(KeyCode.Joystick1Button7) || Input.GetKey(KeyCode.Joystick2Button7) || Input.GetKey(KeyCode.Escape))
			{
				introSkiped = true;
				skipCanvas.SetActive(true);
			}
			yield return new WaitForEndOfFrame();
		}

		yield return new WaitForSeconds(0.5f);

		bool introSkipedTwice = false;
		while (!introSkipedTwice)
		{
			if (Input.GetKey(KeyCode.Joystick1Button7) || Input.GetKey(KeyCode.Joystick2Button7) || Input.GetKey(KeyCode.Escape))
			{
				introSkipedTwice = true;
			}
			yield return new WaitForEndOfFrame();
		}

		GameData.introSkiped = true;
		director.Stop();
		skipCanvas.SetActive(false);
		GameManager.gameManager.UIManager.gameObject.SetActive(false);

		StartCoroutine(GameManager.gameManager.FadeCoroutine("FadeOut"));
		yield return new WaitUntil(() => GameManager.gameManager.isPaused == false);

		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	}
}
