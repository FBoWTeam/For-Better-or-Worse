using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerController : MonoBehaviour
{
    public GameManager.PowerType elementalPower;
    public GameManager.PowerType behavioralPower;

	public int baseDamage;

	public List<bool> canBeActivated;

	//LargeOrb
	[Header("[LargeOrb Param]")]
	public float largeOrbCooldown;
    public float minScale;
    public float maxScale;
    public int largeOrbDamage;

	//Vortex
	[Header("[Vortex Param]")]
	public float vortexCooldown;

	//LeechLife
	[Header("[LeechLife Param]")]
	public float leechLifeCooldown;
    [Range(0, 100f)]
    public float lifeSteel;

	//Slug
	[Header("[Slug Param]")]
	public float slugCooldown;
    public GameObject slug;
    public float durationSlugPower;

	//Shield
	[Header("[Shield Param]")]
	public float shieldCooldown;
    [Tooltip("Reduce the damage of the orb")]
    public int mitigatedDamage;
    public int shieldAmount;
    [Tooltip("number of stacks that gives shield when the orb is hit")]
    public int currentShieldStack;


	//Ice
	[Header("[Ice Param]")]
	public float iceCooldown;

	//Fire
	[Header("[Fire Param]")]
	public float fireCooldown;
    public GameObject fireParticleSystem;
    [Tooltip("Damage is over time , should be >= to fireDuration")]
    public int fireDamage = 5;
    public float fireDuration = 5;
    public float fireCoolDown = 8;
    private float nextAttack = 0f;

	//Electric
	[Header("[Electric Param]")]
	public float electricCooldown;

	//Weakness
	[Header("[Weakness Param]")]
	public float weaknessCooldown;


	private void Start()
    {
		canBeActivated = new List<bool> {true, true, true, true, true, true, true, true, true};
	}

    /// <summary>
    /// Activate the powerToActivate, deactivate the power of the same type if there's already an active one
    /// </summary>
    /// <param name="powerToActivate"></param>
    public void ActivatePower(GameManager.PowerType powerToActivate)
    {
		if(canBeActivated[(int)powerToActivate - 1])
		{
			if (powerToActivate == GameManager.PowerType.Elemental && elementalPower != GameManager.PowerType.None)
			{
				DeactivatePower(elementalPower);
			}
			else if (powerToActivate == GameManager.PowerType.Behavioral && behavioralPower != GameManager.PowerType.None)
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
					StartCoroutine(cooldownCoroutine(GameManager.PowerType.Fire, fireCooldown));
					ActivateFire();
					break;
				case GameManager.PowerType.Electric:
					StartCoroutine(cooldownCoroutine(GameManager.PowerType.Electric, electricCooldown));
					ActivateElectric();
					break;
				case GameManager.PowerType.Weakness:
					StartCoroutine(cooldownCoroutine(GameManager.PowerType.Weakness, weaknessCooldown));
					ActivateWeakness();
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
            case GameManager.PowerType.Weakness:
                DeactivateWeakness();
                break;
            default:
                break;
        }
    }

    //==========BEHAVIORAL POWERS FUNCTIONS==========

    #region LargeOrb
    //==========LARGEORB==========
    /// <summary>
    /// set the behavioral power to LargeOrb and resize the orb
    /// </summary>
    void ActivateLargeOrb()
    {
        behavioralPower = GameManager.PowerType.LargeOrb;
        transform.localScale = new Vector3(maxScale, maxScale, maxScale);
        baseDamage += largeOrbDamage;
    }

    void DeactivateLargeOrb()
    {
        behavioralPower = GameManager.PowerType.None;
        transform.localScale = new Vector3(minScale, minScale, minScale);
        baseDamage -= largeOrbDamage;
    }

    #endregion

    #region Vortex
    //==========VORTEX==========

    void ActivateVortex()
    {
        behavioralPower = GameManager.PowerType.Vortex;
    }

    void DeactivateVortex()
    {
        behavioralPower = GameManager.PowerType.None;
    }

    #endregion

    #region LeechLife
    //==========LEACHLIFE==========

    void ActivateLeechLife()
    {
        behavioralPower = GameManager.PowerType.LeechLife;
    }

    void DeactivateLeechLife()
    {
        behavioralPower = GameManager.PowerType.None;
    }

    #endregion

    #region Slug
    //==========SLUG==========

    void ActivateSlug()
    {
        behavioralPower = GameManager.PowerType.Slug;
        StartCoroutine("InstanciateSlug");
    }

    void DeactivateSlug()
    {
        behavioralPower = GameManager.PowerType.None;
    }

    IEnumerator InstanciateSlug()
    {
        float timeStamp = Time.time;
        while (Time.time - timeStamp <= durationSlugPower)
        {
            Instantiate(slug, new Vector3(transform.position.x, 0.2f, transform.position.z), Quaternion.identity);
            yield return new WaitForSeconds(0.2f);
        }
    }

    #endregion

    #region Shield
    //==========SHIELD==========

    void ActivateShield()
    {
        behavioralPower = GameManager.PowerType.Shield;
        baseDamage -= mitigatedDamage;
        currentShieldStack = 2;
    }

    void DeactivateShield()
    {
        behavioralPower = GameManager.PowerType.None;
        baseDamage += mitigatedDamage;
        GameManager.gameManager.shieldP1 = 0;
        GameManager.gameManager.shieldP2 = 0;
    }

    #endregion

    //==========ELEMENTAL POWERS FUNCTIONS==========

    #region Ice
    //==========ICE==========

    void ActivateIce()
    {
        elementalPower = GameManager.PowerType.Ice;
    }

    void DeactivateIce()
    {
        elementalPower = GameManager.PowerType.None;
    }

    #endregion

    #region Fire
    //==========FIRE==========
    // QUESTIONS : what do we do if we hit a ennemy that is already inFIRE ?
    IEnumerator FireDamage(Enemy enemy,int totalDamage,float duration,float cooldown) {

        int tickDamage = Mathf.RoundToInt(totalDamage / duration);
        int curentDamage = 0;

        while (curentDamage < totalDamage) {
            enemy.TakeDamage(tickDamage);           
            yield return new WaitForSeconds(1f);         
            curentDamage += tickDamage;
        }
        
        //DestroyImmediate(fireParticleSystem,true);
        yield return null;
    }

    void ActivateFire()
    {
        elementalPower = GameManager.PowerType.Fire;
    }

    void DeactivateFire()
    {
        elementalPower = GameManager.PowerType.None;
    }

    #endregion

    #region Electric
    //==========ELECTRIC==========

    void ActivateElectric()
    {
        elementalPower = GameManager.PowerType.Electric;
    }

    void DeactivateElectric()
    {
        elementalPower = GameManager.PowerType.None;
    }

    #endregion

    #region Weakness
    //==========WEAKNESS==========

    void ActivateWeakness()
    {
        elementalPower = GameManager.PowerType.Weakness;
    }

    void DeactivateWeakness()
    {
        elementalPower = GameManager.PowerType.None;
    }

    #endregion

    public void onEnemyHit(GameObject target)
    {
        Enemy enemy = target.GetComponent<Enemy>();
        enemy.TakeDamage(baseDamage);

        
        //check if the orb has the power LeechLife and apply the effect
        if (gameObject.GetComponent<PowerController>().behavioralPower == GameManager.PowerType.LeechLife)
        {
            GameManager.gameManager.hp += (int)(baseDamage / (100 / gameObject.GetComponent<PowerController>().lifeSteel));
        }

        switch (elementalPower) {          
            case GameManager.PowerType.Ice:
                Debug.Log("Slow down bitch"); 
                break;
            case GameManager.PowerType.Fire:              
                if (Time.time > nextAttack) {
                    StartCoroutine(FireDamage(enemy, fireDamage, fireDuration, fireCoolDown));
                    nextAttack = Time.time + fireCoolDown;
                }
               
                break;
            case GameManager.PowerType.Electric:
                break;
        }
    }

	public IEnumerator cooldownCoroutine(GameManager.PowerType power, float cooldown)
	{
		canBeActivated[(int)power-1] = false;
		yield return new WaitForSeconds(cooldown);
		canBeActivated[(int)power-1] = true;
	}
}


    
