using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Mirror : MonoBehaviour
{
    public float knockbackForce;
    public int mirrorHealth;

	public SoundEmitter soundEmitter;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Orb"))
        {
            if (GameManager.gameManager.orb.GetComponent<PowerController>().behaviouralPower == GameManager.PowerType.LargeOrb && mirrorHealth > 0)
            {
                mirrorHealth--;
                OrbController controller = GameManager.gameManager.orb.GetComponent<OrbController>();
                controller.toPlayer2 = !controller.toPlayer2;
                if (mirrorHealth <= 0)
                {
					soundEmitter.PlaySound(1, true);
                    DestroyShield();
                }
				else
				{
					soundEmitter.PlaySound(0);
				}
            }
            else
            {
				soundEmitter.PlaySound(0);
				OrbController controller = GameManager.gameManager.orb.GetComponent<OrbController>();
                controller.toPlayer2 = !controller.toPlayer2;
            }

            StartCoroutine(ShieldKnockback(15f));
        }
    }


    void DestroyShield()
    {
        EnemyBonus enemy = GetComponentInParent<EnemyBonus>();
        enemy.bonus = EnemyBonus.Bonus.None;
        enemy.StopCoroutine(enemy.shieldDeactivatedCoroutine);
        //update in score manager
        ScoreManager.scoreManager.enemyMirrorBroken++;
        Destroy(this.gameObject);
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