using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

[System.Serializable]
public class ScoreSave
{
    public int maxCombo;
    public int enemyMirrorBroken;
    public int statusAilmentApplied;
    public int killsEnvironment;
    public int damageTakenP1;
    public int damageTakenP2;
    public int numberOfDeaths;
    public int orbHitMissedP1;
    public int orbHitMissedP2;
    public int killsP1;
    public int killsP2;
    public int healPointReceivedP1;
    public int healPointReceivedP2;
    public float completionTime;

    //constructor
    public ScoreSave
        (int _maxCombo,
        int _enemyMirrorBroken,
        int _statusAilmentApplied,
        int _killsEnvironment,
        int _damageTakenP1,
        int _damageTakenP2,
        int _numberOfDeaths,
        int _orbHitMissedP1,
        int _orbHitMissedP2,
        int _killsP1,
        int _killsP2,
        int _healPointReceivedP1,
        int _healPointReceivedP2,
        float _completionTime)
    {
        maxCombo = _maxCombo;
        enemyMirrorBroken = _enemyMirrorBroken;
        statusAilmentApplied = _statusAilmentApplied;
        killsEnvironment = _killsEnvironment;
        damageTakenP1 = _damageTakenP1;
        damageTakenP2 = _damageTakenP2;
        numberOfDeaths = _numberOfDeaths;
        orbHitMissedP1 = _orbHitMissedP1;
        orbHitMissedP2 = _orbHitMissedP2;
        killsP1 = _killsP1;
        killsP2 = _killsP2;
        healPointReceivedP1 = _healPointReceivedP1;
        healPointReceivedP2 = _healPointReceivedP2;
        completionTime = _completionTime;
    }
    
}
