using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PrologueBGM : MonoBehaviour
{
	void Awake()
	{
		DontDestroyOnLoad(this.gameObject);
	}

	private void Update()
	{
		if (SceneManager.GetActiveScene().buildIndex == 6)
		{
			SceneManager.MoveGameObjectToScene(this.gameObject, SceneManager.GetActiveScene());
		}
	}
}
