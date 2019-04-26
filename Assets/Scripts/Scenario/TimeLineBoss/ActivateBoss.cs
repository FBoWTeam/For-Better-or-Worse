using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateBoss : MonoBehaviour
{
    public GameObject boss;


    private void OnEnable()
    {
        boss.GetComponent<BossSystem>().enabled = true;
    }
}
