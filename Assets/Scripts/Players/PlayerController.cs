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

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

	void Update()
    {
        Move();
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

        Vector3 velocity = direction * speed * Time.deltaTime;

		rb.MovePosition(transform.position + velocity);

	}
}
