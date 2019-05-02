using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootTable : MonoBehaviour
{
	[Range(0.0f, 1.0f)]
	public float chanceOfHealing;
	public int healAmount;

	[Range(0.0f, 1.0f)]
	public float chanceOfPowerDrop;
	public GameManager.PowerType droppedPower;

	public SoundEmitter soundEmitter;

	public void LootEnemy()
	{
		if (Random.Range(0.0f, 1.0f) <= chanceOfHealing)
		{
			GameManager.gameManager.spawnHealingOrbs(0, healAmount, "normal");
		}

		if (Random.Range(0.0f, 1.0f) <= chanceOfPowerDrop && GameManager.gameManager.orb.GetComponent<PowerController>().droppedPower == GameManager.PowerType.None)
		{
			soundEmitter.PlaySound(2, true);
			PowerController controller = GameManager.gameManager.orb.GetComponent<PowerController>();
			controller.droppedPower = droppedPower;
			controller.reflectedDrop = false;

            GameManager.gameManager.UIManager.UpdateDroppedPower(droppedPower);
        }
    }
}
