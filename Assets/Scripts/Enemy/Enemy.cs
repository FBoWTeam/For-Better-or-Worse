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
    public bool debug = false;
    [DrawIf(new string[] { "debug" }, true)]
    public static bool drawPath = false;
    #endregion

    public enum Focus
    {
        Player1,
        Player2,
        Nearest,
    }
    public Focus focus = Focus.Nearest;

    public int baseHP;
    public int hp;

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

    }

    // Update is called once per frame
    void Update()
    {
        FocusManagement();

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
}
