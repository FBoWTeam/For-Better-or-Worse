using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEmitter : MonoBehaviour
{
	[Serializable]
	public class PlayableSound
	{
		public AudioClip clip;
		[Range(0.0f,1.0f)]
		public float baseVolume;
		public float maxDistance;
		public bool continuousSound;
		[HideInInspector]
		public bool playing;
		public bool omnipresentSound;
	}

	public List<PlayableSound> sounds;
	public List<AudioSource> audioSources;

    // Start is called before the first frame update
    void Awake()
    {
		audioSources = new List<AudioSource>(GetComponents<AudioSource>());
    }

	private void Update()
	{
		for(int i=0; i<sounds.Count; i++)
		{
			PlayableSound sound = sounds[i];
			if (sound.continuousSound && sound.playing)
			{
				AudioSource source = audioSources[i];
				source.volume = sound.baseVolume * getVolumeCoefficient(sound.maxDistance);
			}
		}
	}

	public void PlaySound(int soundID)
	{
		AudioSource source = audioSources[soundID];
		PlayableSound sound = sounds[soundID];

		source.clip = sound.clip;

		source.volume = sound.baseVolume;
		if (!sound.omnipresentSound)
		{
			getVolumeCoefficient(sound.maxDistance);
		}

		source.Play();

		if(sound.continuousSound)
		{
			sound.playing = true;
		}
	}

	public void StopSound(int soundID)
	{
		AudioSource source = audioSources[soundID];
		PlayableSound sound = sounds[soundID];

		source.Stop();

		sound.playing = false;
	}

	public float getVolumeCoefficient(float maxDist)
	{
		float distance = getNearestPlayerDistance();
		float coefficient = distance / maxDist;
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
