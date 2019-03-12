using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbHitter : MonoBehaviour
{
    OrbController orbController;

    [Tooltip("represents the orb hitting range")]
    public float hitZone;
	bool inRange;
	public GameManager.PowerType powerToApply;

	[Header("[Hit]")]
	public bool hitting;
	public float hitDuration;
    public float hitCooldown;
    float hitTimer;
	public float accelerationFactor;

	[Header("[Amortize]")]
	public bool amortizing;
	public float amortizeDuration;

    void Start()
    {
        orbController = GameManager.gameManager.orb.GetComponent<OrbController>();
        inRange = false;
    }

    // Update is called once per frame
    void Update()
    {
		if(!GameManager.gameManager.isPaused)
		{
			OrbHit();
		}
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

		UpdateInputs();

		if (inRange)
        {            
            bool player1 = GetComponent<PlayerController>().player1;
			if (hitting && ((player1 && !orbController.toPlayer2) || (!player1 && orbController.toPlayer2)))
            {
				StopCoroutine(HitCoroutine());
				hitting = false;
				hitTimer = hitCooldown;
				orbController.toPlayer2 = !orbController.toPlayer2;
                orbController.speed = accelerationFactor * orbController.combo + orbController.minSpeed;
                if(orbController.hasHitEnemy)
                {
                    orbController.combo++;
                    orbController.hasHitEnemy = false;
                }
                //Update combo UI
                GameManager.gameManager.UIManager.UpdateCombo(orbController.combo);               
                CheckPowerActivation();
                GameManager.gameManager.orb.GetComponent<PowerController>().CheckPowerAttribution("hit", player1);
            }
            if (amortizing && !orbController.amortized)
            {
                StartCoroutine(AmortizeCoroutine());
                GameManager.gameManager.orb.GetComponent<PowerController>().CheckPowerAttribution("amortize", player1);
            }
            else if (!amortizing && orbController.amortized)
            {
                StopCoroutine(AmortizeCoroutine());
                orbController.toPlayer2 = !orbController.toPlayer2;
                orbController.amortized = false;
                orbController.speed = orbController.minSpeed;
            }
        }
    }

	public void UpdateInputs()
	{
		bool player1 = GetComponent<PlayerController>().player1;
		if (((Input.GetAxisRaw("OrbHitterP1") != 0 && player1) || (Input.GetAxisRaw("OrbHitterP2") != 0 && !player1)) && hitTimer <= 0.0f && !hitting)
		{
			StartCoroutine(HitCoroutine());
		}

		if ((Input.GetAxisRaw("OrbAmortizerP1") != 0 && player1 && !orbController.toPlayer2) || (Input.GetAxisRaw("OrbAmortizerP2") != 0 && !player1 && orbController.toPlayer2))
		{
			amortizing = true;
		}
		else
		{
			amortizing = false;
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
			string mode = GetComponent<PlayerController>().player1 ? "player1" : "player2";
			orbController.GetComponent<PowerController>().ActivatePower(powerToApply, mode);
            powerToApply = GameManager.PowerType.None;
        }
        
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
        else if (orbController.GetComponent<PowerController>().behavioralPower == GameManager.PowerType.Shield && orbController.GetComponent<PowerController>().currentShieldStack <= 0)
        {
            orbController.GetComponent<PowerController>().DeactivatePower(GameManager.PowerType.Shield);
        }
    }

	/// <summary>
	/// extend the time to hit the orb from a frame to a range of frame
	/// </summary>
	/// <returns></returns>
	IEnumerator HitCoroutine()
	{
		hitting = true;
		GetComponent<Animation>().Play();
		yield return new WaitForSeconds(hitDuration);
		hitting = false;
		hitTimer = hitCooldown;
	}

	/// <summary>
	/// coroutine that manage the amortize of the orb
	/// </summary>
	/// <returns></returns>
	IEnumerator AmortizeCoroutine()
    {
        orbController.speed = 0.0f;
        orbController.amortized = true;
        orbController.speed = orbController.minSpeed;
        orbController.combo = 0;
        //reset combo ui
        GameManager.gameManager.UIManager.UpdateCombo(orbController.combo);
        yield return new WaitForSeconds(amortizeDuration);
        if (orbController.amortized)
        {
            orbController.toPlayer2 = !orbController.toPlayer2;
            orbController.amortized = false;
        }
    }
}
