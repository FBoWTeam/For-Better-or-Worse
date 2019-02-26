using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiftBridge : MonoBehaviour
{
    public GameObject capstan;
    Quaternion startRotation;
    Quaternion endRotation;

    float actualAngle;
    float targetAngle;

    private void Start()
    {
        startRotation = transform.rotation;
        endRotation = Quaternion.Euler(0, 0, 0);
        actualAngle = 50.0f;
    }

    private void Update()
    {
        Capstan cap = capstan.GetComponent<Capstan>();

        targetAngle = ((float)(cap.actualAngle - cap.maxAngle) / (float)cap.maxAngle) * 50.0f * -1.0f;
        

        if ((int)actualAngle != (int)targetAngle)
        {
            int sign = targetAngle > actualAngle ? 1 : -1;
            transform.localEulerAngles = new Vector3(0.0f, 0.0f, actualAngle += (cap.rotationSpeed * sign));
        }
    }

}
