using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
	[HideInInspector]
	public bool active = true;

	[Header("[Main Params]")]
    public bool player1;
    public float initialSpeed;
    private float speed;
    public bool isSlowed;
    public bool isFrozen;
    Rigidbody rb;
	[HideInInspector]
    public Vector3 direction;
	public bool invincible;
	public float invicibilityDuration;
	public int blinkNb;
    public bool isRoot;

    [Header("[Taunt]")]
    public int tauntRange;
    public float tauntCooldown;

    [Header("[Power Slots]")]
    public GameManager.PowerType elementalPowerSlot;
    public GameManager.PowerType behaviouralPowerSlot;


    public GameObject VFXTail;
    
    bool canTaunt = true;
    OrbHitter orbHitter;

	Animator animator;


    public GameObject selectedBehavioralFx;
    public GameObject selectedElementalFx;


    [HideInInspector]
    public Coroutine actualTauntCoroutine;

    [HideInInspector]
    public Coroutine actualBurnCoroutine;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        orbHitter = GetComponent<OrbHitter>();
		animator = GetComponent<Animator>();
    }

    private void Start()
    {
        //Update UI (for development)
        speed = initialSpeed;
        GameManager.gameManager.UIManager.UpdatePowerSlot(1, player1, elementalPowerSlot);
        GameManager.gameManager.UIManager.UpdatePowerSlot(2, player1, behaviouralPowerSlot);
    }

    void Update()
    {
		if (!GameManager.gameManager.isPaused && active)
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
        if (!isFrozen && !isRoot)
        {
            direction = player1 ? new Vector3(Input.GetAxis("HorizontalP1"), 0.0f, Input.GetAxis("VerticalP1")) : new Vector3(Input.GetAxis("HorizontalP2"), 0.0f, Input.GetAxis("VerticalP2"));

            direction = (direction.x * Camera.main.transform.right + direction.z * Camera.main.transform.forward);
			if(direction.magnitude >= 0.01f && direction.magnitude <= 0.2f)
			{
				direction = direction.normalized * 0.2f;
			}

			UpdateAnimatorParams(direction.magnitude);

            Vector3 velocity = direction * speed * Time.deltaTime;

		    checkDistance(ref velocity);

		    rb.MovePosition(transform.position + velocity);
            transform.LookAt(transform.position + direction);
            transform.localEulerAngles = new Vector3(0.0f, transform.localEulerAngles.y, 0.0f);
        }
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
        if (((player1 && (Input.GetKeyDown(KeyCode.Joystick1Button4) 
            || Input.GetKeyDown(KeyCode.Space))) 
            || (!player1 && (Input.GetKeyDown(KeyCode.Joystick2Button4) 
            || Input.GetKeyDown(KeyCode.Keypad0)))) && canTaunt)
        {
            Taunt();
            StartCoroutine(TauntCoolDown(tauntCooldown));
			StartCoroutine(GameManager.gameManager.UIManager.TauntCooldownSystem(player1, tauntCooldown));
        }

    }

    IEnumerator TauntCoolDown(float cd) {
        canTaunt = false;
        yield return new WaitForSeconds(cd);
        canTaunt = true;
    }

    void Taunt()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, tauntRange);
        for (int i = 0; i < hitColliders.Length; i++)
        {
            if (hitColliders[i].CompareTag("Enemy"))
            {
                if (hitColliders[i].GetComponent<Enemy>().actualTauntCoroutine != null)
                {
                    hitColliders[i].GetComponent<Enemy>().StopCoroutine(hitColliders[i].GetComponent<Enemy>().actualTauntCoroutine);
                }

                hitColliders[i].GetComponent<Enemy>().actualTauntCoroutine = hitColliders[i].GetComponent<Enemy>().StartCoroutine(hitColliders[i].GetComponent<Enemy>().TauntCoroutine(player1));
            }
        }
    }
           

    /// <summary>
    /// gets the current power that is going to be apllied on the orb by checking the input
    /// the power to apply on the orb when the player hits the orb
    /// </summary>
    public void GetCurrentPower()
    {
        bool elementalPower = player1 ? Input.GetKeyDown(KeyCode.Joystick1Button3) || Input.GetKeyDown(KeyCode.Joystick1Button1) || Input.GetKeyDown(KeyCode.A) : Input.GetKeyDown(KeyCode.Joystick2Button3) || Input.GetKeyDown(KeyCode.Joystick2Button1) || Input.GetKeyDown(KeyCode.RightShift);
        bool behaviouralPower = player1 ? Input.GetKeyDown(KeyCode.Joystick1Button2) || Input.GetKeyDown(KeyCode.Joystick1Button0) || Input.GetKeyDown(KeyCode.E) : Input.GetKeyDown(KeyCode.Joystick2Button2) || Input.GetKeyDown(KeyCode.Joystick2Button0) || Input.GetKeyDown(KeyCode.RightControl);
        
        
        if (elementalPower && elementalPowerSlot != GameManager.PowerType.None)
        {
            if ((player1 && GameManager.gameManager.orb.GetComponent<PowerController>().canBeActivatedByPlayer1[(int)elementalPowerSlot - 1])
                || (!player1 && GameManager.gameManager.orb.GetComponent<PowerController>().canBeActivatedByPlayer2[(int)elementalPowerSlot - 1]))
            {
                if (orbHitter.powerToApply != elementalPowerSlot)
                {
                    if (!GameManager.isElemental(orbHitter.powerToApply))
                    {
                        selectedBehavioralFx.GetComponent<ParticleSystem>().Stop();
                    }
                    orbHitter.powerToApply = elementalPowerSlot;
                    //activer fx
                    selectedElementalFx.GetComponent<ParticleSystem>().Play();
                }
                else
                {
                    orbHitter.powerToApply = GameManager.PowerType.None;
                    //desactiver fx
                    selectedElementalFx.GetComponent<ParticleSystem>().Stop();
                }
            }
        }
        if (behaviouralPower && behaviouralPowerSlot != GameManager.PowerType.None)
        {
            if ((player1 && GameManager.gameManager.orb.GetComponent<PowerController>().canBeActivatedByPlayer1[(int)behaviouralPowerSlot - 1])
                || (!player1 && GameManager.gameManager.orb.GetComponent<PowerController>().canBeActivatedByPlayer2[(int)behaviouralPowerSlot - 1]))
            {
                if (orbHitter.powerToApply != behaviouralPowerSlot)
                {
                    if (GameManager.isElemental(orbHitter.powerToApply))
                    {
                        selectedElementalFx.GetComponent<ParticleSystem>().Stop();
                    }
                    orbHitter.powerToApply = behaviouralPowerSlot;
                    //activer fx
                    selectedBehavioralFx.GetComponent<ParticleSystem>().Play();
                }
                else
                {
                    orbHitter.powerToApply = GameManager.PowerType.None;
                    //desactiver fx
                    selectedBehavioralFx.GetComponent<ParticleSystem>().Stop();
                }
            }
        }
        
        ApplyFXTail(orbHitter.powerToApply);
    }


    void ApplyFXTail(GameManager.PowerType power)
    {
        if(power.CompareTo(GameManager.PowerType.Darkness) == 0)
            VFXTail.transform.GetChild(0).gameObject.SetActive(true);
        else if(power.CompareTo(GameManager.PowerType.Fire) == 0)
            VFXTail.transform.GetChild(1).gameObject.SetActive(true);
        else if (power.CompareTo(GameManager.PowerType.Electric) == 0)
            VFXTail.transform.GetChild(2).gameObject.SetActive(true);
        else if (power.CompareTo(GameManager.PowerType.Ice) == 0)
            VFXTail.transform.GetChild(3).gameObject.SetActive(true);
        else if (power.CompareTo(GameManager.PowerType.Vortex) == 0)
            VFXTail.transform.GetChild(4).gameObject.SetActive(true);
        else if (power.CompareTo(GameManager.PowerType.LeechLife) == 0)
            VFXTail.transform.GetChild(5).gameObject.SetActive(true);
        else if (power.CompareTo(GameManager.PowerType.Slug) == 0)
            VFXTail.transform.GetChild(6).gameObject.SetActive(true);
        else if (power.CompareTo(GameManager.PowerType.None) == 0)
        {
            for (int i = 0; i < 7; i++)
            {
                VFXTail.transform.GetChild(i).gameObject.SetActive(false);
            }
        }
    }


    /// <summary>
    /// Gives a power dropped by an enemy, and place it on the good slot
    /// </summary>
    public void AttributePower(GameManager.PowerType newPower)
    {
		if (GameManager.isElemental(newPower))
		{
			elementalPowerSlot = newPower;
			GameManager.gameManager.UIManager.UpdatePowerSlot(1, player1, newPower);
            GameManager.gameManager.UIManager.OrbToPowerSlotFeedback(player1, true);
        }
        else
		{
			behaviouralPowerSlot = newPower;
			GameManager.gameManager.UIManager.UpdatePowerSlot(2, player1, newPower);
            GameManager.gameManager.UIManager.OrbToPowerSlotFeedback(player1, false);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, gameObject.GetComponent<OrbHitter>().hitZone * 2);
    }

	public IEnumerator InvincibilityCoroutine()
	{
		SkinnedMeshRenderer renderer = transform.GetChild(0).GetComponent<SkinnedMeshRenderer>();
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

	public void RespawnReset()
	{
		StopAllCoroutines();
		canTaunt = true;
		invincible = false;
	}

    public void SlowSpeed(float slowAmount)
    {
        if (!isSlowed)
        {
            speed = initialSpeed * ((100 - slowAmount) / 100);
            isSlowed = true;
        }
    }

    public void RestoreSpeed()
    {
        if (isSlowed)
        {
            speed = initialSpeed;
            isSlowed = false;
        }
    }
    
    public IEnumerator FreezeCoroutine(float freezeTimer)
    {
        isFrozen = true;
        GameObject iceFx = gameObject.transform.Find("FX/ice").gameObject;
        iceFx.SetActive(true);
        yield return new WaitForSeconds(freezeTimer);
        isFrozen = false;
        iceFx.SetActive(false);
    }

    /*
    public void StartRoot(EnemyMovement eM, float castingTime, GameObject targetPlayer, int damage, Vector3 pos, float rootTime, GameObject rootBranchPrefab)
    {
        StartCoroutine(RootCoroutine(eM, castingTime, targetPlayer, damage, pos, rootTime, rootBranchPrefab));
    }
    */

    public IEnumerator RootCoroutine(EnemySkill eK, EnemyMovement eM, float castingTime, GameObject targetPlayer, int damage, Vector3 pos, float rootTime, GameObject rootBranchPrefab)
    {
        eM.agent.isStopped = true;
        eK.isCasting = true;
        yield return new WaitForSecondsRealtime(castingTime);
        eM.agent.isStopped = false;
        eK.isCasting = false;

        GameManager.gameManager.TakeDamage(targetPlayer, damage, pos, false);
        GameManager.gameManager.UIManager.QuoteOnDamage("enemy", targetPlayer);
        isRoot = true;
        RootBranch branch = Instantiate(rootBranchPrefab).GetComponent<RootBranch>();
        branch.targetPlayer = targetPlayer;
        branch.rootTime = rootTime;
        yield return new WaitForSecondsRealtime(rootTime);
        isRoot = false;
    }

	public void UpdateAnimatorParams(float speed)
	{
		animator.SetFloat("Speed", speed);

		if(speed >= 0.01 && speed < 0.5)
		{
			animator.SetFloat("WalkSpeed", (speed / 0.5f) + 0.5f);
		}
		else if(speed >= 0.5f)
		{
			animator.SetFloat("RunSpeed", 1.0f);
		}
	}
}
