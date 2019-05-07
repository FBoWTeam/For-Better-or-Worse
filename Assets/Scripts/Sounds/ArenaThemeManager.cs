using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArenaThemeManager : MonoBehaviour
{
	public AudioClip theme;
	[HideInInspector]
	public bool launchTheme;

	public void Activate()
	{
		StartCoroutine(StartArenaTheme());
		launchTheme = false;
	}

	IEnumerator StartArenaTheme()
	{
		AudioSource bgm = GetComponent<AudioSource>();

		while (bgm.volume >= 0.01f)
		{
			bgm.volume -= 0.01f;
			yield return new WaitForEndOfFrame();
		}
		bgm.volume = 0.0f;

		yield return new WaitUntil(() => launchTheme);

		bgm.clip = theme;
		bgm.Play();
		bgm.volume = 0.6f;

		Destroy(this);
	}
}
