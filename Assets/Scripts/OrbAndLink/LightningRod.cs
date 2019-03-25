using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningRod : MonoBehaviour
{
	LineRenderer line;

	public int segmentNb;
	public float deviation;

	public GameObject target;

	void Start()
	{
		line = GetComponent<LineRenderer>();
		line.positionCount = segmentNb+1;
		StartCoroutine(LightningRodCoroutine());
    }

	void RandomRodGeneration(float distance)
	{
		float segmentSpacement = distance / (float)segmentNb;
		Vector3[] points = new Vector3[segmentNb+1];

		for(int i=0; i<segmentNb+1; i++)
		{
			points[i] = new Vector3(Random.Range(-deviation, deviation), Random.Range(-deviation, deviation), i*segmentSpacement);
		}

		line.SetPositions(points);
	}

	IEnumerator LightningRodCoroutine()
	{
		while (true)
		{
			if(target!= null)
			{
				transform.LookAt(target.transform.position);
				RandomRodGeneration(Vector3.Distance(transform.position, target.transform.position));
			}
			yield return new WaitForSeconds(0.02f);
		}
	}

}
