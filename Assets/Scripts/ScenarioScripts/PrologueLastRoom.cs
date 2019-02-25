using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrologueLastRoom : MonoBehaviour
{

    public GameObject flammableObject;
    public GameObject waveSystem;

    private float timer;

    // Update is called once per frame
    void Update()
    {
        if (flammableObject.GetComponent<IActivable>().isActive)
        {
            timer = flammableObject.GetComponent<FlammableObjects>().burnTime;
            waveSystem.SetActive(true);

            //AFFICHER L'UI

        }
    }
}
