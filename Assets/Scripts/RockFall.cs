using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockFall : MonoBehaviour
{

    public GameObject rockPrefab;

    public float roomWidth;
    public float roomLength;
    public Vector3 roomCenter;

    IEnumerator RockFallCoroutine()
    {
        Random.Range(roomCenter.x - (roomWidth / 2), roomCenter.z - (roomLength / 2));

        yield return new WaitForSeconds(1.0f);
    }


}
