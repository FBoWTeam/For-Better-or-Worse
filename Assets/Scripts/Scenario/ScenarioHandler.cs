using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class ScenarioHandler : MonoBehaviour
{
	public List<Dialog> dialogs;

	[HideInInspector]
	public PlayableDirector director;
	DialogSystem dialogSystem;
	int actualDialog;
	Dialog dialogToDisplay;

	public void Initialize()
	{
		director = GetComponent<PlayableDirector>();
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
        Destroy(this.gameObject);
	}

	IEnumerator SkipIntroListener()
	{
		bool introNotSkiped = true;

		while(introNotSkiped)
		{
			if(Input.GetKey(KeyCode.Joystick1Button7) || Input.GetKey(KeyCode.Joystick2Button7) || Input.GetKey(KeyCode.Escape))
			{
				introNotSkiped = false;
			}
			yield return new WaitForEndOfFrame();
		}

		director.Stop();

		StartCoroutine(GameManager.gameManager.FadeCoroutine("FadeOut"));

		yield return new WaitUntil(() => GameManager.gameManager.isPaused == false);

		StartCoroutine(GameManager.gameManager.FadeCoroutine("FadeIn"));

		yield return new WaitUntil(() => GameManager.gameManager.isPaused == false);

		Destroy(this.gameObject);
	}
}
