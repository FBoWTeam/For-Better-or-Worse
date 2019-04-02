using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmortizeTutorial : Tutorial
{
	public IEnumerator AmortizeTutorialCoroutine()
	{
		GetsIn();

		bool player1HasAmortized = false;
		bool player2HasAmortized = false;

		while (!(player1HasAmortized && player2HasAmortized))
		{
			if(Input.GetAxisRaw("OrbAmortizerP1") != 0)
			{
				player1HasAmortized = true;
			}
			if(Input.GetAxisRaw("OrbAmortizerP2") != 0)
			{
				player2HasAmortized = true;
			}
			yield return new WaitForEndOfFrame();
		}

		StartCoroutine(GetsOut());
		yield return new WaitForSeconds(4.0f);

		Destroy(this.gameObject);
	}
}
