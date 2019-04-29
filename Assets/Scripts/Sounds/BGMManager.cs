using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BGMManager : MonoBehaviour
{
	static BGMManager manager;

	public int lastScene;

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
		if(SceneManager.GetActiveScene().buildIndex == 11)
		{
			manager = null;
			Destroy(this.gameObject);
		}
		if (SceneManager.GetActiveScene().buildIndex == lastScene)
		{
			manager = null;
			SceneManager.MoveGameObjectToScene(this.gameObject, SceneManager.GetActiveScene());
			Destroy(this);
		}
	}
}
