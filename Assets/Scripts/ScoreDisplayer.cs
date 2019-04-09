using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using UnityEngine;

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
    public GameObject compeltionTime;
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
    }


}
