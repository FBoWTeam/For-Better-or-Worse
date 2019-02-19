using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    public bool player1;
    public float speed;

    Rigidbody rb;
    public Vector3 direction;

    GameManager.PowerType powerSlot1;
    GameManager.PowerType powerSlot2;
    GameManager.PowerType powerSlot3;
    GameManager.PowerType powerSlot4;


    OrbHitter orbHitter;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        orbHitter = gameObject.GetComponent<OrbHitter>();
    }

    void Update()
    {
        Move();
        getCurrentPower();
    }

    /// <summary>
    /// manage the player movement and dash (keyboard and controller)
    /// Player 1 : ZQSD + Space ; Player2 : OKLM + I
    /// </summary>
	public void Move()
    {
        if (player1)
        {
            direction = new Vector3(Input.GetAxis("HorizontalP1"), 0.0f, Input.GetAxis("VerticalP1"));
        }
        else
        {
            direction = new Vector3(Input.GetAxis("HorizontalP2"), 0.0f, Input.GetAxis("VerticalP2"));
        }

        direction = (direction.x * Camera.main.transform.right + direction.z * Camera.main.transform.forward);

        Vector3 velocity = direction * speed * Time.deltaTime;

        rb.MovePosition(transform.position + velocity);
    }

    /// <summary>
    /// gets the current power that is going to be apllied on the orb by checking the input
    /// the power to apply on the orb when the player hits the orb
    /// </summary>
    public void getCurrentPower()
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
