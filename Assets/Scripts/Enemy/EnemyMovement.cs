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
        Fleeing,
        Dodging,
        ZigZag,
        Sentinelle,
        Charge,
        Tunnel,
    };

    [Header("[Movement]")]
    public Movement movement;

    [DrawIf(new string[] { "movement" }, Movement.Basic)]
    public float speed = 2f;

    [DrawIf(new string[] { "movement" }, Movement.Basic)]
    [Tooltip("represents the time of the attack animation")]
    public float stopTime = 2f;

    [DrawIf(new string[] { "movement" }, Movement.Basic)]
    [Tooltip("represents the remaining distance between the enemy and the player")]
    public float distanceBetweenPlayer = 5f;

    private LineRenderer line;
    #endregion

    public NavMeshAgent agent;

    // Start is called before the first frame update
    void Start()
    {
        agent = this.GetComponent<NavMeshAgent>();
        agent.speed = speed;
        line = this.GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {

    }


    #region Movement Methods

    public void DoMovement()
    {
        switch (movement)
        {
            case Movement.Static:
                this.transform.LookAt(Enemy.target.transform);
                break;
            case Movement.Basic:
                StartCoroutine(ClassicMovement());
                break;
            default:
                break;
        }

        if (Enemy.drawPath)
        {
            line.enabled = true;
            DrawPath(agent.path);
        }
        else
        {
            line.enabled = false;
        }
    }

    IEnumerator ClassicMovement()
    {
        agent.destination = Enemy.target.transform.position;

        if (agent.remainingDistance <= distanceBetweenPlayer)
        {
            agent.isStopped = true;
            yield return new WaitForSeconds(stopTime);
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

    #endregion
}
