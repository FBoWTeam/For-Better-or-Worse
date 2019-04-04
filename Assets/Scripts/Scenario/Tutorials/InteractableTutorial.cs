using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableTutorial : Tutorial
{
	bool launched = false;

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player") && !launched)
		{
			launched = true;
			StartCoroutine(InteractableTutorialCoroutine());
		}
	}

	public IEnumerator InteractableTutorialCoroutine()
	{
		GetsIn();
		yield return new WaitForSeconds(5.0f);
		StartCoroutine(GetsOut());
		yield return new WaitForSeconds(4.0f);

		Destroy(this.gameObject);
	}
}
