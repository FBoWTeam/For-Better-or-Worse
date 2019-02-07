using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
	public bool player1;
	public float speed;

	Rigidbody rb;

	void Awake()
	{
		rb = GetComponent<Rigidbody>();
	}

	void Update()
	{
		Move();
	}

	/// <summary>
	/// Move the player based on their inputs
	/// </summary>
	public void Move()
	{
		float x, z;
		if (player1)
		{
			x = Input.GetAxis("HorizontalP1");
			z = Input.GetAxis("VerticalP1");
		}
		else
		{
			x = Input.GetAxis("HorizontalP2");
			z = Input.GetAxis("VerticalP2");
		}

		Vector3 velocity = new Vector3(x, 0, z) * speed * Time.deltaTime;

		rb.MovePosition(transform.position + velocity);
	}


}
