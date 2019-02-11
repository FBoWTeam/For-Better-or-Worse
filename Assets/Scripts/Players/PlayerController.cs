using System.Collections;
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
	[HideInInspector]
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
        if (!isDashing)
        {
            if (player1)
            {
                direction = new Vector3(Input.GetAxis("HorizontalP1"), 0.0f, Input.GetAxis("VerticalP1"));
                if (!isDashing)
                {
                    if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.Joystick1Button5))
                    {
                        StartCoroutine(DashCoroutine());
                    }
                }
            }
            else
            {
                direction = new Vector3(Input.GetAxis("HorizontalP2"), 0.0f, Input.GetAxis("VerticalP2"));
                if (!isDashing)
                {
                    if (Input.GetKeyDown(KeyCode.RightShift) || Input.GetKeyDown(KeyCode.Joystick1Button5))
                    {
                        StartCoroutine(DashCoroutine());
                    }
                }
            }
        }

        Vector3 velocity = direction * speed * Time.deltaTime;
        
        ApplyVelocity(velocity);

    }
    
    /// <summary>
    /// function that prevents the player from dashing through walls with a high velocity
    /// if the player is not dashing, the function simply apply the normal velocity
    /// </summary>
    /// <param name="velocity"></param>
    void ApplyVelocity(Vector3 velocity)
    {
        if (isDashing)
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, direction.normalized, out hit, velocity.magnitude, LayerMask.GetMask("DistanceLimiter", "Wall")))
            {
                transform.position = hit.point - (direction * GetComponent<CapsuleCollider>().radius);
            }
            else
            {
                rb.MovePosition(transform.position + velocity);
            }
        }
        else
        {
            rb.MovePosition(transform.position + velocity);
        }
    }


    /// <summary>
    /// co routine that handle the dash
    /// </summary>
    /// <returns></returns>
	IEnumerator DashCoroutine()
    {
        isDashing = true;
        speed *= dashSpeed;
        yield return new WaitForSeconds(dashTime);
        speed /= dashSpeed;
        isDashing = false;
    }
}
