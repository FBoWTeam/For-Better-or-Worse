using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuddleSystem : MonoBehaviour
{
    [Header("[Puddle Type]")]
    public GameManager.PuddleType puddleType;

    [Header("[Puddle Effect Editing]")]
    public GameManager.PuddleType editingPuddleType;

    #region Puddle parameters
    [Header("[Puddle common param]")]
    public float size;


    [DrawIf(new string[] { "editingPuddleType" }, GameManager.PuddleType.Slug)]
    public float slugLifeTime;
    [Tooltip("Amount in percentage of the slow")]
    [Range(1, 100)]
    public float slugSlowAmount;


    [DrawIf(new string[] { "editingPuddleType" }, GameManager.PuddleType.Acid)]
    public float acidDamage;


    [DrawIf(new string[] { "editingPuddleType" }, GameManager.PuddleType.Water)]
    public bool electrified;
    public bool frozen;


    [DrawIf(new string[] { "editingPuddleType" }, GameManager.PuddleType.Mud)]
    [Range(1, 100)]
    public float mudSlowAmount;


    [DrawIf(new string[] { "editingPuddleType" }, GameManager.PuddleType.Flammable)]
    public bool onFire;

    #endregion


    private void Start()
    {
        switch (puddleType)
        {
            case GameManager.PuddleType.Slug:
                Destroy(gameObject, slugLifeTime);
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
                case GameManager.PuddleType.Mud:
                    break;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy") || other.CompareTag("Player") || other.CompareTag("Orb"))
        {
            switch (puddleType)
            {
                case GameManager.PuddleType.Slug:
                    OnExitSlug(other.gameObject);
                    break;
                case GameManager.PuddleType.Mud:
                    OnExitMud(other.gameObject);
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
                case GameManager.PuddleType.Flammable:
                    OnStayFlammable(other.gameObject);
                    break;
            }
        }
    }

    #endregion

    #region Puddle Effects

    #region Slug Effect
    void OnEnterSlug(GameObject target)
    {
        if (target.tag == "Enemy")
        {
            target.GetComponent<EnemyMovement>().SlowSpeed(slugSlowAmount);
        }
    }

    void OnExitSlug(GameObject target)
    {
        if (target.tag == "Enemy")
        {
            target.GetComponent<EnemyMovement>().RestoreSpeed();
        }
    }
    #endregion

    #region Acid Effect
    void OnEnterAcid(GameObject target)
    {

    }
    void OnStayAcid(GameObject target)
    {

    }

    IEnumerator AcidDamage(GameObject target)
    {
        if (target.tag == "Player")
        {
            //GameManager.gameManager.TakeDamage(target, acidDamage);
        }
        if (target.tag == "Enemy")
        {
            target.GetComponent<Enemy>().TakeDamage((int)acidDamage);
        }
        yield return new WaitForEndOfFrame();
    }


    #endregion

    #region Water Effect
    void OnEnterWater(GameObject target)
    {

    }
    #endregion

    #region Flammable Effect
    void OnEnterFlammable(GameObject target)
    {

    }
    void OnStayFlammable(GameObject target)
    {

    }
    #endregion

    #region Mud Effect
    void OnEnterMud(GameObject target)
    {

    }
    void OnExitMud(GameObject target)
    {

    }
    #endregion

    #endregion
}
