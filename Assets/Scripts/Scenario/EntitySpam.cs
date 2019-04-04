using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EntitySpam : MonoBehaviour
{
	public List<string> sentences;
	TextMeshProUGUI text;
	public float writeSpeed;

	private void OnEnable()
	{
		text = GetComponentInChildren<TextMeshProUGUI>();
		StartCoroutine(EntitySpamCoroutine());
	}

	public IEnumerator EntitySpamCoroutine()
	{
		bool pass = true;
		text.text = "";
		while (true)
		{
			foreach(string s in sentences)
			{
				foreach(char c in s)
				{
					GetComponent<RectTransform>().localPosition += Vector3.up;
					if (c == '\\')
						text.text += '\n';
					else
						text.text += c;

					pass = !pass;
					if(!pass)
					{
						yield return new WaitForSeconds(writeSpeed);
					}
				}
			}
		}
	}
}
