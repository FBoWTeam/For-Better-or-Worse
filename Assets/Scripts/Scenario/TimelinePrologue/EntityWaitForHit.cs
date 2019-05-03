using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityWaitForHit : MonoBehaviour
{
	public SkinnedMeshRenderer rend;

	private void OnTriggerEnter(Collider other)
	{
		if(other.CompareTag("Orb"))
		{
			StartCoroutine(EntityHit());
		}
	}

	IEnumerator EntityHit()
	{
		rend.enabled = false;
		yield return new WaitForSeconds(0.05f);
		rend.enabled = true;
		yield return new WaitForSeconds(0.05f);
		rend.enabled = false;
		yield return new WaitForSeconds(0.05f);
		rend.enabled = true;
		yield return new WaitForSeconds(0.05f);
		rend.enabled = false;
		yield return new WaitForSeconds(0.05f);
		rend.enabled = true;
		yield return new WaitForSeconds(0.05f);
		rend.enabled = false;
		yield return new WaitForSeconds(0.05f);
		rend.enabled = true;

		Destroy(this.gameObject);
	}
}
