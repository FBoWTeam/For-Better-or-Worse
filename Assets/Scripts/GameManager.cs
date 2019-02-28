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
    [HideInInspector]
    public GameObject orb;

    public GameObject linkDeformation;

    public bool player1HasTaunt, player2HasTaunt;
    public int tauntRange = 10;
    
	public GameObject healingOrbPrefab;

	public enum PowerType
    {
        None = 0,

        LargeOrb = 1,
        Vortex = 2,
        LeechLife = 3,
        Slug = 4,
        Shield = 5,

        Ice = 6,
        Fire = 7,
        Electric = 8,
        Weakness = 9
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
        orb = GameObject.Find("Orb");
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
            else if (damage < shieldP1)
            {
                shieldP1 -= damage;
                damage = 0;
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
                damage = 0;
            }
            hp -= damage;
        }
        if (hp <= 0)
        {

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

	public void spawnHealingOrbs(int healAmount)
	{
		OrbController healingOrb1 = Instantiate(healingOrbPrefab, orb.transform.position, Quaternion.identity, orb.GetComponentInParent<Transform>()).GetComponent<OrbController>();

		healingOrb1.healAmount = healAmount;
		healingOrb1.progression = orb.GetComponent<OrbController>().progression;
		healingOrb1.toPlayer2 = false;

		OrbController healingOrb2 = Instantiate(healingOrbPrefab, orb.transform.position, Quaternion.identity, orb.GetComponentInParent<Transform>()).GetComponent<OrbController>();

		healingOrb2.healAmount = healAmount;
		healingOrb2.progression = orb.GetComponent<OrbController>().progression;
		healingOrb2.toPlayer2 = true;
	}
}
