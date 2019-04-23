using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrologueBGMVolumeController : MonoBehaviour
{
	private void OnEnable()
	{
		StartCoroutine(BoostBGMVolume());
	}

	IEnumerator BoostBGMVolume()
	{
		AudioSource bgm = GameObject.Find("BGM").GetComponent<AudioSource>();
		while(bgm.volume <= 1.0f)
		{
			bgm.volume += 0.01f;
			yield return new WaitForEndOfFrame();
		}
		Destroy(this.gameObject);
	}
}
