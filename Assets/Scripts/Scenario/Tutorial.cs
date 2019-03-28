using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour
{
	Canvas tutorialCanvas;

	public string tutorialText;
	public Sprite tutorialIcon;

	public void Awake()
	{
		tutorialCanvas = GameObject.Find("Tutorials").GetComponent<Canvas>();
		GetsIn();
	}

    public void GetsIn()
    {
		tutorialCanvas.transform.Find("Text").GetComponent<TextMeshProUGUI>().text = tutorialText;
		tutorialCanvas.transform.Find("Image").GetComponent<Image>().sprite = tutorialIcon;
		tutorialCanvas.GetComponent<Animator>().SetTrigger("TutoIn");
    }

	public void GetsOut()
	{
		tutorialCanvas.GetComponent<Animator>().SetTrigger("TutoOut");
		Destroy(gameObject);
	}
}
