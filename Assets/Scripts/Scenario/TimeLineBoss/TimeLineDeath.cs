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
    GameObject bossHealthBar;
    GameObject rockFall;

	[HideInInspector]
	public bool arena = false;

    // Start is called before the first frame update
    void Start()
    {
        Boss = GameObject.Find("Boss");
        bossHealthBar = GameObject.Find("Bosshealthbar");
        rockFall = GameObject.Find("RockFall");
    }

    public void Initialize()
    {
        GameManager.gameManager.canActivatePauseMenu = false;
        GameManager.gameManager.orb.GetComponent<OrbController>().canHitPlayer = false;

        StartCoroutine(DeathPoofBoss());
        bossHealthBar.SetActive(false);

        director = GetComponent<PlayableDirector>();
        director.Play();
        director.stopped += WhenEnded;
    }

    IEnumerator DeathPoofBoss()
    {
        yield return new WaitForSeconds(0.75f);
        Boss.GetComponent<BossSystem>().FxStealLeft.SetActive(true);
        Boss.GetComponent<BossSystem>().FxStealRight.SetActive(true);
        rockFall.SetActive(false);
        yield return new WaitForSeconds(4.75f);
        Boss.GetComponent<BossSystem>().FxStealLeft.SetActive(false);
        Boss.GetComponent<BossSystem>().FxStealRight.SetActive(false);
        Instantiate(deathPoof, Boss.transform.position, Quaternion.identity);
    }


    public void WhenEnded(PlayableDirector obj)
    {
        GameManager.gameManager.canActivatePauseMenu = true;
        GameData.previousScene = 9;
		if(ScoreManager.scoreManager.gameMode == ScoreManager.GameMode.Arena)
		{
			SceneManager.LoadScene(15);
		}
		else
		{
			SceneManager.LoadScene(10);
		}
    }
}
