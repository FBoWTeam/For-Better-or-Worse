﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
	public bool player1;
	public float speed;

    public float dashTime;
    public float dashSpeed;

    bool isDashing = false;
	Rigidbody rb;
    Vector3 direction;

    void Awake()
	{
		rb = GetComponent<Rigidbody>();
	}
	
	void Update()
	{
		Move();
	}

	public void Move()
	{
        if (!isDashing)
        {
            if (player1)
            {
               direction = new Vector3(Input.GetAxis("HorizontalP1"), 0.0f, Input.GetAxis("VerticalP1"));
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    StartCoroutine(DashCoroutine());
                }
            }
            else
            {
                direction = new Vector3(Input.GetAxis("HorizontalP2"), 0.0f, Input.GetAxis("VerticalP2"));
                if (Input.GetKeyDown(KeyCode.I))
                {
                    StartCoroutine(DashCoroutine());
                }
            }
        }

        Vector3 velocity = direction * speed * Time.deltaTime;
        rb.MovePosition(transform.position + velocity);
	}

    IEnumerator DashCoroutine()
    {
        isDashing = true;
        speed *= dashSpeed;
        yield return new WaitForSeconds(dashTime);
        speed /= dashSpeed;
        isDashing = false;
    }


}
