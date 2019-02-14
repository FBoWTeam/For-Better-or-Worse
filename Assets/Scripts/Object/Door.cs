using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour, IActivable
{
    //boolean indicating if the Door is activated/open or not
    public bool isActive { get; set; }

    public void Activate()
    {
        isActive = !isActive;
        //play the animation, change the environment
    }
}
