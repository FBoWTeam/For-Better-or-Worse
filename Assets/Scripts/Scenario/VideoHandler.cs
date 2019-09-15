using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class VideoHandler : MonoBehaviour
{
	VideoPlayer player;
	public VideoClip englishIntro;
	public VideoClip frenchIntro;

	bool isSkiping;

    void Awake()
    {
		player = GetComponent<VideoPlayer>();
		if (GameData.english)
		{
			player.clip = englishIntro;
		}
		else
		{
			player.clip = frenchIntro;
		}
		StartCoroutine(PlayIntro());
    }

	private void Update()
	{
		if(!isSkiping && (Input.GetKey(KeyCode.Joystick1Button7) || Input.GetKey(KeyCode.Joystick2Button7) || Input.GetKey(KeyCode.Escape)))
		{
			StartCoroutine(FadeOut());
		}
		else if (isSkiping)
		{
			player.SetDirectAudioVolume(0, player.GetDirectAudioVolume(0) - 0.02f);
		}
	}

	IEnumerator PlayIntro()
	{
		player.Play();
		yield return new WaitForSeconds(2f);
		yield return new WaitUntil(() => player.isPlaying == false);
		SceneManager.LoadScene(2);
	}

	IEnumerator FadeOut()
	{
		isSkiping = true;
		GetComponent<Animation>().Play();
		yield return new WaitForSeconds(0.1f);
		yield return new WaitUntil(() => GetComponent<Animation>().isPlaying == false);
		SceneManager.LoadScene(2);
	}
}
