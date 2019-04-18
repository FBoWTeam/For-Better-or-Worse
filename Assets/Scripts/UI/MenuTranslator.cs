using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MenuTranslator : MonoBehaviour
{

	public List<TextMeshProUGUI> texts;

	private void Awake()
	{
		foreach(TextMeshProUGUI text in texts)
		{
			text.text = I18n.Translate(text.text);
		}
	}
}
