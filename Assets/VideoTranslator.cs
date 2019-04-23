using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class VideoTranslator : MonoBehaviour
{
	VideoPlayer player;
	public VideoClip englishIntro;
	public VideoClip frenchIntro;

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

	IEnumerator PlayIntro()
	{
		player.Play();
		yield return new WaitForSeconds(0.5f);
		yield return new WaitUntil(() => player.isPlaying == false);
		SceneManager.LoadScene(2);
	}
}
