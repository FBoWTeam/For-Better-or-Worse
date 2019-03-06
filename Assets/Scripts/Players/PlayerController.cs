using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
	[Header("[Main Params]")]
    public bool player1;
    public float speed;
    Rigidbody rb;
	[HideInInspector]
    public Vector3 direction;
	public bool invincible;
	public float invicibilityDuration;
	public int blinkNb;

	[Header("[Power Slots]")]
    public GameManager.PowerType powerSlot1;
    public GameManager.PowerType powerSlot2;
    public GameManager.PowerType powerSlot3;
    public GameManager.PowerType powerSlot4;
    public bool oldestSlotIs3;
    
   
    bool canTaunt = true;
    OrbHitter orbHitter;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        orbHitter = gameObject.GetComponent<OrbHitter>();
        oldestSlotIs3 = true;
    }

    private void Start()
    {
        //Update UI (for development)
        GameManager.gameManager.UIManager.UpdatePowerSlot(1, player1, powerSlot1);
        GameManager.gameManager.UIManager.UpdatePowerSlot(2, player1, powerSlot2);
        GameManager.gameManager.UIManager.UpdatePowerSlot(3, player1, powerSlot3);
        GameManager.gameManager.UIManager.UpdatePowerSlot(4, player1, powerSlot4);
    }

    void Update()
    {
		if (!GameManager.gameManager.isPaused)
		{
			Move();
			CheckTaunt();
			GetCurrentPower();
		}
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

		checkDistance(ref velocity);

		rb.MovePosition(transform.position + velocity);
        transform.LookAt(transform.position + direction);
        transform.localEulerAngles = new Vector3(0.0f, transform.localEulerAngles.y, 0.0f);
    }

	void checkDistance(ref Vector3 velocity)
	{
		GameObject otherPlayer;
		if (player1)
			otherPlayer = GameManager.gameManager.player2;
		else
			otherPlayer = GameManager.gameManager.player1;

		if (Vector3.Distance(transform.position + velocity, otherPlayer.transform.position) > GameManager.gameManager.maxDistance)
		{
			Vector3 toPlayer = ((transform.position + velocity) - otherPlayer.transform.position).normalized;

			Vector3 fixedPos = otherPlayer.transform.position + toPlayer * GameManager.gameManager.maxDistance;

			velocity = fixedPos - transform.position;
		}
		if (Vector3.Distance(transform.position + velocity, otherPlayer.transform.position) < GameManager.gameManager.minDistance)
		{
			Vector3 toPlayer = ((transform.position + velocity) - otherPlayer.transform.position).normalized;

			Vector3 fixedPos = otherPlayer.transform.position + toPlayer * GameManager.gameManager.minDistance;

			velocity = fixedPos - transform.position;
		}
	}

    void CheckTaunt()
    {
        if (((player1 && (Input.GetKeyDown(KeyCode.Joystick1Button4) || Input.GetKeyDown(KeyCode.Space))) || (!player1 && (Input.GetKeyDown(KeyCode.Joystick2Button4) || Input.GetKeyDown(KeyCode.Keypad0)))) && canTaunt)
        {
            StartCoroutine(TauntCoroutine());
            StartCoroutine(TauntCoolDown(GameManager.gameManager.tauntCooldown));
        }
    }

    IEnumerator TauntCoolDown(float cd) {
        canTaunt = false;
        yield return new WaitForSeconds(cd);
        canTaunt = true;
    }

    IEnumerator TauntCoroutine()
    {
        yield return new WaitForEndOfFrame();
        if (player1) {
            GameManager.gameManager.player1HasTaunt = true;
           
        } else {
            GameManager.gameManager.player2HasTaunt = true;

        }

        yield return new WaitForEndOfFrame();

        if (player1) {
            GameManager.gameManager.player1HasTaunt = false;
        } else {
            GameManager.gameManager.player2HasTaunt = false;
        }
    }
           

    /// <summary>
    /// gets the current power that is going to be apllied on the orb by checking the input
    /// the power to apply on the orb when the player hits the orb
    /// </summary>
    public void GetCurrentPower()
    {
        bool power1 = player1 ? Input.GetKeyDown(KeyCode.Joystick1Button3) : Input.GetKeyDown(KeyCode.Joystick2Button3);     
        bool power2 = player1 ? Input.GetKeyDown(KeyCode.Joystick1Button2) : Input.GetKeyDown(KeyCode.Joystick2Button2);
        bool power3 = player1 ? Input.GetKeyDown(KeyCode.Joystick1Button1) : Input.GetKeyDown(KeyCode.Joystick2Button1);
        bool power4 = player1 ? Input.GetKeyDown(KeyCode.Joystick1Button0) : Input.GetKeyDown(KeyCode.Joystick2Button0);

        if (power1 && powerSlot1 != GameManager.PowerType.None)
        {           
            orbHitter.powerToApply = powerSlot1;
        }
        if (power2 && powerSlot2 != GameManager.PowerType.None)
        {
            orbHitter.powerToApply = powerSlot2;
        }
        if (power3 && powerSlot3 != GameManager.PowerType.None)
        {
            orbHitter.powerToApply = powerSlot3;
        }
        if (power4 && powerSlot4 != GameManager.PowerType.None)
        {
            orbHitter.powerToApply = powerSlot4;
        }
    }


    /// <summary>
    /// Gives a power dropped by an enemy, and place it on the good slot
    /// </summary>
    public void AttributePower(GameManager.PowerType newPower, bool isFixedPower)
    {
		if (isFixedPower)
		{
			if (powerSlot1 == GameManager.PowerType.None)
			{
				GameManager.gameManager.player1.GetComponent<PlayerController>().powerSlot1 = newPower;
				GameManager.gameManager.player2.GetComponent<PlayerController>().powerSlot1 = newPower;
				GameManager.gameManager.UIManager.UpdatePowerSlot(1, true, newPower);
				GameManager.gameManager.UIManager.UpdatePowerSlot(1, false, newPower);
			}
			else if (powerSlot2 == GameManager.PowerType.None)
			{
				GameManager.gameManager.player1.GetComponent<PlayerController>().powerSlot2 = newPower;
				GameManager.gameManager.player2.GetComponent<PlayerController>().powerSlot2 = newPower;
				GameManager.gameManager.UIManager.UpdatePowerSlot(2, true, newPower);
				GameManager.gameManager.UIManager.UpdatePowerSlot(2, false, newPower);
			}
		}
		else
		{
			if (powerSlot3 != newPower && powerSlot4 != newPower)
			{
				if (powerSlot3 == GameManager.PowerType.None)
				{
					powerSlot3 = newPower;
					GameManager.gameManager.UIManager.UpdatePowerSlot(3, player1, powerSlot3);
				}
				else if (powerSlot4 == GameManager.PowerType.None)
				{
					powerSlot4 = newPower;
					GameManager.gameManager.UIManager.UpdatePowerSlot(4, player1, powerSlot4);
				}
				else
				{

					if (oldestSlotIs3)
					{
						powerSlot3 = newPower;
						GameManager.gameManager.UIManager.UpdatePowerSlot(3, player1, powerSlot3);
					}
					else
					{
						powerSlot4 = newPower;
						GameManager.gameManager.UIManager.UpdatePowerSlot(4, player1, powerSlot4);
					}
					oldestSlotIs3 = !oldestSlotIs3;
				}
			}
		}
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, gameObject.GetComponent<OrbHitter>().hitZone * 2);
    }

	public IEnumerator InvincibilityCoroutine()
	{
		MeshRenderer renderer = GetComponent<MeshRenderer>();
		float blinkTime = invicibilityDuration / blinkNb;

		invincible = true;

		for (int i = 0; i < blinkNb; i++)
		{
			renderer.enabled = false;
			yield return new WaitForSeconds(blinkTime/2.0f);
			renderer.enabled = true;
			yield return new WaitForSeconds(blinkTime/2.0f);
		}

		invincible = false;
	}
}
