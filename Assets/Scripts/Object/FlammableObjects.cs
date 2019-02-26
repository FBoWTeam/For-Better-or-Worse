using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlammableObjects : MonoBehaviour, IActivable
{
    [Tooltip("time needed for the object to be destroyed when burning")]
    public float burnTime;
    [Tooltip("the burn time is reduce by this amount if touched with the fire orb")]
    public float burnAcceleration;

    private bool isBurning;

    public bool isActive { get; set; }
    

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Orb"))
        {
            PowerController orbPower = other.gameObject.GetComponent<PowerController>();
            if (orbPower.elementalPower == GameManager.PowerType.Fire)
            {
                if (!isActive)
                {
                    this.Activate();
                }
            }
            else if (orbPower.elementalPower == GameManager.PowerType.None)
            {
                orbPower.elementalPower = GameManager.PowerType.Fire;
            }
        }
    }

    public void Activate()
    {
        isActive = true;
        isBurning = true;
        gameObject.GetComponent<Renderer>().material.color = Color.red;
        Destroy(gameObject, burnTime);
    }
}

