using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProtectionSystem : MonoBehaviour
{
    public GameObject theBoss;

    public bool waterWallsDestroyed;
    public bool plantWallsDestroyed;

    public void checkProtectionSystemStatus()
    {
        if (waterWallsDestroyed && plantWallsDestroyed)
        {
            theBoss.GetComponent<BossSystem>().canHitBoss = true;
            Destroy(gameObject);
        }
    }
}
