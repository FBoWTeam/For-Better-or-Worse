using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PrologueBGM : MonoBehaviour
{
	static PrologueBGM manager;

	void Awake()
	{
		if (manager == null)
		{
			manager = this;
		}
		else if (manager != this)
		{
			Destroy(this.gameObject);
		}
		DontDestroyOnLoad(this.gameObject);
	}

	private void Update()
	{
		if(SceneManager.GetActiveScene().buildIndex == 10)
		{
			manager = null;
			Destroy(this.gameObject);
		}
	}
}
