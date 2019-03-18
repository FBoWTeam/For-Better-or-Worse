using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuddleSystem : MonoBehaviour
{
    [Header("[Puddle Type]")]
    public GameManager.PuddleType puddleType;

    #region Puddle parameters

    //===== COMMON PARAMETERS
    [Header("[Puddle common param]")]
    public Material standardMaterial;
    [Header("[Puddle Effect Editing]")]
    public GameManager.PuddleType editingPuddleType;

    //===== SLUG
    [DrawIf(new string[] { "editingPuddleType" }, GameManager.PuddleType.Slug)]
    public float slugLifeTime;
    [DrawIf(new string[] { "editingPuddleType" }, GameManager.PuddleType.Slug)]
    [Tooltip("Amount in percentage of the slow")]
    [Range(1, 100)]
    public float slugSlowAmount;
    [DrawIf(new string[] { "editingPuddleType" }, GameManager.PuddleType.Slug)]
    public Material slugMaterial;

    //===== ACID
    [DrawIf(new string[] { "editingPuddleType" }, GameManager.PuddleType.Acid)]
    public int acidDamage;
    [DrawIf(new string[] { "editingPuddleType" }, GameManager.PuddleType.Acid)]
    public Material acidMaterial;

    //===== WATER
    [DrawIf(new string[] { "editingPuddleType" }, GameManager.PuddleType.Water)]
    public int electrifiedWaterLifeTime;
    [DrawIf(new string[] { "editingPuddleType" }, GameManager.PuddleType.Water)]
    public int frozenWaterLifeTime;
    [DrawIf(new string[] { "editingPuddleType" }, GameManager.PuddleType.Water)]
    public int freezeTime;
    [DrawIf(new string[] { "editingPuddleType" }, GameManager.PuddleType.Water)]
    public bool electrified;
    [DrawIf(new string[] { "editingPuddleType" }, GameManager.PuddleType.Water)]
    public bool frozen;
    [DrawIf(new string[] { "editingPuddleType" }, GameManager.PuddleType.Water)]
    public Material waterMaterial;
    [DrawIf(new string[] { "editingPuddleType" }, GameManager.PuddleType.Water)]
    public Material ElectrifiedWaterMaterial;
    [DrawIf(new string[] { "editingPuddleType" }, GameManager.PuddleType.Water)]
    public Material frozenWaterMaterial;
    private Coroutine frozenWaterCoroutine;
    private Coroutine electrifiedWaterCoroutine;
    private List<GameObject> objectsPresent;


    //===== MUD
    [DrawIf(new string[] { "editingPuddleType" }, GameManager.PuddleType.Mud)]
    [Range(1, 100)]
    public float mudSlowAmount;
    [DrawIf(new string[] { "editingPuddleType" }, GameManager.PuddleType.Mud)]
    public Material mudMaterial;

    //===== FLAMMABLE
    [DrawIf(new string[] { "editingPuddleType" }, GameManager.PuddleType.Flammable)]
    public int onFireFlammableLifeTime;
    [DrawIf(new string[] { "editingPuddleType" }, GameManager.PuddleType.Flammable)]
    public bool onFire;
    [DrawIf(new string[] { "editingPuddleType" }, GameManager.PuddleType.Flammable)]
    public int burnTotalDamage;
    [DrawIf(new string[] { "editingPuddleType" }, GameManager.PuddleType.Flammable)]
    public int burnTime;
    [DrawIf(new string[] { "editingPuddleType" }, GameManager.PuddleType.Flammable)]
    public Material flammableMaterial;
    [DrawIf(new string[] { "editingPuddleType" }, GameManager.PuddleType.Flammable)]
    public Material onFireFlammableMaterial;


    //===== TIMERS
    private float timeStamp;
    private float delayDOT;


    #endregion


    private void Start()
    {
        objectsPresent = new List<GameObject>();
        delayDOT = 1f;
        switch (puddleType)
        {
            case GameManager.PuddleType.None:
                GetComponent<MeshRenderer>().material = standardMaterial;
                break;
            case GameManager.PuddleType.Slug:
                GetComponent<MeshRenderer>().material = slugMaterial;
                Destroy(gameObject, slugLifeTime);
                break;
            case GameManager.PuddleType.Acid:
                GetComponent<MeshRenderer>().material = acidMaterial;
                break;
            case GameManager.PuddleType.Water:
                GetComponent<MeshRenderer>().material = waterMaterial;
                break;
            case GameManager.PuddleType.Flammable:
                GetComponent<MeshRenderer>().material = flammableMaterial;
                break;
            case GameManager.PuddleType.Mud:
                GetComponent<MeshRenderer>().material = mudMaterial;
                break;
        }
    }


    #region On trigger Enter / Stay / Exit 
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy") || other.CompareTag("Player") || other.CompareTag("Orb"))
        {
            switch (puddleType)
            {
                case GameManager.PuddleType.Slug:
                    OnEnterSlug(other.gameObject);
                    break;
                case GameManager.PuddleType.Water:
                    OnEnterWater(other.gameObject);
                    break;
                case GameManager.PuddleType.Flammable:
                    OnEnterFlammable(other.gameObject);
                    break;
                case GameManager.PuddleType.Acid:
                    OnEnterAcid(other.gameObject);
                    break;
                case GameManager.PuddleType.Mud:
                    OnEnterMud(other.gameObject);
                    break;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy") || other.CompareTag("Player"))
        {
            switch (puddleType)
            {
                case GameManager.PuddleType.Water:
                    OnExitWater(other.gameObject);
                    break;
                case GameManager.PuddleType.Slug:
                    OnExitSlug(other.gameObject);
                    break;
                case GameManager.PuddleType.Mud:
                    OnExitMud(other.gameObject);
                    break;
                case GameManager.PuddleType.Flammable:
                    OnExitFlammable(other.gameObject);
                    break;
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Enemy") || other.CompareTag("Player"))
        {
            if (Time.time - timeStamp > delayDOT)
            {
                timeStamp = Time.time;
                switch (puddleType)
                {
                    case GameManager.PuddleType.Acid:
                        OnStayAcid(other.gameObject);
                        break;
                    case GameManager.PuddleType.Flammable:
                        OnStayFlammable(other.gameObject);
                        break;
                }
            }
        }
    }

    #endregion

    #region Puddle Effects

    #region Slug Effect
    void OnEnterSlug(GameObject target)
    {
        if (target.CompareTag("Enemy"))
        {
            target.GetComponent<EnemyMovement>().SlowSpeed(slugSlowAmount);
        }
    }

    void OnExitSlug(GameObject target)
    {
        if (target.CompareTag("Enemy"))
        {
            target.GetComponent<EnemyMovement>().RestoreSpeed();
        }
    }
    #endregion


    #region Acid Effect
    void OnEnterAcid(GameObject target)
    {
        if (target.CompareTag("Orb") && target.GetComponent<PowerController>().elementalPower == GameManager.PowerType.Ice)
        {
            target.GetComponent<PowerController>().DeactivatePower(GameManager.PowerType.Ice);
        }
    }

    void OnStayAcid(GameObject target)
    {
        if (target.CompareTag("Player"))
        {
            GameManager.gameManager.TakeDamage(target, acidDamage, Vector3.zero, false);
        }
        if (target.CompareTag("Enemy"))
        {
            target.GetComponent<Enemy>().TakeDamage(acidDamage);
        }
    }
    #endregion


    #region Water Effect
    void OnEnterWater(GameObject target)
    {
        if (target.CompareTag("Orb"))
        {
            if (target.GetComponent<PowerController>().elementalPower == GameManager.PowerType.Ice && !frozen)
            {
                if (electrifiedWaterCoroutine != null)
                {
                    StopCoroutine(electrifiedWaterCoroutine);
                }
                frozen = true;

                GetComponent<MeshRenderer>().material = frozenWaterMaterial;

                for (int i = 0; i < objectsPresent.Count; i++)
                {
                    if (objectsPresent[i].gameObject.CompareTag("Enemy"))
                    {
						Enemy e = objectsPresent[i].gameObject.GetComponent<Enemy>();
						e.actualFreezeCoroutine = e.StartCoroutine(e.FreezeCoroutine(freezeTime));
                    }
                    else if (objectsPresent[i].gameObject.CompareTag("Player"))
                    {
                        objectsPresent[i].gameObject.GetComponent<PlayerController>().StartCoroutine(objectsPresent[i].gameObject.GetComponent<PlayerController>().FreezeCoroutine(freezeTime));
                    }
                }

                frozenWaterCoroutine = StartCoroutine(ReturnToWater(frozenWaterLifeTime));
            }
            else if (target.GetComponent<PowerController>().elementalPower == GameManager.PowerType.Electric && !electrified && !frozen)
            {
                electrified = true;
                GetComponent<Collider>().enabled = false;
                GetComponent<Collider>().enabled = true;
                GetComponent<MeshRenderer>().material = ElectrifiedWaterMaterial;
                electrifiedWaterCoroutine = StartCoroutine(ReturnToWater(electrifiedWaterLifeTime));
            }
            else if (target.GetComponent<PowerController>().elementalPower == GameManager.PowerType.Fire)
            {
                target.GetComponent<PowerController>().DeactivatePower(GameManager.PowerType.Fire);
                GetComponent<MeshRenderer>().material = waterMaterial;
                if (frozen)
                {
                    frozen = false;
                }
            }
        }
        else if (electrified)
        {
            if ((target.CompareTag("Enemy")))
            {
                StartCoroutine(GameManager.gameManager.orb.GetComponent<PowerController>().ElectricZappingCoroutine(transform.position + Vector3.up, null, true));
            }
            else if (target.CompareTag("Player"))
            {
                StartCoroutine(GameManager.gameManager.orb.GetComponent<PowerController>().ElectricZappingCoroutine(transform.position + Vector3.up, null, false));
            }
        }
        else if (target.CompareTag("Enemy") || target.CompareTag("Player"))
        {
            objectsPresent.Add(target.gameObject);
        }
    }

    void OnExitWater(GameObject target)
    {
        objectsPresent.Remove(target.gameObject);
    }

    IEnumerator ReturnToWater(float timeToWater)
    {
        yield return new WaitForSeconds(timeToWater);
        if (frozen)
        {
            frozen = false;
        }
        else if (electrified)
        {
            electrified = false;
        }
        GetComponent<MeshRenderer>().material = waterMaterial;
    }

    #endregion


    #region Flammable Effect
    void OnEnterFlammable(GameObject target)
    {
        if (target.CompareTag("Orb"))
        {
            if (target.GetComponent<PowerController>().elementalPower == GameManager.PowerType.Fire && !onFire)
            {
                onFire = true;
                GetComponent<MeshRenderer>().material = onFireFlammableMaterial;
                Destroy(gameObject, onFireFlammableLifeTime);
            }
            else if (target.GetComponent<PowerController>().elementalPower == GameManager.PowerType.Ice && onFire)
            {
                target.GetComponent<PowerController>().DeactivatePower(GameManager.PowerType.Ice);
            }
        }
    }
    void OnStayFlammable(GameObject target)
    {
        if (onFire)
        {
            if (target.CompareTag("Player"))
            {
                GameManager.gameManager.TakeDamage(target, (int)burnTotalDamage / burnTime, Vector3.zero, false);
            }
            if (target.CompareTag("Enemy"))
            {
                target.GetComponent<Enemy>().TakeDamage((int)burnTotalDamage / burnTime);
            }
        }
    }
    void OnExitFlammable(GameObject target)
    {
        if (onFire)
        {
            StartCoroutine(Burn(target));
        }
    }

    IEnumerator Burn(GameObject target)
    {
        int tickDamage = Mathf.RoundToInt(burnTotalDamage / burnTime);
        int currentDamage = 0;

        if (target.CompareTag("Player"))
        {
            while (currentDamage < burnTotalDamage)
            {
                if (target.gameObject != null)
                {
                    GameManager.gameManager.TakeDamage(target, tickDamage, Vector3.zero, false);
                }
                yield return new WaitForSeconds(1f);
                currentDamage += tickDamage;
            }
        }
        else if (target.CompareTag("Enemy"))
        {
            while (currentDamage < burnTotalDamage)
            {
                if (target.gameObject != null)
                {
                    target.GetComponent<Enemy>().TakeDamage(tickDamage);
                }
                yield return new WaitForSeconds(1f);
                currentDamage += tickDamage;
            }
        }
    }
    #endregion


    #region Mud Effect
    void OnEnterMud(GameObject target)
    {
        if (target.CompareTag("Orb") && target.GetComponent<PowerController>().elementalPower == GameManager.PowerType.Electric)
        {
            target.GetComponent<PowerController>().DeactivatePower(GameManager.PowerType.Electric);
        }
        else if (target.CompareTag("Player"))
        {
            target.GetComponent<PlayerController>().SlowSpeed(mudSlowAmount);
        }
        else if (target.CompareTag("Enemy"))
        {
            target.GetComponent<EnemyMovement>().SlowSpeed(mudSlowAmount);
        }
    }
    void OnExitMud(GameObject target)
    {
        if (target.CompareTag("Player"))
        {
            target.GetComponent<PlayerController>().RestoreSpeed();
        }
        if (target.CompareTag("Enemy"))
        {
            target.GetComponent<EnemyMovement>().RestoreSpeed();
        }
    }
    #endregion

    #endregion
}
