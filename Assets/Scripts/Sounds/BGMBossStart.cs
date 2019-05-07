using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMBossStart : MonoBehaviour
{
	public AudioClip bossTheme;

	private void OnEnable()
	{
		StartCoroutine(startBGM());
	}

	IEnumerator startBGM()
	{
		AudioSource bgm = GameObject.Find("BGM").GetComponent<AudioSource>();
		bgm.clip = bossTheme;
		bgm.Play();

		bgm.volume = 0.01f;
		while (bgm.volume <= 0.6f)
		{
			bgm.volume += 0.01f;
			yield return new WaitForEndOfFrame();
		}

		Destroy(this.gameObject);
	}
}
