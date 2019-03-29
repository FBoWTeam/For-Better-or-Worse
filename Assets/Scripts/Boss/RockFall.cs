using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockFall : MonoBehaviour
{

    public GameObject rockPrefab;

    public float roomWidth;
    public float roomLength;
    public Vector3 roomCenter;

    private void Start()
    {
        StartCoroutine(RockFallCoroutine());
    }

    IEnumerator RockFallCoroutine()
    {
        while (true)
        {
            //take a random location in the room
            float abscissaLocation = Random.Range(roomCenter.x - (roomWidth / 2), roomCenter.x + (roomWidth / 2));
            float ordinateLocation = Random.Range(roomCenter.z - (roomLength / 2), roomCenter.z + (roomLength / 2));

            Instantiate(rockPrefab, new Vector3(abscissaLocation, 10f, ordinateLocation), Quaternion.identity);

            yield return new WaitForSeconds(1.0f);
        }
    }


}
