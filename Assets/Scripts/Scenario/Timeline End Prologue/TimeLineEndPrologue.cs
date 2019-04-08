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
        director = GetComponent<PlayableDirector>();
        GameManager.gameManager.orb.GetComponent<OrbController>().canHitPlayer = false;
        GameManager.gameManager.UIManager.gameObject.SetActive(false);
        GameManager.gameManager.blackBands.SetActive(true);
        GameObject.Find("SceneLoader").SetActive(false);

        director.Play();
        director.stopped += WhenEnded;
    }

    public void WhenEnded(PlayableDirector obj)
    {
        Debug.Log("End Prologue");
        //SceneManager.LoadScene("test");
    }
}
