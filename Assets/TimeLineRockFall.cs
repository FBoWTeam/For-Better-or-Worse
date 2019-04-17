using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class TimeLineRockFall : MonoBehaviour
{
    PlayableDirector director;
    GameObject WallForTimeLine;

    // Start is called before the first frame update
    void Start()
    {
        WallForTimeLine = GameObject.Find("Wall Reverse");
        Debug.Log("wall");
        WallForTimeLine.SetActive(false);
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
        GameManager.gameManager.player1.GetComponent<CapsuleCollider>().isTrigger = false;
        GameManager.gameManager.player2.GetComponent<CapsuleCollider>().isTrigger = false;
        GameManager.gameManager.isPaused = false;
        GameManager.gameManager.player1.GetComponent<PlayerController>().active = true;
        GameManager.gameManager.player2.GetComponent<PlayerController>().active = true;
        GameManager.gameManager.player1.GetComponent<OrbHitter>().active = true;
        GameManager.gameManager.player2.GetComponent<OrbHitter>().active = true;
        GameManager.gameManager.UIManager.gameObject.SetActive(true);
        GameManager.gameManager.blackBands.SetActive(false);
    }

    IEnumerator InitCoroutine()
    {
        yield return new WaitForSeconds(1.5f);
        WallForTimeLine.SetActive(true);
        GameManager.gameManager.orb.GetComponent<OrbController>().canHitPlayer = false;
        GameManager.gameManager.UIManager.gameObject.SetActive(false);
        GameManager.gameManager.blackBands.SetActive(true);
        GameManager.gameManager.player1.GetComponent<CapsuleCollider>().isTrigger = true;
        GameManager.gameManager.player2.GetComponent<CapsuleCollider>().isTrigger = true;
    }

}
