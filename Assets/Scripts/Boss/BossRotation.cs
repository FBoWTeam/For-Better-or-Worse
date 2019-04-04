using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRotation : MonoBehaviour
{
    BossSystem bossSystem;
    bool rightRotation;

    public float rotationSpeed;
    public float minRotationTime;
    public float maxRotationTime;
    Coroutine rotationCoroutine;
    
    private void OnEnable()
    {
        bossSystem = gameObject.GetComponent<BossSystem>();
        rotationCoroutine = StartCoroutine(RotationCoroutine());
    }


    IEnumerator RotationCoroutine()
    {
        while (bossSystem.actualPhase == 2 || bossSystem.actualPhase == 3)
        {
            float rotationTime = Random.Range(minRotationTime, maxRotationTime);
            float timeStamp = Time.time;
            while (Time.time - timeStamp < rotationTime)
            {
                if (rightRotation)
                {
                    transform.Rotate(Vector3.up * Time.deltaTime * rotationSpeed);
                }
                else
                {
                    transform.Rotate(Vector3.down * Time.deltaTime * rotationSpeed);
                }

                yield return new WaitForEndOfFrame();
            }

            rightRotation = !rightRotation;

            yield return new WaitForEndOfFrame();
        }
    }

}
