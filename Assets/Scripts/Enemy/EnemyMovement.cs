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
        Classic,
        Ranged,
        Fleeing,
        Dodging,
        ZigZag,
        Sentinelle,
        Charge,
        Tunnel,
    };


    public Movement movement;

    [DrawIf(new string[] { "movement" }, Movement.Classic)]
    public float initialSpeed = 2f;

    [DrawIf(new string[] { "movement" }, Movement.Classic)]
    [Tooltip("represents the time of the attack animation")]
    public float stopTime = 2f;

    [DrawIf(new string[] { "movement" }, Movement.Ranged)]
    public float timeBeforeFleeing;
    //[DrawIf(new string[] { "movement" }, Movement.Basic)]
    //[Tooltip("represents the remaining distance between the enemy and the player")]
    //public float distanceBetweenPlayer = 5f;

    #endregion

    [HideInInspector]
    public NavMeshAgent agent;



    //private
    public bool isSlowed;
    public bool isStrafing;
    public bool isFleeing;
    Coroutine strafingCoroutine;
    Coroutine fleeingCoroutine;

    EnemySkill enemySkill;

    

    // Start is called before the first frame update
    void Start()
    {
        enemySkill = this.GetComponent<EnemySkill>();
        agent = this.GetComponent<NavMeshAgent>();
        agent.speed = initialSpeed;
        agent.isStopped = false;
    }

    #region Movement Methods

    public void DoMovement()
    {
        switch (movement)
        {
            case Movement.Static:
                StaticMovement();
                break;
            case Movement.Classic:
                ClassicMovement();
                break;
            case Movement.Ranged:
                RangedMovement();
                break;
            default:
                Debug.LogWarning("Movement not implemented");
                break;
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
		if (enemySkill.isInRange)
		{
			agent.destination = transform.position;
		}
		else
		{
			agent.destination = Enemy.aimPlayer.transform.position;
		}
    }

    void RangedMovement()
    {
        Tuple<GameObject, float> nearestPlayer = ClosestPlayer();
        if (nearestPlayer.Item2 < enemySkill.range/2)
        {
            StopCoroutine(strafingCoroutine);
            isStrafing = false;
            EnemyEscape(nearestPlayer.Item1, enemySkill.range);
        }
        else if (nearestPlayer.Item2 < enemySkill.range - 2 && !isStrafing)
        {
            strafingCoroutine = StartCoroutine(Strafing(nearestPlayer.Item1, enemySkill.range));
        }
        else if (nearestPlayer.Item2 > enemySkill.range)
        {
            StopCoroutine(strafingCoroutine);
            isStrafing = false;
            MoveToPlayer(nearestPlayer.Item1, nearestPlayer.Item2);
        }

    }

    /// <summary>
    /// the enemy begins to rotate around the players and attack at the same time
    /// </summary>
    /// <param name="target"></param>
    /// <param name="distance"></param>
    /// <returns></returns>
    IEnumerator Strafing(GameObject target, float distance)
    {
        float strafeDuration = UnityEngine.Random.Range(1f, 3f);
        isStrafing = true;
        float timer = 0f;
        bool goRight = UnityEngine.Random.Range(0, 1) < 0.5f ? true : false;
        float x = target.transform.position.x + distance * Mathf.Cos(goRight ? Mathf.PI / 2 : -Mathf.PI / 2);
        float z = target.transform.position.z + distance * Mathf.Sin(goRight ? Mathf.PI / 2 : -Mathf.PI / 2);
        Vector3 destination = new Vector3(x, transform.position.y, z);
        agent.destination = destination;
        while (timer < strafeDuration)
        {
            timer += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        isStrafing = false;
        //Debug.Log("oui");
    }
    

    /// <summary>
    /// run away from the orb (because why not)
    /// </summary>
    void EnemyEscape(GameObject target, float enemyRange)
    {
        float timeStamp = Time.time;

        Vector3 dir = (this.transform.position - target.transform.position).normalized * enemyRange/2;
        agent.destination = this.transform.position + dir;
    }

    void MoveToPlayer(GameObject target,float enemyRange)
    {
        agent.destination = Enemy.aimPlayer.transform.position;
    }

    /// <summary>
    /// returns the nearest player from the enemy
    /// </summary>
    /// <returns></returns>
    Tuple<GameObject, float> ClosestPlayer()
    {
        float distanceP1 = Vector3.Distance(this.transform.position, GameManager.gameManager.player1.transform.position);
        float distanceP2 = Vector3.Distance(this.transform.position, GameManager.gameManager.player2.transform.position);
        return distanceP1 > distanceP2 ? Tuple.Create(GameManager.gameManager.player2, distanceP2) : Tuple.Create(GameManager.gameManager.player1,distanceP1);
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
