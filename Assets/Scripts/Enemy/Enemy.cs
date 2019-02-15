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
    public bool drawPath = false;
    #endregion

    #region Movement Variables
    public enum Movement
    {
        Immobile,
        Classic,
    };
    [Header("[Movement]")]
    public Movement movement;

    [DrawIf(new string[] { "movement" }, Movement.Classic)]
    public float speed = 2f;

    [DrawIf(new string[] { "movement" }, Movement.Classic)]
    [Tooltip("represents the time of the attack animation")]
    public float stopTime = 2f;

    [DrawIf(new string[] { "movement" }, Movement.Classic)]
    [Tooltip("represents the remaining distance between the enemy and the player")]
    public float distanceBetweenPlayer = 5f;

    private LineRenderer line;
    #endregion

    #region Bonus Variables
    public enum Bonus
    {
        Mirror,
        None,
    };
    [Header("[Bonus]")]
    public Bonus bonusOne;
    public Bonus bonusTwo;
    #endregion


    private bool coliding = false;
    private NavMeshAgent agent;
    private GameObject[] players;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        agent = this.GetComponent<NavMeshAgent>();
        agent.speed = speed;
        players = GameObject.FindGameObjectsWithTag("Player");
        line = this.GetComponent<LineRenderer>();
		hp = baseHP;
    }

    // Update is called once per frame
    void Update()
    {
       
		if(!agent.isStopped)
		{
			DoMovement(movement);
		}

        Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 5, Color.yellow);
		//Debug.DrawRay(transform.position, transform.forward, Color.green);

		if (hp <= 0)
		{
			agent.isStopped = true;
			StopAllCoroutines();
			Destroy(this.gameObject);
		}
	}
   

    #region Movement

    void DoMovement(Movement movement)
    {
        switch (movement)
        {
            case Movement.Immobile:
                this.transform.LookAt(players[0].transform);
                break;
            case Movement.Classic:
                StartCoroutine(ClassicMovement());
                break;
            default:
                break;
        }

        if (drawPath)
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
        agent.destination = players[0].transform.position;

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
    

    GameObject GetNearestGO(GameObject[] gos)
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
