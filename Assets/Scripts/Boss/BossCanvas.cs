using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BossCanvas : MonoBehaviour
{
	public BossSystem boss;

	Image healthRemainingBar;

	// Start is called before the first frame update
	void Start()
	{
		healthRemainingBar = transform.GetChild(0).GetChild(0).GetComponent<Image>();
	}

	// Update is called once per frame
	void Update()
	{
		transform.LookAt(Camera.main.transform.position);
		healthRemainingBar.fillAmount = (float)boss.hp / (float)boss.baseHP;
	}
}
