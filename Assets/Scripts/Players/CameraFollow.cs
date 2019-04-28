using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
	public Vector3 angle;
	public float smoothTime;
	Vector3 velocity;

	public float minDist;
	public float maxDist;

	Camera cam;

	private void Awake()
	{
		cam = transform.GetChild(0).GetComponent<Camera>();
	}

	// Update is called once per frame
	void FixedUpdate()
    {
		if(!GameManager.gameManager.isPaused)
		{
			Move();
			Zoom();
		}
    }

	/// <summary>
	/// move the camera towards the center of mass of player 1 and 2 + an offset 
	/// </summary>
	void Move()
	{
		Vector3 centerPoint = GetCenterPoint();
		transform.position = Vector3.SmoothDamp(transform.position, centerPoint, ref velocity, smoothTime);
		transform.localEulerAngles = angle;
	}

	/// <summary>
	/// zoom the cam on the players based on the distance between them
	/// </summary>
	void Zoom()
	{
		float distancePlayer = Vector3.Distance(GameManager.gameManager.player1.transform.position, GameManager.gameManager.player2.transform.position);
		float newZoom = (((distancePlayer - GameManager.gameManager.minDistance) * (maxDist - minDist)) / (GameManager.gameManager.maxDistance - GameManager.gameManager.minDistance)) + minDist;
		transform.position = Vector3.SmoothDamp(cam.transform.position, transform.position - cam.transform.forward * newZoom, ref velocity, smoothTime);
	}

	/// <summary>
	/// return the center of mass of player 1 and 2
	/// </summary>
	/// <returns></returns>
	Vector3 GetCenterPoint()
	{
		//return GameManager.gameManager.player1.transform.position;
		return (GameManager.gameManager.player1.transform.position + GameManager.gameManager.player2.transform.position) / 2;
	}
}
