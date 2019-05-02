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
    GameObject bossHealthBar;

    // Start is called before the first frame update
    void Start()
    {
        Boss = GameObject.Find("Boss");
        bossHealthBar = GameObject.Find("Bosshealthbar");
    }

    public void Initialize()
    {
        GameManager.gameManager.orb.GetComponent<OrbController>().canHitPlayer = false;

        StartCoroutine(DeathPoofBoss());
        bossHealthBar.SetActive(false);

        director = GetComponent<PlayableDirector>();
        director.Play();
        director.stopped += WhenEnded;
    }

    IEnumerator DeathPoofBoss()
    {
        yield return new WaitForSeconds(0.75f);
        Boss.GetComponent<BossSystem>().FxStealLeft.SetActive(true);
        Boss.GetComponent<BossSystem>().FxStealRight.SetActive(true);
        yield return new WaitForSeconds(4.75f);
        Instantiate(deathPoof, Boss.transform.position, Quaternion.identity);
    }


    public void WhenEnded(PlayableDirector obj)
    {
        GameData.previousScene = 9;
        SceneManager.LoadScene(10);
    }
}
