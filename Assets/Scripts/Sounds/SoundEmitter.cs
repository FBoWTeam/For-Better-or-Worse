using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEmitter : MonoBehaviour
{
	[Serializable]
	public class PlayableSound
	{
		public AudioClip sound;
		public bool continuousSound;
		public float maxDistance;
		[HideInInspector]
		public bool playing;
	}

	public List<PlayableSound> sounds;
	List<AudioSource> audioSources;

    // Start is called before the first frame update
    void Awake()
    {
        foreach(PlayableSound sound in sounds)
		{
			audioSources.Add(gameObject.AddComponent(typeof(AudioSource)) as AudioSource);
		}
    }

    // Update is called once per frame
    void Update()
	{

    }

	public void PlaySound()
	{

	}

	public float getVolumeCoefficient(int soundID)
	{
		float distance = getNearestPlayerDistance();
		float coefficient = distance / sounds[soundID].maxDistance;
		Mathf.Clamp01(coefficient);

		return coefficient;
	}

	public float getNearestPlayerDistance()
	{
		float Player1Dist = Vector3.Distance(transform.position, GameManager.gameManager.player1.transform.position);
		float Player2Dist = Vector3.Distance(transform.position, GameManager.gameManager.player2.transform.position);

		if(Player1Dist <= Player2Dist)
		{
			return Player1Dist;
		}
		else
		{
			return Player2Dist;
		}
	}
}
