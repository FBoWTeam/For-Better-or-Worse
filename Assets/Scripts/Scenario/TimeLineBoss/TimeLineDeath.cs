using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;


public class TimeLineDeath : MonoBehaviour
{
    PlayableDirector director;

    // Start is called before the first frame update
    void Start()
    {

    }

    public void Initialize()
    {
        GameManager.gameManager.orb.GetComponent<OrbController>().canHitPlayer = false;

        director = GetComponent<PlayableDirector>();
        director.Play();
        director.stopped += WhenEnded;
    }

    public void WhenEnded(PlayableDirector obj)
    {
        GameData.previousScene = 9;
        SceneManager.LoadScene(10);
    }
}
