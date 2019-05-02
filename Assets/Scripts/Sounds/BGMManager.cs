using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BGMManager : MonoBehaviour
{
	static BGMManager manager;

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
		switch(SceneManager.GetActiveScene().buildIndex)
		{
			case 5:
			case 7:
			case 8:
			case 9:
			case 11:
			case 10:
			case 12:
				manager = null;
				Destroy(this.gameObject);
				break;
		}
	}
}
