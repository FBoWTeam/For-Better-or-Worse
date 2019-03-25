using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrologueLastRoom : MonoBehaviour
{
    [Tooltip("the flammable obstacle of the prologue")]
    public GameObject flammableObject;
    [Tooltip("the gameobject that contains the script of enemy wave system and the prefabs of the enemies")]
    public GameObject waveSystem;

    private float timer;
    private bool alreadyActive;

    /// <summary>
    /// when the final obstacle of the prologue is activated
    /// start the event of the last room
    /// </summary>
    void Update()
    {
        if (flammableObject != null && flammableObject.GetComponent<IActivable>().isActive)
        {
            if (!alreadyActive)
            {
                waveSystem.GetComponent<EnemyWaveSystem>().Activate();
            }

            timer += Time.deltaTime;
            if (timer >= flammableObject.GetComponent<FlammableObjects>().burnTime)
            {
                Destroy(gameObject);
            }
        }

    }
}
