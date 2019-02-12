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

    /// <summary>
    /// Script managing the minimum and maximum distance between the two players
    /// using colliders positioned with the player's positions
    /// using a sphere collider to handle the minimum distance
    /// using two box colliders to handle the maximum distance
    /// </summary>
    private void Awake()
    {
        boxList = gameObject.GetComponents<BoxCollider>();
        if (boxList.Length > 0)
        {
            boxList[0].center = new Vector3(0.0f, 0.0f, maxDistance / 2);
            boxList[1].center = new Vector3(0.0f, 0.0f, -maxDistance / 2);
            gameObject.GetComponent<SphereCollider>().radius = minDistance / 2;
        }
    }

    void Update()
    {
        Vector3 p1 = player1.transform.position;
        Vector3 p2 = player2.transform.position;

        transform.position = p1 + (p2 - p1) / 2;
        transform.LookAt(p1);
    }
}
