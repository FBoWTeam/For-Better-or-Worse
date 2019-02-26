using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbHitter : MonoBehaviour
{
	OrbController orbController;

    [Header("[Parameters]")]
    [Tooltip("represents the orb hitting range")]
    public float hitZone;
    public float accelerationFactor;
    public float maxAmortizeTime;

    bool inRange;

    public float hitCooldown;
    float hitTimer;

	public GameManager.PowerType powerToApply;

	void Start()
    {
		orbController = GameManager.gameManager.orb.GetComponent<OrbController>();
		inRange = false;
    }

    // Update is called once per frame
    void Update()
    {
        OrbHit();
    }

    /// <summary>
    /// function that let the players hit and amortize the ball
    /// </summary>
    void OrbHit()
    {
        if (hitTimer > 0.0f)
        {
            hitTimer -= Time.deltaTime;
        }
        CheckRange();

		if (GetComponent<PlayerController>().player1)
        {
            if (inRange)
            {
                if (!orbController.toPlayer2)
                {
                    if (Input.GetAxisRaw("OrbHitterP1") != 0 && hitTimer <= 0.0f)
                    {
                        hitTimer = hitCooldown;
                        orbController.toPlayer2 = !orbController.toPlayer2;
                        orbController.speed += accelerationFactor;
                        CheckPowerActivation();
						GameManager.gameManager.orb.GetComponent<PowerController>().CheckPowerAttribution("hit", true);
                    }
                    if (Input.GetAxisRaw("OrbAmortizerP1") != 0 && !orbController.amortized)
                    {
                        StartCoroutine(AmortizeCoroutine());
						GameManager.gameManager.orb.GetComponent<PowerController>().CheckPowerAttribution("amortize", true);
					}
                    else if (Input.GetAxisRaw("OrbAmortizerP1") == 0 && orbController.amortized)
                    {
                        StopCoroutine(AmortizeCoroutine());
                        orbController.toPlayer2 = !orbController.toPlayer2;
                        orbController.amortized = false;
                        orbController.speed = orbController.minSpeed;
                    }
                }
            }
        }
        else
        {
            if (inRange)
            {
                if (orbController.toPlayer2)
                {
                    if (Input.GetAxisRaw("OrbHitterP2") != 0)
                    {
                        orbController.toPlayer2 = !orbController.toPlayer2;
                        orbController.speed += accelerationFactor;
                        CheckPowerActivation();
						GameManager.gameManager.orb.GetComponent<PowerController>().CheckPowerAttribution("hit", false);
					}
                    if (Input.GetAxisRaw("OrbAmortizerP2") != 0 && !orbController.amortized)
                    {
                        StartCoroutine(AmortizeCoroutine());
						GameManager.gameManager.orb.GetComponent<PowerController>().CheckPowerAttribution("amortize", false);
					}
                    else if (Input.GetAxisRaw("OrbAmortizerP2") == 0 && orbController.amortized)
                    {
                        StopCoroutine(AmortizeCoroutine());
                        orbController.toPlayer2 = !orbController.toPlayer2;
                        orbController.amortized = false;
                        orbController.speed = orbController.minSpeed;
                    }
                }
            }
        }
    }

    /// <summary>
    /// function that check if the orb is close enough to let the player to hit the ball
    /// the range is defined by the default hitzone AND the radius of the orb (so that we can hit the orb on it's border no matter it's size)
    /// orb.transform.localScale.x / 2 is the radius of the orb
    /// </summary>
    void CheckRange()
    {
        if (Vector3.Distance(transform.position, orbController.transform.position) < hitZone + orbController.transform.localScale.x / 2)
        {
            inRange = true;
        }
        else
        {
            inRange = false;
        }
    }

    /// <summary>
    /// apply the power on the orb if not None
    /// </summary>
    void CheckPowerActivation()
    {
		if (powerToApply != GameManager.PowerType.None)
        {
            orbController.GetComponent<PowerController>().ActivatePower(powerToApply);
            
            powerToApply = GameManager.PowerType.None;
        }

        //TEST ZONE si l'orb a le pouvoir shield
        if (orbController.GetComponent<PowerController>().behavioralPower == GameManager.PowerType.Shield && orbController.GetComponent<PowerController>().currentShieldStack > 0)
        {
            if (gameObject.GetComponent<PlayerController>().player1)
            {
                GameManager.gameManager.shieldP1 = orbController.GetComponent<PowerController>().shieldAmount;
                orbController.GetComponent<PowerController>().currentShieldStack--;
            }
            else if (!gameObject.GetComponent<PlayerController>().player1)
            {
                GameManager.gameManager.shieldP2 = orbController.GetComponent<PowerController>().shieldAmount;
                orbController.GetComponent<PowerController>().currentShieldStack--;
            }
        }
		//FIN TEST ZONE
	}

	/// <summary>
	/// coroutine that manage the amortize of the orb
	/// </summary>
	/// <returns></returns>
	IEnumerator AmortizeCoroutine()
    {
        orbController.speed = 0.0f;
        orbController.amortized = true;
        yield return new WaitForSeconds(maxAmortizeTime);
        if (orbController.amortized)
        {
            orbController.toPlayer2 = !orbController.toPlayer2;
            orbController.amortized = false;
            orbController.speed = orbController.minSpeed;
        }
    }
}
