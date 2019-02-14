using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hatch : MonoBehaviour
{
    //boolean indicating if the hatch is activated or not
    public bool isActive { get; set; }

    public void Activate()
    {
        isActive = !isActive;
        //play the animation, change the environment
    }
}
