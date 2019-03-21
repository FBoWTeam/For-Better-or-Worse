using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class swordHitter : MonoBehaviour
{
	EnemySkill enemy;

	private void Start()
	{
		enemy = GetComponentInParent<EnemySkill>();
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			GameManager.gameManager.TakeDamage(other.gameObject, enemy.damage, enemy.transform.position, true);
		}
	}
}
