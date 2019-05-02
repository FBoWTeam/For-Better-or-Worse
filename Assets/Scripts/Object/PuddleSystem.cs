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
    public bool electrified;
    [DrawIf(new string[] { "editingPuddleType" }, GameManager.PuddleType.Water)]
    public int electrifiedWaterLifeTime;
    [DrawIf(new string[] { "editingPuddleType" }, GameManager.PuddleType.Water)]
    public int electricDamage;
    [DrawIf(new string[] { "editingPuddleType" }, GameManager.PuddleType.Water)]
    public bool frozen;
    [DrawIf(new string[] { "editingPuddleType" }, GameManager.PuddleType.Water)]
    public int frozenWaterLifeTime;
    [DrawIf(new string[] { "editingPuddleType" }, GameManager.PuddleType.Water)]
    public int freezeTime;


    [DrawIf(new string[] { "editingPuddleType" }, GameManager.PuddleType.Water)]
    public Material waterMaterial;
    [DrawIf(new string[] { "editingPuddleType" }, GameManager.PuddleType.Water)]
    public Material ElectrifiedWaterMaterial;
    [DrawIf(new string[] { "editingPuddleType" }, GameManager.PuddleType.Water)]
    public Material frozenWaterMaterial;
    private Coroutine frozenWaterCoroutine;
    private Coroutine electrifiedWaterCoroutine;


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



    private List<GameObject> objectsInPuddle;


    //===== TIMERS
    private float timeStamp;
    private float delayDOT;
    
    #endregion

    private void Start()
    {
        objectsInPuddle = new List<GameObject>();
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
                //GetComponent<MeshRenderer>().material = waterMaterial;
                transform.GetChild(0).gameObject.SetActive(true);
                break; 
            case GameManager.PuddleType.Flammable:
                GetComponent<MeshRenderer>().material = flammableMaterial;
                if (onFire)
                {
                    ActivateFlammable();
                }
                break;
            case GameManager.PuddleType.Mud:
                //GetComponent<MeshRenderer>().material = mudMaterial;
                transform.GetChild(4).gameObject.SetActive(true);
                break;
        }
    }


    private void Update()
    {
        if (Time.time - timeStamp > delayDOT)
        {
            timeStamp = Time.time;
            CleanNullInEnemyList();
            switch (puddleType)
            {
                case GameManager.PuddleType.Acid:
                    if (objectsInPuddle.Count > 0)
                    {
                        for (int i = 0; i < objectsInPuddle.Count; i++)
                        {
                            if (objectsInPuddle[i].CompareTag("Enemy"))
                            {
                                objectsInPuddle[i].GetComponent<Enemy>().TakeDamage(acidDamage);
                            }
                            if (objectsInPuddle[i].CompareTag("Player"))
                            {
                                GameManager.gameManager.TakeDamage(objectsInPuddle[i], acidDamage, Vector3.zero, false);
                            }
                        }
                    }
                    break;

                case GameManager.PuddleType.Flammable:
                    if (objectsInPuddle.Count > 0 && onFire)
                    {
                        for (int i = 0; i < objectsInPuddle.Count; i++)
                        {
                            if (objectsInPuddle[i].CompareTag("Enemy"))
                            {
                                objectsInPuddle[i].GetComponent<Enemy>().TakeDamage((int)burnTotalDamage / burnTime);
                            }
                            if (objectsInPuddle[i].CompareTag("Player"))
                            {
                                GameManager.gameManager.TakeDamage(objectsInPuddle[i], (int)burnTotalDamage / burnTime, Vector3.zero, false);
                            }
                        }
                    }
                    break;
            }
            CleanNullInEnemyList();
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

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Enemy") || other.CompareTag("Player") || other.CompareTag("Orb"))
        {
            switch (puddleType)
            {
                case GameManager.PuddleType.Acid:
                    OnStayAcid(other.gameObject);
                    break;
                case GameManager.PuddleType.Water:
                    OnStayWater(other.gameObject);
                    break;
                case GameManager.PuddleType.Flammable:
                    OnStayFlammable(other.gameObject);
                    break;
                case GameManager.PuddleType.Mud:
                    OnStayMud(other.gameObject);
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
                case GameManager.PuddleType.Acid:
                    OnExitAcid(other.gameObject);
                    break;
            }
        }
    }

    private void OnDestroy()
    {
        if (puddleType == GameManager.PuddleType.Flammable && onFire)
        {
            for (int i = 0; i < objectsInPuddle.Count; i++)
            {
                if (objectsInPuddle[i].CompareTag("Enemy"))
                {
                    objectsInPuddle[i].GetComponent<EnemyMovement>().StartCoroutine(Burn(objectsInPuddle[i]));
                }
                else if (objectsInPuddle[i].CompareTag("Player"))
                {
                    objectsInPuddle[i].GetComponent<PlayerController>().StartCoroutine(Burn(objectsInPuddle[i]));
                }
            }
        }
        if (puddleType == GameManager.PuddleType.Mud)
        {
            for (int i = 0; i < objectsInPuddle.Count; i++)
            {
                if (objectsInPuddle[i].CompareTag("Player"))
                {
                    objectsInPuddle[i].GetComponent<PlayerController>().RestoreSpeed();
                }
                if (objectsInPuddle[i].CompareTag("Enemy"))
                {
                    objectsInPuddle[i].GetComponent<EnemyMovement>().RestoreSpeed();
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
        if (target.CompareTag("Player"))
        {
            target.GetComponent<PlayerController>().SlowSpeed(slugSlowAmount);
        }
    }

    void OnExitSlug(GameObject target)
    {
        if (target.CompareTag("Enemy"))
        {
            target.GetComponent<EnemyMovement>().RestoreSpeed();
        }
        if (target.CompareTag("Player"))
        {
            target.GetComponent<PlayerController>().RestoreSpeed();
        }
    }
    #endregion


    #region Acid Effect
    void OnEnterAcid(GameObject target)
    {
        if (target.CompareTag("Player") || target.CompareTag("Enemy"))
        {
            objectsInPuddle.Add(target);
        }
    }

    void OnStayAcid(GameObject target)
    {
        if (target.CompareTag("Orb") && target.GetComponent<PowerController>().elementalPower == GameManager.PowerType.Ice)
        {
            target.GetComponent<PowerController>().DeactivatePower(GameManager.PowerType.Ice);
        }
    }

    void OnExitAcid(GameObject target)
    {
        objectsInPuddle.Remove(target);
    }
    #endregion


    #region Water Effect
    void OnEnterWater(GameObject target)
    {
        if (electrified)
        {
            if (target.CompareTag("Enemy"))
            {
                target.GetComponent<Enemy>().TakeDamage(electricDamage);
                GameObject electricityFx = target.transform.Find("FX/electricity").gameObject;
                electricityFx.SetActive(false);
                electricityFx.SetActive(true);
            }
            else if (target.CompareTag("Player"))
            {
                GameObject electricityFx = target.transform.Find("FX/electricity").gameObject;
                GameManager.gameManager.TakeDamage(target, electricDamage, Vector3.zero, false);
                electricityFx.SetActive(false);
                electricityFx.SetActive(true);
            }
        }
        else if (target.CompareTag("Enemy") || target.CompareTag("Player"))
        {
            objectsInPuddle.Add(target.gameObject);
        }
    }

    void OnStayWater(GameObject target)
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

                //GetComponent<MeshRenderer>().material = frozenWaterMaterial;
                transform.GetChild(0).gameObject.SetActive(false);
                transform.GetChild(1).gameObject.SetActive(true);

                for (int i = 0; i < objectsInPuddle.Count; i++)
                {
                    if (objectsInPuddle[i].gameObject.CompareTag("Enemy"))
                    {
                        Enemy e = objectsInPuddle[i].gameObject.GetComponent<Enemy>();
                        e.actualFreezeCoroutine = e.StartCoroutine(e.FreezeCoroutine(freezeTime));
                    }
                    else if (objectsInPuddle[i].gameObject.CompareTag("Player"))
                    {
                        objectsInPuddle[i].gameObject.GetComponent<PlayerController>().StartCoroutine(objectsInPuddle[i].gameObject.GetComponent<PlayerController>().FreezeCoroutine(freezeTime));
                    }
                }
                frozenWaterCoroutine = StartCoroutine(ReturnToWater(frozenWaterLifeTime));
            }

            else if (target.GetComponent<PowerController>().elementalPower == GameManager.PowerType.Electric && !electrified && !frozen)
            {
                Debug.Log("elec");
                electrified = true;/*
                transform.GetChild(0).GetComponent<Collider>().enabled = false;
                transform.GetChild(0).GetComponent<Collider>().enabled = true;*/
                //GetComponent<MeshRenderer>().material = ElectrifiedWaterMaterial;
                transform.GetChild(3).gameObject.SetActive(true);
                electrifiedWaterCoroutine = StartCoroutine(ReturnToWater(electrifiedWaterLifeTime));
            }

            else if (target.GetComponent<PowerController>().elementalPower == GameManager.PowerType.Fire)
            {
                target.GetComponent<PowerController>().DeactivatePower(GameManager.PowerType.Fire);
                if (frozen)
                {
                    frozen = false;
                    //GetComponent<MeshRenderer>().material = waterMaterial;
                    transform.GetChild(1).gameObject.SetActive(false);
                    transform.GetChild(0).gameObject.SetActive(true);
                }
            }
        }

        if (electrified)
        {
            if (target.CompareTag("Enemy"))
            {
                target.GetComponent<Enemy>().TakeDamage(electricDamage);
                GameObject electricityFx = target.transform.Find("FX/electricity").gameObject;
                electricityFx.SetActive(false);
                electricityFx.SetActive(true);
            }
            else if (target.CompareTag("Player"))
            {
                GameObject electricityFx = target.transform.Find("FX/electricity").gameObject;
                GameManager.gameManager.TakeDamage(target, electricDamage, Vector3.zero, false);
                electricityFx.SetActive(false);
                electricityFx.SetActive(true);
            }
        }


        for (int i = 0; i < objectsInPuddle.Count; i++)
        {
            if (objectsInPuddle[i].CompareTag("Enemy"))
            {
                if (objectsInPuddle[i].GetComponent<Enemy>().actualFireCoroutine != null)
                {
                    objectsInPuddle[i].GetComponent<Enemy>().StopCoroutine(objectsInPuddle[i].GetComponent<Enemy>().actualFireCoroutine);
                    objectsInPuddle[i].transform.Find("FX/fire").gameObject.SetActive(false);
                }

            }
            else if (objectsInPuddle[i].CompareTag("Player"))
            {
                if (objectsInPuddle[i].GetComponent<PlayerController>().actualBurnCoroutine != null)
                {
                    objectsInPuddle[i].GetComponent<PlayerController>().StopCoroutine(objectsInPuddle[i].GetComponent<PlayerController>().actualBurnCoroutine);
                    objectsInPuddle[i].transform.Find("FX/fire").gameObject.SetActive(false);
                }

            }
        }

    }


    void OnExitWater(GameObject target)
    {
        if (target.CompareTag("Enemy") || target.CompareTag("Player"))
        {
            objectsInPuddle.Remove(target.gameObject);
        }
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
        //GetComponent<MeshRenderer>().material = waterMaterial;
        transform.GetChild(1).gameObject.SetActive(false);
        transform.GetChild(3).gameObject.SetActive(false);
        transform.GetChild(0).gameObject.SetActive(true);
    }

    #endregion


    #region Flammable Effect
    void OnEnterFlammable(GameObject target)
    {
        if (target.CompareTag("Player") || target.CompareTag("Enemy"))
        {
            objectsInPuddle.Add(target);
            if (target.CompareTag("Player"))
            {
                target.transform.Find("FX/fire").gameObject.SetActive(true);
            }
            else if (target.CompareTag("Enemy"))
            {
                target.transform.Find("FX/fire").gameObject.SetActive(true);
            }
        }
    }

    void OnStayFlammable(GameObject target)
    {
        if (!onFire && target.CompareTag("Orb") && target.GetComponent<PowerController>().elementalPower == GameManager.PowerType.Fire)
        {
            ActivateFlammable();
        }
        if (onFire && target.CompareTag("Orb") && target.GetComponent<PowerController>().elementalPower == GameManager.PowerType.Ice)
        {
            target.GetComponent<PowerController>().DeactivatePower(GameManager.PowerType.Ice);
        }
    }

    
    void OnExitFlammable(GameObject target)
    {
        if (target.CompareTag("Player") || target.CompareTag("Enemy"))
        {
            objectsInPuddle.Remove(target);
            if (onFire)
            {
                if (target.CompareTag("Player"))
                {
                    PlayerController playerController = target.GetComponent<PlayerController>();
                    if (playerController.actualBurnCoroutine != null)
                    {
                        playerController.StopCoroutine(playerController.actualBurnCoroutine);
                    }
                    //start the coroutine on the playercontroller monobehavior to keep the coroutine running even if the fire puddle is destroyed
                    playerController.actualBurnCoroutine = playerController.StartCoroutine(Burn(target));
                }
                if (target.CompareTag("Enemy"))
                {
                    Enemy enemy = target.GetComponent<Enemy>();
                    if (enemy.actualFireCoroutine != null)
                    {
                        enemy.StopCoroutine(enemy.actualFireCoroutine);
                    }
                    enemy.actualFireCoroutine = enemy.StartCoroutine(Burn(target));
                }
            }
        }
    }

    void ActivateFlammable()
    {
        onFire = true;
        GetComponent<MeshRenderer>().material = onFireFlammableMaterial;
        Destroy(gameObject, onFireFlammableLifeTime);
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
            target.transform.Find("FX/fire").gameObject.SetActive(false);
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
            target.transform.Find("FX/fire").gameObject.SetActive(false);
        }
        
    }
    #endregion


    #region Mud Effect
    void OnEnterMud(GameObject target)
    {
        if (target.CompareTag("Player"))
        {
            target.GetComponent<PlayerController>().SlowSpeed(mudSlowAmount);
        }
        if (target.CompareTag("Enemy"))
        {
            target.GetComponent<EnemyMovement>().SlowSpeed(mudSlowAmount);
        }
        if (target.CompareTag("Player") || target.CompareTag("Enemy"))
        {
            objectsInPuddle.Add(target);
        }
    }

    void OnStayMud(GameObject target)
    {
        if (target.CompareTag("Orb") && target.GetComponent<PowerController>().elementalPower == GameManager.PowerType.Electric)
        {
            target.GetComponent<PowerController>().DeactivatePower(GameManager.PowerType.Electric);
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
        if (target.CompareTag("Player") || target.CompareTag("Enemy"))
        {
            objectsInPuddle.Remove(target);
        }
    }
    #endregion

    #endregion


    public void CleanNullInEnemyList()
    {
        if (objectsInPuddle.Exists(x => x.Equals(null)))
        {
            objectsInPuddle.RemoveAll(x => x.Equals(null));
        }
    }


}
