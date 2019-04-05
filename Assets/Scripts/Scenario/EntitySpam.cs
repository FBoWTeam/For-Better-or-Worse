using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EntitySpam : MonoBehaviour
{
	public List<string> sentences;
	List<TextMeshProUGUI> textList;
	public float writeSpeed;

	private void OnEnable()
	{
		textList = new List<TextMeshProUGUI>(GetComponentsInChildren<TextMeshProUGUI>());
		StartCoroutine(EntitySpamCoroutine());
	}

	public IEnumerator EntitySpamCoroutine()
	{
		TextMeshProUGUI actualText;
		bool pass = true;
		while (true)
		{
			foreach(string s in sentences)
			{
				actualText = textList[Random.Range(0, textList.Count)];
				foreach(char c in s)
				{
					if (c == '\\')
						actualText.text += '\n';
					else
						actualText.text += c;

					pass = !pass;
					if(!pass)
					{
						yield return new WaitForSeconds(writeSpeed);
					}
				}
				actualText.text = "";
			}
		}
	}
}
