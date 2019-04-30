using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScoreDisplayer : MonoBehaviour
{
    public enum ScoreMode { Story, Arena };

    public ScoreMode scoreMode;

    public GameObject levelName;

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

    [DrawIf(new string[] { "scoreMode" }, ScoreMode.Story)]
    public GameObject numberOfDeath;

    public GameObject overallKills;
    public GameObject overallDamageTaken;
    public GameObject overallOrbMissed;

    [Header("Final Score")]
    public GameObject finalScore;

    [DrawIf(new string[] { "scoreMode" }, ScoreMode.Story)]
    public GameObject starCanvas;
    

    private void Start()
    {

        ScoreManager.scoreManager.completionTime = Time.time - ScoreManager.scoreManager.timeStamp;

        switch (GameData.previousScene)
		{
			case 6:
				levelName.GetComponent<TextMeshProUGUI>().text = I18n.Translate("menu.score.texte1");
                ScoreManager.scoreManager.score = ScoreManager.scoreManager.CalculatePrologueScore();
                break;
			case 7:
				levelName.GetComponent<TextMeshProUGUI>().text = I18n.Translate("menu.score.texte2");
                ScoreManager.scoreManager.score = ScoreManager.scoreManager.CalculateJungle1Score();
                break;
			case 8:
				levelName.GetComponent<TextMeshProUGUI>().text = I18n.Translate("menu.score.texte3");
                ScoreManager.scoreManager.score = ScoreManager.scoreManager.CalculateJungle2Score();
                break;
			case 9:
				levelName.GetComponent<TextMeshProUGUI>().text = I18n.Translate("menu.score.texte4");
                ScoreManager.scoreManager.score = ScoreManager.scoreManager.CalculateBossScore();
                break;
			case 12:
				levelName.GetComponent<TextMeshProUGUI>().text = I18n.Translate("menu.score.texte15");
                ScoreManager.scoreManager.score = ScoreManager.scoreManager.CalculateArenaScore();
                break;
			case 13:
				levelName.GetComponent<TextMeshProUGUI>().text = I18n.Translate("menu.score.texte16");
                ScoreManager.scoreManager.score = ScoreManager.scoreManager.CalculateArenaScore();
                break;
			case 14:
				levelName.GetComponent<TextMeshProUGUI>().text = I18n.Translate("menu.score.texte17");
                ScoreManager.scoreManager.score = ScoreManager.scoreManager.CalculateArenaScore();
                break;
			default:
				levelName.GetComponent<TextMeshProUGUI>().text = "not a normal scene";
				break;
		}
        

        killsP1.GetComponent<TextMeshProUGUI>().text = ScoreManager.scoreManager.killsP1.ToString();
        damageTakenP1.GetComponent<TextMeshProUGUI>().text = ScoreManager.scoreManager.damageTakenP1.ToString();
        orbMissedP1.GetComponent<TextMeshProUGUI>().text = ScoreManager.scoreManager.orbHitMissedP1.ToString();

        killsP2.GetComponent<TextMeshProUGUI>().text = ScoreManager.scoreManager.killsP2.ToString();
        damageTakenP2.GetComponent<TextMeshProUGUI>().text = ScoreManager.scoreManager.damageTakenP2.ToString();
        orbMissedP2.GetComponent<TextMeshProUGUI>().text = ScoreManager.scoreManager.orbHitMissedP2.ToString();

        completionTime.GetComponent<TextMeshProUGUI>().text = ((float)Mathf.Round(ScoreManager.scoreManager.completionTime * 100f) / 100f).ToString();
        maxCombo.GetComponent<TextMeshProUGUI>().text = ScoreManager.scoreManager.maxCombo.ToString();
        useOfPower.GetComponent<TextMeshProUGUI>().text = (ScoreManager.scoreManager.enemyMirrorBroken + ScoreManager.scoreManager.statusAilmentApplied).ToString();

        overallKills.GetComponent<TextMeshProUGUI>().text = (ScoreManager.scoreManager.killsP1 + ScoreManager.scoreManager.killsP2).ToString();
        overallDamageTaken.GetComponent<TextMeshProUGUI>().text = (ScoreManager.scoreManager.damageTakenP1 + ScoreManager.scoreManager.damageTakenP2).ToString();
        overallOrbMissed.GetComponent<TextMeshProUGUI>().text = (ScoreManager.scoreManager.orbHitMissedP1 + ScoreManager.scoreManager.orbHitMissedP2).ToString();


        if (scoreMode == ScoreMode.Story)
        {
            numberOfDeath.GetComponent<TextMeshProUGUI>().text = ScoreManager.scoreManager.numberOfDeaths.ToString();
            float finalScoreValue = ScoreManager.scoreManager.score;
            finalScore.GetComponent<TextMeshProUGUI>().text = (finalScoreValue).ToString();

            if (finalScoreValue > 90)
            {
                starCanvas.transform.GetChild(4).gameObject.SetActive(true);
            }
            else if (finalScoreValue > 70)
            {
                starCanvas.transform.GetChild(3).gameObject.SetActive(true);
            }
            else if (finalScoreValue > 50)
            {
                starCanvas.transform.GetChild(2).gameObject.SetActive(true);
            }
            else if (finalScoreValue > 30)
            {
                starCanvas.transform.GetChild(1).gameObject.SetActive(true);
            }
            else
            {
                starCanvas.transform.GetChild(0).gameObject.SetActive(true);
            }
        }
        else
        {
            finalScore.GetComponent<TextMeshProUGUI>().text =  ScoreManager.scoreManager.totalWave.ToString();
        }
	}
}
