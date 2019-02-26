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

    // Start is called before the first frame update
    void Start()
    {
        agent = this.GetComponent<NavMeshAgent>();
        agent.speed = initialSpeed;
        agent.isStopped = false;
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
                StaticMovement();
                break;
            case Movement.Basic:
                ClassicMovement();
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
    }

    void StaticMovement()
    {
        this.transform.LookAt(Enemy.aimPlayer.transform);
        this.transform.eulerAngles = new Vector3(0, this.transform.eulerAngles.y, 0);
        this.GetComponent<Rigidbody>().isKinematic = true;

    }

    void ClassicMovement()
    {
        agent.destination = Enemy.aimPlayer.transform.position;
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

    #endregion
}
