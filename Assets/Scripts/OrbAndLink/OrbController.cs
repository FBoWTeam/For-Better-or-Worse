using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbController : MonoBehaviour
{
	public float speed;

	Vector3 oldPos;

	bool toPlayer1;
	float progression;
	float step;

	void Start()
	{
		oldPos = transform.position;
		toPlayer1 = true;
		progression = 0.5f;
		transform.position = BezierCurve.CalculateCubicBezierPoint(progression);
	}

	void FixedUpdate()
	{
		Debug.Log(Vector3.Distance(oldPos, transform.position));
		oldPos = transform.position;

		step = (speed / BezierCurve.GetPlayersDistance()) * Time.deltaTime;
		progression = toPlayer1 ? progression + step : progression - step;
		transform.position = BezierCurve.CalculateCubicBezierPoint(progression);

		if (progression >= 1.0f || progression <= 0.0f)
			toPlayer1 = !toPlayer1;
	}
}
