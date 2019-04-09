using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScoreDisplayer : MonoBehaviour
{
    [Header("Score P1")]
    public GameObject killsP1;
    public GameObject damageTakenP1;
    public GameObject orbMissedP1;

    [Header("Score P2")]
    public GameObject killsP2;
    public GameObject damageTakenP2;
    public GameObject orbMissedP2;

    [Header("Shared Score")]
    public GameObject completionTime;
    public GameObject maxCombo;
    public GameObject useOfPower;
    public GameObject numberOfDeath;
    public GameObject overallKills;
    public GameObject overallDamageTaken;
    public GameObject overallOrbMissed;

    [Header("Final Score")]
    public GameObject finalScore;
    
    public List<GameObject> stars;

    private void Start()
    {
        killsP1.GetComponent<TextMeshProUGUI>().text = ScoreManager.scoreManager.killsP1.ToString();
        damageTakenP1.GetComponent<TextMeshProUGUI>().text = ScoreManager.scoreManager.damageTakenP1.ToString();
        orbMissedP1.GetComponent<TextMeshProUGUI>().text = ScoreManager.scoreManager.orbHitMissedP1.ToString();

        killsP2.GetComponent<TextMeshProUGUI>().text = ScoreManager.scoreManager.killsP2.ToString();
        damageTakenP2.GetComponent<TextMeshProUGUI>().text = ScoreManager.scoreManager.damageTakenP2.ToString();
        orbMissedP2.GetComponent<TextMeshProUGUI>().text = ScoreManager.scoreManager.orbHitMissedP2.ToString();
        
        completionTime.GetComponent<TextMeshProUGUI>().text = ((float)Mathf.Round(ScoreManager.scoreManager.completionTime * 100f) / 100f).ToString();
        maxCombo.GetComponent<TextMeshProUGUI>().text = ScoreManager.scoreManager.maxCombo.ToString();
        useOfPower.GetComponent<TextMeshProUGUI>().text = (ScoreManager.scoreManager.enemyMirrorBroken + ScoreManager.scoreManager.statusAilmentApplied).ToString();
        numberOfDeath.GetComponent<TextMeshProUGUI>().text = ScoreManager.scoreManager.numberOfDeaths.ToString();
        overallKills.GetComponent<TextMeshProUGUI>().text = (ScoreManager.scoreManager.killsP1 + ScoreManager.scoreManager.killsP2).ToString();
        overallDamageTaken.GetComponent<TextMeshProUGUI>().text = (ScoreManager.scoreManager.damageTakenP1 + ScoreManager.scoreManager.damageTakenP2).ToString();
        overallOrbMissed.GetComponent<TextMeshProUGUI>().text = (ScoreManager.scoreManager.orbHitMissedP1 + ScoreManager.scoreManager.orbHitMissedP2).ToString();



        float finalScoreValue = ScoreManager.scoreManager.CalculatePrologueScore();
        finalScore.GetComponent<TextMeshProUGUI>().text = (finalScoreValue).ToString();

        if (finalScoreValue > 90)
        {
            ShowStars(5);
        }
        else if (finalScoreValue > 70)
        {
            ShowStars(4);
        }
        else if (finalScoreValue > 50)
        {
            ShowStars(3);
        }
        else if (finalScoreValue > 30)
        {
            ShowStars(2);
        }
        else
        {
            ShowStars(1);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SceneManager.LoadScene(0);
        }
    }


    void ShowStars(int nbStars)
    {
        for (int i = 0; i < nbStars; i++)
        {
            stars[i].SetActive(true);
        }
    }


}
