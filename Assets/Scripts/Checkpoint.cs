using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
	public GameObject content;
	GameObject actualContent;

	bool checkpointActivated;
	
	void Awake()
    {
		actualContent = transform.GetChild(1).gameObject;
		checkpointActivated = false;
	}

	public void RespawnContent()
	{
		Destroy(actualContent);
		actualContent = Instantiate(content, this.transform);
	}

	private void OnTriggerEnter(Collider other)
	{
		if(other.CompareTag("Player") && !checkpointActivated)
		{
			GameManager.gameManager.actualCheckpoint = this;
			GameManager.gameManager.RecordPower();

			Debug.Log(GameManager.gameManager.respawnPowerRecord.player1ElementalPower);
			Debug.Log(GameManager.gameManager.respawnPowerRecord.player1BehaviouralPower);
			Debug.Log(GameManager.gameManager.respawnPowerRecord.player2ElementalPower);
			Debug.Log(GameManager.gameManager.respawnPowerRecord.player2BehaviouralPower);

			checkpointActivated = true;
		}
	}
}
