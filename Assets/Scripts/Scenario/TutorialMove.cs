using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialMove : Tutorial
{
    // Start is called before the first frame update
    void Start()
    {
		StartCoroutine(test());
    }

	public IEnumerator test()
	{
		yield return new WaitForSeconds(2.0f);
		GetsOut();
	}
}
