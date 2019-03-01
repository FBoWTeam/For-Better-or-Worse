using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlugGenerator : MonoBehaviour
{
    //slug life time
    public float slugDurationEffect;

    [Tooltip("Amount in percentage of the slow")]
    [Range(1,100)]
    public float slowAmount;

    void Start()
    {
        Destroy(gameObject, slugDurationEffect);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            other.GetComponent<EnemyMovement>().SlowSpeed(slowAmount);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            other.GetComponent<EnemyMovement>().RestoreSpeed();
        }
    }
}
