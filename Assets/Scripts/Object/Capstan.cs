using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Capstan : MonoBehaviour
{
    GameObject orb;
    Transform border;
    Transform pivot;
    private bool orbOnTheRight;

    int rotationSpeed = 1;

    [Tooltip("how many times the capstan must be turned clockwise to lower the bridge")]
    public int notchNumber;
    public int actualAngle;
    public int targetAngle;
    public int maxAngle;

    private void Start()
    {
        orb = GameManager.gameManager.orb;
        border = transform.GetChild(1);
        pivot = transform.GetChild(2);

        actualAngle = 0;
        targetAngle = 0;

        maxAngle = 180 * notchNumber;
    }

    void Update()
    {
        if (OnTheRight(pivot, border, orb.transform))
        {
            transform.GetChild(0).GetComponent<Renderer>().material.color = Color.red;
            orbOnTheRight = true;
        }
        else
        {
            transform.GetChild(0).GetComponent<Renderer>().material.color = Color.blue;
            orbOnTheRight = false;
        }

        if(actualAngle != targetAngle)
        {
            int sign = targetAngle > actualAngle ? 1 : -1;
            transform.localEulerAngles = new Vector3(0.0f, actualAngle += (rotationSpeed * sign), 0.0f);
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
