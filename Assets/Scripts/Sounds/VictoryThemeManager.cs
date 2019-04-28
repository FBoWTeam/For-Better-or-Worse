using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VictoryThemeManager : MonoBehaviour
{
	public AudioClip intro;
	public AudioClip theme;
	AudioSource source;

	void Awake()
	{
		source = GetComponent<AudioSource>();
		StartCoroutine(IntroToThemeTransition());
	}

	IEnumerator IntroToThemeTransition()
	{
		yield return new WaitForSeconds(0.2f);

		source.clip = intro;
		source.loop = false;
		source.Play();

		yield return new WaitUntil(() => source.isPlaying == false);

		source.clip = theme;
		source.loop = true;
		source.Play();

		Destroy(this);
	}
}
