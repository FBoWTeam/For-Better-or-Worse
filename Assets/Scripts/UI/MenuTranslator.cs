using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MenuTranslator : MonoBehaviour
{
	private void Awake()
	{
		foreach(TextMeshProUGUI text in GetComponentsInChildren<TextMeshProUGUI>())
		{
			text.text = I18n.Translate(text.text);
		}
	}
}
