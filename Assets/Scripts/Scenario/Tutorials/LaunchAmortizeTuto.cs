using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaunchAmortizeTuto : MonoBehaviour
{
	public AmortizeTutorial tuto;

	private void OnDestroy()
	{
		tuto.StartCoroutine(tuto.AmortizeTutorialCoroutine());
	}
}
