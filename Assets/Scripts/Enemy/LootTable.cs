using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootTable : MonoBehaviour
{
	[Range(0.0f, 1.0f)]
	public float chanceOfHealing;

	[Range(0.0f, 1.0f)]
	public float chanceOfPowerDrop;
	public GameManager.PowerType droppedPower;

	public void LootEnemy()
	{
		//if (Random.Range(0.0f, 1.0f) <= chanceOfHealing)
			//do the heal thing

		if (Random.Range(0.0f, 1.0f) <= chanceOfPowerDrop && GameManager.gameManager.orb.GetComponent<OrbHitter>().powerToApply == GameManager.PowerType.None)
		{
			OrbHitter hitter = GameManager.gameManager.orb.GetComponent<OrbHitter>();
			hitter.droppedPower = droppedPower;
			hitter.reflectedDrop = false;
		}
	}
}
