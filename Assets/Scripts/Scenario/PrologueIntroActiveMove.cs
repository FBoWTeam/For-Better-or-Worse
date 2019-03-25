using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrologueIntroActiveMove : MonoBehaviour
{
	private void OnEnable()
	{
		GameManager.gameManager.player1.GetComponent<PlayerController>().active = true;
		GameManager.gameManager.player2.GetComponent<PlayerController>().active = true;
		Destroy(this.gameObject);
	}
}
