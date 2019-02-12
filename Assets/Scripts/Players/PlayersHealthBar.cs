using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayersHealthBar : MonoBehaviour
{
	Image barL, barR;

	// Start is called before the first frame update
	void Start()
	{
		barL = transform.GetChild(0).GetChild(0).GetComponent<Image>();
		barR = transform.GetChild(1).GetChild(0).GetComponent<Image>();
	}

    // Update is called once per frame
    void Update()
    {
		float fillAmnt = (float) GameManager.gameManager.hp / (float) GameManager.gameManager.baseHP;
		barL.fillAmount = fillAmnt;
		barR.fillAmount = fillAmnt;
	}
}
