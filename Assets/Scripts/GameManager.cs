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
    [HideInInspector]
    public UIManager UIManager;

    public GameObject linkDeformation;

    public bool player1HasTaunt, player2HasTaunt;
    public int tauntRange = 10;

	public GameObject normalHealingOrbPrefab, leechLifeHealingOrbPrefab;

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
        Darkness
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
        UIManager = GameObject.FindGameObjectWithTag("UI").GetComponent<UIManager>();

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
    



	public void spawnHealingOrbs(int playerHealed, int healAmount, string mode)
	{
		GameObject healingOrbPrefab = normalHealingOrbPrefab;
		if(mode == "leechLife")
		{
			healingOrbPrefab = leechLifeHealingOrbPrefab;
		}

		if(playerHealed == 0 || playerHealed == 1)
		{
			OrbController healingOrb1 = Instantiate(healingOrbPrefab, orb.transform.position, Quaternion.identity, orb.GetComponentInParent<Transform>()).GetComponent<OrbController>();

			healingOrb1.healAmount = healAmount;
			healingOrb1.progression = orb.GetComponent<OrbController>().progression;
			healingOrb1.toPlayer2 = false;
		}

		if (playerHealed == 0 || playerHealed == 2)
		{
			OrbController healingOrb2 = Instantiate(healingOrbPrefab, orb.transform.position, Quaternion.identity, orb.GetComponentInParent<Transform>()).GetComponent<OrbController>();

			healingOrb2.healAmount = healAmount;
			healingOrb2.progression = orb.GetComponent<OrbController>().progression;
			healingOrb2.toPlayer2 = true;
		}
	}
}
