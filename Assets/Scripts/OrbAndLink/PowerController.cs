using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerController : MonoBehaviour
{
    public GameManager.PowerType elementalPower;
    public GameManager.PowerType behavioralPower;

    OrbController orbController;

    public Material normalMaterial;
    public int baseDamage;

    public List<bool> canBeActivated;

    [Header("[Drop Container]")]
    public GameManager.PowerType droppedPower;
    public bool isFixedPower;
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
        canBeActivated = new List<bool> { true, true, true, true, true, true, true, true, true };
        orbController = gameObject.GetComponent<OrbController>();
    }

    #region Activation and Deactivation Functions

    /// <summary>
    /// Activate the powerToActivate, deactivate the power of the same type if there's already an active one
    /// </summary>
    /// <param name="powerToActivate"></param>
    public void ActivatePower(GameManager.PowerType powerToActivate)
    {
		if(canBeActivated[(int)powerToActivate - 1])
		{
			if (!isBehavioral(powerToActivate) && elementalPower != GameManager.PowerType.None)
			{
				DeactivatePower(elementalPower);
			}
			else if (isBehavioral(powerToActivate) && behavioralPower != GameManager.PowerType.None)
			{
				DeactivatePower(behavioralPower);
			}

			switch (powerToActivate)
			{
				case GameManager.PowerType.LargeOrb:
					StartCoroutine(cooldownCoroutine(GameManager.PowerType.LargeOrb, largeOrbCooldown));
					ActivateLargeOrb();
					break;
				case GameManager.PowerType.Vortex:
					StartCoroutine(cooldownCoroutine(GameManager.PowerType.Vortex, vortexCooldown));
					ActivateVortex();
					break;
				case GameManager.PowerType.LeechLife:
					StartCoroutine(cooldownCoroutine(GameManager.PowerType.LeechLife, leechLifeCooldown));
					ActivateLeechLife();
					break;
				case GameManager.PowerType.Slug:
					StartCoroutine(cooldownCoroutine(GameManager.PowerType.Slug, slugCooldown));
					ActivateSlug();
					break;
				case GameManager.PowerType.Shield:
					StartCoroutine(cooldownCoroutine(GameManager.PowerType.Shield, shieldCooldown));
					ActivateShield();
					break;
				case GameManager.PowerType.Ice:
					StartCoroutine(cooldownCoroutine(GameManager.PowerType.Ice, iceCooldown));
					ActivateIce();
					break;
				case GameManager.PowerType.Fire:
                    if (!isActivatedByBrazier)
                    {
                        StartCoroutine(cooldownCoroutine(GameManager.PowerType.Fire, fireCooldown));
                    }
					ActivateFire();
					break;
				case GameManager.PowerType.Electric:
					StartCoroutine(cooldownCoroutine(GameManager.PowerType.Electric, electricCooldown));
					ActivateElectric();
					break;
				case GameManager.PowerType.Darkness:
					StartCoroutine(cooldownCoroutine(GameManager.PowerType.Darkness, darknessCooldown));
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
        StartCoroutine(DurationCoroutine(GameManager.PowerType.LargeOrb, largeOrbDuration));
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
        transform.GetChild(0).GetComponent<MeshRenderer>().material = vortexMaterial;
        StartCoroutine(DurationCoroutine(GameManager.PowerType.Vortex, vortexDuration));
    }

    void DeactivateVortex()
    {
        behavioralPower = GameManager.PowerType.None;
        transform.GetChild(0).GetComponent<MeshRenderer>().material = normalMaterial;
    }

    #endregion

    #region LeechLife
    //==========LEACHLIFE==========

    void ActivateLeechLife()
    {
        behavioralPower = GameManager.PowerType.LeechLife;
        transform.GetChild(0).GetComponent<MeshRenderer>().material = leechLifeMaterial;
        StartCoroutine(DurationCoroutine(GameManager.PowerType.LeechLife, leechLifeDuration));
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
        StartCoroutine("InstanciateSlug");
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
        StartCoroutine(DurationCoroutine(GameManager.PowerType.Ice, iceDuration));
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
            StartCoroutine(DurationCoroutine(GameManager.PowerType.Fire, fireDurationBrazier));
            isActivatedByBrazier = false;
        }
        else
        {
            StartCoroutine(DurationCoroutine(GameManager.PowerType.Fire, fireDuration));
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
			enemy.TakeDamage(tickDamage, enemy.transform.position);
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
        StartCoroutine(DurationCoroutine(GameManager.PowerType.Electric, electricDuration));
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
        StartCoroutine(DurationCoroutine(GameManager.PowerType.Darkness, darknessDuration));
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

    public IEnumerator cooldownCoroutine(GameManager.PowerType power, float cooldown)
    {
        canBeActivated[(int)power - 1] = false;
        yield return new WaitForSeconds(cooldown);
        canBeActivated[(int)power - 1] = true;
    }

    //==========OTHERS==========

    public bool isBehavioral(GameManager.PowerType power)
    {
        if (power == GameManager.PowerType.LargeOrb || power == GameManager.PowerType.LeechLife || power == GameManager.PowerType.Shield || power == GameManager.PowerType.Slug || power == GameManager.PowerType.Vortex)
            return true;
        else
            return false;
    }

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
        
		enemy.TakeDamage(damageTaken, transform.position);

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
            switch (mode)
            {
                case "hit":
                    if (player1)
                        GameManager.gameManager.player1.GetComponent<PlayerController>().AttributePower(droppedPower, isFixedPower);
                    else
                        GameManager.gameManager.player2.GetComponent<PlayerController>().AttributePower(droppedPower, isFixedPower);
                    droppedPower = GameManager.PowerType.None;
                    //UpdateUI
                    GameManager.gameManager.UIManager.UpdateDroppedPower(droppedPower);
                    break;
                case "amortize":
                case "miss":
                    if (reflectedDrop)
                    {
                        if (player1)
                            GameManager.gameManager.player1.GetComponent<PlayerController>().AttributePower(droppedPower, isFixedPower);
                        else
                            GameManager.gameManager.player2.GetComponent<PlayerController>().AttributePower(droppedPower, isFixedPower);
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