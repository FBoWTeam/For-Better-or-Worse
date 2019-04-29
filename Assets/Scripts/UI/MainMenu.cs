using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Audio;
using TMPro;
using UnityEngine.EventSystems;


public class MainMenu : MonoBehaviour
{
	public Animation fader;
	bool active;

	public EventSystem eS;

	public GameObject MainMenuCanvas;
	public GameObject MainMenuFirstSelected;
	public GameObject QuitMenuCanvas;
	public GameObject QuitMenuFirstSelected;

	public void Awake()
	{
		active = false;
		StartCoroutine(FadeIn());
	}

	public void LoadHistory()
	{
		if (active)
		{
			StartCoroutine(FadeOut(3));
		}
	}

	public void LoadArena()
	{
		if (active)
		{
			GameData.nextSceneToLoad = 12;
			StartCoroutine(FadeOut(4));
		}
	}

	public void LoadCredits()
	{
		if (active)
		{
			StartCoroutine(FadeOut(11));
		}
	}

	public void OnQuitButtonClick()
	{
		if (active)
		{
			MainMenuCanvas.SetActive(false);
			QuitMenuCanvas.SetActive(true);
			eS.SetSelectedGameObject(QuitMenuFirstSelected);
		}
	}

	public void OnCancelQuitButtonClick()
	{
		if (active)
		{
			QuitMenuCanvas.SetActive(false);
			MainMenuCanvas.SetActive(true);
			eS.SetSelectedGameObject(MainMenuFirstSelected);
		}
	}

	public void QuitGame()
	{
		if(active)
		{
			Application.Quit();
			//UnityEditor.EditorApplication.isPlaying = false;
		}
	}

	IEnumerator FadeIn()
	{
		fader.Play();
		yield return new WaitForSeconds(0.1f);
		yield return new WaitUntil(() => fader.isPlaying == false);
		active = true;
	}

	IEnumerator FadeOut(int sceneToLoad)
	{
		active = false;
		fader.clip = fader.GetClip("FadeOut");
		fader.Play();
		yield return new WaitForSeconds(0.1f);
		yield return new WaitUntil(() => fader.isPlaying == false);
		SceneManager.LoadScene(sceneToLoad);
	}
}
