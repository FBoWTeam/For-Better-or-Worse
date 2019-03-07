using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brazier : MonoBehaviour, IActivable
{
    //boolean indicating if the brazier is activated or not
    public bool isActive { get; set; }

    public bool onFire;

    [Tooltip("indicates if the brazier can be activated by the fire orb")]
    public bool activatedByOrb;

    //the brazier activates an other object
    [Tooltip("list of objects to activate to activate the brazier")]
    public List<GameObject> objectToActivate;

    [Tooltip("list of activated objects needed to activate the brazier")]
    public List<GameObject> objectsConditions;

    private void Start()
    {
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
            //if the brazier is not active and the orb is on fire, set the brazier on and activates the object if not null
            if (!isActive && powerController.elementalPower == GameManager.PowerType.Fire)
            {
                if (activatedByOrb)
                {
                    this.Activate();
                }
            }
            //if the brazier is active and the orb isn't, set the orb on fire
            else if (isActive && powerController.elementalPower != GameManager.PowerType.Fire)
            {
                powerController.ActivatePower(GameManager.PowerType.Fire, "forced");
                powerController.isActivatedByBrazier = true;
            }
        }
    }

    public void Activate()
    {
        if (CheckValidObjects())
        {
            gameObject.GetComponent<Renderer>().material.color = Color.red;
            isActive = true;
            onFire = true;
            if (objectToActivate.Count != 0)
            {
                for (int i = 0; i < objectToActivate.Count; i++)
                {
                    Debug.Log("oui");
                    objectToActivate[i].GetComponent<IActivable>().Activate();
                }
            }
        }
    }

    public void Deactivate()
    {
        gameObject.GetComponent<Renderer>().material.color = Color.grey;
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

}
