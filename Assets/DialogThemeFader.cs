using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogThemeFader : MonoBehaviour
{
	private void OnEnable()
	{
		StartCoroutine(FadeVolume());
	}

	IEnumerator FadeVolume()
	{
		AudioSource theme = GameObject.Find("DialogTheme").GetComponent<AudioSource>();
		while (theme.volume >= 0.0f)
		{
			theme.volume -= 0.01f;
			yield return new WaitForEndOfFrame();
		}
		Destroy(this.gameObject);
	}
}
