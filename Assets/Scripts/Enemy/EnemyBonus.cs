using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBonus : MonoBehaviour
{
	public enum Bonus
	{
		Mirror,
		ElementalShield,
		None
	}

	public Bonus bonus;

	//Mirror
	[DrawIf(new string[] { "bonus" }, Bonus.Mirror)]
	public GameObject mirrorPrefab;
	GameObject mirrorChild;
	[DrawIf(new string[] { "bonus" }, Bonus.Mirror)]
	[Tooltip("represent the mirror's deactivation duration when the enemy is hit")]
	public float deactivationDuration;
	[DrawIf(new string[] { "bonus" }, Bonus.Mirror)]
	public float knockbackForce;

	// Start is called before the first frame update
	void Start()
    {
		if (bonus == Bonus.Mirror)
		{
			mirrorChild = Instantiate(mirrorPrefab, transform);
			mirrorChild.GetComponent<Mirror>().knockbackForce = knockbackForce;
		}
    }

	IEnumerator DeactivateShieldCoroutine()
	{
		mirrorChild.GetComponent<BoxCollider>().enabled = false;
		yield return new WaitForSeconds(deactivationDuration);
		mirrorChild.GetComponent<BoxCollider>().enabled = true;
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Orb") && bonus == Bonus.Mirror)
		{
			StopCoroutine(DeactivateShieldCoroutine());
			StartCoroutine(DeactivateShieldCoroutine());
		}
	}
}
