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
			OrbController controller = GameManager.gameManager.orb.GetComponent<OrbController>();
			controller.toPlayer2 = !controller.toPlayer2;
			GetComponentInParent<NavMeshAgent>().velocity = -GetComponentInParent<NavMeshAgent>().velocity*knockbackForce;
		}
	}
}
