using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class TimelineTutorialPause : MonoBehaviour
{
	public PlayableDirector director;

	private void OnEnable()
	{
		director.Pause();
	}

	private void OnDestroy()
	{
		director.Resume();
	}
}
