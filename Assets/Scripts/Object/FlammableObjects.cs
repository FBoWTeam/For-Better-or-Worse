using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlammableObjects : MonoBehaviour, IActivable
{
    [Tooltip("time needed for the object to be destroyed when burning")]
    public float burnTime;

    public bool isActive { get; set; }
    

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Orb"))
        {
            PowerController orbPower = other.gameObject.GetComponent<PowerController>();
            if (orbPower.elementalPower == GameManager.PowerType.Fire)
            {
                if (isActive == false)
                {
                    this.Activate();
                }
            }
        }
    }

    public void Activate()
    {
        isActive = true;
        Destroy(gameObject, burnTime);
    }
}

