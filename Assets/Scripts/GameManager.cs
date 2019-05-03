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
    [HideInInspector]
    public GameObject fader;
    [HideInInspector]
    public GameObject blackBands;
    [HideInInspector]
    public GameObject tutorials;

    public bool arena;

    public bool boss;

    public bool isPaused;

    [HideInInspector]
    public bool isCutScene = false;

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
    public bool canDie;
    bool respawning = false;

    [Header("[HealingOrbs]")]
    public GameObject normalHealingOrbPrefab;
    public GameObject leechLifeHealingOrbPrefab;

    public bool breakComboWhenEnemyHit;

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
            Destroy(this.gameObject);
        }

        player1 = GameObject.Find("Player1");
        player2 = GameObject.Find("Player2");
        orb = GameObject.Find("Orb");
        UIManager = GameObject.Find("UI").GetComponent<UIManager>();
        fader = GameObject.Find("Fader");
        blackBands = GameObject.Find("BlackBands");
        tutorials = GameObject.Find("Tutorials");
        tutorials.SetActive(false);
        GameObject introScenario = GameObject.Find("IntroScenario");
        if (introScenario != null)
        {
            if (GameData.introSkiped)
            {
                Destroy(introScenario);
            }
            else
            {
                isCutScene = true;
                UIManager.gameObject.SetActive(false);
                player1.GetComponent<PlayerController>().active = false;
                player2.GetComponent<PlayerController>().active = false;
                player1.GetComponent<OrbHitter>().active = false;
                player2.GetComponent<OrbHitter>().active = false;
                GameObject.Find("IntroScenario").GetComponent<ScenarioHandler>().Initialize();
            }
        }
        if (introScenario == null || GameData.introSkiped)
        {
            GameData.introSkiped = false;
            GameObject.Find("DialogSystem").SetActive(false);
            blackBands.SetActive(false);
            player1.GetComponent<PlayerController>().active = true;
            player2.GetComponent<PlayerController>().active = true;
            player1.GetComponent<OrbHitter>().active = true;
            player2.GetComponent<OrbHitter>().active = true;
            StartCoroutine(FadeCoroutine("FadeIn"));
        }

        damageTakenP1 = 0;
        damageTakenP2 = 0;
    }

    /// <summary>
    /// Handle taking damage from an Ennemy or other things
    /// </summary>
    /// <param name="impactDamage"></param>
    public void TakeDamage(GameObject targetPlayer, int damage, Vector3 hitPosition, bool applyKnockback)
    {
        if (!targetPlayer.GetComponent<PlayerController>().invincible)
        {
            if (targetPlayer == player1)
            {
                //update in score mamager
                ScoreManager.scoreManager.damageTakenP1 += damage;


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
                //update in score mamager
                ScoreManager.scoreManager.damageTakenP2 += damage;


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
            if ((damageTakenP1 + damageTakenP2 >= hp) && canDie && !respawning)
            {
                StartCoroutine(deathCoroutine());
            }

            if (applyKnockback)
            {
                hitPosition = new Vector3(hitPosition.x, 0.0f, hitPosition.z);
                targetPlayer.GetComponent<Rigidbody>().AddForce((targetPlayer.transform.position - hitPosition).normalized * knockBackForce);
            }

            StartCoroutine(targetPlayer.GetComponent<PlayerController>().InvincibilityCoroutine());
            UIManager.UpdateHealthBar();
            if (breakComboWhenEnemyHit)
            {
                UIManager.UpdateCombo(0);
                orb.GetComponent<OrbController>().combo = 0;
            }

            targetPlayer.GetComponent<PlayerController>().isRoot = false;
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
        respawnPowerRecord = new PowerRecord
        {
            player1ElementalPower = player1.GetComponent<PlayerController>().elementalPowerSlot,
            player1BehaviouralPower = player1.GetComponent<PlayerController>().behaviouralPowerSlot,
            player2ElementalPower = player2.GetComponent<PlayerController>().elementalPowerSlot,
            player2BehaviouralPower = player2.GetComponent<PlayerController>().behaviouralPowerSlot
        };
    }

    public void Heal(bool player1, int healAmount)
    {
        if (player1)
        {
            if (damageTakenP1 > healAmount)
            {
                //update score manager
                ScoreManager.scoreManager.healPointReceivedP1 += healAmount;
                damageTakenP1 -= healAmount;
            }
            else
            {
                //update score manager
                ScoreManager.scoreManager.healPointReceivedP1 += healAmount - damageTakenP1;
                damageTakenP1 = 0;
            }
        }
        else
        {
            if (damageTakenP2 > healAmount)
            {
                //update score manager
                ScoreManager.scoreManager.healPointReceivedP2 += healAmount;
                damageTakenP2 -= healAmount;
            }
            else
            {
                //update score manager
                ScoreManager.scoreManager.healPointReceivedP2 += healAmount - damageTakenP2;
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

    public IEnumerator FadeCoroutine(string fadeName)
    {
        GameManager.gameManager.isPaused = true;
        fader.GetComponent<Animator>().SetTrigger(fadeName);
        yield return new WaitForSeconds(fader.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length);
        GameManager.gameManager.isPaused = false;
		if (fadeName == "FadeIn")
        {
            orb.GetComponent<OrbController>().FreezeOrb(0.5f);
        }
    }

    IEnumerator deathCoroutine()
    {
        respawning = true;
        //update in score manager
        ScoreManager.scoreManager.numberOfDeaths++;

        StartCoroutine(FadeCoroutine("FadeOut"));
        yield return new WaitUntil(() => isPaused == false);

        PlayerController player1Controller = player1.GetComponent<PlayerController>();
        PlayerController player2Controller = player2.GetComponent<PlayerController>();

        if (player1Controller.actualBurnCoroutine != null)
        {
            player1Controller.StopCoroutine(player1Controller.actualBurnCoroutine);
            player1.transform.Find("FX/fire").gameObject.SetActive(false);
        }
        if (player1Controller.actualBurnCoroutine != null)
        {
            player1Controller.StopCoroutine(player1Controller.actualBurnCoroutine);
            player2.transform.Find("FX/fire").gameObject.SetActive(false);
        }

        if (arena)
        {
            GameData.previousScene = SceneManager.GetActiveScene().buildIndex;
            SceneManager.LoadScene(15);
        }
        else if (boss)
        {
            GameData.introSkiped = true;
            SceneManager.LoadScene(9);
        }
        else
        {
            isPaused = true;

            player1.transform.position = actualCheckpoint.transform.GetChild(0).position + new Vector3(-5, 0, 0);
            player2.transform.position = actualCheckpoint.transform.GetChild(0).position + new Vector3(5, 0, 0);
			Camera.main.transform.parent.position = actualCheckpoint.transform.GetChild(0).position;

			damageTakenP1 = 0;
            damageTakenP2 = 0;
            shieldP1 = 0;
            shieldP2 = 0;
            UIManager.UpdateHealthBar();

            player1.GetComponent<PlayerController>().elementalPowerSlot = respawnPowerRecord.player1ElementalPower;
            player1.GetComponent<PlayerController>().behaviouralPowerSlot = respawnPowerRecord.player1BehaviouralPower;
            player2.GetComponent<PlayerController>().elementalPowerSlot = respawnPowerRecord.player2ElementalPower;
            player2.GetComponent<PlayerController>().behaviouralPowerSlot = respawnPowerRecord.player2BehaviouralPower;
            UIManager.UpdatePowerSlot(true, respawnPowerRecord.player1ElementalPower);
            UIManager.UpdatePowerSlot(true, respawnPowerRecord.player1BehaviouralPower);
            UIManager.UpdatePowerSlot(false, respawnPowerRecord.player2ElementalPower);
            UIManager.UpdatePowerSlot(false, respawnPowerRecord.player2BehaviouralPower);

            player1.GetComponent<PlayerController>().RespawnReset();
            player2.GetComponent<PlayerController>().RespawnReset();
            player1.GetComponent<OrbHitter>().RespawnReset();
            player2.GetComponent<OrbHitter>().RespawnReset();
            orb.GetComponent<OrbController>().RespawnReset();
            orb.GetComponent<PowerController>().RespawnReset();
            UIManager.RespawnReset();

            actualCheckpoint.RespawnContent();

            StartCoroutine(FadeCoroutine("FadeIn"));

            respawning = false;
        }
    }



}
