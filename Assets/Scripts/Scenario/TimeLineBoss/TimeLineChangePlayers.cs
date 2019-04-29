using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;


public class TimeLineChangePlayers : MonoBehaviour
{
    PlayableDirector director;
    GameObject Boss;
    GameObject bossHealthBar;
    public float angleMysticLine;


    // Start is called before the first frame update
    void Start()
    {
        Boss = GameObject.Find("Boss");
        bossHealthBar = GameObject.Find("Bosshealthbar");
    }

    public void Initialize()
    {
        GameManager.gameManager.isPaused = true;
        GameManager.gameManager.player1.GetComponent<PlayerController>().active = false;
        GameManager.gameManager.player2.GetComponent<PlayerController>().active = false;
        GameManager.gameManager.player1.GetComponent<OrbHitter>().active = false;
        GameManager.gameManager.player2.GetComponent<OrbHitter>().active = false;
        Boss.GetComponent<BossRotation>().enabled = false;
        director = GetComponent<PlayableDirector>();
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
        Boss.GetComponent<BossSystem>().pivotLeft.transform.localRotation = Quaternion.Euler(new Vector3(0f, -(angleMysticLine + 90), 0f));
        Boss.GetComponent<BossSystem>().pivotRight.transform.localRotation = Quaternion.Euler(new Vector3(0f, angleMysticLine + 90, 0f));

        yield return new WaitForSeconds(3f);//wait the animation
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
