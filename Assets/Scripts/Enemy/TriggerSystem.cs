using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerSystem : MonoBehaviour
{
    public enum TriggerMode { Enter, Exit, Stay };
    public TriggerMode triggerMode;

    [Header("Elements to trigger")]
    public GameObject[] enemies;

    // Start is called before the first frame update
    void Start()
    {
        if(enemies.Length == 0)
        {
            Debug.LogError("Enemies list is empty", this);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (triggerMode == TriggerMode.Enter)
        {
            if (other.gameObject.tag == "Player")
            {
                foreach (GameObject enemy in enemies)
                {
                    enemy.GetComponent<Enemy>().enabled = true;
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (triggerMode == TriggerMode.Exit)
        {
            if (other.gameObject.tag == "Player")
            {
                foreach (GameObject enemy in enemies)
                {
                    enemy.GetComponent<Enemy>().enabled = true;
                }
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (triggerMode == TriggerMode.Stay)
        {
            if (other.gameObject.tag == "Player")
            {
                foreach (GameObject enemy in enemies)
                {
                    enemy.GetComponent<Enemy>().enabled = true;
                }
            }
        }
    }

}
