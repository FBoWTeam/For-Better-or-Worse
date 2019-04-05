using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;


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
    public int damageDealtBossP1;
    public int damageDealtBossP2;
    public bool bossKilledByP1;

    [Header("Time Score")]
    public float completionTime;

    private int numberOfPlayer;

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


    public void Save()
    {
        string fileName = System.DateTime.Now.ToString();

        fileName = fileName.Replace(@"\", "-");
        fileName = fileName.Replace("/", "-");
        fileName = fileName.Replace(":", "-");
        

        if (!Directory.Exists(Application.persistentDataPath + "/Saves"))
        {
            Directory.CreateDirectory(Application.persistentDataPath + "/Saves");
            Debug.Log("Saves Folder Created");
        }

        string destination = Application.persistentDataPath + "/Saves/" + fileName + ".txt";
        
        StreamWriter sw = File.CreateText(destination);

        sw.WriteLine("maxCombo " + maxCombo);
        sw.WriteLine("enemyMirrorBroken " + enemyMirrorBroken);
        sw.WriteLine("statusAilmentApplied " + statusAilmentApplied);
        sw.WriteLine("killsEnvironment " + killsEnvironment);
        sw.WriteLine("damageTakenP1 " + damageTakenP1);
        sw.WriteLine("damageTakenP2 " + damageTakenP2);
        sw.WriteLine("numberOfDeaths " + numberOfDeaths);
        sw.WriteLine("orbHitMissedP1 " + orbHitMissedP1);
        sw.WriteLine("orbHitMissedP2 " + orbHitMissedP2);
        sw.WriteLine("killsP1 " + killsP1);
        sw.WriteLine("killsP2 " + killsP2);
        sw.WriteLine("healPointReceivedP1 " + healPointReceivedP1);
        sw.WriteLine("healPointReceivedP2 " + healPointReceivedP2);

        sw.WriteLine("damageDealtBossP1 " + damageDealtBossP1);
        sw.WriteLine("damageDealtBossP2 " + damageDealtBossP2);
        sw.WriteLine("bossKilledByP1 " + bossKilledByP1);

        sw.WriteLine("completionTime " + completionTime);

        sw.Close();
    }

}
