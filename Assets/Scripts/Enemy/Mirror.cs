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
			if(GameManager.gameManager.orb.GetComponent<PowerController>().behaviouralPower == GameManager.PowerType.LargeOrb)
			{
                //update in score manager
                ScoreManager.scoreManager.enemyMirrorBroken++;

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
			//GetComponentInParent<NavMeshAgent>().velocity = -GetComponentInParent<NavMeshAgent>().velocity * knockbackForce;
            StartCoroutine(ShieldKnockback(15f));
		}
	}


    private IEnumerator ShieldKnockback(float tick)
    {
        Vector3 dir = -GetComponentInParent<NavMeshAgent>().velocity.normalized;
        for (int i = 0; i < tick; i++)
        {
            GetComponentInParent<NavMeshAgent>().velocity = (dir * knockbackForce) / tick;
            yield return new WaitForEndOfFrame();
        }
    }


}
