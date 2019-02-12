using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerSystem : MonoBehaviour
{
    public enum TriggerMode { Enter, Exit, Stay };
    public TriggerMode triggerMode;

    [Header("Elements to trigger")]
    public GameObject[] Enemies;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (triggerMode == TriggerMode.Enter)
        {
            if (other.gameObject.tag == "Player")
            {
                foreach (GameObject enemy in Enemies)
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
                foreach (GameObject enemy in Enemies)
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
                foreach (GameObject enemy in Enemies)
                {
                    enemy.GetComponent<Enemy>().enabled = true;
                }
            }
        }
    }

}
