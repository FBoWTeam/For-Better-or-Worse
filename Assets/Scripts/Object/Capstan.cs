using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Capstan : MonoBehaviour, IActivable
{
    //boolean indicating if the hatch is activated or not
    public bool isActive { get; set; }

    [Tooltip("list of activated objects needed to activate the capstan")]
    public List<GameObject> objectsConditions;

    GameObject orb;
    Transform border;
    Transform pivot;
    private bool orbOnTheRight;

    [Tooltip("how many times the capstan must be turned clockwise to lower the bridge")]
    public int notchNumber;

    [HideInInspector]
    public int actualAngle;
    [HideInInspector]
    public int targetAngle;
    [HideInInspector]
    public int maxAngle;

    public int rotationSpeed = 1;

    private void Start()
    {
        orb = GameManager.gameManager.orb;
        border = transform.GetChild(0);
        pivot = transform.GetChild(1);

        actualAngle = 0;
        targetAngle = 0;

        maxAngle = 180 * notchNumber;
    }

    void Update()
    {
        if (OnTheRight(pivot, border, orb.transform))
        {
            GetComponent<Renderer>().material.color = Color.red;
            orbOnTheRight = true;
        }
        else
        {
            GetComponent<Renderer>().material.color = Color.blue;
            orbOnTheRight = false;
        }

        if(actualAngle != targetAngle)
        {
            int sign = targetAngle > actualAngle ? 1 : -1;
            transform.parent.localEulerAngles = new Vector3(0.0f, actualAngle += (rotationSpeed * sign), 0.0f);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Orb"))
        {
            if (orbOnTheRight == false && targetAngle < maxAngle)
            {
                targetAngle += 180;
            }
            else if (orbOnTheRight == true && targetAngle > 0)
            {
                targetAngle -= 180;
            }
        }
    }

    public void Activate()
    {
        if (CheckValidObjects())
        {
            if (isActive == false)
            {
                GetComponentInParent<Animation>().Play("CapstanUp");
            }
            isActive = true;
        }
        else
        {
            if (isActive == true)
            {
                GetComponentInParent<Animation>().Play("CapstanDown");
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

    /// <summary>
    /// function that return a bool
    /// check if the object (otherPos) is on the right or left compared to the vector (pivot,border)
    /// </summary>
    /// <param name="pivot"></param>
    /// <param name="border"></param>
    /// <param name="otherPos"></param>
    /// <returns></returns>
    bool OnTheRight(Transform pivot, Transform border, Transform otherPos)
    {
        Vector3 ab = border.position - pivot.position;
        Vector3 ac = otherPos.position - pivot.position;
        Vector3 crossProduct = Vector3.Cross(ab, ac);

        if (crossProduct.y < 0)
        {
            return false;
        }
        return true;
    }
    

}
