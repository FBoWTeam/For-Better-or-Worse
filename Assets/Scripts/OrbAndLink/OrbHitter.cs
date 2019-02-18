using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbHitter : MonoBehaviour
{
    OrbController orb;

    [Header("[Parameters]")]
    [Tooltip("represents the orb hitting range")]
    public float hitZone;
    public float accelerationFactor;
	public float maxAmortizeTime;

    bool canHit;

    void Start()
    {
        orb = GameObject.Find("Orb").GetComponent<OrbController>();
        canHit = false;
    }

    // Update is called once per frame
    void Update()
    {
        OrbHit();
    }

    /// <summary>
    /// function that let the players hit the ball
    /// </summary>
    void OrbHit()
    {
        checkHit();
        if (GetComponent<PlayerController>().player1)
        {
            if (canHit)
            {
                if (!orb.GetComponent<OrbController>().toPlayer2)
                {
                    if (Input.GetAxisRaw("OrbHitterP1") != 0)
                    {
                        orb.toPlayer2 = !orb.toPlayer2;
                        orb.speed += accelerationFactor;
                    }
					if(Input.GetAxisRaw("OrbAmortizerP1") != 0 && !orb.amortized)
					{
						StartCoroutine(AmortizeCoroutine());
					}
					else if (Input.GetAxisRaw("OrbAmortizerP1") == 0 && orb.amortized)
					{
						StopCoroutine(AmortizeCoroutine());
						orb.toPlayer2 = !orb.toPlayer2;
						orb.amortized = false;
						orb.speed = orb.minSpeed;
					}
				}
            }
        }
        else
        {
            if (canHit)
            {
                if (orb.GetComponent<OrbController>().toPlayer2)
                {
                    if (Input.GetAxisRaw("OrbHitterP2") != 0)
					{
						orb.toPlayer2 = !orb.toPlayer2;
						orb.speed += accelerationFactor;
					}
					if (Input.GetAxisRaw("OrbAmortizerP2") != 0 && !orb.amortized)
					{
						StartCoroutine(AmortizeCoroutine());
					}
					else if (Input.GetAxisRaw("OrbAmortizerP2") == 0 && orb.amortized)
					{
						StopCoroutine(AmortizeCoroutine());
						orb.toPlayer2 = !orb.toPlayer2;
						orb.amortized = false;
						orb.speed = orb.minSpeed;
					}
				}
            }
        }
    }

    /// <summary>
    /// function that check if the orb is close enough to let the player to hit the ball
    /// </summary>
    void checkHit()
    {
        if (Vector3.Distance(transform.position, orb.transform.position) < hitZone)
        {
            canHit = true;
        }
        else
        {
            canHit = false;
        }
    }


	/// <summary>
	/// coroutine that manage the amortize of the orb
	/// </summary>
	/// <returns></returns>
	IEnumerator AmortizeCoroutine()
	{
		orb.speed = 0.0f;
		orb.amortized = true;
		yield return new WaitForSeconds(maxAmortizeTime);
		if(orb.amortized)
		{
			orb.toPlayer2 = !orb.toPlayer2;
			orb.amortized = false;
			orb.speed = orb.minSpeed;
		}
	}


}
