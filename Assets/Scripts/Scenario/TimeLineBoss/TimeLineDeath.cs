using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;


public class TimeLineDeath : MonoBehaviour
{
    PlayableDirector director;

    // Start is called before the first frame update
    void Start()
    {

    }

    public void Initialize()
    {
        director.Play();
    }

}
