using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaunchAmortizeTuto : MonoBehaviour
{
	public AmortizeTutorial tuto;

	private void OnDestroy()
	{
		Debug.Log("oui");
		tuto.StartCoroutine(tuto.AmortizeTutorialCoroutine());
	}
}
