using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaunchTimeLineEndPrologue : MonoBehaviour
{
    public TimeLineEndPrologue endPrologue;

    private void OnDestroy()
    {
        Debug.Log("End prologue timeline launch");
        endPrologue.Initialize();
    }
}
