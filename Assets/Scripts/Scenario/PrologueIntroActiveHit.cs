using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class PrologueIntroActiveHit : Tutorial
{
	PlayableDirector director;
	bool entityHasBeenHit;

	private void OnEnable()
	{
		director = GameObject.Find("IntroScenario").GetComponent<PlayableDirector>();
		StartCoroutine(ActivateHit());
	}

	public IEnumerator ActivateHit()
	{
		director.Pause();
		GetsIn();

		GameManager.gameManager.player1.GetComponent<OrbHitter>().active = true;
		GameManager.gameManager.player2.GetComponent<OrbHitter>().active = true;
		entityHasBeenHit = false;

		while(!entityHasBeenHit)
		{
			yield return new WaitForEndOfFrame();
		}

		Coroutine end = StartCoroutine(GetsOut());
		yield return new WaitForSeconds(1.0f);

		director.Resume();
		Destroy(gameObject);
	}

	public IEnumerator EntityHit()
	{
		MeshRenderer renderer = transform.parent.GetComponentInChildren<MeshRenderer>();
		renderer.enabled = false;
		yield return new WaitForSeconds(0.2f);
		renderer.enabled = true;
		yield return new WaitForSeconds(0.2f);
		renderer.enabled = false;
		yield return new WaitForSeconds(0.2f);
		renderer.enabled = true;
		yield return new WaitForSeconds(0.2f);
		renderer.enabled = false;
		yield return new WaitForSeconds(0.2f);
		renderer.enabled = true;
	}

	public void OnTriggerEnter(Collider other)
	{
		if(other.CompareTag("Orb"))
		{
			entityHasBeenHit = true;
			StartCoroutine(EntityHit());
		}
	}
}
