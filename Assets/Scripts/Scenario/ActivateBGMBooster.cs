using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateBGMBooster : MonoBehaviour
{
	public GameObject booster;

	private void OnDestroy()
	{
		booster.SetActive(true);
	}
}
