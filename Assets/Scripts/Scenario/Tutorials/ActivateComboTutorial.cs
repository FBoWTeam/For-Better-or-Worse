using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateComboTutorial : MonoBehaviour
{
	public GameObject comboTutorial;

	private void OnDestroy()
	{
		comboTutorial.SetActive(true);
	}
}
