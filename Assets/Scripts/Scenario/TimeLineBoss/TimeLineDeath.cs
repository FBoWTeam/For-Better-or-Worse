﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;


public class TimeLineDeath : MonoBehaviour
{
    PlayableDirector director;
    GameObject Boss;
    public GameObject deathPoof;

    // Start is called before the first frame update
    void Start()
    {
        Boss = GameObject.Find("Boss");
    }

    public void Initialize()
    {
        GameManager.gameManager.orb.GetComponent<OrbController>().canHitPlayer = false;

        StartCoroutine(DeathPoofBoss());

        director = GetComponent<PlayableDirector>();
        director.Play();
        director.stopped += WhenEnded;
    }

    IEnumerator DeathPoofBoss()
    {
        yield return new WaitForSeconds(2.5f);
        Instantiate(deathPoof, Boss.transform.position, Quaternion.identity);
    }


    public void WhenEnded(PlayableDirector obj)
    {
        GameData.previousScene = 9;
        SceneManager.LoadScene(10);
    }
}
