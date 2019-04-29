using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityWaitForHit : MonoBehaviour
{
	public SkinnedMeshRenderer renderer;

	private void OnTriggerEnter(Collider other)
	{
		if(other.CompareTag("Orb"))
		{
			StartCoroutine(EntityHit());
		}
	}

	IEnumerator EntityHit()
	{
		renderer.enabled = false;
		yield return new WaitForSeconds(0.05f);
		renderer.enabled = true;
		yield return new WaitForSeconds(0.05f);
		renderer.enabled = false;
		yield return new WaitForSeconds(0.05f);
		renderer.enabled = true;
		yield return new WaitForSeconds(0.05f);
		renderer.enabled = false;
		yield return new WaitForSeconds(0.05f);
		renderer.enabled = true;
		yield return new WaitForSeconds(0.05f);
		renderer.enabled = false;
		yield return new WaitForSeconds(0.05f);
		renderer.enabled = true;

		Destroy(this.gameObject);
	}
}
