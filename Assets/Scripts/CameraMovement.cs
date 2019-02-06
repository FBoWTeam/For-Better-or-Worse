using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class CameraMovement : MonoBehaviour
{
	public Transform player1, player2;

	public Vector3 offset;
	public float smoothTime = 0.5f;
	Vector3 velocity;

	public float minZoom = 25f;
	public float maxZoom = 10f;
	public float zoomLimiter = 45f;

	Camera cam;
	
	void Awake()
	{
		cam = GetComponent<Camera>();
	}
	
	void LateUpdate()
	{
		Move();
		Zoom();
	}

	void Zoom()
	{
		float distancePlayer = Vector3.Distance(player1.transform.position, player2.transform.position);
		float newZoom = Mathf.Lerp(maxZoom, minZoom, distancePlayer / zoomLimiter);
		cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, newZoom, Time.deltaTime);
	}

	void Move()
	{
		Vector3 centerPoint = GetCenterPoint();
		transform.position = Vector3.SmoothDamp(transform.position, centerPoint + offset, ref velocity, smoothTime);
		transform.LookAt(GetCenterPoint());
		transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, 0.0f, transform.localEulerAngles.z);
	}

	Vector3 GetCenterPoint()
	{
		return (player1.transform.position + player2.transform.position) / 2;
	}

}
