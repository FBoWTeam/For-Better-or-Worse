using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(NavMeshAgent))]
public class Enemy : MonoBehaviour
{
    #region All Variables

    #region Debug Variables
    public bool debug;
    [DrawIf(new string[] { "debug" }, true)]
    public bool drawPath = false;
    public bool drawView = false;
    [DrawIf(new string[] { "drawView" }, true)]
    public float lengthView = 2f;


    public static bool sdrawPath;
    #endregion

    public enum Focus
    {
        Player1,
        Player2,
        Nearest,
    }
    public Focus focus = Focus.Nearest;

    public enum Taunt
    {
        Taunter,
        Other,
    }
    public Taunt taunt = Taunt.Taunter;
    private GameObject taunter;
    public bool isTaunted = false;

    public int baseHP = 100;
    public int hp;

    [HideInInspector]
    public EnemyMovement enemyMovement;

    GameObject[] players;
    public static GameObject target;

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        hp = baseHP;
        players = new GameObject[] { GameManager.gameManager.player1, GameManager.gameManager.player2 };
        enemyMovement = GetComponent<EnemyMovement>();
        sdrawPath = drawPath;
    }

    // Update is called once per frame
    void Update()
    {
        FocusManagement();
        TauntManagement();

        if (!enemyMovement.agent.isStopped)
        {
            enemyMovement.DoMovement();
        }

        if (hp <= 0)
        {
            enemyMovement.agent.isStopped = true;
            StopAllCoroutines();
            Destroy(this.gameObject);
        }

        if (drawView)
        {
            Debug.DrawRay(this.transform.position, this.transform.forward * lengthView, Color.magenta);
        }
    }

    #region Focus Methods
    /// <summary>
    /// Manage which target assigned to the enemy
    /// </summary>
    public void FocusManagement()
    {
        switch (focus)
        {
            case Focus.Player1:
                target = GameManager.gameManager.player1;
                break;
            case Focus.Player2:
                target = GameManager.gameManager.player2;
                break;
            case Focus.Nearest:
                target = GetNearestGO(players);
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// Determines the nearest Gameobject (according to position) in the list
    /// </summary>
    /// <param name="gos">Take a GameObject list</param>
    /// <returns></returns>
    public GameObject GetNearestGO(GameObject[] gos)
    {
        float min = Mathf.Infinity;
        GameObject nearest = null;
        foreach (var item in gos)
        {
            float dist = Vector3.Distance(transform.position, item.transform.position);
            if (dist < min)
            {
                min = dist;
                nearest = item.gameObject;
            }
        }
        return nearest;
    }

    #endregion

    private void TauntManagement()
    {
        if (!isTaunted)
        {
            if (enemyMovement.agent.remainingDistance <= GameManager.gameManager.tauntRange)
            {
                if (GameManager.gameManager.player1HasTaunt)
                {
                    taunter = players[0];
                }
                else if (GameManager.gameManager.player2HasTaunt)
                {
                    taunter = players[1];
                }

                if (taunter != null)
                {
                    switch (taunt)
                    {
                        case Taunt.Taunter:
                            target = taunter;
                            break;
                        case Taunt.Other:
                            target = (taunter.Equals(players[0])) ? players[1] : players[0];
                            break;
                        default:
                            break;
                    }

                    isTaunted = true;
                }
            }
        }
    }
}
