using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnemyCanvas : MonoBehaviour
{
	Enemy enemy;

	Image healthRemainingBar;
	TextMeshProUGUI tauntText;
	Color player1ColorTaunt = new Color(255, 96, 0);
	Color player2ColorTaunt = new Color(82, 82, 82);

	// Start is called before the first frame update
	void Start()
    {
		healthRemainingBar = transform.GetChild(0).GetChild(0).GetComponent<Image>();
		tauntText = transform.GetChild(1).GetComponent<TextMeshProUGUI>();
		enemy = GetComponentInParent<Enemy>();
    }

    // Update is called once per frame
    void Update()
    {
		transform.LookAt(Camera.main.transform.position);
		healthRemainingBar.fillAmount = (float)enemy.hp / (float)enemy.baseHP;

		if(enemy.isTaunted)
		{
			if(enemy.taunter)
			{
				tauntText.color = player1ColorTaunt;
			}
			else
			{
				tauntText.color = player2ColorTaunt;
			}
			tauntText.text = "!";
		}
		else
		{
			tauntText.text = "";
		}
    }
}
