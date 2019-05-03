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

		float maxVolume = 0.4f;
		if(SceneManager.GetActiveScene().buildIndex == 9)
		{
			maxVolume = 0.5f;
		}

		while(bgm.volume <= maxVolume)
		{
			bgm.volume += 0.01f;
			yield return new WaitForEndOfFrame();
		}
		Destroy(this.gameObject);
	}
}
