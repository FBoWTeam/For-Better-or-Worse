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

    bool inRange;

    public float hitCooldown;
    float hitTimer;


    public GameManager.PowerType powerToApply;

    void Start()
    {
        orb = GameObject.Find("Orb").GetComponent<OrbController>();
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
                if (!orb.GetComponent<OrbController>().toPlayer2)
                {
                    if (Input.GetAxisRaw("OrbHitterP1") != 0 && hitTimer <= 0.0f)
                    {
                        hitTimer = hitCooldown;
                        orb.toPlayer2 = !orb.toPlayer2;
                        orb.speed += accelerationFactor;
                        CheckPowerActivation();
                    }
                    if (Input.GetAxisRaw("OrbAmortizerP1") != 0 && !orb.amortized)
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
            if (inRange)
            {
                if (orb.GetComponent<OrbController>().toPlayer2)
                {
                    if (Input.GetAxisRaw("OrbHitterP2") != 0)
                    {
                        orb.toPlayer2 = !orb.toPlayer2;
                        orb.speed += accelerationFactor;
                        CheckPowerActivation();
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
    /// the range is defined by the default hitzone AND the radius of the orb (so that we can hit the orb on it's border no matter it's size)
    /// orb.transform.localScale.x / 2 is the radius of the orb
    /// </summary>
    void CheckRange()
    {
        if (Vector3.Distance(transform.position, orb.transform.position) < hitZone + orb.transform.localScale.x / 2)
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
            orb.GetComponent<PowerController>().ActivatePower(powerToApply);
            
            powerToApply = GameManager.PowerType.None;
        }

        //TEST ZONE si l'orb a le pouvoir shield
        if (orb.GetComponent<PowerController>().behavioralPower == GameManager.PowerType.Shield && orb.GetComponent<PowerController>().currentShieldStack > 0)
        {
            if (gameObject.GetComponent<PlayerController>().player1)
            {
                GameManager.gameManager.shieldP1 = orb.GetComponent<PowerController>().shieldAmount;
                orb.GetComponent<PowerController>().currentShieldStack--;
            }
            else if (!gameObject.GetComponent<PlayerController>().player1)
            {
                GameManager.gameManager.shieldP2 = orb.GetComponent<PowerController>().shieldAmount;
                orb.GetComponent<PowerController>().currentShieldStack--;
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
        orb.speed = 0.0f;
        orb.amortized = true;
        yield return new WaitForSeconds(maxAmortizeTime);
        if (orb.amortized)
        {
            orb.toPlayer2 = !orb.toPlayer2;
            orb.amortized = false;
            orb.speed = orb.minSpeed;
        }
    }
}
