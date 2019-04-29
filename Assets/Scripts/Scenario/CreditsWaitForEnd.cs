using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class CreditsWaitForEnd : MonoBehaviour
{
	PlayableDirector director;
	public Animation fader;
	GameObject skipCanvas;

	void Awake()
	{
		director = GetComponent<PlayableDirector>();
		skipCanvas = transform.GetChild(0).gameObject;
		skipCanvas.SetActive(false);
		GetComponent<PlayableDirector>().stopped += WhenEnded;
		StartCoroutine(SkipCreditsListener());
	}

	private void WhenEnded(PlayableDirector obj)
	{
		SceneManager.LoadScene(2);
	}

	IEnumerator SkipCreditsListener()
	{
		bool introSkiped = false;
		while (!introSkiped)
		{
			if (Input.GetKey(KeyCode.Joystick1Button7) || Input.GetKey(KeyCode.Joystick2Button7) || Input.GetKey(KeyCode.Escape))
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

		skipCanvas.SetActive(false);
		StartCoroutine(FadeOut(2));
	}

	IEnumerator FadeOut(int sceneToLoad)
	{
		fader.clip = fader.GetClip("FadeOut");
		fader.Play();
		yield return new WaitForSeconds(0.1f);
		yield return new WaitUntil(() => fader.isPlaying == false);
		SceneManager.LoadScene(sceneToLoad);
	}
}
