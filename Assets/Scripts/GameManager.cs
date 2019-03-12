using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager gameManager;
	[HideInInspector]
	public GameObject player1;
	[HideInInspector]
	public GameObject player2;
	[HideInInspector]
	public GameObject orb;
	[HideInInspector]
	public UIManager UIManager;

	public bool isPaused;

	[Header("[Distance Limits]")]
	public float minDistance;
	public float maxDistance;

	[Header("[Hps]")]
	public int hp;
	public int damageTakenP1;
	public int damageTakenP2;
	public int shieldP1;
	public int shieldP2;
	public float knockBackForce;
	public bool restartWhenDead;

    [Header("[HealingOrbs]")]
    public GameObject normalHealingOrbPrefab;
    public GameObject leechLifeHealingOrbPrefab;


	public Checkpoint actualCheckpoint;
	public struct PowerRecord
	{
		public PowerType player1ElementalPower;
		public PowerType player1BehaviouralPower;
		public PowerType player2ElementalPower;
		public PowerType player2BehaviouralPower;
	}
	public PowerRecord respawnPowerRecord;

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

    public enum PuddleType
    {
        None,
        Slug,
        Acid,
        Water,
        Flammable,
        Mud
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
        //DontDestroyOnLoad(gameManager);

        player1 = GameObject.Find("Player1");
        player2 = GameObject.Find("Player2");
        orb = GameObject.Find("Orb");
        UIManager = GameObject.FindGameObjectWithTag("UI").GetComponent<UIManager>();

        damageTakenP1 = 0;
        damageTakenP2 = 0;

        StartCoroutine(UIManager.FadeCoroutine("FadeIn"));
    }

    /// <summary>
    /// Handle taking damage from an Ennemy or other things
    /// </summary>
    /// <param name="impactDamage"></param>
    public void TakeDamage(GameObject targetPlayer, int damage, Vector3 hitPosition, bool doKnockBack)
    {
        if (!targetPlayer.GetComponent<PlayerController>().invincible)
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
                damageTakenP1 += damage;
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
                damageTakenP2 += damage;
            }
            if ((damageTakenP1 + damageTakenP2 >= hp) && restartWhenDead)
            {
                StartCoroutine(deathCoroutine());
            }

            hitPosition = new Vector3(hitPosition.x, 0.0f, hitPosition.z);
            if(doKnockBack)
                targetPlayer.GetComponent<Rigidbody>().AddForce((targetPlayer.transform.position - hitPosition).normalized * knockBackForce);
            StartCoroutine(targetPlayer.GetComponent<PlayerController>().InvincibilityCoroutine());
            UIManager.UpdateHealthBar();
            UIManager.UpdateCombo(0);
        }
    }

    public static bool isElemental(PowerType power)
	{
		if (power == PowerType.LargeOrb || power == PowerType.LeechLife || power == PowerType.Shield || power == PowerType.Slug || power == PowerType.Vortex)
			return false;
		else
			return true;
	}

	public void RecordPower()
	{
		respawnPowerRecord.player1ElementalPower = player1.GetComponent<PlayerController>().elementalPowerSlot;
		respawnPowerRecord.player1BehaviouralPower = player1.GetComponent<PlayerController>().behaviouralPowerSlot;
		respawnPowerRecord.player2ElementalPower = player2.GetComponent<PlayerController>().elementalPowerSlot;
		respawnPowerRecord.player2BehaviouralPower = player2.GetComponent<PlayerController>().behaviouralPowerSlot;
	}

    public void Heal(bool player1, int healAmount)
    {
        if (player1)
        {
            if (damageTakenP1 > healAmount)
            {
                damageTakenP1 -= healAmount;
            }
            else
            {
                damageTakenP1 = 0;
            }
        }
        else
        {
            if (damageTakenP2 > healAmount)
            {
                damageTakenP2 -= healAmount;
            }
            else
            {
                damageTakenP2 = 0;
            }
        }

        UIManager.UpdateHealthBar();
    }

    public void spawnHealingOrbs(int playerHealed, int healAmount, string mode)
    {
        GameObject healingOrbPrefab = normalHealingOrbPrefab;
        if (mode == "leechLife")
        {
            healingOrbPrefab = leechLifeHealingOrbPrefab;
        }

        if (playerHealed == 0 || playerHealed == 1)
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

    public bool Player1IsNearest(Vector3 position)
    {
        if (Vector3.Distance(position, player1.transform.position) < Vector3.Distance(position, player2.transform.position))
        {
            return true;
        }
        return false;
    }


    IEnumerator deathCoroutine()
    {
        StartCoroutine(UIManager.FadeCoroutine("FadeOut"));
        yield return new WaitUntil(() => isPaused == false);

		isPaused = true;

		player1.transform.position = actualCheckpoint.transform.GetChild(0).position - 5 * Camera.main.transform.right;
		player2.transform.position = actualCheckpoint.transform.GetChild(0).position + 5 * Camera.main.transform.right;

		damageTakenP1 = 0;
		damageTakenP2 = 0;
		shieldP1 = 0;
		shieldP2 = 0;
		UIManager.UpdateHealthBar();

		player1.GetComponent<PlayerController>().elementalPowerSlot = respawnPowerRecord.player1ElementalPower;
		player1.GetComponent<PlayerController>().behaviouralPowerSlot = respawnPowerRecord.player1BehaviouralPower;
		player2.GetComponent<PlayerController>().elementalPowerSlot = respawnPowerRecord.player2ElementalPower;
		player2.GetComponent<PlayerController>().behaviouralPowerSlot = respawnPowerRecord.player2BehaviouralPower;
		UIManager.UpdatePowerSlot(1, true, respawnPowerRecord.player1ElementalPower);
		UIManager.UpdatePowerSlot(2, true, respawnPowerRecord.player1BehaviouralPower);
		UIManager.UpdatePowerSlot(1, false, respawnPowerRecord.player2ElementalPower);
		UIManager.UpdatePowerSlot(2, false, respawnPowerRecord.player2BehaviouralPower);

		player1.GetComponent<PlayerController>().RespawnReset();
		player2.GetComponent<PlayerController>().RespawnReset();
		player1.GetComponent<OrbHitter>().RespawnReset();
		player2.GetComponent<OrbHitter>().RespawnReset();
		orb.GetComponent<OrbController>().RespawnReset();
		orb.GetComponent<PowerController>().RespawnReset();
		UIManager.RespawnReset();

		actualCheckpoint.RespawnContent();

		StartCoroutine(UIManager.FadeCoroutine("FadeIn"));
	}

    

}
