using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeformLinkTutorial : Tutorial
{
	LinkDeformation linkDeform;
	bool launched = false;

	private void Awake()
	{
		linkDeform = GameObject.Find("Deformation").GetComponent<LinkDeformation>();
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player") && !launched)
		{
			launched = true;
			StartCoroutine(DeformLinkTutorialCoroutine());
		}
	}

	public IEnumerator DeformLinkTutorialCoroutine()
	{
		GetsIn();

		bool player1HasDeformed = false;
		bool player2HasDeformed = false;
		float deformAmountP1, deformAmountP2;

		while (!(player1HasDeformed && player2HasDeformed))
		{
			(deformAmountP1, deformAmountP2) = linkDeform.GetDeformInputs();

			if (deformAmountP1 != 0.0f)
			{
				player1HasDeformed = true;
			}
			if (deformAmountP2 != 0.0f)
			{
				player2HasDeformed = true;
			}
			yield return new WaitForEndOfFrame();
		}

		StartCoroutine(GetsOut());
		yield return new WaitForSeconds(4.0f);

		Destroy(this.gameObject);
	}
}
