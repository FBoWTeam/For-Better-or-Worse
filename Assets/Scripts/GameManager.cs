using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public static GameManager gameManager;
    public int baseHP;
    public int hp;

    public int shieldP1;
    public int shieldP2;

    [HideInInspector]
    public GameObject player1;
    [HideInInspector]
    public GameObject player2;

    public GameObject linkDeformation;

	public bool player1HasTaunt, player2HasTaunt;
    public int tauntRange = 10;

	public enum PowerType
    {
        None,

        LargeOrb,
        Vortex,
        LeechLife,
        Slug,
        Shield,

        Ice,
        Fire,
        Electric,
        Weakness,

        Elemental = Ice | Fire | Electric | Weakness,
        Behavioral = LargeOrb | Vortex | LeechLife | Slug | Shield
    }


    // Start is called before the first frame update
    void Awake()
    {
        if (gameManager == null)
        {
            gameManager = this;
        }
        else if (gameManager != this)
        {
            Destroy(this);
        }
        DontDestroyOnLoad(gameManager);

        player1 = GameObject.Find("Player1");
        player2 = GameObject.Find("Player2");
        linkDeformation = GameObject.Find("Deformation");

        hp = baseHP;
    }

    // Update is called once per frame
    void Update()
    {

    }
    /// <summary>
    /// Handle taking damage from an Ennemy or other things
    /// </summary>
    /// <param name="impactDamage"></param>
    public void TakeDamage(GameObject targetPlayer, int damage)
    {
        if (targetPlayer == player1)
        {
            if (damage >= shieldP1)
            {
                damage -= shieldP1;
                shieldP1 = 0;
            }
            else if(damage < shieldP1)
            {
                shieldP1 -= damage;
            }
            hp -= damage;
        }
        if (targetPlayer == player2)
        {
            if (damage >= shieldP2)
            {
                damage -= shieldP2;
                shieldP2 = 0;
            }
            else if (damage < shieldP2)
            {
                shieldP2 -= damage;
            }
            hp -= damage;
        }

        if (hp <= 0)
        {
            Debug.Log("DED");
        }
    }
    
    public void SlowSpeed(GameObject target)
    {
        if (target.GetComponent<EnemyMovement>() != null)
        {
            target.GetComponent<EnemyMovement>().agent.speed /= 2;
        }
        else if (target.GetComponent<PlayerController>() != null)
        {
            target.GetComponent<PlayerController>().speed /= 2;
        }
    }

    public void RestoreSpeed(GameObject target)
    {
        if (target.GetComponent<EnemyMovement>() != null)
        {
            target.GetComponent<EnemyMovement>().agent.speed = target.GetComponent<EnemyMovement>().initialSpeed;
        }
        else if (target.GetComponent<PlayerController>() != null)
        {
            target.GetComponent<PlayerController>().speed = target.GetComponent<PlayerController>().initialSpeed;
        }
    }

}
