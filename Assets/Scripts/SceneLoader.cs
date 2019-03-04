using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
	public int sceneToLoad;
	public bool player1, player2;


	[Header("[UI References]")]
	public Canvas charactersHead;

	public Image fox, racoon;

	public Sprite foxSprite, foxGreySprite;
	public Sprite racoonSprite, racoonGreySprite;

    // Update is called once per frame
    void Update()
    {
		charactersHead.transform.LookAt(Camera.main.transform);
		if(player1 && player2)
		{
			SceneManager.LoadScene(sceneToLoad);
		}
		else if(player1 || player2)
		{
			charactersHead.enabled = true;
		}
		else
		{
			charactersHead.enabled = false;
		}
    }

	private void OnTriggerEnter(Collider other)
	{
		if(other.CompareTag("Player"))
		{
			if (other.GetComponent<PlayerController>().player1)
			{
				player1 = true;
				fox.sprite = foxSprite;
			}
			else
			{
				player2 = true;
				racoon.sprite = racoonSprite;
			}
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			if (other.GetComponent<PlayerController>().player1)
			{
				player1 = false;
				fox.sprite = foxGreySprite;
			}
			else
			{
				player2 = false;
				racoon.sprite = racoonGreySprite;
			}
		}
	}
}
