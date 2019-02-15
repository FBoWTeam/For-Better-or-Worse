using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(NavMeshAgent))]
public class Enemy : MonoBehaviour
{
    #region All Variables

    public int baseHP;
    public int hp;

    #region Debug Variables
    public bool debug;
    [DrawIf(new string[] { "debug" }, true)]
    public static bool drawPath = false;
    #endregion

    private bool coliding = false;

    public EnemyMovement enemyMovement;

    public static GameObject target;

    GameObject[] players = { GameManager.gameManager.player1.gameObject, GameManager.gameManager.player2.gameObject };

    #endregion


    // Start is called before the first frame update
    void Start()
    {
        hp = baseHP;

        enemyMovement = new EnemyMovement();

    }
    // Update is called once per frame
    void Update() {

        if (!enemyMovement.agent.isStopped)
        {
            enemyMovement.DoMovement();
        }

        target = GetNearestGO(players);

        if (hp <= 0)
        {
            enemyMovement.agent.isStopped = true;
            StopAllCoroutines();
            Destroy(this.gameObject);
        }
    }



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

}
