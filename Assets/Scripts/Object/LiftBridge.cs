using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiftBridge : MonoBehaviour
{
    [Tooltip("the coresponding capstan to manipulate the bridge")]
    public GameObject capstan;
    [Tooltip("invisible wall that prevents the players to cross the bridge if the bridge is not totally down")]
    public GameObject invisibleWall;
    private bool crossableBridge;

    float actualAngle;
    float targetAngle;

    private void Start()
    {
        actualAngle = -50;
    }

    private void Update()
    {
        Capstan cap = capstan.GetComponent<Capstan>();

        targetAngle = ((float)(cap.actualAngle - cap.maxAngle) / (float)cap.maxAngle) * -50.0f * -1.0f;

        //moves the bridge in function of the desired angle
        if ((int)actualAngle != (int)targetAngle)
        {
            int sign = targetAngle > actualAngle ? 1 : -1;
            transform.localEulerAngles = new Vector3(0, transform.localEulerAngles.y, actualAngle += (cap.rotationSpeed * sign));
        }


        if (actualAngle == 0 && !crossableBridge)
        {
            invisibleWall.SetActive(false);
            crossableBridge = true;
        }
        else if (actualAngle != 0 && crossableBridge)
        {
            invisibleWall.SetActive(true);
            crossableBridge = false;
        }
    }

}
