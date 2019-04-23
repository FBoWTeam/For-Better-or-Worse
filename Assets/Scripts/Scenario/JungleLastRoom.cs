using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class JungleLastRoom : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ScoreManager.scoreManager.Save();
        }
    }
}
