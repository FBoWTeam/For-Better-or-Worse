using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brazier : MonoBehaviour, IActivable
{
    //boolean indicating if the brazier is activated or not
    public bool isActive { get; set; }

    //the brazier activates an other object
    public GameObject objectToActivate;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Orb"))
        {
            //if the brazier is not active and the orb is on fire, set the brazier on and activates the object if not null
            if (isActive == false && other)
            {
                this.Activate();
                if (objectToActivate != null)
                {
                    objectToActivate.GetComponent<IActivable>().Activate();
                }
            }
            //if the brazier is active and the orb isn't, set the orb on fire
            else if (isActive == true && other)
            {
                //FOUT LE FEU A LA BALLE !
            }
        }
    }

    public void Activate()
    {
        isActive = !isActive;
    }
}
