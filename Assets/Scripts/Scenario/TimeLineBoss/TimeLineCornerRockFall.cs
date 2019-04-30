﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class TimeLineCornerRockFall : MonoBehaviour
{
    PlayableDirector director;
    GameObject Boss;
    GameObject bossHealthBar;
    GameObject RockCornerAnimatation;
    GameObject RockCorner1;
    GameObject RockCorner2;
    GameObject RockCorner3;
    GameObject RockCorner4;

    GameObject Brazier1;
    GameObject Brazier2;
    GameObject ElectricPylon1;
    GameObject ElectricPylon2;
    
    public RuntimeAnimatorController RAC;

    // Start is called before the first frame update
    void Start()
    {
        Boss = GameObject.Find("Boss");
        bossHealthBar = GameObject.Find("Bosshealthbar");
        RockCornerAnimatation = GameObject.Find("Rock corner Animation");
        RockCorner1 = GameObject.Find("Rock corner 1");
        RockCorner1.SetActive(false);
        RockCorner2 = GameObject.Find("Rock corner 2");
        RockCorner2.SetActive(false);
        RockCorner3 = GameObject.Find("Rock corner 3");
        RockCorner3.SetActive(false);
        RockCorner4 = GameObject.Find("Rock corner 4");
        RockCorner4.SetActive(false);
        Brazier1 = GameObject.Find("Brazier");
        Brazier2 = GameObject.Find("Brazier (1)");
        ElectricPylon1 = GameObject.Find("ElectricPylon");
        ElectricPylon2 = GameObject.Find("ElectricPylon (1)");
    }

    public void Initialize()
    {
        GameManager.gameManager.isPaused = true;
        GameManager.gameManager.player1.GetComponent<PlayerController>().active = false;
        GameManager.gameManager.player2.GetComponent<PlayerController>().active = false;
        GameManager.gameManager.player1.GetComponent<OrbHitter>().active = false;
        GameManager.gameManager.player2.GetComponent<OrbHitter>().active = false;
        director = GetComponent<PlayableDirector>();
        Boss.GetComponent<BossRotation>().enabled = false;
        Boss.GetComponent<Animator>().runtimeAnimatorController = null;
        StartCoroutine(InitCoroutine());

        director.Play();
        director.stopped += WhenEnded;
    }
    

    IEnumerator InitCoroutine()
    {
        yield return new WaitForSeconds(1.5f);//fade out
        Boss.GetComponent<BossSystem>().isAttacking = true;
        Boss.GetComponent<BossSystem>().CleanProjectorList();
        GameManager.gameManager.orb.GetComponent<OrbController>().canHitPlayer = false;
        GameManager.gameManager.UIManager.gameObject.SetActive(false);
        GameManager.gameManager.blackBands.SetActive(true);
        bossHealthBar.SetActive(false);
        GameManager.gameManager.player1.GetComponent<CapsuleCollider>().isTrigger = true;
        GameManager.gameManager.player2.GetComponent<CapsuleCollider>().isTrigger = true;

        yield return new WaitForSeconds(3f);

        Destroy(Boss.GetComponent<BossSystem>().shrinkLeft);
        Destroy(Boss.GetComponent<BossSystem>().shrinkRight);
        Boss.GetComponent<BossSystem>().isShrinkMysticLineCreated = false;

        yield return new WaitForSeconds(7.5f);

    }


    public void WhenEnded(PlayableDirector obj)
    {
        GameManager.gameManager.player1.GetComponent<CapsuleCollider>().isTrigger = false;
        GameManager.gameManager.player2.GetComponent<CapsuleCollider>().isTrigger = false;
        GameManager.gameManager.isPaused = false;
        GameManager.gameManager.player1.GetComponent<PlayerController>().active = true;
        GameManager.gameManager.player2.GetComponent<PlayerController>().active = true;
        GameManager.gameManager.player1.GetComponent<OrbHitter>().active = true;
        GameManager.gameManager.player2.GetComponent<OrbHitter>().active = true;

        GameManager.gameManager.UIManager.gameObject.SetActive(true);
        GameManager.gameManager.blackBands.SetActive(false);
        bossHealthBar.SetActive(true);

        RockCornerAnimatation.SetActive(false);
        RockCorner1.SetActive(true);
        RockCorner2.SetActive(true);
        RockCorner3.SetActive(true);
        RockCorner4.SetActive(true);
        Brazier1.SetActive(false);
        Brazier2.SetActive(false);
        ElectricPylon1.SetActive(false);
        ElectricPylon2.SetActive(false);

        Destroy(GetComponent<PlayableDirector>());

        StartCoroutine(End());
    }

    IEnumerator End()
    {
        yield return new WaitForSeconds(1.5f);
        Boss.GetComponent<Animator>().runtimeAnimatorController = RAC;
        Boss.GetComponent<BossSystem>().isAttacking = false;
        GameManager.gameManager.orb.GetComponent<OrbController>().canHitPlayer = GameData.worseModeActivated;
    }

}
