using System.Collections;
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
    public bool drawPath;
    [DrawIf(new string[] { "debug" }, true)]
    public bool drawView;
    [DrawIf(new string[] { "drawView" }, true)]
    public float lengthView;


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

    [HideInInspector]
    public Coroutine actualTauntCoroutine;

    #endregion

    public int baseHP;
    public int hp;

    public float hitStunTime; //stun time when getting hit by the orb

    public bool isWeaken;
    public bool isFrozen;

    [HideInInspector]
    public EnemyMovement enemyMovement;
    [HideInInspector]
    public EnemySkill enemySkill;

    GameObject[] players;
    public static GameObject aimPlayer;

    //to stop when another freeze corout is launch
    [HideInInspector]
    public Coroutine actualFreezeCoroutine;

    [HideInInspector]
    public Coroutine actualDarknessCoroutine;

    [HideInInspector]
    public Coroutine actualFireCoroutine;

    Animator animator;

    public bool lastHitByP1;
    public bool lastHitByP2;
    [HideInInspector]
    public bool isAttacking;

	public GameObject VFX;
	public GameObject deathPoofPrefab;



	#endregion

	public SoundEmitter soundEmitter;

    // Start is called before the first frame update
    void Start()
    {
        hp = baseHP;
        players = new GameObject[] { GameManager.gameManager.player1, GameManager.gameManager.player2 };
        enemyMovement = GetComponent<EnemyMovement>();
        enemySkill = GetComponent<EnemySkill>();
        sdrawPath = drawPath;
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.gameManager.isPaused && !isAttacking)
        {
            FocusManagement();

            if (!enemyMovement.agent.isStopped && !isFrozen)
            {
                enemyMovement.DoMovement();
                animator.SetFloat("Speed", enemyMovement.agent.velocity.magnitude / enemyMovement.initialSpeed);
            }

            if (enemySkill.InRange(aimPlayer) && !isFrozen)
            {
                enemySkill.DoSkill(aimPlayer);
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
    }

    #endregion

    public IEnumerator TauntCoroutine(bool player1)
    {
        isTaunted = true;
        taunter = player1;
        yield return new WaitForSeconds(tauntDuration);
        isTaunted = false;
    }

    public void TakeDamage(int damage)
    {
        if (isWeaken)
        {
            hp -= damage + GameManager.gameManager.orb.GetComponent<PowerController>().darknessDamage;
        }
        else
        {
            hp -= damage;
        }
        GameManager.gameManager.orb.GetComponent<OrbController>().hasHitEnemy = true;
        if (hp <= 0)
        {
			soundEmitter.PlaySound(1, true);
            //update in score manager
            if (lastHitByP1 && !lastHitByP2)
            {
                ScoreManager.scoreManager.killsP1++;
            }
            else if (!lastHitByP1 && lastHitByP2)
            {
                ScoreManager.scoreManager.killsP2++;
            }
            else if (!lastHitByP1 && !lastHitByP2)
            {
                ScoreManager.scoreManager.killsEnvironment++;
            }

			Instantiate(deathPoofPrefab, transform.position, Quaternion.identity);
            GetComponent<LootTable>().LootEnemy();
            enemyMovement.agent.isStopped = true;
            StopAllCoroutines();
            Destroy(this.gameObject);
        }

        if (GetComponent<EnemySkill>().isCasting)
        {
            StopCoroutine(GetComponent<EnemySkill>().rootCoroutine);
            GetComponent<EnemyMovement>().agent.isStopped = false;
            GetComponent<EnemySkill>().isCasting = false;
        }

    }

    IEnumerator HitStun()
    {
        enemyMovement.agent.isStopped = true;
        yield return new WaitForSecondsRealtime(hitStunTime);
        enemyMovement.agent.isStopped = false;
    }

    public IEnumerator FreezeCoroutine(float freezeTimer)
    {
        if (actualFreezeCoroutine != null)
        {
            StopCoroutine(actualFreezeCoroutine);
        }
        GameObject iceFx = gameObject.transform.Find("FX/ice").gameObject;
        iceFx.SetActive(true);
        enemyMovement.agent.isStopped = true;
        isFrozen = true;
        yield return new WaitForSeconds(freezeTimer);
        enemyMovement.agent.isStopped = false;
        isFrozen = false;
        iceFx.SetActive(false);
    }


    public IEnumerator DarknessCoroutine(float darknessTimer)
    {
        if (actualDarknessCoroutine != null)
        {
            StopCoroutine(actualDarknessCoroutine);
        }
        GameObject darknessFx = gameObject.transform.Find("FX/darkness").gameObject;
        darknessFx.SetActive(true);

        yield return new WaitForEndOfFrame();

        isWeaken = true;
        yield return new WaitForSecondsRealtime(darknessTimer);
        isWeaken = false;
        darknessFx.SetActive(false);
    }

    public IEnumerator FireDamage(GameObject target, int totalDamage, float duration)
    {
        int tickDamage = Mathf.RoundToInt(totalDamage / duration);
        int curentDamage = 0;

        Enemy enemy = target.GetComponent<Enemy>();

        if (enemy != null)
        {
            target.GetComponent<Enemy>().VFX.transform.GetChild(0).gameObject.SetActive(true);
        }
        
        while (curentDamage < totalDamage)
        {
            if (enemy != null)
            {
                enemy.TakeDamage(tickDamage);
            }

            yield return new WaitForSeconds(1f);
            curentDamage += tickDamage;
        }

        if (enemy != null)
        {
            target.GetComponent<Enemy>().VFX.transform.GetChild(0).gameObject.SetActive(false);
        }
    }
}
