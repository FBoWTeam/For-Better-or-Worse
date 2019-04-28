using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableCameraFollow : MonoBehaviour
{
	public GameObject cameraFollow;

	private void OnEnable()
	{
		cameraFollow.GetComponent<CameraFollow>().enabled = false;
	}

	private void OnDisable()
	{
		cameraFollow.GetComponent<CameraFollow>().enabled = true;
	}
}
