using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{

    public static ScoreManager scoreManager;
    public float timeToWait;

    [Header("Orb Score")]
    public int maxCombo;
    

    [Header("Enemy Score")]
    public int enemyMirrorBroken;
    public int statusAilmentApplied;

    [Header("Environment Score")]
    public int killsEnvironment;

    [Header("Player Score")]
    public int damageTakenP1;
    public int damageTakenP2;
    public int numberOfDeaths;
    public int orbHitMissedP1;
    public int orbHitMissedP2;
    public int killsP1;
    public int killsP2;
    public int healPointReceivedP1;
    public int healPointReceivedP2;

    [Header("Time Score")]
    public float completionTime;


    // Start is called before the first frame update
    void Start()
    {
        //StartCoroutine(StatPrintCoroutine(timeToWait));
    }

    // Update is called once per frame
    void Update()
    {
        completionTime += Time.deltaTime;

    }

    void Awake()
    {
        if (scoreManager == null)
        {
            scoreManager = this;
        }
        else if (scoreManager != this)
        {
            Destroy(this.gameObject);
        }
        
        completionTime = 0;

        DontDestroyOnLoad(this.gameObject);
    }

    void PrintStat()
    {
        Debug.Log("========== Stats Orb ==========");
        Debug.Log("Combo Max : " + maxCombo);
        Debug.Log("========= Stats Enemy =========");
        Debug.Log("Shield Brisé : " + enemyMirrorBroken);
        Debug.Log("Altération d'etat appliquées : " + statusAilmentApplied);
        Debug.Log("Ennemies morts tout seul comme des grands : " + killsEnvironment);
        Debug.Log("======== Stats Players ========");
        Debug.Log("Nombre de morts : " + numberOfDeaths);

        Debug.Log("======== Stats Player 1 ========");
        Debug.Log("Kills : " + killsP1);
        Debug.Log("Degat reçu : " + damageTakenP1);
        Debug.Log("Point de vie reçu : " + healPointReceivedP1);
        Debug.Log("Renvoie d'orbe raté : " + orbHitMissedP1);
        Debug.Log("======== Stats Player 2 ========");
        Debug.Log("Kills : " + killsP2);
        Debug.Log("Degat reçu : " + damageTakenP2);
        Debug.Log("Point de vie reçu : " + healPointReceivedP2);
        Debug.Log("Renvoie d'orbe raté : " + orbHitMissedP2);
        
        
        float minutes = completionTime / 60;
        float seconds = completionTime % 60;
        Debug.Log("====== Temps de complétion ======");
        Debug.Log("Temps de complétion : " + minutes + ":" + seconds);

    }

    IEnumerator StatPrintCoroutine(float timeToWait)
    {
        while (true)
        {
            yield return new WaitForSeconds(timeToWait);
            PrintStat();
        }
    }

    
    public void KeepMaxCombo(int currentCombo)
    {
        if (currentCombo > maxCombo)
        {
            maxCombo = currentCombo;
        }
    }


}
