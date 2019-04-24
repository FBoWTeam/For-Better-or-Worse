using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DifficultySelection : MonoBehaviour
{
	public Animation fader;
	bool active;

	public void Awake()
	{
		active = false;
		StartCoroutine(FadeIn());
	}

	public void SetDifficultyMode(bool worseActivated)
    {
		if(active)
		{
			GameData.worseModeActivated = worseActivated;
			StartCoroutine(FadeOut(GameData.nextSceneToLoad));
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
		GameObject.Find("ThemeBGM").GetComponent<Animation>().Play();
		fader.clip = fader.GetClip("FadeOut");
		fader.Play();
		yield return new WaitForSeconds(0.1f);
		yield return new WaitUntil(() => fader.isPlaying == false);
		SceneManager.LoadScene(sceneToLoad);
	}
}
