using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrologueLastRoom : MonoBehaviour
{

    public GameObject flammableObject;
    public GameObject waveSystem;

    private float timer;
    private bool alreadyActive;

    // Update is called once per frame
    void Update()
    {
        if (flammableObject.GetComponent<IActivable>().isActive && flammableObject != null)
        {
            if (!alreadyActive)
            {
                waveSystem.SetActive(true);
            }

            timer += Time.deltaTime;
            if (timer >= flammableObject.GetComponent<FlammableObjects>().burnTime)
            {
                Destroy(flammableObject);
                Destroy(gameObject);
            }
        }

    }
}
