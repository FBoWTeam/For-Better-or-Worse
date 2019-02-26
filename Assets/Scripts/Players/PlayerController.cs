﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    public bool player1;
    public float speed;

    [HideInInspector]
    public float initialSpeed;

    Rigidbody rb;
    public Vector3 direction;

    public GameManager.PowerType powerSlot1;
    public GameManager.PowerType powerSlot2;
    public GameManager.PowerType powerSlot3;
    public GameManager.PowerType powerSlot4;


    OrbHitter orbHitter;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        orbHitter = gameObject.GetComponent<OrbHitter>();
        initialSpeed = speed;
    }

    void Update()
    {
        Move();
		CheckTaunt();
        GetCurrentPower();
    }

    /// <summary>
    /// manage the player movement and dash (keyboard and controller)
    /// Player 1 : ZQSD + Space ; Player2 : OKLM + I
    /// </summary>
	public void Move()
    {
		direction = player1 ? new Vector3(Input.GetAxis("HorizontalP1"), 0.0f, Input.GetAxis("VerticalP1")) : new Vector3(Input.GetAxis("HorizontalP2"), 0.0f, Input.GetAxis("VerticalP2"));

		direction = (direction.x * Camera.main.transform.right + direction.z * Camera.main.transform.forward);

        Vector3 velocity = direction * speed * Time.deltaTime;

        rb.MovePosition(transform.position + velocity);
    }



	void CheckTaunt()
	{
		if ( (player1 && (Input.GetKeyDown(KeyCode.Joystick1Button4) || Input.GetKeyDown(KeyCode.Space))) || (!player1 && (Input.GetKeyDown(KeyCode.Joystick2Button4) || Input.GetKeyDown(KeyCode.Keypad0))))
		{
			StartCoroutine(TauntCoroutine());
		}
	}

	IEnumerator TauntCoroutine()
	{
		yield return new WaitForEndOfFrame();

		if (player1)
			GameManager.gameManager.player1HasTaunt = true;
		else
			GameManager.gameManager.player2HasTaunt = true;

		yield return new WaitForEndOfFrame();

		if (player1)
			GameManager.gameManager.player1HasTaunt = false;
		else
			GameManager.gameManager.player2HasTaunt = false;
	}

    /// <summary>
    /// gets the current power that is going to be apllied on the orb by checking the input
    /// the power to apply on the orb when the player hits the orb
    /// </summary>
    public void GetCurrentPower()
    {
        bool power1 = player1 ? Input.GetKeyDown(KeyCode.Joystick1Button0) : Input.GetKeyDown(KeyCode.Joystick2Button0);
        bool power2 = player1 ? Input.GetKeyDown(KeyCode.Joystick1Button1) : Input.GetKeyDown(KeyCode.Joystick2Button1);
        bool power3 = player1 ? Input.GetKeyDown(KeyCode.Joystick1Button2) : Input.GetKeyDown(KeyCode.Joystick2Button2);
        bool power4 = player1 ? Input.GetKeyDown(KeyCode.Joystick1Button3) : Input.GetKeyDown(KeyCode.Joystick2Button3);

        if (power1)
        {
            orbHitter.powerToApply = powerSlot1;
        }
        if (power2)
        {
            orbHitter.powerToApply = powerSlot2;
        }
        if (power3)
        {
            orbHitter.powerToApply = powerSlot3;
        }
        if (power4)
        {
            orbHitter.powerToApply = powerSlot3;
        }
    }
}
