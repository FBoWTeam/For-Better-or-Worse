﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Random = UnityEngine.Random;
using UnityEngine.SceneManagement;

public class BossSystem : MonoBehaviour
{

    #region enum and struct

    public enum BossPattern
    {
        MysticLine,
        FireBall,
        ElectricZone,
        ShrinkMysticLines,
        ElectricCone,
        Charge,
        ElectricAOE
    }

    [System.Serializable]
    public struct PatternProbability
    {
        public BossPattern pattern;
        [Range(0.0f, 1.0f)]
        public float probability;
    }

    #endregion

    [Header("[Base Params]")]
    public int baseHP;
    public int hp;
    public float stunTime;
    private bool isStuned;
    bool dead;

    public int actualPhase;
    [Range(0.0f, 1.0f)]
    public float phase2Threshold;
    [Range(0.0f, 1.0f)]
    public float phase3Threshold;
    [Range(0.0f, 1.0f)]
    public float phase4Threshold;

    [Header("[Probability Tables]")]
    public List<PatternProbability> phase1;
    public List<PatternProbability> phase2;
    public List<PatternProbability> phase3;
    public List<PatternProbability> phase4;
    private List<PatternProbability> probabilityTable;

    [Header("[Attack Params]")]
    GameObject aimedPlayer;
    public float minWaitTime;
    public float maxWaitTime;
    float nextAttack;
    public bool isAttacking;
    public LayerMask targetMask;


    [Header("[Electric Cone Params]")]
    public float electricConeAngle;
    public int electricConeDamage;
    public float electricConeTimeBetweenFeedbackAndCast;
    public float electricConeChannelingTime;

    [Header("[Electric Zone Params]")]
    public float electricZoneRadius;
    public float electricZoneChannelingTime;
    public float electricZoneTimeBetweenFeedbackAndCast;
    public int electricZoneDamage;


    [Header("[Electric AOE Params]")]
    public float electricAoeRadius;
    public float electricAoeChannelingTime;
    public float electricAoeTimeBetweenFeedbackAndCast;
    public int electricAoeDamage;


    [Header("[FireBall Params]")]
    public GameObject fireBallPrefab;
    public int fireBallDamage;
    public int fireBallDamageExplosion;
    public float fireBallRangeExplosion;
    public float fireBallSpeed;

    [Header("[Mystic Line Params]")]
    public GameObject mysticLinePrefab;
    public int mysticLineLineDamage;
    public float mysticLineHeight;
    public float mysticLineWidth;
    public float lifeTime;
    public GameObject pivotLeft;
    public GameObject pivotRight;
    [HideInInspector]
    public GameObject shrinkLeft;
    [HideInInspector]
    public GameObject shrinkRight;
    [HideInInspector]
    public bool isShrinkMysticLineCreated;
    private bool isMysticLineCreated;
    private bool isShrinking;
    private bool isLeft;
    public float shrinkDuration;
    public float limitAngle;
    public float shrinkSpeed;
    private float angle;
    public float mysticLineTimeBetweenFeedbackAndCast;
    List<GameObject> mysticLineList = new List<GameObject>();

    [Header("[Charge Params]")]
    public int collisionDamage;
    public int chargeDamage;
    private int originalDamage;

    public float chargeCastingTime;
    public float chargeSpeed;
    public float chargeOffset;
    bool willBeStun = false;
    public bool isStun;

    [Header("[Projectors]")]
    public GameObject aoeCircleProjector;
    public GameObject circleProjector;
    public GameObject coneProjector;
    public GameObject lineProjector;
    public GameObject fireBallProjector;
    public GameObject chargeProjector;
    public GameObject mysticLineProjector;
    List<GameObject> projectorList = new List<GameObject>();

    [Range(0.8f, 1.2f)]
    public float toleranceCoef;

    [Header("[Score Values]")]
    public bool lastHitByP1;
    public bool lastHitByP2;

    GameObject player1;
    GameObject player2;

    Animator anim;
    private float electricAoeAnimationTime;
    private float electricConeAnimationTime;
    private float electricZoneAnimationTime;

    [HideInInspector]
    public Coroutine actualFireCoroutine;

    public bool canHitBoss;

    public GameObject rockFall;

    public GameObject FxElectricityLeft;
    public GameObject FxFireLeft;
    public GameObject FxMysticLeft;
    public GameObject FxElectricityRight;
    public GameObject FxFireRight;
    public GameObject FxMysticRight;

