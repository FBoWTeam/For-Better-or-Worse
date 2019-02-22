using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiftBridge : MonoBehaviour
{
    public GameObject capstan;
    Quaternion startRotation;
    Quaternion endRotation;

    private void Start()
    {
        startRotation = transform.rotation;
        endRotation = Quaternion.Euler(0, 0, 50);
    }

    private void Update()
    {
        Capstan cap = capstan.GetComponent<Capstan>();
        transform.rotation = Quaternion.Lerp(startRotation, endRotation, (float)cap.targetAngle / (float)cap.maxAngle);
    }

}
