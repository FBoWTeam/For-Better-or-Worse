using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateDrainEffect : MonoBehaviour
{
    public GameObject handEffect;


    private void OnEnable()
    {
        handEffect.SetActive(true);
        handEffect.GetComponent<ParticleSystem>().Play();
    }

    private void OnDisable()
    {
        handEffect.GetComponent<ParticleSystem>().Stop();
    }
}
