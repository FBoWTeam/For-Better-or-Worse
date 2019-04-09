using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;



public class TimeLineEndPrologue : MonoBehaviour
{

    PlayableDirector director;

    public void Initialize()
    {
        GameManager.gameManager.isPaused = true;
        director = GetComponent<PlayableDirector>();
        GameManager.gameManager.orb.GetComponent<OrbController>().canHitPlayer = false;
        GameManager.gameManager.UIManager.gameObject.SetActive(false);
        GameManager.gameManager.blackBands.SetActive(true);
        GameObject.Find("SceneLoader").SetActive(false);
        GameObject.Find("Enemy_wave").SetActive(false);
        GameObject.Find("Ennemis").SetActive(false);
        GameManager.gameManager.player1.GetComponent<CapsuleCollider>().isTrigger = true;
        GameManager.gameManager.player2.GetComponent<CapsuleCollider>().isTrigger = true;

        director.Play();
        director.stopped += WhenEnded;
    }

    public void WhenEnded(PlayableDirector obj)
    {
        Debug.Log("End Prologue");
        GameManager.gameManager.player1.GetComponent<CapsuleCollider>().isTrigger = false;
        GameManager.gameManager.player2.GetComponent<CapsuleCollider>().isTrigger = false;
        GameManager.gameManager.isPaused = false;
        //SceneManager.LoadScene("test");
    }
}
