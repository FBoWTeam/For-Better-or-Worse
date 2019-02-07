using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class BezierCurve : MonoBehaviour
{
	LineRenderer lineRenderer;

	static Transform player1, player2;
	static Transform deformPoint1, deformPointMid, deformPoint2;

	static private int numberPoints = 20;
	private Vector3[] positions = new Vector3[numberPoints];

	void Awake()
	{
		player1 = GameObject.Find("Player1").transform;
		player2 = GameObject.Find("Player2").transform;
		deformPoint1 = transform.GetChild(0).GetChild(0);
		deformPointMid = transform.GetChild(0).GetChild(1);
		deformPoint2 = transform.GetChild(0).GetChild(2);
	}

	void Start()
	{
		lineRenderer = GetComponent<LineRenderer>();
		lineRenderer.positionCount = numberPoints;
		DrawCubicCurve();
		transform.position = player1.position + (player2.position - player1.position) / 2;
	}

	void FixedUpdate()
	{
		DrawCubicCurve();
		transform.position = player1.position + (player2.position - player1.position) / 2;
	}

	/// <summary>
	///	Set the positions of the points in the LineRenderer
	/// </summary>
	private void DrawCubicCurve()
	{
		for (int i = 0; i < positions.Length; i++)
		{
			float t = i / (float)(numberPoints - 1);
			positions[i] = CalculateCubicBezierPoint(t);
		}
		lineRenderer.SetPositions(positions);
	}

	/// <summary>
	///	Calculate a point on the cubic bezier curve based on the param "t" between 0 (begining of the curve) and 1 (end of the curve)
	/// </summary>
	/// <param name="t"></param>
	/// <returns></returns>
	public static Vector3 CalculateCubicBezierPoint(float t)
	{
		//return Mathf.Pow((1 - t), 3) * player1.position + 3 * Mathf.Pow((1 - t), 2) * t * deformPoint1.position + 3 * (1 - t) * t * t * deformPoint2.position + Mathf.Pow(t, 3) * player2.position;
		return Mathf.Pow((1 - t), 4) * player1.position + 4 * t * Mathf.Pow((1 - t), 3) * deformPoint1.position + 6 * Mathf.Pow(t, 2) * Mathf.Pow((1 - t), 2) * deformPointMid.position + 4 * Mathf.Pow(t, 3) * (1 - t) * deformPoint2.position + Mathf.Pow(t, 4) * player2.position;
	}

	/// <summary>
	///	return the distance between player 1 and 2
	/// </summary>
	/// <returns></returns>
	public static float GetPlayersDistance()
	{
		return Vector3.Distance(player1.position, player2.position);
	}

}
