using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinkDeformation : MonoBehaviour
{
	public Transform player1, player2;
	public Transform minDeform, maxDeform;
	public Transform deformPoint1, deformPoint2;

	void Update()
	{
		Vector3 p1 = player1.localPosition;
		p1.y = 1;
		Vector3 p2 = player2.localPosition;
		p2.y = 1;
		transform.position = p1 + (p2 - p1) / 2;
		transform.LookAt(p1);

		Vector3 player1DeformDir = new Vector3(Input.GetAxis("DeformP1X"), 0.0f, Input.GetAxis("DeformP1Z"));
		Vector3 player2DeformDir = new Vector3(Input.GetAxis("DeformP2X"), 0.0f, Input.GetAxis("DeformP2Z"));

		Vector3 player1DeformAxis = maxDeform.position - transform.position;
		player1DeformAxis = player1DeformAxis.normalized;
		Vector3 player2DeformAxis = maxDeform.position - transform.position;
		player2DeformAxis = player2DeformAxis.normalized;

		float player1DeformAmount = Vector3.Dot(player1DeformAxis, player1DeformDir);
		float player2DeformAmount = Vector3.Dot(player2DeformAxis, player2DeformDir);

		deformPoint1.position = Vector3.Lerp(minDeform.position, maxDeform.position, (player1DeformAmount + 1) / 2);
		deformPoint2.position = Vector3.Lerp(minDeform.position, maxDeform.position, (player2DeformAmount + 1) / 2);
	}
}
