using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;


public class TimeLineDeath : MonoBehaviour
{
    PlayableDirector director;
    GameObject bossHealthBar;

    // Start is called before the first frame update
    void Start()
    {
        bossHealthBar = GameObject.Find("Bosshealthbar");
    }

    public void Initialize()
    {
        director.Play();
        bossHealthBar.SetActive(false);
    }

}
