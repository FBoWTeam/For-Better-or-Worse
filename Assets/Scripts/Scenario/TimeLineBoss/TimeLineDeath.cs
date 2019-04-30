using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;


public class TimeLineDeath : MonoBehaviour
{
    PlayableDirector director;
    GameObject Boss;
    public GameObject deathPoof;
<<<<<<< HEAD
    GameObject bossHealthBar;
=======
>>>>>>> parent of dbab6b20... Merge branch 'Develop' of https://github.com/FBoWTeam/For-Better-or-Worse into Develop

    // Start is called before the first frame update
    void Start()
    {
        Boss = GameObject.Find("Boss");
<<<<<<< HEAD
        bossHealthBar = GameObject.Find("Bosshealthbar");
=======
>>>>>>> parent of dbab6b20... Merge branch 'Develop' of https://github.com/FBoWTeam/For-Better-or-Worse into Develop
    }

    public void Initialize()
    {
        GameManager.gameManager.orb.GetComponent<OrbController>().canHitPlayer = false;

        StartCoroutine(DeathPoofBoss());
<<<<<<< HEAD
        bossHealthBar.SetActive(false);
=======
>>>>>>> parent of dbab6b20... Merge branch 'Develop' of https://github.com/FBoWTeam/For-Better-or-Worse into Develop

        director = GetComponent<PlayableDirector>();
        director.Play();
        director.stopped += WhenEnded;
    }

    IEnumerator DeathPoofBoss()
    {
        yield return new WaitForSeconds(3.5f);
        Instantiate(deathPoof, Boss.transform.position, Quaternion.identity);
    }


    public void WhenEnded(PlayableDirector obj)
    {
        GameData.previousScene = 9;
        SceneManager.LoadScene(10);
    }
}
