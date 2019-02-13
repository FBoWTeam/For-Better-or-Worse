﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinkDeformation : MonoBehaviour
{
	Transform deformPoint1, deformPointMid, deformPoint2;
	Transform minDeform, maxDeform;

	public float maxDeformHeight;

	(float deformInputP1, float deformInputP2) deformInputs;
	float deformAmountP1, deformAmountP2;
	public float smoothTime;

	public bool canBeDeformed;

	private void Awake()
	{
		deformPoint1 = transform.GetChild(0);
		deformPointMid = transform.GetChild(1);
		deformPoint2 = transform.GetChild(2);

		minDeform = transform.GetChild(3);
		maxDeform = transform.GetChild(4);

		canBeDeformed = true;
	}

	private void Start()
	{
		FixPosition();
	}

	void Update()
	{
		FixPosition();

		deformInputs = canBeDeformed ? GetDeformInputs() : deformInputs;

		deformAmountP1 = Mathf.Lerp(deformAmountP1, deformInputs.deformInputP1, smoothTime);
		deformAmountP2 = Mathf.Lerp(deformAmountP2, deformInputs.deformInputP2, smoothTime);

		float playersDistance = Vector3.Distance(GameManager.gameManager.player1.transform.position, GameManager.gameManager.player2.transform.position);

		deformPoint1.localPosition = new Vector3(deformAmountP1 * maxDeformHeight, 0.0f, (playersDistance / 4.0f));
		deformPoint2.localPosition = new Vector3(deformAmountP2 * maxDeformHeight, 0.0f, -(playersDistance / 4.0f));
		deformPointMid.localPosition = new Vector3(((deformPoint1.localPosition.x + deformPoint2.localPosition.x) / 2.0f), 0.0f, 0.0f);
	}

	/// <summary>
	///	Set the position of the gameobject between player 1 and 2 looking at player 1
	/// </summary>
	void FixPosition()
	{
		transform.position = GameManager.gameManager.player1.transform.position + (GameManager.gameManager.player2.transform.position - GameManager.gameManager.player1.transform.position) / 2;
		transform.LookAt(GameManager.gameManager.player1.transform.position);

		minDeform.localPosition = new Vector3(transform.localPosition.x - maxDeformHeight, transform.localPosition.y, transform.localPosition.z);
		maxDeform.localPosition = new Vector3(transform.localPosition.x + maxDeformHeight, transform.localPosition.y, transform.localPosition.z);
	}

	/// <summary>
	///	return a tuple of the deformation amounts ([-1; 1]) of player 1 and 2 based on their inputs
	/// </summary>
	/// <returns></returns>
	(float, float) GetDeformInputs()
	{
		Vector3 player1DeformInput = new Vector3(Input.GetAxis("DeformP1X"), 0.0f, Input.GetAxis("DeformP1Z"));
		Vector3 player2DeformInput = new Vector3(Input.GetAxis("DeformP2X"), 0.0f, Input.GetAxis("DeformP2Z"));

		Vector3 deformAxis = maxDeform.position - transform.position;
		deformAxis = deformAxis.normalized;

		return (Vector3.Dot(deformAxis, player1DeformInput), Vector3.Dot(deformAxis, player2DeformInput));
	}
}
