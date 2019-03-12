﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerController : MonoBehaviour
{
    public GameManager.PowerType elementalPower;
    public GameManager.PowerType behavioralPower;
	public Coroutine actualDurationCoroutine;

    OrbController orbController;

    public Material normalMaterial;
    public int baseDamage;

	public List<bool> canBeActivatedByPlayer1;
	public List<bool> canBeActivatedByPlayer2;

	[Header("[Drop Container]")]
    public GameManager.PowerType droppedPower;
    public bool reflectedDrop;

    [Header("[Power Editing]")]
    public GameManager.PowerType editingPower;

    #region Large Orb Param
    //LargeOrb
    [DrawIf(new string[] { "editingPower" }, GameManager.PowerType.LargeOrb)]
    public float largeOrbDuration;
    [DrawIf(new string[] { "editingPower" }, GameManager.PowerType.LargeOrb)]
    public float largeOrbCooldown;
    [DrawIf(new string[] { "editingPower" }, GameManager.PowerType.LargeOrb)]
    public float minScale;
    [DrawIf(new string[] { "editingPower" }, GameManager.PowerType.LargeOrb)]
    public float maxScale;
    [DrawIf(new string[] { "editingPower" }, GameManager.PowerType.LargeOrb)]
    public int largeOrbDamage;
    #endregion

    #region Vortex Param
    //Vortex
    [DrawIf(new string[] { "editingPower" }, GameManager.PowerType.Vortex)]
    public Material vortexMaterial;
    [DrawIf(new string[] { "editingPower" }, GameManager.PowerType.Vortex)]
    public float vortexDuration;
    [DrawIf(new string[] { "editingPower" }, GameManager.PowerType.Vortex)]
    public float vortexCooldown;
    [DrawIf(new string[] { "editingPower" }, GameManager.PowerType.Vortex)]
    public float vortexRangeOfEffect;
    [DrawIf(new string[] { "editingPower" }, GameManager.PowerType.Vortex)]
    public float vortexAttractionPower;
    #endregion

    #region Leech Life Param
    //LeechLife
    [DrawIf(new string[] { "editingPower" }, GameManager.PowerType.LeechLife)]
    public Material leechLifeMaterial;
    [DrawIf(new string[] { "editingPower" }, GameManager.PowerType.LeechLife)]
    public float leechLifeDuration;
    [DrawIf(new string[] { "editingPower" }, GameManager.PowerType.LeechLife)]
    public float leechLifeCooldown;
    [DrawIf(new string[] { "editingPower" }, GameManager.PowerType.LeechLife)]
    [Range(0, 100f)]
    public float lifeSteel;
    #endregion

    #region Slug Param
    //Slug
    [DrawIf(new string[] { "editingPower" }, GameManager.PowerType.Slug)]
    public Material slugMaterial;
    [DrawIf(new string[] { "editingPower" }, GameManager.PowerType.Slug)]
    public float slugDuration;
    [DrawIf(new string[] { "editingPower" }, GameManager.PowerType.Slug)]
    public int mitigatedDamageSlug;
    [DrawIf(new string[] { "editingPower" }, GameManager.PowerType.Slug)]
    public float slugCooldown;
    [DrawIf(new string[] { "editingPower" }, GameManager.PowerType.Slug)]
    public GameObject slug;
    #endregion

    #region Shield Param
    //Shield
    [DrawIf(new string[] { "editingPower" }, GameManager.PowerType.Shield)]
    public Material shieldMaterial;
    [DrawIf(new string[] { "editingPower" }, GameManager.PowerType.Shield)]
    public float shieldCooldown;
    [DrawIf(new string[] { "editingPower" }, GameManager.PowerType.Shield)]
    [Tooltip("Reduce the damage of the orb")]
    public int mitigatedDamage;
    [DrawIf(new string[] { "editingPower" }, GameManager.PowerType.Shield)]
    public int shieldAmount;
    [DrawIf(new string[] { "editingPower" }, GameManager.PowerType.Shield)]
    [Tooltip("number of stacks that gives shield when the orb is hit")]
    public int currentShieldStack;
	#endregion

	#region Ice Param
	//Ice
	[DrawIf(new string[] { "editingPower" }, GameManager.PowerType.Ice)]
	public Material iceMaterial;
	[DrawIf(new string[] { "editingPower" }, GameManager.PowerType.Ice)]
	public float iceDuration;
	[DrawIf(new string[] { "editingPower" }, GameManager.PowerType.Ice)]
	public float iceCooldown;
	[DrawIf(new string[] { "editingPower" }, GameManager.PowerType.Ice)]
	public int iceDamage;
	#endregion

	#region Fire Param
	//Fire 
	[DrawIf(new string[] { "editingPower" }, GameManager.PowerType.Fire)]
	public Material fireMaterial;
	[DrawIf(new string[] { "editingPower" }, GameManager.PowerType.Fire)]
	public float fireDuration;
    [DrawIf(new string[] { "editingPower" }, GameManager.PowerType.Fire)]
    public float fireDurationBrazier;
    [DrawIf(new string[] { "editingPower" }, GameManager.PowerType.Fire)]
	public float fireCooldown;
	[DrawIf(new string[] { "editingPower" }, GameManager.PowerType.Fire)]
	public int fireDamage;

	[DrawIf(new string[] { "editingPower" }, GameManager.PowerType.Fire)]
	[Tooltip("Damage is over time , should be >= to fireDuration")]
    public int fireTicksDamage = 5;
	[DrawIf(new string[] { "editingPower" }, GameManager.PowerType.Fire)]
	public float fireTickDuration = 5;
	[DrawIf(new string[] { "editingPower" }, GameManager.PowerType.Fire)]
	private float nextAttack = 0f;


    public bool isActivatedByBrazier;
    #endregion

    #region Electric Param
    //Electric
    [DrawIf(new string[] { "editingPower" }, GameManager.PowerType.Electric)]
	public Material electricMaterial;
	[DrawIf(new string[] { "editingPower" }, GameManager.PowerType.Electric)]
	public float electricDuration;
	[DrawIf(new string[] { "editingPower" }, GameManager.PowerType.Electric)]
	public float electricCooldown;
	[DrawIf(new string[] { "editingPower" }, GameManager.PowerType.Electric)]
	public int electricDamage;
	#endregion

	#region Darkness Param
	//Darkness
	[DrawIf(new string[] { "editingPower" }, GameManager.PowerType.Darkness)]
	public Material darknessMaterial;
	[DrawIf(new string[] { "editingPower" }, GameManager.PowerType.Darkness)]
	public float darknessDuration;
	[DrawIf(new string[] { "editingPower" }, GameManager.PowerType.Darkness)]
	public float darknessCooldown;
	#endregion


	private void Start()
    {
		canBeActivatedByPlayer1 = new List<bool> { true, true, true, true, true, true, true, true, true };
		canBeActivatedByPlayer2 = new List<bool> { true, true, true, true, true, true, true, true, true };

		orbController = gameObject.GetComponent<OrbController>();
    }

	#region Activation and Deactivation Functions

	/// <summary>
	/// Activate the powerToActivate, deactivate the power of the same type if there's already an active one
	/// </summary>
	/// <param name="powerToActivate"></param>
	public void ActivatePower(GameManager.PowerType powerToActivate, string mode)
    {
		bool activate = false;
		switch(mode)
		{
			case "forced":
				activate = true;
				break;
			case "player1":
				activate = canBeActivatedByPlayer1[(int)powerToActivate - 1];
				break;
			case "player2":
				activate = canBeActivatedByPlayer2[(int)powerToActivate - 1];
				break;
		}

		if (activate)
		{
			if(actualDurationCoroutine != null)
				StopCoroutine(actualDurationCoroutine);

			if (GameManager.isElemental(powerToActivate) && elementalPower != GameManager.PowerType.None)
			{
				DeactivatePower(elementalPower);
			}
			else if (!GameManager.isElemental(powerToActivate) && behavioralPower != GameManager.PowerType.None)
			{
				DeactivatePower(behavioralPower);
			}

			switch (powerToActivate)
			{
				case GameManager.PowerType.LargeOrb:
					StartCoroutine(cooldownCoroutine(GameManager.PowerType.LargeOrb, largeOrbCooldown, mode));
					ActivateLargeOrb();
					break;
				case GameManager.PowerType.Vortex:
					StartCoroutine(cooldownCoroutine(GameManager.PowerType.Vortex, vortexCooldown, mode));
					ActivateVortex();
					break;
				case GameManager.PowerType.LeechLife:
					StartCoroutine(cooldownCoroutine(GameManager.PowerType.LeechLife, leechLifeCooldown, mode));
					ActivateLeechLife();
					break;
				case GameManager.PowerType.Slug:
					StartCoroutine(cooldownCoroutine(GameManager.PowerType.Slug, slugCooldown, mode));
					ActivateSlug();
					break;
				case GameManager.PowerType.Shield:
					StartCoroutine(cooldownCoroutine(GameManager.PowerType.Shield, shieldCooldown, mode));
					ActivateShield();
					break;
				case GameManager.PowerType.Ice:
					StartCoroutine(cooldownCoroutine(GameManager.PowerType.Ice, iceCooldown, mode));
					ActivateIce();
					break;
				case GameManager.PowerType.Fire:
                    StartCoroutine(cooldownCoroutine(GameManager.PowerType.Fire, fireCooldown, mode));
                    ActivateFire();
					break;
				case GameManager.PowerType.Electric:
					StartCoroutine(cooldownCoroutine(GameManager.PowerType.Electric, electricCooldown, mode));
					ActivateElectric();
					break;
				case GameManager.PowerType.Darkness:
					StartCoroutine(cooldownCoroutine(GameManager.PowerType.Darkness, darknessCooldown, mode));
					ActivateDarkness();
					break;
			}
		}
    }

    /// <summary>
    /// Deactivate the powerToDeactivate
    /// </summary>
    /// <param name="powerToDeactivate"></param>
    public void DeactivatePower(GameManager.PowerType powerToDeactivate)
    {
        switch (powerToDeactivate)
        {
            case GameManager.PowerType.LargeOrb:
                DeactivateLargeOrb();
                break;
            case GameManager.PowerType.Vortex:
                DeactivateVortex();
                break;
            case GameManager.PowerType.LeechLife:
                DeactivateLeechLife();
                break;
            case GameManager.PowerType.Slug:
                DeactivateSlug();
                break;
            case GameManager.PowerType.Shield:
                DeactivateShield();
                break;
            case GameManager.PowerType.Ice:
                DeactivateIce();
                break;
            case GameManager.PowerType.Fire:
                DeactivateFire();
                break;
            case GameManager.PowerType.Electric:
                DeactivateElectric();
                break;
            case GameManager.PowerType.Darkness:
                DeactivateDarkness();
                break;
            default:
                break;
        }
    }

    #endregion

    //==========BEHAVIORAL POWERS FUNCTIONS==========

    #region LargeOrb
    //==========LARGEORB==========

    void ActivateLargeOrb()
    {
        behavioralPower = GameManager.PowerType.LargeOrb;
        transform.localScale = new Vector3(maxScale, maxScale, maxScale);
        transform.GetChild(0).GetComponent<MeshRenderer>().material = normalMaterial;
        actualDurationCoroutine = StartCoroutine(DurationCoroutine(GameManager.PowerType.LargeOrb, largeOrbDuration));
    }

    void DeactivateLargeOrb()
    {
        behavioralPower = GameManager.PowerType.None;
        transform.localScale = new Vector3(minScale, minScale, minScale);
        transform.GetChild(0).GetComponent<MeshRenderer>().material = normalMaterial;
    }

    #endregion

    #region Vortex
    //==========VORTEX==========

    void ActivateVortex()
    {
        behavioralPower = GameManager.PowerType.Vortex;
		actualDurationCoroutine = StartCoroutine(VortexPower());
        transform.GetChild(0).GetComponent<MeshRenderer>().material = vortexMaterial;
    }

    void DeactivateVortex()
    {
        behavioralPower = GameManager.PowerType.None;
        transform.GetChild(0).GetComponent<MeshRenderer>().material = normalMaterial;
    }

    IEnumerator VortexPower()
    {
        float timeStamp = Time.time;
        while (Time.time - timeStamp <= vortexDuration)
        {
            yield return new WaitUntil(() => orbController.progression <= 0.51f && orbController.progression >= 0.49);
            AttractEnemies();
        }
        DeactivateVortex();
    }
    void AttractEnemies()
    {
        Collider[] enemiesInRange = Physics.OverlapSphere(transform.position, vortexRangeOfEffect);
        for (int i = 0; i < enemiesInRange.Length; i++)
        {
            if (enemiesInRange[i].transform.gameObject.CompareTag("Enemy"))
            {
                GameObject currentEnemy = enemiesInRange[i].transform.gameObject;
                currentEnemy.GetComponent<EnemyMovement>().agent.velocity = (transform.position - currentEnemy.transform.position) * vortexAttractionPower;
            }
        }
    }

    #endregion

    #region LeechLife
    //==========LEACHLIFE==========

    void ActivateLeechLife()
    {
        behavioralPower = GameManager.PowerType.LeechLife;
        transform.GetChild(0).GetComponent<MeshRenderer>().material = leechLifeMaterial;
		actualDurationCoroutine = StartCoroutine(DurationCoroutine(GameManager.PowerType.LeechLife, leechLifeDuration));
    }

    void DeactivateLeechLife()
    {
        behavioralPower = GameManager.PowerType.None;
        transform.GetChild(0).GetComponent<MeshRenderer>().material = normalMaterial;
    }

    #endregion

    #region Slug
    //==========SLUG==========

    void ActivateSlug()
    {
        behavioralPower = GameManager.PowerType.Slug;
		actualDurationCoroutine = StartCoroutine(InstanciateSlug());
        transform.GetChild(0).GetComponent<MeshRenderer>().material = slugMaterial;
    }

    void DeactivateSlug()
    {
        behavioralPower = GameManager.PowerType.None;
        transform.GetChild(0).GetComponent<MeshRenderer>().material = normalMaterial;
    }

    IEnumerator InstanciateSlug()
    {
        float timeStamp = Time.time;
        while (Time.time - timeStamp <= slugDuration)
        {
            Instantiate(slug, new Vector3(transform.position.x, 0.2f, transform.position.z), Quaternion.identity);
            yield return new WaitForSeconds(0.2f);
        }
        DeactivateSlug();
    }

    #endregion

    #region Shield
    //==========SHIELD==========

    void ActivateShield()
    {
        behavioralPower = GameManager.PowerType.Shield;
        currentShieldStack = 2;
        transform.GetChild(0).GetComponent<MeshRenderer>().material = shieldMaterial;
    }

    void DeactivateShield()
    {
        behavioralPower = GameManager.PowerType.None;
        GameManager.gameManager.shieldP1 = 0;
        GameManager.gameManager.shieldP2 = 0;
        transform.GetChild(0).GetComponent<MeshRenderer>().material = normalMaterial;
    }

    #endregion

    //==========ELEMENTAL POWERS FUNCTIONS==========

    #region Ice
    //==========ICE==========

    void ActivateIce()
    {
        elementalPower = GameManager.PowerType.Ice;
        GetComponent<MeshRenderer>().material = iceMaterial;
		actualDurationCoroutine = StartCoroutine(DurationCoroutine(GameManager.PowerType.Ice, iceDuration));
    }

    void DeactivateIce()
    {
        elementalPower = GameManager.PowerType.None;
        GetComponent<MeshRenderer>().material = normalMaterial;
    }

    #endregion

    #region Fire
    //==========FIRE==========

    void ActivateFire()
    {
        elementalPower = GameManager.PowerType.Fire;
		GetComponent<MeshRenderer>().material = fireMaterial;

        if (isActivatedByBrazier)
		{
			actualDurationCoroutine = StartCoroutine(DurationCoroutine(GameManager.PowerType.Fire, fireDurationBrazier));
            isActivatedByBrazier = false;
        }
        else
		{
			actualDurationCoroutine = StartCoroutine(DurationCoroutine(GameManager.PowerType.Fire, fireDuration));
        }
    }

    void DeactivateFire()
    {
        elementalPower = GameManager.PowerType.None;
        GetComponent<MeshRenderer>().material = normalMaterial;
    }

    IEnumerator FireDamage(Enemy enemy, int totalDamage, float duration)
    {
        int tickDamage = Mathf.RoundToInt(totalDamage / duration);
        int curentDamage = 0;

		while (curentDamage < totalDamage)
		{
			enemy.TakeDamage(tickDamage);
			yield return new WaitForSeconds(1f);
			curentDamage += tickDamage;
		}
	}

    #endregion

    #region Electric
    //==========ELECTRIC==========

    void ActivateElectric()
    {
        elementalPower = GameManager.PowerType.Electric;
        GetComponent<MeshRenderer>().material = electricMaterial;
		actualDurationCoroutine = StartCoroutine(DurationCoroutine(GameManager.PowerType.Electric, electricDuration));
    }

    void DeactivateElectric()
    {
        elementalPower = GameManager.PowerType.None;
        GetComponent<MeshRenderer>().material = normalMaterial;
    }

    #endregion

    #region Darkness
    //==========DARKNESS==========

    void ActivateDarkness()
    {
        elementalPower = GameManager.PowerType.Darkness;
        GetComponent<MeshRenderer>().material = darknessMaterial;
		actualDurationCoroutine = StartCoroutine(DurationCoroutine(GameManager.PowerType.Darkness, darknessDuration));
    }

    void DeactivateDarkness()
    {
        elementalPower = GameManager.PowerType.None;
        GetComponent<MeshRenderer>().material = normalMaterial;
    }

    #endregion

    //==========DURATION AND COOLDOWN==========

    IEnumerator DurationCoroutine(GameManager.PowerType power, float duration)
    {
        yield return new WaitForSeconds(duration);
        DeactivatePower(power);
    }

    public IEnumerator cooldownCoroutine(GameManager.PowerType power, float cooldown, string mode)
    {   
		switch (mode)
		{
			case "forced":
				break;
			case "player1":
				canBeActivatedByPlayer1[(int)power - 1] = false;
				GameManager.gameManager.UIManager.Cooldown(power, cooldown, true);
				yield return new WaitForSeconds(cooldown);
				canBeActivatedByPlayer1[(int)power - 1] = true;
				break;
			case "player2":
				canBeActivatedByPlayer2[(int)power - 1] = false;
				GameManager.gameManager.UIManager.Cooldown(power, cooldown, false);
				yield return new WaitForSeconds(cooldown);
				canBeActivatedByPlayer2[(int)power - 1] = true;
				break;
		}
	}

    //==========OTHERS==========

    public void onEnemyHit(GameObject target)
    {
        Enemy enemy = target.GetComponent<Enemy>();
        int bonusDamage = (orbController.combo / orbController.damageIncreaseStep) * orbController.damageComboIncrease;
        if (bonusDamage > orbController.maxComboDamage)
        {
            bonusDamage = orbController.maxComboDamage;
        }
        int damageTaken = baseDamage + bonusDamage;

        switch (behavioralPower)
        {
            case GameManager.PowerType.LargeOrb:
                damageTaken += largeOrbDamage;
                break;
            case GameManager.PowerType.Shield:
                damageTaken -= mitigatedDamage;
                break;
            case GameManager.PowerType.Slug:
                damageTaken -= mitigatedDamageSlug;
                break;
        }

        switch (elementalPower)
        {
            case GameManager.PowerType.Ice:
                Debug.Log("Slow down bitch");
                damageTaken += iceDamage;
                break;
            case GameManager.PowerType.Fire:
                StopCoroutine("FireDamage");
                StartCoroutine(FireDamage(enemy, fireTicksDamage, fireTickDuration));
                damageTaken += fireDamage;
                break;
            case GameManager.PowerType.Electric:
                damageTaken += electricDamage;
                break;
        }
        
		enemy.TakeDamage(damageTaken);

        if (behavioralPower == GameManager.PowerType.LeechLife)
        {
            OrbController controller = GameManager.gameManager.orb.GetComponent<OrbController>();
            if (controller.toPlayer2)
                GameManager.gameManager.spawnHealingOrbs(1, (int)(damageTaken / (100 / lifeSteel)), "leechLife");
            else
                GameManager.gameManager.spawnHealingOrbs(2, (int)(damageTaken / (100 / lifeSteel)), "leechLife");
        }
    }

    /// <summary>
    /// Check if a dropped power is in the orb to give it to a player
    /// </summary>
    public void CheckPowerAttribution(string mode, bool player1)
    {
        if (droppedPower != GameManager.PowerType.None)
        {
			PlayerController player, otherPlayer;
			if (player1)
			{
				player = GameManager.gameManager.player1.GetComponent<PlayerController>();
				otherPlayer = GameManager.gameManager.player2.GetComponent<PlayerController>();
			}
			else
			{
				player = GameManager.gameManager.player2.GetComponent<PlayerController>();
				otherPlayer = GameManager.gameManager.player1.GetComponent<PlayerController>();
			}

			if ((GameManager.isElemental(droppedPower) && player.elementalPowerSlot == GameManager.PowerType.None) || (!GameManager.isElemental(droppedPower) && player.behaviouralPowerSlot == GameManager.PowerType.None))
			{
				player.AttributePower(droppedPower);
				droppedPower = GameManager.PowerType.None;
				//UpdateUI
				GameManager.gameManager.UIManager.UpdateDroppedPower(droppedPower);
			}
			else if ((GameManager.isElemental(droppedPower) && otherPlayer.elementalPowerSlot == GameManager.PowerType.None) || (!GameManager.isElemental(droppedPower) && otherPlayer.behaviouralPowerSlot == GameManager.PowerType.None))
			{
				reflectedDrop = true;
			}
			else
			{
				switch(mode)
				{
					case "hit":
						player.AttributePower(droppedPower);
						droppedPower = GameManager.PowerType.None;
						//UpdateUI
						GameManager.gameManager.UIManager.UpdateDroppedPower(droppedPower);
						break;
					case "amortize":
					case "miss":
						if(reflectedDrop)
						{
							player.AttributePower(droppedPower);
							droppedPower = GameManager.PowerType.None;
							//UpdateUI
							GameManager.gameManager.UIManager.UpdateDroppedPower(droppedPower);
						}
						else
						{
							reflectedDrop = true;
						}
						break;
				}
			}
		}
    }

    private void OnDrawGizmos()
    {
        if (behavioralPower == GameManager.PowerType.Vortex)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, vortexRangeOfEffect);
        }
    }


}