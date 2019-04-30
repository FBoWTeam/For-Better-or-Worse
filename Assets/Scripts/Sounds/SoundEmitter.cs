using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEmmiter : MonoBehaviour
{
	[HideInInspector]
	public AudioClip soundToPlay;
	public float volumeCoefficient;
	public float maxDistance;

	public bool continuousSound;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
	{

    }

	public void updateVolumeCoefficient()
	{
		float distance = getNearestPlayerDistance();
		volumeCoefficient = distance / maxDistance;
		Mathf.Clamp01(volumeCoefficient);
	}

	public float getNearestPlayerDistance()
	{
		float Player1Dist = Vector3.Distance(transform.position, GameManager.gameManager.player1.transform.position);
		float Player2Dist = Vector3.Distance(transform.position, GameManager.gameManager.player2.transform.position);

		if(Player1Dist <= Player2Dist)
		{
			return Player1Dist;
		}
		else
		{
			return Player2Dist;
		}
	}
}
