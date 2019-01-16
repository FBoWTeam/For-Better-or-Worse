using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Camera))]
public class CameraMovement : MonoBehaviour
{

	public Transform Player1;
	public Transform Player2;

	public Vector3 offset;
	public float smoothTime = 0.5f;
	private Vector3 velocity;

	public float minZoom = 25f;
	public float maxZoom = 10f;
	public float zoomLimiter = 45f;

	private Camera cam;

	// Use this for initialization
	void Start()
	{
		cam = GetComponent<Camera>();
	}

	// Update is called once per frame
	void LateUpdate()
	{
		Move();
		Zoom();
	}

	void Zoom()
	{
		float distancePlayer = Vector3.Distance(Player1.transform.position, Player2.transform.position);
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
		return (Player1.transform.position + Player2.transform.position) / 2;
	}

}
