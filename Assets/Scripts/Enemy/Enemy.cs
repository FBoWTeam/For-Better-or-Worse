using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(NavMeshAgent))]
public class Enemy : MonoBehaviour
{
    #region All Variables

    #region Debug Variables
    public bool debug;
    [DrawIf(new string[] { "debug" }, true)]
    public bool drawPath = false;
    [DrawIf(new string[] { "debug" }, true)]
    public bool drawView = false;
    [DrawIf(new string[] { "drawView" }, true)]
    public float lengthView = 2f;


    public static bool sdrawPath;
    #endregion

    #region Focus Variables
    public enum Focus
    {
        Player1,
        Player2,
        Nearest,
    }
    public Focus focus = Focus.Nearest;
    #endregion

    #region Taunt Variables
    public enum Taunt
    {
        Taunter,
        Other,
    }
    public Taunt taunt = Taunt.Taunter;
    private GameObject taunter;
    public bool isTaunted = false;

    GameObject tauntCanvas;
    Color player1ColorTaunt = new Color(255, 96, 0);
    Color player2ColorTaunt = new Color(82, 82, 82);

    #endregion

    public int baseHP = 100;
    public int hp;

	public float knockBackForce;

    public float timeImo; //temps d'immobilisation quand un ennemi se fait toucher par l'orbe

    [HideInInspector]
    public EnemyMovement enemyMovement;

    GameObject[] players;
    public static GameObject aimPlayer;

    

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        hp = baseHP;
        players = new GameObject[] { GameManager.gameManager.player1, GameManager.gameManager.player2 };
        enemyMovement = GetComponent<EnemyMovement>();
        sdrawPath = drawPath;
        tauntCanvas = transform.GetChild(2).gameObject;
    }

    // Update is called once per frame
    void Update()
    {
		if (!GameManager.gameManager.isPaused)
		{
			FocusManagement();
			TauntManagement();

			if (!enemyMovement.agent.isStopped)
			{
				enemyMovement.DoMovement();
			}

			if (drawView)
			{
				Debug.DrawRay(this.transform.position, this.transform.forward * lengthView, Color.magenta);
			}
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
                aimPlayer = GameManager.gameManager.player1;
                break;
            case Focus.Player2:
                aimPlayer = GameManager.gameManager.player2;
                break;
            case Focus.Nearest:
                aimPlayer = GetNearestGO(players);
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

    #region Taunt Methods

    private void TauntManagement()
    {
        if (GameManager.gameManager.player1HasTaunt && Vector3.Distance(transform.position, GameManager.gameManager.player1.transform.position) <= GameManager.gameManager.tauntRange)
        {
            taunter = players[0];
            isTaunted = true;
            tauntCanvas.GetComponentInChildren<Text>().color = player1ColorTaunt;
        }
        else if (GameManager.gameManager.player2HasTaunt && Vector3.Distance(transform.position, GameManager.gameManager.player2.transform.position) <= GameManager.gameManager.tauntRange)
        {
            taunter = players[1];
            isTaunted = true;
            tauntCanvas.GetComponentInChildren<Text>().color = player2ColorTaunt;
        }

        if (taunter != null)
        {
            switch (taunt)
            {
                case Taunt.Taunter:
                    aimPlayer = taunter;
                    break;
                case Taunt.Other:
                    aimPlayer = (taunter.Equals(players[0])) ? players[1] : players[0];
                    break;
                default:
                    break;
            }
        }

        TauntFeedback();
    }

    private void TauntFeedback()
    { 
        if (isTaunted)
        {
            tauntCanvas.SetActive(true);
            tauntCanvas.transform.LookAt(Camera.main.transform.position);
        }
        else
        {
            tauntCanvas.SetActive(false);
        }
    }

    #endregion

    public void TakeDamage(int damage, Vector3 hitPosition)
    { 
        hp -= damage;
        GameManager.gameManager.orb.GetComponent<OrbController>().hasHitEnemy = true;
        //enemyMovement.agent.velocity = (transform.position - hitPosition) * knockBackForce;
        StartCoroutine("Stun");
		if (hp <= 0)
		{
			GetComponent<LootTable>().LootEnemy();
			enemyMovement.agent.isStopped = true;
			StopAllCoroutines();
			Destroy(this.gameObject);
		}
    }



    IEnumerator Stun()
    {
        enemyMovement.agent.isStopped = true;

        yield return new WaitForSecondsRealtime(timeImo);
        enemyMovement.agent.isStopped = false;
    }


}
