using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class CreditsWaitForEnd : MonoBehaviour
{
    void Awake()
    {
		GetComponent<PlayableDirector>().stopped += WhenEnded;

	}

	private void WhenEnded(PlayableDirector obj)
	{
		SceneManager.LoadScene(2);
	}
}
