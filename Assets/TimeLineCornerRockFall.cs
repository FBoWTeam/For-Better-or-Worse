using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class TimeLineCornerRockFall : MonoBehaviour
{
    PlayableDirector director;
    GameObject WallForTimeLine;
    GameObject Boss;
    GameObject RockLineAnimatation;
    GameObject RockLine1;
    GameObject RockLine2;

    // Start is called before the first frame update
    void Start()
    {
        WallForTimeLine = GameObject.Find("Wall Reverse");
        WallForTimeLine.SetActive(false);
        Boss = GameObject.Find("Boss");
        RockLineAnimatation = GameObject.Find("Rock line Animation");
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

    public void WhenEnded(PlayableDirector obj)
    {
        WallForTimeLine.SetActive(false);
        Boss.GetComponent<BossRotation>().enabled = true;
        GameManager.gameManager.player1.GetComponent<CapsuleCollider>().isTrigger = false;
        GameManager.gameManager.player2.GetComponent<CapsuleCollider>().isTrigger = false;
        GameManager.gameManager.isPaused = false;
        GameManager.gameManager.player1.GetComponent<PlayerController>().active = true;
        GameManager.gameManager.player2.GetComponent<PlayerController>().active = true;
        GameManager.gameManager.player1.GetComponent<OrbHitter>().active = true;
        GameManager.gameManager.player2.GetComponent<OrbHitter>().active = true;
        GameManager.gameManager.UIManager.gameObject.SetActive(true);
        GameManager.gameManager.blackBands.SetActive(false);
        Boss.SetActive(true);
        Boss.GetComponent<BossSystem>().isAttacking = false;
        RockLineAnimatation.SetActive(false);
        RockLine1.SetActive(true);
        RockLine2.SetActive(true);
    }

    IEnumerator InitCoroutine()
    {
        yield return new WaitForSeconds(1.5f);//fade out
        WallForTimeLine.SetActive(true);
        Boss.GetComponent<BossSystem>().CleanProjectorList();
        Boss.GetComponent<BossSystem>().CleanMysticLineList();
        GameManager.gameManager.orb.GetComponent<OrbController>().canHitPlayer = false;
        GameManager.gameManager.UIManager.gameObject.SetActive(false);
        GameManager.gameManager.blackBands.SetActive(true);
        GameManager.gameManager.player1.GetComponent<CapsuleCollider>().isTrigger = true;
        GameManager.gameManager.player2.GetComponent<CapsuleCollider>().isTrigger = true;

        yield return new WaitForSeconds(4f);//wait the animation
        StartCoroutine(Boss.GetComponent<BossSystem>().ShrinkMysticLinesCoroutine());
    }

}
