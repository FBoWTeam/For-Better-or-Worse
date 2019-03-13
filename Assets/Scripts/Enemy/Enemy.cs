﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(EnemyMovement))]
[RequireComponent(typeof(EnemySkill))]
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
    public Focus focus;
    #endregion

    #region Taunt Variables
    public enum Taunt
    {
        Taunter,
        Other,
    }
    public Taunt taunt;
    //if true player1, if false, player2
    public bool taunter;
    public bool isTaunted;

    public float tauntDuration;

    GameObject tauntCanvas;
    Color player1ColorTaunt = new Color(255, 96, 0);
    Color player2ColorTaunt = new Color(82, 82, 82);


    #endregion

    public int baseHP = 100;
    public int hp;


    public float timeImoIfHit; //temps d'immobilisation quand un ennemi se fait toucher par l'orbe


    [HideInInspector]
    public EnemyMovement enemyMovement;
    [HideInInspector]
    public EnemySkill enemySkill;

    GameObject[] players;
    public static GameObject aimPlayer;

    

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        hp = baseHP;
        players = new GameObject[] { GameManager.gameManager.player1, GameManager.gameManager.player2 };
        enemyMovement = GetComponent<EnemyMovement>();
        enemySkill = GetComponent<EnemySkill>();
        sdrawPath = drawPath;
        tauntCanvas = transform.GetChild(1).gameObject;

    }

    // Update is called once per frame
    void Update()
    {
        
        if (!GameManager.gameManager.isPaused)
        {
            FocusManagement();

            if (enemySkill.isAttacking && enemySkill.stopWhenAttacking && enemyMovement.movement == EnemyMovement.Movement.Basic) 
            {
                enemyMovement.agent.isStopped = true;
            }
            else { enemyMovement.agent.isStopped = false; }

            if (!enemyMovement.agent.isStopped)
            {
                enemySkill.DoAttack();
                
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
        if (isTaunted)
        {
            switch (taunt)
            {
                case Taunt.Taunter:
                    aimPlayer = taunter ? GameManager.gameManager.player1 : GameManager.gameManager.player2;
                    break;
                case Taunt.Other:
                    aimPlayer = taunter ? GameManager.gameManager.player2 : GameManager.gameManager.player1;
                    break;
                default:
                    break;
            }
        }
        else
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
                    aimPlayer = GameManager.gameManager.Player1IsNearest(transform.position) ? GameManager.gameManager.player1 : GameManager.gameManager.player2;
                    break;
                default:
                    break;
            }
        }

        TauntFeedback();
    }

    #endregion

    #region Taunt Methods

    public IEnumerator TauntCoroutine(bool player1)
    {
        isTaunted = true;
        taunter = player1;
        if (player1)
        {
            tauntCanvas.GetComponentInChildren<Text>().color = player1ColorTaunt;
        }
        else
        {
            tauntCanvas.GetComponentInChildren<Text>().color = player2ColorTaunt;
        }
        yield return new WaitForSeconds(tauntDuration);
        isTaunted = false;
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

    public void TakeDamage(int damage)
    { 
        hp -= damage;
        GameManager.gameManager.orb.GetComponent<OrbController>().hasHitEnemy = true;
        //StartCoroutine("Stun");
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

        yield return new WaitForSecondsRealtime(timeImoIfHit);
        enemyMovement.agent.isStopped = false;
    }


}
