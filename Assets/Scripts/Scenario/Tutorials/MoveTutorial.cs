using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class MoveTutorial : Tutorial
{
	PlayableDirector director;
	bool player1HasMoved, player2HasMoved;

	private void OnEnable()
	{
		director = GameObject.Find("IntroScenario").GetComponent<PlayableDirector>();
		StartCoroutine(ActivateMove());
	}

	public IEnumerator ActivateMove()
	{
		director.Pause();
		GetsIn();

		GameManager.gameManager.player1.GetComponent<PlayerController>().active = true;
		GameManager.gameManager.player2.GetComponent<PlayerController>().active = true;
		player1HasMoved = false;
		player2HasMoved = false;

		while (!(player1HasMoved && player2HasMoved))
		{
			if (GameManager.gameManager.player1.GetComponent<PlayerController>().direction != Vector3.zero)
			{
				player1HasMoved = true;
			}
			if (GameManager.gameManager.player2.GetComponent<PlayerController>().direction != Vector3.zero)
			{
				player2HasMoved = true;
			}

			yield return new WaitForEndOfFrame();
		}

		StartCoroutine(GetsOut());
		yield return new WaitForSeconds(4.0f);

		director.Resume();
		Destroy(gameObject);
	}
}
