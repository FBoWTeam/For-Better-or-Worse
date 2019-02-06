using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistanceLimiter : MonoBehaviour
{
    public GameObject player1;
    public GameObject player2;

    public float maxDistance = 20;
    public float minDistance = 5;

    private BoxCollider[] boxList;

    private void Start()
    {
        boxList = gameObject.GetComponents<BoxCollider>();
        boxList[0].center = new Vector3(0.0f, 0.0f, maxDistance / 2);
        boxList[1].center = new Vector3(0.0f, 0.0f, -maxDistance / 2);
        gameObject.GetComponent<SphereCollider>().radius = minDistance / 2;
    }
    void Update()
    {
        Vector3 p1 = player1.transform.position;
        Vector3 p2 = player2.transform.position;

        transform.position = p1 + (p2 - p1) / 2;
        transform.LookAt(p1);
    }

    private void OnValidate()
    {
        boxList[0].center = new Vector3(0.0f, 0.0f, maxDistance / 2);
        boxList[1].center = new Vector3(0.0f, 0.0f, -maxDistance / 2);
        gameObject.GetComponent<SphereCollider>().radius = minDistance / 2;
    }

}
