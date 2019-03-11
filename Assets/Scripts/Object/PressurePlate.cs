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

    private Animation animation;


    private void Start()
    {
        animation = GetComponentInParent<Animation>();
    }

    /// <summary>
    /// activates the pressure plate if the orb enter and stays on it's collider
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !animation.IsPlaying("PressurePlateOff") && !isActive)
        {
            this.Activate();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && !animation.IsPlaying("PressurePlateOn") && isActive)
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
            animation.Play("PressurePlateOn");
        }
        else
        {
            animation.Play("PressurePlateOff");
        }
    }

}
