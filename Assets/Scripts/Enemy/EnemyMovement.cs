using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{

    #region Movement Variables
    public enum Movement
    {
        Static,
        Basic,
        Ranged,
        Fleeing,
        Dodging,
        ZigZag,
        Sentinelle,
        Charge,
        Tunnel,
    };


    public Movement movement;

    [DrawIf(new string[] { "movement" }, Movement.Basic)]
    public float initialSpeed = 2f;

    [DrawIf(new string[] { "movement" }, Movement.Basic)]
    [Tooltip("represents the time of the attack animation")]
    public float stopTime = 2f;

    //[DrawIf(new string[] { "movement" }, Movement.Basic)]
    //[Tooltip("represents the remaining distance between the enemy and the player")]
    //public float distanceBetweenPlayer = 5f;

    private LineRenderer line;
    #endregion

    [HideInInspector]
    public NavMeshAgent agent;

    //private
    public bool isSlowed;

    //private
    public bool isMoving;

    EnemySkill enemySkill;

    // Start is called before the first frame update
    void Start()
    {
        enemySkill = this.GetComponent<EnemySkill>();
        agent = this.GetComponent<NavMeshAgent>();
        agent.speed = initialSpeed;
        agent.isStopped = false;
        isMoving = false;
        line = this.GetComponent<LineRenderer>();
    }

    #region Movement Methods

    public void DoMovement()
    {
        switch (movement)
        {
            case Movement.Static:
                StaticMovement();
                break;
            case Movement.Basic:
                ClassicMovement();
                break;
            case Movement.Ranged:
                RangedMovement();
                break;
            default:
                Debug.LogWarning("Movement not implemented");
                break;
        }

        if (Enemy.sdrawPath)
        {
            line.enabled = true;
            DrawPath(agent.path);
        }
        else
        {
            line.enabled = false;
        }

        isMoving = true;

    }

    void StaticMovement()
    {
        this.transform.LookAt(Enemy.aimPlayer.transform);
        this.transform.eulerAngles = new Vector3(0, this.transform.eulerAngles.y, 0);
        this.GetComponent<Rigidbody>().isKinematic = true;
        Debug.Log("static");

    }

    void ClassicMovement()
    {
        agent.destination = Enemy.aimPlayer.transform.position;
        Debug.Log("classic");
    }

    void RangedMovement()
    {
        Tuple<GameObject, float> nearestPlayer = ClosestPlayer();

        if (nearestPlayer.Item2 < enemySkill.range)
        {
            EnemyEscape(nearestPlayer.Item1);
            Debug.Log("fleeing ...");
        }
        Debug.Log("ranged");
    }

    /// <summary>
    /// will move around the players (the point between the players)
    /// </summary>
    void MoveAndAttack(float distanceToKeep)
    {
        Vector3 origin = GameManager.gameManager.player1.transform.position - GameManager.gameManager.player2.transform.position;
        float rotationMove = UnityEngine.Random.Range((float)-Math.PI / 2, (float)Mathf.PI / 2);
        Vector3 destination = new Vector3(origin.x + distanceToKeep * Mathf.Cos(rotationMove), Enemy.aimPlayer.transform.position.y, origin.z + distanceToKeep * Mathf.Sin(rotationMove));
        agent.destination = destination;
    }

    /// <summary>
    /// run away from the orb (because why not)
    /// </summary>
    void EnemyEscape(GameObject target)
    {
        Vector3 dir = this.transform.position - target.transform.position;
        agent.destination = this.transform.position + dir;
    }

    void MoveToPlayer(GameObject target,float enemyRange)
    {
        agent.destination = (Enemy.aimPlayer.transform.position - target.transform.position).normalized * enemyRange;
    }

    /// <summary>
    /// returns the nearest player from the enemy
    /// </summary>
    /// <returns></returns>
    Tuple<GameObject, float> ClosestPlayer()
    {
        float distanceP1 = Vector3.Distance(this.transform.position, GameManager.gameManager.player1.transform.position);
        float distanceP2 = Vector3.Distance(this.transform.position, GameManager.gameManager.player2.transform.position);
        return distanceP1 > distanceP2 ? Tuple.Create(GameManager.gameManager.player2, distanceP2) : Tuple.Create(GameManager.gameManager.player1,distanceP2);
    }



    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            agent.isStopped = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            agent.isStopped = false;
        }
    }

    void DrawPath(NavMeshPath path)
    {
        if (path.corners.Length < 2) //if the path has 1 or no corners, there is no need
        {
            return;
        }
        line.SetPositions(path.corners); //set the array of positions to the amount of corners
    }


    public void SlowSpeed(float slowAmount)
    {
        if (!isSlowed)
        {
            agent.speed = agent.speed * ((100 - slowAmount)/100);
            isSlowed = true;
        }
    }

    public void RestoreSpeed()
    {
        if (isSlowed)
        {
            agent.speed = initialSpeed;
            isSlowed = false;
        }
    }

    #endregion
}
