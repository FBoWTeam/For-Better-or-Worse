using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Capstan : MonoBehaviour
{
    GameObject orb;
    Transform border;
    Transform pivot;
    private bool orbOnTheRight;

    private void Start()
    {
        orb = GameManager.gameManager.orb;
        border = transform.GetChild(1);
        pivot = transform.GetChild(2);
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
    }

    private void OnTriggerEnter(Collider other)
    {
        if (orbOnTheRight == false)
        {
            gameObject.transform.Rotate(new Vector3(0, 180, 0));
        }
        else
        {
            gameObject.transform.Rotate(new Vector3(0, -180, 0));
        }
    }

    bool OnTheRight (Transform pivot, Transform border, Transform otherPos)
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
