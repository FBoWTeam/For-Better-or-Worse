using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateOnDeath : MonoBehaviour
{
    public GameObject objectToEnable;

    private void OnDestroy()
    {
        objectToEnable.SetActive(true);
    }
}
