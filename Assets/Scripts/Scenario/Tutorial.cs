using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour
{
	public string tutorialText;
	public Sprite tutorialIcon;

    public void GetsIn()
    {
		GameObject tutorialCanvas = GameManager.gameManager.tutorials;
		tutorialCanvas.SetActive(true);
		tutorialCanvas.transform.Find("Text").GetComponent<TextMeshProUGUI>().text = tutorialText;
		tutorialCanvas.transform.Find("Image").GetComponent<Image>().sprite = tutorialIcon;
		tutorialCanvas.GetComponent<Animator>().SetTrigger("TutoIn");
    }

	public IEnumerator GetsOut()
	{
		GameObject tutorialCanvas = GameManager.gameManager.tutorials;
		tutorialCanvas.GetComponent<Animator>().SetTrigger("TutoOut");
		yield return new WaitForSeconds(tutorialCanvas.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length);
		tutorialCanvas.SetActive(false);
	}
}
