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
}
