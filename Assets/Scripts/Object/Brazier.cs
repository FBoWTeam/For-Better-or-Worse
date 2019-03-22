using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brazier : MonoBehaviour, IActivable
{
    //boolean indicating if the brazier is activated or not
    public bool isActive { get; set; }

    public bool onFire;

    public enum BrazierType
    {
        Classic,
        ArenaBrazier
    }

    public BrazierType type;

    [DrawIf(new string[] { "type" }, BrazierType.ArenaBrazier)]
    public float reActivationTime;

    [Tooltip("indicates if the brazier can be activated by the fire orb")]
    [DrawIf(new string[] { "type" }, BrazierType.Classic)]
    public bool activatedByOrb;

    //the brazier activates an other object
    [Tooltip("list of objects to activate to activate the brazier")]
    public List<GameObject> objectToActivate;

    [Tooltip("list of activated objects needed to activate the brazier")]
    public List<GameObject> objectsConditions;

    private Animator anim;

    private void Start()
    {
        anim = GetComponentInParent<Animator>();
        anim.SetBool("isUp", true);
        if (onFire)
        {
            this.Activate();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Orb"))
        {
            PowerController powerController = other.GetComponent<PowerController>();

            if (type == BrazierType.Classic)
            {
                //if the brazier is not active and the orb is on fire, set the brazier on and activates the object if not null
                if (!isActive && powerController.elementalPower == GameManager.PowerType.Fire && activatedByOrb)
                {
                    this.Activate();
                }
                //if the brazier is active and the orb isn't, set the orb on fire
                else if (isActive && powerController.elementalPower != GameManager.PowerType.Fire && activatedByOrb)
                {
                    powerController.ActivatePower(GameManager.PowerType.Fire, "forced");
                    powerController.isActivatedByBrazier = true;
                }
            }

            if (type == BrazierType.ArenaBrazier)
            {
                if (isActive && powerController.elementalPower != GameManager.PowerType.Fire)
                {
                    powerController.ActivatePower(GameManager.PowerType.Fire, "forced");
                    powerController.isActivatedByBrazier = true;
                    Deactivate();
                    StartCoroutine(ReActivateArenaBrazier());
                }
                else if (!isActive && powerController.elementalPower == GameManager.PowerType.Fire)
                {
                    this.Activate();
                }
            }
        }
    }

    IEnumerator ReActivateArenaBrazier()
    {
        yield return new WaitForSeconds(reActivationTime);
        this.Activate();
    }


    public void Activate()
    {
        if (CheckValidObjects())
        {
            ActivateFireParticles();
            isActive = true;
            if (type == BrazierType.ArenaBrazier)
            {
                anim.SetBool("isUp", true);
            }
            onFire = true;
            if (objectToActivate.Count != 0)
            {
                for (int i = 0; i < objectToActivate.Count; i++)
                {
                    objectToActivate[i].GetComponent<IActivable>().Activate();
                }
            }
        }
    }

    public void Deactivate()
    {
        if (type == BrazierType.ArenaBrazier)
        {
            anim.SetBool("isUp", false);
        }
        DeactivateFireParticles();
        isActive = false;
        onFire = false;
    }

    /// <summary>
    /// check if all the necesary objects are activated to activate the brazier
    /// </summary>
    /// <returns></returns>
    bool CheckValidObjects()
    {
        for (int i = 0; i < objectsConditions.Count; i++)
        {
            if (objectsConditions[i].GetComponent<IActivable>().isActive != true)
            {
                return false;
            }
        }
        return true;
    }

    //function used to activate the fire particles
    void ActivateFireParticles()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(true);
        }
    }

    void DeactivateFireParticles()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }
    }

}
