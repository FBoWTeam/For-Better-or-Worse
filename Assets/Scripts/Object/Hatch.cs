using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hatch : MonoBehaviour, IActivable
{
    //boolean indicating if the hatch is activated or not
    public bool isActive { get; set; }

    [Tooltip("list of activated objects needed to open the hatch")]
    public List<GameObject> objectsConditions;

    /// <summary>
    /// opens the hatch
    /// </summary>
    public void Activate()
    {
        if (CheckValidObjects())
        {
            if (isActive == false)
            {
                GetComponentInParent<Animation>().Play("HatchOpen");
            }
            isActive = true;
        }
        else
        {
            if (isActive == true)
            {
                GetComponentInParent<Animation>().Play("HatchClose");
            }
            isActive = false;
        }
    }

    /// <summary>
    /// check if all the necesary object are activated to open the door
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
