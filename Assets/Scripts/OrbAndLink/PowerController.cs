using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerController : MonoBehaviour
{
    public GameManager.PowerType elementalPower;
    public GameManager.PowerType behaviouralPower;
	public Coroutine elementalDurationCoroutine;
	public Coroutine behaviouralDurationCoroutine;

	OrbController orbController;

    public Material normalMaterial;
    GameObject VFX;
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
	[DrawIf(new string[] { "editingPower" }, GameManager.PowerType.Ice)]
	public float freezeDuration;
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
	[Tooltip("Damage is over time , should be >= to fireDuration")]
	[DrawIf(new string[] { "editingPower" }, GameManager.PowerType.Fire)]
	public int fireTicksDamage = 5;
	[DrawIf(new string[] { "editingPower" }, GameManager.PowerType.Fire)]
	public float fireTickDuration = 5;
	[DrawIf(new string[] { "editingPower" }, GameManager.PowerType.Fire)]
	public int fireTicksDamageBrazier = 5;
	[DrawIf(new string[] { "editingPower" }, GameManager.PowerType.Fire)]
	public float fireTickDurationBrazier = 5;
	[DrawIf(new string[] { "editingPower" }, GameManager.PowerType.Fire)]
	private float nextAttack = 0f;
	[DrawIf(new string[] { "editingPower" }, GameManager.PowerType.Fire)]
	public bool isActivatedByBrazier;
	[DrawIf(new string[] { "editingPower" }, GameManager.PowerType.Fire)]
	Coroutine actualFireDOTCoroutine;
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
	[DrawIf(new string[] { "editingPower" }, GameManager.PowerType.Electric)]
	public GameObject lightningRodPrefab;
	[DrawIf(new string[] { "editingPower" }, GameManager.PowerType.Electric)]
	public float zapRange;
    [DrawIf(new string[] { "editingPower" }, GameManager.PowerType.Electric)]
    public int zapDamageEnemy;
    [DrawIf(new string[] { "editingPower" }, GameManager.PowerType.Electric)]
    public int zapDamagePlayer;
    [DrawIf(new string[] { "editingPower" }, GameManager.PowerType.Electric)]
	public int maxZapNb;
	[DrawIf(new string[] { "editingPower" }, GameManager.PowerType.Electric)]
	public float timeBetweenZap;
    [DrawIf(new string[] { "editingPower" }, GameManager.PowerType.Electric)]
    public LayerMask enemyLayerMask;
    [DrawIf(new string[] { "editingPower" }, GameManager.PowerType.Electric)]
    public LayerMask playerLayerMask;
    #endregion

    #region Darkness Param
    //Darkness
    [DrawIf(new string[] { "editingPower" }, GameManager.PowerType.Darkness)]
	public Material darknessMaterial;
	[DrawIf(new string[] { "editingPower" }, GameManager.PowerType.Darkness)]
	public float darknessDuration;
	[DrawIf(new string[] { "editingPower" }, GameManager.PowerType.Darkness)]
	public float darknessCooldown;
    [DrawIf(new string[] { "editingPower" }, GameManager.PowerType.Darkness)]
    public float darknessTimer;//duration of the state weaken on the ennemi
    [DrawIf(new string[] { "editingPower" }, GameManager.PowerType.Darkness)]
    public int darknessDamage;//bonus damage when an weaken ennemi get hit bby the orb
    #endregion


    private void Start()
    {
		canBeActivatedByPlayer1 = new List<bool> { true, true, true, true, true, true, true, true, true };
		canBeActivatedByPlayer2 = new List<bool> { true, true, true, true, true, true, true, true, true };

		orbController = gameObject.GetComponent<OrbController>();
        VFX = transform.GetChild(0).gameObject;
        
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
			if (GameManager.isElemental(powerToActivate) && elementalPower != GameManager.PowerType.None)
			{
				DeactivatePower(elementalPower);
			}
			else if (!GameManager.isElemental(powerToActivate) && behaviouralPower != GameManager.PowerType.None)
			{
				DeactivatePower(behaviouralPower);
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
					if (mode == "forced")
						ActivateFire(true);
					else
						ActivateFire(false);
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
		if(GameManager.isElemental(powerToDeactivate) && elementalDurationCoroutine != null)
		{
			StopCoroutine(elementalDurationCoroutine);
		}
		else if (!GameManager.isElemental(powerToDeactivate) && behaviouralDurationCoroutine != null)
		{
			StopCoroutine(behaviouralDurationCoroutine);
		}

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
        behaviouralPower = GameManager.PowerType.LargeOrb;
        //transform.localScale = new Vector3(maxScale, maxScale, maxScale);
        //transform.GetChild(0).GetComponent<MeshRenderer>().material = normalMaterial;

        for(int i = 0; i< 5; i++)
        {
            if(VFX.transform.GetChild(i).gameObject.activeSelf)
            {
                for(int j=0; j< 4; j++)
                {
                    VFX.transform.GetChild(i).gameObject.transform.GetChild(j).transform.localScale *= maxScale;
                }                
            }
        }

        
		behaviouralDurationCoroutine = StartCoroutine(DurationCoroutine(GameManager.PowerType.LargeOrb, largeOrbDuration));
    }

    void DeactivateLargeOrb()
    {
        behaviouralPower = GameManager.PowerType.None;
        //transform.localScale = new Vector3(minScale, minScale, minScale);
        //transform.GetChild(0).GetComponent<MeshRenderer>().material = normalMaterial;

        for (int i = 0; i < 5; i++)
        {
            if (VFX.transform.GetChild(i).gameObject.activeSelf)
            {
                for (int j = 0; j < 4; j++)
                {
                    VFX.transform.GetChild(i).gameObject.transform.GetChild(j).transform.localScale /= maxScale;
                }
            }
        }
    }

    #endregion

    #region Vortex
    //==========VORTEX==========

    void ActivateVortex()
    {
        behaviouralPower = GameManager.PowerType.Vortex;
		behaviouralDurationCoroutine = StartCoroutine(VortexPower());
        //transform.GetChild(0).GetComponent<MeshRenderer>().material = vortexMaterial;
        VFX.transform.GetChild(5).gameObject.SetActive(true);

        for(int i=0; i<5;  i++)
        {
            if(VFX.transform.GetChild(i).gameObject.activeSelf)
            {
                VFX.transform.GetChild(i).gameObject.transform.GetChild(0).gameObject.SetActive(false);
                VFX.transform.GetChild(i).gameObject.transform.GetChild(4).gameObject.SetActive(true);
            }
        }
    }

    void DeactivateVortex()
    {
        behaviouralPower = GameManager.PowerType.None;
        //transform.GetChild(0).GetComponent<MeshRenderer>().material = normalMaterial;
        VFX.transform.GetChild(5).gameObject.SetActive(false);

        for (int i = 0; i < 5; i++)
        {
            if (VFX.transform.GetChild(i).gameObject.activeSelf)
            {
                VFX.transform.GetChild(i).gameObject.transform.GetChild(4).gameObject.SetActive(false);
                VFX.transform.GetChild(i).gameObject.transform.GetChild(0).gameObject.SetActive(true);
            }
        }
        
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
                //currentEnemy.GetComponent<EnemyMovement>().agent.velocity = (transform.position - currentEnemy.transform.position) * vortexAttractionPower;
                StartCoroutine(Attraction(30, currentEnemy));
            }
        }
    }

    IEnumerator Attraction(float tick, GameObject currentEnemy)
    {
        for (int i = 0; i < tick; i++)
        {
            currentEnemy.GetComponent<EnemyMovement>().agent.velocity = ((transform.position - currentEnemy.transform.position) * vortexAttractionPower)/ tick;
            yield return new WaitForEndOfFrame();
        }
        
    }

    #endregion

    #region LeechLife
    //==========LEACHLIFE==========

    void ActivateLeechLife()
    {
        behaviouralPower = GameManager.PowerType.LeechLife;
        //transform.GetChild(0).GetComponent<MeshRenderer>().material = leechLifeMaterial;

        VFX.transform.GetChild(6).gameObject.SetActive(true);

        for (int i = 0; i < 5; i++)
        {
            if (VFX.transform.GetChild(i).gameObject.activeSelf)
            {
                VFX.transform.GetChild(i).gameObject.transform.GetChild(0).gameObject.SetActive(false);
                VFX.transform.GetChild(i).gameObject.transform.GetChild(5).gameObject.SetActive(true);
            }
        }


        behaviouralDurationCoroutine = StartCoroutine(DurationCoroutine(GameManager.PowerType.LeechLife, leechLifeDuration));
    }

    void DeactivateLeechLife()
    {
        behaviouralPower = GameManager.PowerType.None;
        //transform.GetChild(0).GetComponent<MeshRenderer>().material = normalMaterial;

        VFX.transform.GetChild(6).gameObject.SetActive(false);

        for (int i = 0; i < 5; i++)
        {
            if (VFX.transform.GetChild(i).gameObject.activeSelf)
            {
                VFX.transform.GetChild(i).gameObject.transform.GetChild(5).gameObject.SetActive(false);
                VFX.transform.GetChild(i).gameObject.transform.GetChild(0).gameObject.SetActive(true);
            }
        }
    }

    #endregion

    #region Slug
    //==========SLUG==========

    void ActivateSlug()
    {
        behaviouralPower = GameManager.PowerType.Slug;
		behaviouralDurationCoroutine = StartCoroutine(InstanciateSlug());
        //transform.GetChild(0).GetComponent<MeshRenderer>().material = slugMaterial;

        VFX.transform.GetChild(7).gameObject.SetActive(true);

        for (int i = 0; i < 5; i++)
        {
            if (VFX.transform.GetChild(i).gameObject.activeSelf)
            {
                VFX.transform.GetChild(i).gameObject.transform.GetChild(0).gameObject.SetActive(false);
                VFX.transform.GetChild(i).gameObject.transform.GetChild(6).gameObject.SetActive(true);
            }
        }
    }

    void DeactivateSlug()
    {
        behaviouralPower = GameManager.PowerType.None;
        //transform.GetChild(0).GetComponent<MeshRenderer>().material = normalMaterial;

        VFX.transform.GetChild(7).gameObject.SetActive(false);

        for (int i = 0; i < 5; i++)
        {
            if (VFX.transform.GetChild(i).gameObject.activeSelf)
            {
                VFX.transform.GetChild(i).gameObject.transform.GetChild(6).gameObject.SetActive(false);
                VFX.transform.GetChild(i).gameObject.transform.GetChild(0).gameObject.SetActive(true);
            }
        }
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
        behaviouralPower = GameManager.PowerType.Shield;
        currentShieldStack = 2;
        transform.GetChild(0).GetComponent<MeshRenderer>().material = shieldMaterial;
    }

    void DeactivateShield()
    {
        behaviouralPower = GameManager.PowerType.None;
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
        //GetComponent<MeshRenderer>().material = iceMaterial;
        VFX.transform.GetChild(4).gameObject.transform.GetChild(0).gameObject.SetActive(VFX.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.activeSelf);
        VFX.transform.GetChild(4).gameObject.transform.GetChild(4).gameObject.SetActive(VFX.transform.GetChild(0).gameObject.transform.GetChild(4).gameObject.activeSelf);
        VFX.transform.GetChild(4).gameObject.transform.GetChild(5).gameObject.SetActive(VFX.transform.GetChild(0).gameObject.transform.GetChild(5).gameObject.activeSelf);
        VFX.transform.GetChild(4).gameObject.transform.GetChild(6).gameObject.SetActive(VFX.transform.GetChild(0).gameObject.transform.GetChild(6).gameObject.activeSelf);

        if(behaviouralPower == GameManager.PowerType.LargeOrb)
        {
            for (int i = 0; i < 4; i++)
            {
                VFX.transform.GetChild(0).gameObject.transform.GetChild(i).transform.localScale /= maxScale;
            }

            for (int i = 0; i < 4; i++)
            {
                VFX.transform.GetChild(4).gameObject.transform.GetChild(i).transform.localScale *= maxScale;
            }            
        }
        
        VFX.transform.GetChild(0).gameObject.SetActive(false);
        VFX.transform.GetChild(4).gameObject.SetActive(true);
		elementalDurationCoroutine = StartCoroutine(DurationCoroutine(GameManager.PowerType.Ice, iceDuration));
    }

    void DeactivateIce()
    {
        elementalPower = GameManager.PowerType.None;
        //GetComponent<MeshRenderer>().material = normalMaterial;


        VFX.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.SetActive(VFX.transform.GetChild(4).gameObject.transform.GetChild(0).gameObject.activeSelf);
        VFX.transform.GetChild(0).gameObject.transform.GetChild(4).gameObject.SetActive(VFX.transform.GetChild(4).gameObject.transform.GetChild(4).gameObject.activeSelf);
        VFX.transform.GetChild(0).gameObject.transform.GetChild(5).gameObject.SetActive(VFX.transform.GetChild(4).gameObject.transform.GetChild(5).gameObject.activeSelf);
        VFX.transform.GetChild(0).gameObject.transform.GetChild(6).gameObject.SetActive(VFX.transform.GetChild(4).gameObject.transform.GetChild(6).gameObject.activeSelf);

        if (behaviouralPower == GameManager.PowerType.LargeOrb)
        {

            for (int i = 0; i < 4; i++)
            {
                VFX.transform.GetChild(4).gameObject.transform.GetChild(i).transform.localScale /= maxScale;
            }

            for (int i = 0; i < 4; i++)
            {
                VFX.transform.GetChild(0).gameObject.transform.GetChild(i).transform.localScale *= maxScale;
            }
        }



        VFX.transform.GetChild(4).gameObject.SetActive(false);
        VFX.transform.GetChild(0).gameObject.SetActive(true);
    }

    #endregion

    #region Fire
    //==========FIRE==========

    void ActivateFire(bool forced)
    {
        elementalPower = GameManager.PowerType.Fire;
        //GetComponent<MeshRenderer>().material = fireMaterial;
        VFX.transform.GetChild(2).gameObject.transform.GetChild(0).gameObject.SetActive(VFX.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.activeSelf);
        VFX.transform.GetChild(2).gameObject.transform.GetChild(4).gameObject.SetActive(VFX.transform.GetChild(0).gameObject.transform.GetChild(4).gameObject.activeSelf);
        VFX.transform.GetChild(2).gameObject.transform.GetChild(5).gameObject.SetActive(VFX.transform.GetChild(0).gameObject.transform.GetChild(5).gameObject.activeSelf);
        VFX.transform.GetChild(2).gameObject.transform.GetChild(6).gameObject.SetActive(VFX.transform.GetChild(0).gameObject.transform.GetChild(6).gameObject.activeSelf);

        if (behaviouralPower == GameManager.PowerType.LargeOrb)
        {
            for (int i = 0; i < 4; i++)
            {
                VFX.transform.GetChild(0).gameObject.transform.GetChild(i).transform.localScale /= maxScale;
            }

            for (int i = 0; i < 4; i++)
            {
                VFX.transform.GetChild(2).gameObject.transform.GetChild(i).transform.localScale *= maxScale;
            }
        }
        
        VFX.transform.GetChild(0).gameObject.SetActive(false);
        VFX.transform.GetChild(2).gameObject.SetActive(true);
        
        if (forced)
		{
			elementalDurationCoroutine = StartCoroutine(DurationCoroutine(GameManager.PowerType.Fire, fireDurationBrazier));
			isActivatedByBrazier = true;
        }
        else
		{
			elementalDurationCoroutine = StartCoroutine(DurationCoroutine(GameManager.PowerType.Fire, fireDuration));
        }
    }

    void DeactivateFire()
    {
        elementalPower = GameManager.PowerType.None;
        //GetComponent<MeshRenderer>().material = normalMaterial;


        VFX.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.SetActive(VFX.transform.GetChild(2).gameObject.transform.GetChild(0).gameObject.activeSelf);
        VFX.transform.GetChild(0).gameObject.transform.GetChild(4).gameObject.SetActive(VFX.transform.GetChild(2).gameObject.transform.GetChild(4).gameObject.activeSelf);
        VFX.transform.GetChild(0).gameObject.transform.GetChild(5).gameObject.SetActive(VFX.transform.GetChild(2).gameObject.transform.GetChild(5).gameObject.activeSelf);
        VFX.transform.GetChild(0).gameObject.transform.GetChild(6).gameObject.SetActive(VFX.transform.GetChild(2).gameObject.transform.GetChild(6).gameObject.activeSelf);

        if (behaviouralPower == GameManager.PowerType.LargeOrb)
        {

            for (int i = 0; i < 4; i++)
            {
                VFX.transform.GetChild(2).gameObject.transform.GetChild(i).transform.localScale /= maxScale;
            }

            for (int i = 0; i < 4; i++)
            {
                VFX.transform.GetChild(0).gameObject.transform.GetChild(i).transform.localScale *= maxScale;
            }
        }

        VFX.transform.GetChild(2).gameObject.SetActive(false);
        VFX.transform.GetChild(0).gameObject.SetActive(true);
        isActivatedByBrazier = false;
	}



    #endregion

    #region Electric
    //==========ELECTRIC==========

    void ActivateElectric()
    {
        elementalPower = GameManager.PowerType.Electric;
        //GetComponent<MeshRenderer>().material = electricMaterial;
        VFX.transform.GetChild(3).gameObject.transform.GetChild(0).gameObject.SetActive(VFX.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.activeSelf);
        VFX.transform.GetChild(3).gameObject.transform.GetChild(4).gameObject.SetActive(VFX.transform.GetChild(0).gameObject.transform.GetChild(4).gameObject.activeSelf);
        VFX.transform.GetChild(3).gameObject.transform.GetChild(5).gameObject.SetActive(VFX.transform.GetChild(0).gameObject.transform.GetChild(5).gameObject.activeSelf);
        VFX.transform.GetChild(3).gameObject.transform.GetChild(6).gameObject.SetActive(VFX.transform.GetChild(0).gameObject.transform.GetChild(6).gameObject.activeSelf);

        if (behaviouralPower == GameManager.PowerType.LargeOrb)
        {
            for (int i = 0; i < 4; i++)
            {
                VFX.transform.GetChild(0).gameObject.transform.GetChild(i).transform.localScale /= maxScale;
            }

            for (int i = 0; i < 4; i++)
            {
                VFX.transform.GetChild(3).gameObject.transform.GetChild(i).transform.localScale *= maxScale;
            }
        }

        VFX.transform.GetChild(0).gameObject.SetActive(false);
        VFX.transform.GetChild(3).gameObject.SetActive(true);
        elementalDurationCoroutine = StartCoroutine(DurationCoroutine(GameManager.PowerType.Electric, electricDuration));
    }

    void DeactivateElectric()
    {
        elementalPower = GameManager.PowerType.None;
        //GetComponent<MeshRenderer>().material = normalMaterial;


        VFX.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.SetActive(VFX.transform.GetChild(3).gameObject.transform.GetChild(0).gameObject.activeSelf);
        VFX.transform.GetChild(0).gameObject.transform.GetChild(4).gameObject.SetActive(VFX.transform.GetChild(3).gameObject.transform.GetChild(4).gameObject.activeSelf);
        VFX.transform.GetChild(0).gameObject.transform.GetChild(5).gameObject.SetActive(VFX.transform.GetChild(3).gameObject.transform.GetChild(5).gameObject.activeSelf);
        VFX.transform.GetChild(0).gameObject.transform.GetChild(6).gameObject.SetActive(VFX.transform.GetChild(3).gameObject.transform.GetChild(6).gameObject.activeSelf);

        if (behaviouralPower == GameManager.PowerType.LargeOrb)
        {

            for (int i = 0; i < 4; i++)
            {
                VFX.transform.GetChild(3).gameObject.transform.GetChild(i).transform.localScale /= maxScale;
            }

            for (int i = 0; i < 4; i++)
            {
                VFX.transform.GetChild(0).gameObject.transform.GetChild(i).transform.localScale *= maxScale;
            }
        }

        VFX.transform.GetChild(3).gameObject.SetActive(false);
        VFX.transform.GetChild(0).gameObject.SetActive(true);
    }

	public IEnumerator ElectricZappingCoroutine(Vector3 startPos, GameObject firstObject, bool isEnemy)
	{
		Vector3 actualPos = startPos;
		List<GameObject> zappedObjects = new List<GameObject>();
        zappedObjects.Add(firstObject);
		int zapNb = 0;
		bool hasHitObject = true;

		LightningRod rod = Instantiate(lightningRodPrefab, startPos, Quaternion.identity, transform).GetComponent<LightningRod>();
		rod.target = firstObject;

		while(zapNb <= maxZapNb && hasHitObject)
		{
			GameObject nearestObject = null;
            float minDist;
            if (isEnemy)
            {
                minDist = zapRange + 1.0f;
            }
            else
            {
                minDist = GameManager.gameManager.maxDistance + 2;
            }
			
            hasHitObject = false;

            Collider[] objectsInRange;
            if (isEnemy)
            {
                objectsInRange = Physics.OverlapSphere(actualPos, zapRange, enemyLayerMask);
            }
            else
            {
                objectsInRange = Physics.OverlapSphere(actualPos, GameManager.gameManager.maxDistance + 1, playerLayerMask);
            }

			foreach(Collider col in objectsInRange)
			{
				float dist = Vector3.Distance(actualPos, col.transform.position);
				if (dist < minDist && !zappedObjects.Contains(col.gameObject))
				{
					minDist = dist;
                    nearestObject = col.gameObject;
				}
			}

			if (nearestObject != null)
			{
				rod.transform.position = actualPos;
				rod.target = nearestObject;
                if (isEnemy)
                {
                    nearestObject.GetComponent<Enemy>().TakeDamage(zapDamageEnemy);
                }
                else
                {
                    GameManager.gameManager.TakeDamage(nearestObject, zapDamagePlayer, Vector3.zero, false);
                }
				actualPos = nearestObject.transform.position;
                if (!isEnemy)
                {
                    actualPos += Vector3.up;
                }
                zappedObjects.Add(nearestObject);
				zapNb++;
                hasHitObject = true;
			}
			yield return new WaitForSeconds(timeBetweenZap);
		}

		Destroy(rod.gameObject);
	}

    #endregion

    #region Darkness
    //==========DARKNESS==========

    void ActivateDarkness()
    {
        elementalPower = GameManager.PowerType.Darkness;
        //GetComponent<MeshRenderer>().material = darknessMaterial;
        VFX.transform.GetChild(1).gameObject.transform.GetChild(0).gameObject.SetActive(VFX.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.activeSelf);
        VFX.transform.GetChild(1).gameObject.transform.GetChild(4).gameObject.SetActive(VFX.transform.GetChild(0).gameObject.transform.GetChild(4).gameObject.activeSelf);
        VFX.transform.GetChild(1).gameObject.transform.GetChild(5).gameObject.SetActive(VFX.transform.GetChild(0).gameObject.transform.GetChild(5).gameObject.activeSelf);
        VFX.transform.GetChild(1).gameObject.transform.GetChild(6).gameObject.SetActive(VFX.transform.GetChild(0).gameObject.transform.GetChild(6).gameObject.activeSelf);

        if (behaviouralPower == GameManager.PowerType.LargeOrb)
        {
            for (int i = 0; i < 4; i++)
            {
                VFX.transform.GetChild(0).gameObject.transform.GetChild(i).transform.localScale /= maxScale;
            }

            for (int i = 0; i < 4; i++)
            {
                VFX.transform.GetChild(1).gameObject.transform.GetChild(i).transform.localScale *= maxScale;
            }
        }

        VFX.transform.GetChild(0).gameObject.SetActive(false);
        VFX.transform.GetChild(1).gameObject.SetActive(true);
        elementalDurationCoroutine = StartCoroutine(DurationCoroutine(GameManager.PowerType.Darkness, darknessDuration));
    }

    void DeactivateDarkness()
    {
        elementalPower = GameManager.PowerType.None;
        //GetComponent<MeshRenderer>().material = normalMaterial;
        VFX.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.SetActive(VFX.transform.GetChild(1).gameObject.transform.GetChild(0).gameObject.activeSelf);
        VFX.transform.GetChild(0).gameObject.transform.GetChild(4).gameObject.SetActive(VFX.transform.GetChild(1).gameObject.transform.GetChild(4).gameObject.activeSelf);
        VFX.transform.GetChild(0).gameObject.transform.GetChild(5).gameObject.SetActive(VFX.transform.GetChild(1).gameObject.transform.GetChild(5).gameObject.activeSelf);
        VFX.transform.GetChild(0).gameObject.transform.GetChild(6).gameObject.SetActive(VFX.transform.GetChild(1).gameObject.transform.GetChild(6).gameObject.activeSelf);

        if (behaviouralPower == GameManager.PowerType.LargeOrb)
        {
            for (int i = 0; i < 4; i++)
            {
                VFX.transform.GetChild(1).gameObject.transform.GetChild(i).transform.localScale /= maxScale;
            }

            for (int i = 0; i < 4; i++)
            {
                VFX.transform.GetChild(0).gameObject.transform.GetChild(i).transform.localScale *= maxScale;
            }
        }

        VFX.transform.GetChild(1).gameObject.SetActive(false);
        VFX.transform.GetChild(0).gameObject.SetActive(true);
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

        switch (behaviouralPower)
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
                //update in score manager
                ScoreManager.scoreManager.statusAilmentApplied++;

                enemy.actualFreezeCoroutine = enemy.StartCoroutine(enemy.FreezeCoroutine(freezeDuration));
                damageTaken += iceDamage;
                break;
            case GameManager.PowerType.Fire:
                //update in score manager
                ScoreManager.scoreManager.statusAilmentApplied++;

                if (enemy.actualFireCoroutine != null)
					StopCoroutine(enemy.actualFireCoroutine);
				if(isActivatedByBrazier)
                    enemy.actualFireCoroutine = enemy.StartCoroutine(enemy.FireDamage(enemy.gameObject, fireTicksDamageBrazier, fireTickDurationBrazier));
				else
                    enemy.actualFireCoroutine = enemy.StartCoroutine(enemy.FireDamage(enemy.gameObject, fireTicksDamage, fireTickDuration));
                damageTaken += fireDamage;
                break;
            case GameManager.PowerType.Electric:
                //update in score manager
                ScoreManager.scoreManager.statusAilmentApplied++;

                StartCoroutine(ElectricZappingCoroutine(transform.position, target, true));
                damageTaken += electricDamage;
				DeactivatePower(elementalPower);
                break;
            case GameManager.PowerType.Darkness:
                //update in score manager
                ScoreManager.scoreManager.statusAilmentApplied++;

                enemy.actualDarknessCoroutine = enemy.StartCoroutine(enemy.DarknessCoroutine(darknessTimer));
                break;
        }

        //update in score manager (to keep track of who's given the last hit)
        if (GameManager.gameManager.orb.GetComponent<OrbController>().toPlayer2)
        {
            enemy.lastHitByP1 = true;
            enemy.lastHitByP2 = false;
        }
        else
        {
            enemy.lastHitByP1 = false;
            enemy.lastHitByP2 = true;
        }

        enemy.TakeDamage(damageTaken);
        

        if (behaviouralPower == GameManager.PowerType.LeechLife)
        {
            OrbController controller = GameManager.gameManager.orb.GetComponent<OrbController>();
            if (controller.toPlayer2)
                GameManager.gameManager.spawnHealingOrbs(1, (int)(damageTaken / (100 / lifeSteel)), "leechLife");
            else
                GameManager.gameManager.spawnHealingOrbs(2, (int)(damageTaken / (100 / lifeSteel)), "leechLife");
        }
    }

    
    public void onBossHit(GameObject target)
    {
        BossSystem bossSystem = target.GetComponent<BossSystem>();
        

        int bonusDamage = (orbController.combo / orbController.damageIncreaseStep) * orbController.damageComboIncrease;
        if (bonusDamage > orbController.maxComboDamage)
        {
            bonusDamage = orbController.maxComboDamage;
        }
        int damageTaken = baseDamage + bonusDamage;

        switch (behaviouralPower)
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
            case GameManager.PowerType.Fire:
                //update in score manager
                ScoreManager.scoreManager.statusAilmentApplied++;

                if (bossSystem.actualFireCoroutine != null)
                    StopCoroutine(bossSystem.actualFireCoroutine);
                if (isActivatedByBrazier)
                    bossSystem.actualFireCoroutine = bossSystem.StartCoroutine(bossSystem.FireDamage(bossSystem.gameObject, fireTicksDamageBrazier, fireTickDurationBrazier));
                else
                    bossSystem.actualFireCoroutine = bossSystem.StartCoroutine(bossSystem.FireDamage(bossSystem.gameObject, fireTicksDamage, fireTickDuration));
                damageTaken += fireDamage;
                break;

            case GameManager.PowerType.Electric:
                //update in score manager
                ScoreManager.scoreManager.statusAilmentApplied++;

                StartCoroutine(ElectricZappingCoroutine(transform.position, target, true));
                damageTaken += electricDamage;
                DeactivatePower(elementalPower);
                break;
        }
        
        //apply damage to boss
        bossSystem.TakeDamage(damageTaken);


        //update in score manager (to keep track of who's given the last hit)
        if (GameManager.gameManager.orb.GetComponent<OrbController>().toPlayer2)
        {
            ScoreManager.scoreManager.damageDealtBossP1 += damageTaken;
            bossSystem.lastHitByP1 = true;
            bossSystem.lastHitByP2 = false;
        }
        else
        {
            ScoreManager.scoreManager.damageDealtBossP2 += damageTaken;
            bossSystem.lastHitByP1 = false;
            bossSystem.lastHitByP2 = true;
        }

        //heals players if leachlife is on
        if (behaviouralPower == GameManager.PowerType.LeechLife)
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
        if (behaviouralPower == GameManager.PowerType.Vortex)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, vortexRangeOfEffect);
        }
    }

	public void RespawnReset()
	{
		droppedPower = GameManager.PowerType.None;
		GameManager.gameManager.UIManager.UpdateDroppedPower(droppedPower);
		DeactivatePower(elementalPower);
		DeactivatePower(behaviouralPower);
		StopAllCoroutines();
		canBeActivatedByPlayer1 = new List<bool> { true, true, true, true, true, true, true, true, true };
		canBeActivatedByPlayer2 = new List<bool> { true, true, true, true, true, true, true, true, true };
	}
}