	public SoundEmitter soundEmitter;

    //======================================================================================== AWAKE AND UPDATE

    void Awake()
    {
        hp = baseHP;
        isAttacking = false;
        actualPhase = 0;
        checkPhaseTransition();
        isMysticLineCreated = false;
        isShrinkMysticLineCreated = false;
        anim = GetComponent<Animator>();
        originalDamage = collisionDamage;
    }

    private void Start()
    {
        player1 = GameManager.gameManager.player1;
        player2 = GameManager.gameManager.player2;
        mysticLinePrefab.GetComponentInChildren<MysticLine>().damage = mysticLineLineDamage;
        canHitBoss = false;
        dead = false;
    }

    // Update is called once per frame
    void Update()
    {
        checkPhaseTransition();
        if (!GameManager.gameManager.isPaused && !isAttacking)
        {


            if (Time.time >= nextAttack && !isStuned)
            {
                SetFocus();
                LaunchPattern(RandomPattern());
            }

            if (actualPhase == 4)
            {
                Vector3 targetPos = aimedPlayer.transform.position;
                targetPos.y += transform.position.y;
                transform.LookAt(targetPos);
            }

        }

        if (isShrinkMysticLineCreated)
        {
            UpdateScaleShrinkMysticLine();
        }

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            GameManager.gameManager.TakeDamage(collision.gameObject, collisionDamage, collision.contacts[0].point, true);
        }
    }

    //======================================================================================== SET FOCUS

    /// <summary>
    /// set the focus on one of the player at random
    /// </summary>
    public void SetFocus()
    {
        int rand = Random.Range(0, 2);
        if (rand == 0)
        {
            aimedPlayer = GameManager.gameManager.player1;
        }
        else
        {
            aimedPlayer = GameManager.gameManager.player2;
        }
    }

    //======================================================================================== CHECK PHASE TRANSITION

    /// <summary>
    /// check the health based on the actual phase to see if we need to change to the next one
    /// </summary>
    public void checkPhaseTransition()
    {

        switch (actualPhase)
        {
            case 0:
                actualPhase++;
                Debug.Log("Passage phase 1");
                probabilityTable = phase1;
                nextAttack = Time.time + Random.Range(minWaitTime, maxWaitTime);
                //scenaristic start
                break;
            case 1:
                if (hp <= phase2Threshold * baseHP)
                {
                    actualPhase++;
                    Debug.Log("Passage phase 2");
                    probabilityTable = phase2;
                    nextAttack = Time.time + Random.Range(minWaitTime, maxWaitTime);
                    //infinite mystic line same side / level shrink
                    GameObject.Find("Rock Lines").GetComponent<TimeLineRockFall>().Initialize();

                    StopAllCoroutines();
                    DeactivateFXHand();
                    anim.SetTrigger("Stop");
                }
                break;
            case 2:
                if (hp <= phase3Threshold * baseHP)
                {
                    actualPhase++;
                    Debug.Log("Passage phase 3");
                    probabilityTable = phase3;
                    nextAttack = Time.time + Random.Range(minWaitTime, maxWaitTime);
                    //infinite mystic line separation / etc
                    GameObject.Find("TimelineChangePlayers").GetComponent<TimeLineChangePlayers>().Initialize();

                    StopAllCoroutines();
                    DeactivateFXHand();
                    anim.SetTrigger("Stop");
                    CleanProjectorList();
                }
                break;
            case 3:
                if (hp <= phase4Threshold * baseHP)
                {
                    actualPhase++;
                    Debug.Log("Passage phase 4");
                    probabilityTable = phase4;
                    nextAttack = Time.time + Random.Range(minWaitTime, maxWaitTime);
                    //fall to ground / level shrink / rock fall activation
                    GameObject.Find("Rock Corners").GetComponent<TimeLineCornerRockFall>().Initialize();

                    StopAllCoroutines();
                    DeactivateFXHand();
                    anim.SetTrigger("Stop");
                    CleanProjectorList();
                }
                break;
            case 4:
                if (hp <= 0.0f && !dead)
                {
                    dead = true;
                    StopAllCoroutines();
                    DeactivateFXHand();
                    anim.SetTrigger("Stop");
                    CleanProjectorList();
                    GameObject.Find("TimelineDeath").GetComponent<TimeLineDeath>().Initialize();
                }
                break;
        }
    }


    public void CleanProjectorList()
    {
        foreach (GameObject indic in projectorList)
        {
            Destroy(indic);
        }
        projectorList.Clear();
    }

    public void CleanMysticLineList()
    {
        foreach (GameObject myst in mysticLineList)
        {
            Destroy(myst);
        }
        mysticLineList.Clear();
    }

    public void DeactivateFXHand()
    {
        FxElectricityLeft.SetActive(false);
        FxElectricityRight.SetActive(false);
        FxFireLeft.SetActive(false);
        FxFireRight.SetActive(false);
        FxMysticLeft.SetActive(false);
        FxMysticRight.SetActive(false);
    }

    //======================================================================================== RANDOM PATTERN

    /// <summary>
    /// pick a pattern at random based on the actual probability table
    /// </summary>
    /// <returns></returns>
    public BossPattern RandomPattern()
    {
        float randomPick = Random.Range(0.0f, 1.0f);
        float actualProb = 0.0f;

        foreach (PatternProbability patternProb in probabilityTable)
        {
            actualProb += patternProb.probability;
            if (randomPick <= actualProb)
            {
                return patternProb.pattern;
            }
        }

        return BossPattern.MysticLine;
    }

    //======================================================================================== LAUNCH PATTERN

    /// <summary>
    /// launch the selected pattern
    /// </summary>
    /// <param name="pattern"></param>
    public void LaunchPattern(BossPattern pattern)
    {
        switch (pattern)
        {
            case BossPattern.MysticLine:
                StartCoroutine(MysticLineCoroutine());
                break;
            case BossPattern.FireBall:
                StartCoroutine(FireBallCoroutine());
                break;
            case BossPattern.ElectricZone:
                StartCoroutine(ElectricZoneCoroutine());
                break;
            case BossPattern.ShrinkMysticLines:
                StartCoroutine(Shrink());
                break;
            case BossPattern.ElectricCone:
                StartCoroutine(ElectricConeCoroutine());
                break;
            case BossPattern.Charge:
                StartCoroutine(ChargeCoroutine());
                break;
            case BossPattern.ElectricAOE:
                StartCoroutine(ElectricAOECoroutine());
                break;
        }
    }

    #region pattern coroutines

    //======================================================================================== MYSTIC LINE

    public IEnumerator MysticLineCoroutine()
    {
        isAttacking = true;
        //Debug.Log("Mystic Line");

        //canalisation + feedbacks
        anim.SetTrigger("LineFireBallShrink");

        FxMysticLeft.SetActive(true);
        FxMysticRight.SetActive(true);

        yield return new WaitForSeconds(2.8f);

        Vector3 raycastPosition = new Vector3(transform.position.x, 1f, transform.position.z);
        RaycastHit hit;
        Vector3 direction = (new Vector3(aimedPlayer.transform.position.x, raycastPosition.y, aimedPlayer.transform.position.z) - raycastPosition).normalized;

        //show feedback
        //instanciate the circle indicator
        GameObject mysticLineIndicator = Instantiate(mysticLineProjector, transform.position, Quaternion.identity) as GameObject;
        projectorList.Add(mysticLineIndicator);

        float timeStamp = Time.time;
        Color tempColor = Color.magenta;

        mysticLineIndicator.transform.Rotate(Vector3.up, -Vector3.SignedAngle(direction, Vector3.back, Vector3.up));

        while (Time.time - timeStamp < 1.2f)
        {
            //alpha starting from 0 finishing to 0.33333
            tempColor.a = ((Time.time - timeStamp) / mysticLineTimeBetweenFeedbackAndCast) / 3;
            mysticLineIndicator.transform.GetChild(0).gameObject.GetComponent<Projector>().material.color = tempColor;
            yield return new WaitForEndOfFrame();
        }

		soundEmitter.PlaySound(0);

        if (Physics.Raycast(raycastPosition, direction, out hit, 50, LayerMask.GetMask("Wall")))
        {
            StartCoroutine(CreateMysticLineCoroutine(transform.position, hit.transform.position, hit.distance));
            FxMysticLeft.SetActive(false);
            FxMysticRight.SetActive(false);
        }

        Destroy(mysticLineIndicator);

        nextAttack = Time.time + Random.Range(minWaitTime, maxWaitTime);
        isAttacking = false;
    }

    public IEnumerator CreateMysticLineCoroutine(Vector3 position, Vector3 target, float length)
    {
        GameObject mysticLine = Instantiate(mysticLinePrefab, position, Quaternion.identity, transform);
        mysticLine.transform.LookAt(new Vector3(target.x, position.y, target.z));
        mysticLine.transform.localScale = new Vector3(mysticLineWidth / transform.localScale.x, mysticLineHeight / transform.localScale.y, length / transform.localScale.z);

        isMysticLineCreated = true;

        mysticLineList.Add(mysticLine);

        yield return new WaitForSeconds(lifeTime);

        Destroy(mysticLine);
        isMysticLineCreated = false;
    }

    //======================================================================================== SHRINK MYSTIC LINES

    public IEnumerator ShrinkMysticLinesCoroutine()
    {
        //Debug.Log("Shrink MysticLines");

        //canalisation + feedbacks
        anim.SetTrigger("LineFireBallShrink");

        FxMysticLeft.SetActive(true);
        FxMysticRight.SetActive(true);

        yield return new WaitForSeconds(4.2f);

        if (!isShrinkMysticLineCreated)
        {
            Vector3 raycastPosition = new Vector3(transform.position.x, 1f, transform.position.z);//(pivotLeft.transform.position.x, 1, transform.position.z);
            RaycastHit hit;
            //Debug.DrawRay(raycastPosition, pivotLeft.transform.forward * 50, Color.blue, 20);

            if (Physics.Raycast(raycastPosition, pivotLeft.transform.forward, out hit, 50, LayerMask.GetMask("Wall")))
            {
                shrinkLeft = Instantiate(mysticLinePrefab, pivotLeft.transform.position, Quaternion.identity, pivotLeft.transform);
                shrinkLeft.transform.LookAt(hit.transform);

                Physics.Raycast(raycastPosition, pivotRight.transform.forward, out hit, 50, LayerMask.GetMask("Wall"));
                shrinkRight = Instantiate(mysticLinePrefab, pivotRight.transform.position, Quaternion.identity, pivotRight.transform);
                shrinkRight.transform.LookAt(hit.transform);
            }
            isShrinkMysticLineCreated = true;

            FxMysticLeft.SetActive(false);
            FxMysticRight.SetActive(false);
        }

    }

    public void UpdateScaleShrinkMysticLine()
    {
        Vector3 raycastPosition = new Vector3(transform.position.x, 1f, transform.position.z);
        RaycastHit hit;

        Physics.Raycast(raycastPosition, pivotLeft.transform.forward, out hit, 500, LayerMask.GetMask("Wall"));
        //Debug.DrawRay(raycastPosition, pivotLeft.transform.forward * hit.distance, Color.blue, 1);
        shrinkLeft.transform.localScale = new Vector3(mysticLineWidth / transform.localScale.x, mysticLineHeight / transform.localScale.y, hit.distance / transform.localScale.z);

        Physics.Raycast(raycastPosition, pivotRight.transform.forward, out hit, 50, LayerMask.GetMask("Wall"));
        //Debug.DrawRay(raycastPosition, pivotRight.transform.forward * hit.distance, Color.red, 1);
        shrinkRight.transform.localScale = new Vector3(mysticLineWidth / transform.localScale.x, mysticLineHeight / transform.localScale.y, hit.distance / transform.localScale.z);
    }

    public IEnumerator Shrink()
    {
        yield return new WaitForSeconds(1);
        Vector3 newDirLeft;
        Vector3 newDirRight;
        float step = shrinkSpeed * Time.deltaTime;

        int rand = Random.Range(0, 2);

        //Forward
        if (rand == 0)
        {
            while (Vector3.Angle(pivotLeft.transform.forward, Quaternion.Euler(0, -limitAngle, 0) * transform.forward) > 0.4 || Vector3.Angle(pivotRight.transform.forward, Quaternion.Euler(0, limitAngle, 0) * transform.forward) > 0.4)
            {
                //print("Angle PivotLeft: " + Vector3.Angle(pivotLeft.transform.forward, Quaternion.Euler(0, -limitAngle, 0) * transform.forward - pivotLeft.transform.position));
                Vector3 vectorLeft = Quaternion.Euler(0, -limitAngle, 0) * transform.forward;
                Vector3 vectorRight = Quaternion.Euler(0, limitAngle, 0) * transform.forward;
                newDirLeft = Vector3.RotateTowards(pivotLeft.transform.forward, vectorLeft, step, 0.0f);
                newDirRight = Vector3.RotateTowards(pivotRight.transform.forward, vectorRight, step, 0.0f);

                pivotLeft.transform.rotation = Quaternion.LookRotation(newDirLeft);
                pivotRight.transform.rotation = Quaternion.LookRotation(newDirRight);
                yield return new WaitForEndOfFrame();
            }
            yield return new WaitForSeconds(shrinkDuration);

            while (Vector3.Angle(pivotLeft.transform.forward, Quaternion.Euler(0, -90, 0) * transform.forward) > 0.4 || Vector3.Angle(pivotRight.transform.forward, Quaternion.Euler(0, 90, 0) * transform.forward) > 0.4)
            {
                Vector3 vectorLeft = Quaternion.Euler(0, -90, 0) * transform.forward;
                Vector3 vectorRight = Quaternion.Euler(0, 90, 0) * transform.forward;

                newDirLeft = Vector3.RotateTowards(pivotLeft.transform.forward, vectorLeft, step, 0.0f);
                newDirRight = Vector3.RotateTowards(pivotRight.transform.forward, vectorRight, step, 0.0f);

                pivotLeft.transform.rotation = Quaternion.LookRotation(newDirLeft);
                pivotRight.transform.rotation = Quaternion.LookRotation(newDirRight);

                yield return new WaitForEndOfFrame();
            }

        }
        //Backward
        else
        {
            while (Vector3.Angle(pivotLeft.transform.forward, Quaternion.Euler(0, limitAngle, 0) * -transform.forward) > 0.4 || Vector3.Angle(pivotRight.transform.forward, Quaternion.Euler(0, -limitAngle, 0) * -transform.forward) > 0.4)
            {
                Vector3 vectorLeft = Quaternion.Euler(0, limitAngle, 0) * -transform.forward;
                Vector3 vectorRight = Quaternion.Euler(0, -limitAngle, 0) * -transform.forward;
                newDirLeft = Vector3.RotateTowards(pivotLeft.transform.forward, vectorLeft, step, 0.0f);
                newDirRight = Vector3.RotateTowards(pivotRight.transform.forward, vectorRight, step, 0.0f);

                pivotLeft.transform.rotation = Quaternion.LookRotation(newDirLeft);
                pivotRight.transform.rotation = Quaternion.LookRotation(newDirRight);
                yield return new WaitForEndOfFrame();

            }
            yield return new WaitForSeconds(shrinkDuration);

            while (Vector3.Angle(pivotLeft.transform.forward, Quaternion.Euler(0, 90, 0) * -transform.forward) > 0.4 || Vector3.Angle(pivotRight.transform.forward, Quaternion.Euler(0, -90, 0) * -transform.forward) > 0.4)
            {
                Vector3 vectorLeft = Quaternion.Euler(0, 90, 0) * -transform.forward;
                Vector3 vectorRight = Quaternion.Euler(0, -90, 0) * -transform.forward;

                newDirLeft = Vector3.RotateTowards(pivotLeft.transform.forward, vectorLeft, step, 0.0f);
                newDirRight = Vector3.RotateTowards(pivotRight.transform.forward, vectorRight, step, 0.0f);

                pivotLeft.transform.rotation = Quaternion.LookRotation(newDirLeft);
                pivotRight.transform.rotation = Quaternion.LookRotation(newDirRight);

                yield return new WaitForEndOfFrame();

            }

        }

    }


    //======================================================================================== FIREBALL

    public IEnumerator FireBallCoroutine()
    {
        isAttacking = true;

        anim.SetTrigger("LineFireBallShrink");

        FxFireLeft.SetActive(true);
        FxFireRight.SetActive(true);

        yield return new WaitForSeconds(4.2f);

        FxFireLeft.SetActive(false);
        FxFireRight.SetActive(false);

        Vector3 target = aimedPlayer.transform.position;

        Vector3 fireBallStartingPoint = transform.position + new Vector3(0f, 8.5f, 0f);// + 2.8f * dir;

        GameObject projectileFireBall = Instantiate(fireBallPrefab, fireBallStartingPoint, Quaternion.identity);
        FireBall fireBall = projectileFireBall.GetComponent<FireBall>();

        float travelTime = 0;
        if (fireBall != null)
        {
            fireBall.Init(fireBallDamage, fireBallDamageExplosion, fireBallRangeExplosion, fireBallSpeed);
            travelTime = fireBall.Launch(target + new Vector3(0f, 1.5f, 0f), fireBallStartingPoint);//offset so the fireball aims for the body of the player and not his/her feet
        }

		soundEmitter.PlaySound(1);

        //show indicator feedback
        //instanciate the fireball indicator
        GameObject fireBallIndicator = Instantiate(fireBallProjector, target + new Vector3(0f, 1.5f, 0f), Quaternion.identity) as GameObject;
        projectorList.Add(fireBallIndicator);
        fireBallProjector.transform.GetChild(0).gameObject.GetComponent<Projector>().orthographicSize = fireBallRangeExplosion * toleranceCoef;
        float timeStamp = Time.time;
        Color tempColor = Color.red;

        while (Time.time - timeStamp < travelTime)
        {
            //alpha starting from 0 finishing to 0.5
            tempColor.a = ((Time.time - timeStamp) / electricAoeTimeBetweenFeedbackAndCast) / 2;
            fireBallIndicator.transform.GetChild(0).gameObject.GetComponent<Projector>().material.color = tempColor;
            yield return new WaitForEndOfFrame();
        }

        
        yield return new WaitUntil(() => fireBall.willBeDestroyed);
        Destroy(fireBallIndicator);
		soundEmitter.PlaySound(2);

        nextAttack = Time.time + Random.Range(minWaitTime, maxWaitTime);
        isAttacking = false;
    }

    //======================================================================================== ELECTRIC ZONE

    public IEnumerator ElectricZoneCoroutine()
    {
        isAttacking = true;

        //start chaneling anim
        //Debug.Log("channeling electric zone");
        anim.SetTrigger("Electricity");

        StartCoroutine(FxElectricity());

        yield return new WaitForSeconds(2.6f);

        Vector3 electricZoneLocation = aimedPlayer.transform.position;

        //show feedback
        //instanciate the circle indicator
        GameObject circleIndicator = Instantiate(circleProjector, electricZoneLocation, Quaternion.identity) as GameObject;
        projectorList.Add(circleIndicator);
        circleIndicator.transform.GetChild(0).gameObject.GetComponent<Projector>().orthographicSize = electricZoneRadius * toleranceCoef;
        float timeStamp = Time.time;
        Color tempColor = Color.blue;

        while (Time.time - timeStamp < 2f)
        {
            //alpha starting from 0 finishing to 0.33333
            tempColor.a = ((Time.time - timeStamp) / electricZoneTimeBetweenFeedbackAndCast) / 3;
            circleIndicator.transform.GetChild(0).gameObject.GetComponent<Projector>().material.color = tempColor;
            yield return new WaitForEndOfFrame();
        }

		//Debug.Log("casting electric zone");
		soundEmitter.PlaySound(3);

		//check if the players are in the area of effect
		Collider[] playersInRange = Physics.OverlapSphere(electricZoneLocation, electricZoneRadius, targetMask);

        //apply damage to the players in the area of effect
        for (int i = 0; i < playersInRange.Length; i++)
        {
            GameManager.gameManager.TakeDamage(playersInRange[i].gameObject, electricZoneDamage, electricZoneLocation, true);
        }

        Destroy(circleIndicator);
        yield return new WaitForSeconds(1.0f);

        nextAttack = Time.time + Random.Range(minWaitTime, maxWaitTime);
        isAttacking = false;
    }

    //======================================================================================== ELECTRIC CONE

    public IEnumerator ElectricConeCoroutine()
    {
        isAttacking = true;

        //start chaneling anim
        //Debug.Log("channeling electric cone");
        anim.SetTrigger("Electricity");
        StartCoroutine(FxElectricity());
        yield return new WaitForSeconds(2.6f);

        //determining the area where to cast the spell
        Vector3 bossPos = transform.position;
        bossPos.y = 0;

        Vector3 targetVector = (aimedPlayer.transform.position - bossPos).normalized;
        Quaternion leftRotation = Quaternion.Euler(0, -electricConeAngle / 2, 0);
        Quaternion rightRotation = Quaternion.Euler(0, electricConeAngle / 2, 0);
        Vector3 minRange = leftRotation * targetVector;
        Vector3 maxRange = rightRotation * targetVector;

        //instanciate the circle indicator
        GameObject coneIndicator = Instantiate(coneProjector, transform.position, Quaternion.identity) as GameObject;
        projectorList.Add(coneIndicator);

        float timeStamp = Time.time;
        Color tempColor = Color.blue;

        //warning : there is a ' - ' before 'Vector3.Angle(targetVector, Vector3.back)' because the sprite of the cone is reversed
        //the ' - ' is necessary to turn in the right sens
        coneIndicator.transform.Rotate(Vector3.up, -Vector3.SignedAngle(targetVector, Vector3.back, Vector3.up));

        while (Time.time - timeStamp < 2f)
        {
            //alpha starting from 0 finishing to 0.33333
            tempColor.a = ((Time.time - timeStamp) / electricConeTimeBetweenFeedbackAndCast) / 3;
            coneIndicator.transform.GetChild(0).gameObject.GetComponent<Projector>().material.color = tempColor;
            yield return new WaitForEndOfFrame();
        }

		//Debug.Log("casting electric cone");
		soundEmitter.PlaySound(3);

		//check if players are in the area of effect to apply damages
		Vector3 dirToTarget;
        dirToTarget = (player1.transform.position - bossPos).normalized;
        dirToTarget.y = 0;


        if (Vector3.Angle(targetVector, dirToTarget) < electricConeAngle / 2)
        {
            GameManager.gameManager.TakeDamage(player1, electricConeDamage, Vector3.zero, false);
        }

        dirToTarget = (player2.transform.position - bossPos).normalized;
        dirToTarget.y = 0;

        if (Vector3.Angle(targetVector, dirToTarget) < electricConeAngle / 2)
        {
            GameManager.gameManager.TakeDamage(player2, electricConeDamage, Vector3.zero, false);
        }


        Destroy(coneIndicator);
        yield return new WaitForSeconds(1.0f);

        nextAttack = Time.time + Random.Range(minWaitTime, maxWaitTime);
        isAttacking = false;
    }

    //======================================================================================== CHARGE

    public IEnumerator ChargeCoroutine()
    {
        isAttacking = true;
        collisionDamage = chargeDamage;

        Vector3 target = aimedPlayer.transform.position;
        Vector3 posStart = transform.position;

        Vector3 vectCharge = target - posStart;
        Vector3 newTarget = target + chargeOffset * vectCharge.normalized;//aiming for behind the target player by an offset

        RaycastHit hit;
        int layerMask = 1 << 11;//to only hit the walls

        if (Physics.Raycast(posStart, vectCharge, out hit, vectCharge.magnitude + chargeOffset, layerMask))
        {
            willBeStun = true;
            newTarget = hit.point - 2f * vectCharge.normalized;
        }

        float distCharge = vectCharge.magnitude;
        float chargeTime = distCharge / chargeSpeed;


        //show indicator feedback
        //instanciate the charge indicator
        GameObject chargeIndicator = Instantiate(chargeProjector, transform.position + vectCharge / 2f + (chargeOffset / 2f) * vectCharge.normalized, Quaternion.identity) as GameObject;
        projectorList.Add(chargeIndicator);
        float yComp = 0f;
        if (vectCharge.x < 0)
        {
            yComp = Vector3.Angle(new Vector3(0f, 0f, -1f), vectCharge);
        }
        else
        {
            yComp = -Vector3.Angle(new Vector3(0f, 0f, -1f), vectCharge);

        }
        chargeIndicator.transform.eulerAngles = new Vector3(0f, yComp + 90f, 0f);

        chargeIndicator.GetComponentInChildren<Projector>().aspectRatio = vectCharge.magnitude / 18f;

        float timeStamp = Time.time;
        Color tempColor = Color.red;

        while (Time.time - timeStamp < chargeCastingTime)
        {
            //alpha starting from 0.25 finishing to 0.75
            tempColor.a = 0.25f + ((Time.time - timeStamp) / electricAoeTimeBetweenFeedbackAndCast) / 2;
            chargeIndicator.transform.GetChild(0).gameObject.GetComponent<Projector>().material.color = tempColor;
            yield return new WaitForEndOfFrame();
		}

		anim.SetBool("IsDashing", true);
		soundEmitter.PlaySound(4);

		float t = 0f;
        while (t < 1)
        {
            transform.position = Vector3.Lerp(posStart, newTarget, t);
            t += 1f / (100f * chargeTime);
            yield return new WaitForEndOfFrame();
        }

        anim.SetBool("IsDashing", false);
        anim.SetBool("DashWillStun", willBeStun);

        if (willBeStun)
        {
            willBeStun = false;
            isStun = true;
            yield return new WaitForSeconds(0.8f);
            isStun = false;
        }

        Destroy(chargeIndicator);

        collisionDamage = originalDamage;
        yield return new WaitForSeconds(0.7f);//wait for the end animation

        nextAttack = Time.time + Random.Range(minWaitTime, maxWaitTime);
        isAttacking = false;
    }

    //======================================================================================== ELECTRIC AOE

    public IEnumerator ElectricAOECoroutine()
    {
        isAttacking = true;

        //start chaneling anim
       // Debug.Log("channeling AOE zone");
        anim.SetTrigger("Electricity");

        StartCoroutine(FxElectricity());

        //wait 75% of the cast time, 2.8s in total
        yield return new WaitForSeconds(2.8f);

        //show indicator feedback
        //instanciate the circle indicator
        GameObject circleIndicator = Instantiate(aoeCircleProjector, transform.position, Quaternion.identity) as GameObject;
        projectorList.Add(circleIndicator);
        circleIndicator.transform.GetChild(0).gameObject.GetComponent<Projector>().orthographicSize = electricAoeRadius * toleranceCoef;
        //the instanciated circle indicator is a child of the boss
        circleIndicator.transform.parent = transform;
        float timeStamp = Time.time;
        Color tempColor = Color.blue;

        while (Time.time - timeStamp < 1.8f)
        {
            //alpha starting from 0 finishing to 0.33333
            tempColor.a = ((Time.time - timeStamp) / electricAoeTimeBetweenFeedbackAndCast) / 3;
            circleIndicator.transform.GetChild(0).gameObject.GetComponent<Projector>().material.color = tempColor;
            yield return new WaitForEndOfFrame();
        }

		soundEmitter.PlaySound(3);

		//check if the players are in the area of effect
		Collider[] playersInRange = Physics.OverlapSphere(transform.position, electricAoeRadius, targetMask);

        //apply damage to the players in the area of effect
        for (int i = 0; i < playersInRange.Length; i++)
        {
            GameManager.gameManager.TakeDamage(playersInRange[i].gameObject, electricZoneDamage, transform.position, true);
        }

        Destroy(circleIndicator);
        yield return new WaitForSeconds(1.0f);

        nextAttack = Time.time + Random.Range(minWaitTime, maxWaitTime);
        isAttacking = false;
    }


    IEnumerator FxElectricity()
    {
        yield return new WaitForSeconds(1.7f);
        FxElectricityRight.SetActive(true);
        yield return new WaitForSeconds(1.7f);
        FxElectricityLeft.SetActive(true);
        yield return new WaitForSeconds(1.2f);
        FxElectricityRight.SetActive(false);
        FxElectricityLeft.SetActive(false);
    }

    #endregion

    public void TakeDamage(int damage)
    {
        if (canHitBoss)
        {
            hp -= damage;
            GameManager.gameManager.orb.GetComponent<OrbController>().hasHitEnemy = true;
            if (hp <= 0)
            {
                //update in score manager
                if (lastHitByP1 && !lastHitByP2)
                {
                    ScoreManager.scoreManager.bossKilledByP1 = true;
                }
                else if (!lastHitByP1 && lastHitByP2)
                {
                    ScoreManager.scoreManager.bossKilledByP1 = false;
                }
                else if (!lastHitByP1 && !lastHitByP2)
                {
                    ScoreManager.scoreManager.killsEnvironment++;
                }/*
                StopAllCoroutines();
                GameData.previousScene = 9;
                SceneManager.LoadScene(10);*/
            }
        }
    }

    public IEnumerator Stun()
    {
		soundEmitter.PlaySound(5);
        isStuned = true;
        yield return new WaitForSeconds(stunTime);
        isStuned = false;
    }

    public IEnumerator FireDamage(GameObject target, int totalDamage, float duration)
    {
        int tickDamage = Mathf.RoundToInt(totalDamage / duration);
        int curentDamage = 0;

        BossSystem bossSystem = target.GetComponent<BossSystem>();

        if (bossSystem != null)
        {
            //activer les fx de feu sur le boss
        }

        while (curentDamage < totalDamage)
        {
            if (bossSystem != null)
            {
                bossSystem.TakeDamage(tickDamage);
            }

            yield return new WaitForSeconds(1f);
            curentDamage += tickDamage;
        }

        if (bossSystem != null)
        {
            //désactiver les fx de feu sur le boss
        }
    }

    //function to visualize the effect zone of an AOE
    void DrawAOE(Vector3 position, float radius)
    {
        position.y = 0;
        for (int i = 0; i < 16; i++)
        {
            float angle = i * Mathf.PI * 2 / 16f;
            Vector3 destination = new Vector3(Mathf.Cos(angle) * radius, 0, Mathf.Sin(angle) * radius);
            Debug.DrawLine(position, destination + position, Color.red, 10f);
        }
    }

}
