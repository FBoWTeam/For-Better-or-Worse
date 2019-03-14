using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlate : MonoBehaviour, IActivable
{
    //boolean indicating if the pressure plate is pressed or not
    public bool isActive { get; set; }

    //the pressure plate activates an other object
    [Tooltip("the object to activate id the current lever is active")]
    public GameObject objectToActivate;

    private Animator anim;


    private void Start()
    {
        anim = GetComponentInParent<Animator>();
    }

    /// <summary>
    /// activates the pressure plate if the orb enter and stays on it's collider
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isActive)
        {
            this.Activate();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && isActive)
        {
            this.Activate();
        }
    }

    public void Activate()
    {
        isActive = !isActive;
        //activates the other object
        objectToActivate.GetComponent<IActivable>().Activate();
        if (isActive)
        {
            anim.SetBool("isActivated", true);
        }
        else
        {
            anim.SetBool("isActivated", false);
        }
    }

}
