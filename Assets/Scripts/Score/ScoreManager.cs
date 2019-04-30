using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.SceneManagement;



public class ScoreManager : MonoBehaviour
{
    public enum GameMode
    {
        Story,
        Arena
    }

    public static ScoreManager scoreManager;
    
    public GameMode gameMode;

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

    [Header("Total Wave")]
    public int totalWave;

    private int numberOfPlayer;

    public float timeStamp;
    public float score;

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

		DontDestroyOnLoad(this.gameObject);

		completionTime = 0;

        timeStamp = Time.time;
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

        completionTime = Time.time - timeStamp;


        if (gameMode == GameMode.Story)
        {
            score = CalculatePrologueScore();
        }
        else if (gameMode == GameMode.Arena)
        {
            score = CalculateArenaScore();
        }

        
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

        sw.WriteLine("score " + score);

        sw.Close();
    }


    public float CalculatePrologueScore()
    {
        float timeScore = 1 / (0.00007f * completionTime);
        float bonus = timeScore + maxCombo + (statusAilmentApplied + enemyMirrorBroken + killsP1 + killsP2) / 2;
        float malus = (damageTakenP1 + damageTakenP2) / 50 + (orbHitMissedP1 + orbHitMissedP2) / 10 + numberOfDeaths * 5;
        float result = bonus - malus;
        return result;
    }


    public float CalculateJungle1Score()
    {
        float timeScore = 1 / (0.00005f * completionTime);
        float bonus = timeScore + maxCombo + (statusAilmentApplied + enemyMirrorBroken + killsP1 + killsP2) / 2;
        float malus = (damageTakenP1 + damageTakenP2) / 50 + (orbHitMissedP1 + orbHitMissedP2) / 10 + numberOfDeaths * 5;
        float result = bonus - malus;
        return result;
    }

    public float CalculateJungle2Score()
    {
        float timeScore = 1 / (0.00007f * completionTime);
        float bonus = timeScore + maxCombo + (statusAilmentApplied + enemyMirrorBroken + killsP1 + killsP2) / 2;
        float malus = (damageTakenP1 + damageTakenP2) / 50 + (orbHitMissedP1 + orbHitMissedP2) / 10 + numberOfDeaths * 5;
        float result = bonus - malus;
        return result;
    }

    public float CalculateBossScore()
    {
        float timeScore = 1 / (0.00007f * completionTime);
        float bonus = timeScore + maxCombo + (statusAilmentApplied);
        float malus = (damageTakenP1 + damageTakenP2) / 10 + (orbHitMissedP1 + orbHitMissedP2) / 10;
        float result = bonus - malus;
        return result;
    }

    public float CalculateArenaScore()
    {
        return totalWave;
    }

}
