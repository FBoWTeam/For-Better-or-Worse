using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour, IActivable
{
    //boolean indicating if the Door is activated/open or not
    public bool isActive { get; set; }

    public enum DoorType
    {
        None,
        Standard,
        Electric,
        ArenaDoor
    }

    public DoorType doorType;

    [DrawIf(new string[] { "doorType" }, DoorType.Electric)]
    public int doorMaxCharge;
    [DrawIf(new string[] { "doorType" }, DoorType.Electric)]
    public int doorCurrentCharge;
    [DrawIf(new string[] { "doorType" }, DoorType.Electric)]
    public int chargingAmount;


    [Tooltip("list of activated objects needed to open the door")]
    public List<GameObject> objectsConditions;

    /// <summary>
    /// opens the door
    /// </summary>
    public void Activate()
    {
        switch (doorType)
        {
            case DoorType.Standard:
                ActivateStandardDoor();
                break;
            case DoorType.Electric:
                ActivateElectricDoor();
                Debug.Log("active");
                break;
            case DoorType.ArenaDoor:
                ActivateArenaDoor();
                break;
            default:
                break;
        }

    }

    public void Deactivate()
    {
        if (isActive == true)
        {
            GetComponentInParent<Animation>().Play("DoorClose");
        }
        isActive = false;
    }

    void ActivateArenaDoor()
    {
        if (CheckValidObjects())
        {
            if (isActive == false)
            {
                GetComponentInParent<Animation>().Play("DoorOpen");
            }
            isActive = true;
        }
    }

    void ActivateStandardDoor()
    {
        if (CheckValidObjects())
        {
            if (isActive == false)
            {
                GetComponentInParent<Animation>().Play("DoorOpen");
            }
            isActive = true;
        }
        else
        {
            if (isActive == true)
            {
                GetComponentInParent<Animation>().Play("DoorClose");
            }
            isActive = false;
        }
    }

    void ActivateElectricDoor()
    {
        Debug.Log("fct");
        if (doorCurrentCharge >= doorMaxCharge)
        {
            doorCurrentCharge = doorMaxCharge;
            if (isActive == false && CheckValidObjects())
            {
                GetComponentInParent<Animation>().Play("DoorOpen");
                isActive = true;
            }

        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Orb") && doorType == DoorType.Electric && other.gameObject.GetComponent<PowerController>().elementalPower == GameManager.PowerType.Electric)
        {
            Debug.Log("ta mere la collider");
            doorCurrentCharge += chargingAmount;
            this.Activate();
        }
    }

    /// <summary>
    /// check if all the necesary objects are activated to open the door
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
