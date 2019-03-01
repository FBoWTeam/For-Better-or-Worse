using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Mirror : MonoBehaviour
{
	public float knockbackForce;

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Orb"))
		{
			if(GameManager.gameManager.orb.GetComponent<PowerController>().behavioralPower == GameManager.PowerType.LargeOrb)
			{
				EnemyBonus enemy = GetComponentInParent<EnemyBonus>();
				enemy.bonus = EnemyBonus.Bonus.None;
				enemy.StopCoroutine(enemy.DeactivateShieldCoroutine());
				Destroy(this.gameObject);
			}
			else
			{
				OrbController controller = GameManager.gameManager.orb.GetComponent<OrbController>();
				controller.toPlayer2 = !controller.toPlayer2;
			}
			GetComponentInParent<NavMeshAgent>().velocity = -GetComponentInParent<NavMeshAgent>().velocity * knockbackForce;
		}
	}
}
