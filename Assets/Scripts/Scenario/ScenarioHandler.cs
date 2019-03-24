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
		Debug.Log("oui");
		director = GetComponent<PlayableDirector>();
		dialogSystem = GameObject.Find("DialogSystem").GetComponent<DialogSystem>();
		dialogSystem.gameObject.SetActive(false);
		actualDialog = 0;
		director.Play();
	}

	public void LaunchNextDialog()
	{
		dialogSystem.gameObject.SetActive(true);
		director.Pause();
		StartCoroutine(dialogSystem.StartDialog(dialogs[actualDialog++], director));
	}
}
