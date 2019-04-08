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

        director.Play();
        director.stopped += WhenEnded;
    }

    public void WhenEnded(PlayableDirector obj)
    {
        SceneManager.LoadScene("test");
    }
}
