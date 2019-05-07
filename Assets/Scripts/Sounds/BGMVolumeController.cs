using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BGMVolumeController : MonoBehaviour
{
	private void OnEnable()
	{
		StartCoroutine(BoostBGMVolume());
	}

	IEnumerator BoostBGMVolume()
	{
		AudioSource bgm = GameObject.Find("BGM").GetComponent<AudioSource>();

		while(bgm.volume <= 0.6f)
		{
			bgm.volume += 0.01f;
			yield return new WaitForEndOfFrame();
		}
		Destroy(this.gameObject);
	}
}
