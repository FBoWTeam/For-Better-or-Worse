using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowersTutorial : MonoBehaviour
{
	public Tutorial activeTutorial;
	public Tutorial passingPowerToOrbTutorial;

	private void Start()
	{
		StartCoroutine(PowersTutorialCoroutine());
	}

	public IEnumerator PowersTutorialCoroutine()
	{
		yield return new WaitUntil(() => (GameManager.gameManager.player1.GetComponent<PlayerController>().behaviouralPowerSlot != GameManager.PowerType.None || GameManager.gameManager.player2.GetComponent<PlayerController>().behaviouralPowerSlot != GameManager.PowerType.None));

		activeTutorial.GetsIn();
		yield return new WaitUntil(() => (GameManager.gameManager.player1.GetComponent<OrbHitter>().powerToApply != GameManager.PowerType.None || GameManager.gameManager.player2.GetComponent<OrbHitter>().powerToApply != GameManager.PowerType.None));
		activeTutorial.StartCoroutine(activeTutorial.GetsOut());
		yield return new WaitForSeconds(2.0f);

		passingPowerToOrbTutorial.GetsIn();
		yield return new WaitUntil(() => GameManager.gameManager.orb.GetComponent<PowerController>().behaviouralPower != GameManager.PowerType.None);
		passingPowerToOrbTutorial.StartCoroutine(passingPowerToOrbTutorial.GetsOut());
		yield return new WaitForSeconds(4.0f);

		Destroy(this.gameObject);
	}
}
