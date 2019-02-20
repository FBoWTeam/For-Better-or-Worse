using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerController : MonoBehaviour
{
    public GameManager.PowerType elementalPower;
    public GameManager.PowerType behavioralPower;

    public int baseDamage;

    //LargeOrb
    [Header("[GreatOrb Param]")]
    public float minScale;
    public float maxScale;
    public int LargeOrbDamage;

    //LeechLife
    [Header("[LeechLife Param]")]
    [Range(0, 100f)]
    public float lifeSteel;

    //Slug
    [Header("[Slug Param]")]
    public GameObject slug;
    public float durationSlugPower;
    
    //Shield
    [Header("[Shield Param]")]
    [Tooltip("Reduce the damage of the orb")]
    public int mitigatedDamage;
    public int shieldAmount;
    [Tooltip("number of stacks that gives shield when the orb is hit")]
    public int currentShieldStack;


    
    //Fire
    [Header("[Fire Param]")]
    public GameObject fireParticleSystem;
    [Tooltip("Damage is over time , should be >= to fireDuration")]
    public int fireDamage = 5;
    public float fireDuration = 5;
    public float fireCoolDown = 8;

    private float nextAttack = 0f;

    private void Start()
    {
        elementalPower = GameManager.PowerType.Fire;
    }

    /// <summary>
    /// Activate the powerToActivate, deactivate the power of the same type if there's already an active one
    /// </summary>
    /// <param name="powerToActivate"></param>
    public void ActivatePower(GameManager.PowerType powerToActivate)
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
                ActivateLargeOrb();
                break;
            case GameManager.PowerType.Vortex:
                ActivateVortex();
                break;
            case GameManager.PowerType.LeechLife:
                ActivateLeechLife();
                break;
            case GameManager.PowerType.Slug:
                ActivateSlug();
                break;
            case GameManager.PowerType.Shield:
                ActivateShield();
                break;
            case GameManager.PowerType.Ice:
                ActivateIce();
                break;
            case GameManager.PowerType.Fire:
                ActivateFire();
                break;
            case GameManager.PowerType.Electric:
                ActivateElectric();
                break;
            case GameManager.PowerType.Weakness:
                ActivateWeakness();
                break;
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
        baseDamage += LargeOrbDamage;
    }

    void DeactivateLargeOrb()
    {
        behavioralPower = GameManager.PowerType.None;
        transform.localScale = new Vector3(minScale, minScale, minScale);
        baseDamage -= LargeOrbDamage;
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
        Instantiate(fireParticleSystem, enemy.transform.position, Quaternion.identity);
        fireParticleSystem.GetComponent<ParticleSystem>().Play();

        while (curentDamage < totalDamage) {
            enemy.TakeDamage(tickDamage);           
            yield return new WaitForSeconds(1f);         
            curentDamage += tickDamage;
        }

        print("stop");      
        fireParticleSystem.GetComponent<ParticleSystem>().Stop();
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


}


    
