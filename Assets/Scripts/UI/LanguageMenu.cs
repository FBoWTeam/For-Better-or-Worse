using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LanguageMenu : MonoBehaviour
{
	public Animation fader;
	bool active;

	public void Awake()
	{
		active = false;
		StartCoroutine(FadeIn());
	}

	public void SetLanguage(bool french)
	{
		if(active)
		{
			if (french)
			{
				GameData.english = false;
				I18n.LoadLang("fr_FR");
			}
			else
			{
				GameData.english = true;
				I18n.LoadLang("en_US");
			}

			StartCoroutine(FadeOut(1));
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