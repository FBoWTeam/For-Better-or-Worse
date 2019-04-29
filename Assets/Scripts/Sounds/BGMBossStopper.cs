using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMBossStopper : MonoBehaviour
{
	private void OnEnable()
	{
		StartCoroutine(stopBGM());
	}

	IEnumerator stopBGM()
	{
		AudioSource bgm = GameObject.Find("BGM").GetComponent<AudioSource>();
		while (bgm.volume >= 0.01f)
		{
			bgm.volume -= 0.01f;
			yield return new WaitForEndOfFrame();
		}
		bgm.volume = 0.0f;
		Destroy(this.gameObject);
	}
}
