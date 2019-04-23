using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainThemeManager : MonoBehaviour
{
	static MainThemeManager manager;

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
		if (SceneManager.GetActiveScene().buildIndex == 4)
		{
			manager = null;
			SceneManager.MoveGameObjectToScene(this.gameObject, SceneManager.GetActiveScene());
		}
	}
}
