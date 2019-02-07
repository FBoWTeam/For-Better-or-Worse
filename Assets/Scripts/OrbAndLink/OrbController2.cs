using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbController2 : MonoBehaviour
{
	public float speed;

    [HideInInspector]
	public bool ascending;
	float progression;
	float step;

	void Start()
	{
		ascending = true;
		progression = 0.5f;
		transform.position = BezierCurve.CalculateCubicBezierPoint(progression);
	}

	void FixedUpdate()
	{
		step = (speed / BezierCurve.GetPlayersDistance()) * Time.deltaTime;
		progression = ascending ? progression + step : progression - step;
		transform.position = BezierCurve.CalculateCubicBezierPoint(progression);

		if (progression >= 1.0f || progression <= 0.0f)
			ascending = !ascending;
	}
}
