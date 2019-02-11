using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever : MonoBehaviour, IActivable
{
    //boolean indicating if the lever is activated or not
    public bool isActive { get; set; }

    //the lever activates an other object
    public GameObject objectToActivate;

    /// <summary>
    /// activates the lever if the orb enter it's collider
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Orb"))
        {
            this.Activate();
        }
    }

    /// <summary>
    /// switch the boolean when activated and activate the door or hatch
    /// </summary>
    public void Activate()
    {
        isActive = !isActive;
        objectToActivate.GetComponent<IActivable>().Activate();
    }


}
