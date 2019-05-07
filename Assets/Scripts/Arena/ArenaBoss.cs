using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArenaBoss : MonoBehaviour
{
    void Start()
    {
        if(ScoreManager.scoreManager.gameMode == ScoreManager.GameMode.Arena)
		{
			GameManager.gameManager.boss = false;
			GameManager.gameManager.arena = true;
		}
		Destroy(this.gameObject);
    }
}
