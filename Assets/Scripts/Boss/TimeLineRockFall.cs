using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class TimeLineRockFall : MonoBehaviour
{
    PlayableDirector director;
    GameObject Boss;
    GameObject bossHealthBar;
    GameObject RockLineAnimation;
    GameObject RockLine1;
    GameObject RockLine2;

    // Start is called before the first frame update
    void Start()
    {
        Boss = GameObject.Find("Boss");
        bossHealthBar = GameObject.Find("Bosshealthbar");
        RockLineAnimation = GameObject.Find("Rock line Animation");
        RockLine1 = GameObject.Find("Rock line 1");
        RockLine1.SetActive(false);
        RockLine2 = GameObject.Find("Rock line 2");
        RockLine2.SetActive(false);
    }

    public void Initialize()
    {
        GameManager.gameManager.isPaused = true;
        GameManager.gameManager.player1.GetComponent<PlayerController>().active = false;
        GameManager.gameManager.player2.GetComponent<PlayerController>().active = false;
        GameManager.gameManager.player1.GetComponent<OrbHitter>().active = false;
        GameManager.gameManager.player2.GetComponent<OrbHitter>().active = false;
        director = GetComponent<PlayableDirector>();
        StartCoroutine(InitCoroutine());

        director.Play();
        director.stopped += WhenEnded;
    }


    IEnumerator InitCoroutine()
    {
        yield return new WaitForSeconds(1.5f);//fade out
        PlayersContent.transform.position += new Vector3(0.0f, 0.0f, -7.0f);
        Boss.GetComponent<BossSystem>().isAttacking = true;
        Boss.GetComponent<BossSystem>().CleanProjectorList();
        Boss.GetComponent<BossSystem>().CleanMysticLineList();
        GameManager.gameManager.orb.GetComponent<OrbController>().canHitPlayer = false;
        GameManager.gameManager.UIManager.gameObject.SetActive(false);
        GameManager.gameManager.blackBands.SetActive(true);
        bossHealthBar.SetActive(false);
        GameManager.gameManager.player1.GetComponent<CapsuleCollider>().isTrigger = true;
        GameManager.gameManager.player2.GetComponent<CapsuleCollider>().isTrigger = true;

        yield return new WaitForSeconds(4f);//wait the animation
        StartCoroutine(Boss.GetComponent<BossSystem>().ShrinkMysticLinesCoroutine());        
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
        Boss.SetActive(true);
        RockLineAnimation.SetActive(false);
        RockLine1.SetActive(true);
        RockLine2.SetActive(true);
        Destroy(GetComponent<PlayableDirector>());

        StartCoroutine(End());
    }


    IEnumerator End()
    {
        yield return new WaitForSeconds(1.5f);
        Boss.GetComponent<BossRotation>().enabled = true;
        Boss.GetComponent<BossSystem>().isAttacking = false;
        GameManager.gameManager.orb.GetComponent<OrbController>().canHitPlayer = GameData.worseModeActivated;
    }

}
