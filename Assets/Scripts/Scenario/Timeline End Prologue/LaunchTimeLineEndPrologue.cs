using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaunchTimeLineEndPrologue : MonoBehaviour
{
    private void OnDestroy()
    {
        if(GetComponent<FlammableObjects>().isDestroyedByFire)
            GameObject.Find("TimelineEnd").GetComponent<TimeLineEndPrologue>().Initialize();
    }
}
