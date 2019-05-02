using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DifficultySelection : MonoBehaviour
{
	public Animation fader;
	bool active;

	public SoundEmitter soundEmitter;

	public void Awake()
	{
		active = false;
		StartCoroutine(FadeIn());
	}

	public void SetDifficultyMode(bool worseActivated)
    {
		if(active)
		{
			soundEmitter.PlaySound(0);
			GameData.worseModeActivated = worseActivated;
			GameObject.Find("ThemeBGM").GetComponent<Animation>().Play();
			StartCoroutine(FadeOut(GameData.nextSceneToLoad));
		}
	}

	IEnumerator FadeIn()
	{
		fader.Play();
		yield return new WaitForSeconds(0.1f);
		yield return new WaitUntil(() => fader.isPlaying == false);
		active = true;
		StartCoroutine(CancelListener());
	}

	IEnumerator CancelListener()
	{
		while (true)
		{
			if (Input.GetKey(KeyCode.Joystick1Button1) || Input.GetKey(KeyCode.Escape))
			{
				soundEmitter.PlaySound(0);
				if (GameData.previousScene == 2)
				{
					StartCoroutine(FadeOut(GameData.previousScene));
				}
				else
				{
					StartCoroutine(FadeOut(3));
				}
			}
			yield return new WaitForEndOfFrame();
		}
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
