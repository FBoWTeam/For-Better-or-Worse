using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlugGenerator : MonoBehaviour
{
    //slug life time
    public float slugDurationEffect;

    void Start()
    {
        Destroy(gameObject, slugDurationEffect);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            GameManager.gameManager.SlowSpeed(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            GameManager.gameManager.RestoreSpeed(other.gameObject);
        }
    }
}
