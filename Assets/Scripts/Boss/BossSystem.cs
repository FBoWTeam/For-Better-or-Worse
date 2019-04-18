using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Random = UnityEngine.Random;

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
    bool isAttacking;
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
    private GameObject shrinkLeft;
    private GameObject shrinkRight;
    private bool isMysticLineCreated;
    private bool isShrinkMysticLineCreated;
    private bool isShrinking;
    private bool isLeft;
    public float shrinkDuration;
    public float limitAngle;
    public float shrinkSpeed;
    private float angle;
    public float mysticLineTimeBetweenFeedbackAndCast;

    [Header("[Charge Params]")]
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
    }

    private void Start()
    {
        player1 = GameManager.gameManager.player1;
        player2 = GameManager.gameManager.player2;
        mysticLinePrefab.GetComponentInChildren<MysticLine>().damage = mysticLineLineDamage;
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.gameManager.isPaused && !isAttacking)
        {
            checkPhaseTransition();

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

    //======================================================================================== SET FOCUS

    /// <summary>
    /// set the focus on one of the player at random
    /// </summary>
    public void SetFocus()
    {
        int rand = Random.Range(0, 2);
        if (rand == 0)
        {
            Debug.Log("Aim Player 1");
            aimedPlayer = GameManager.gameManager.player1;
        }
        else
        {
            Debug.Log("Aim Player 2");
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
                    transform.localScale = new Vector3(0.75f, 0.75f, 0.75f);
                    //infinite mystic line same side / level shrink
                    GameObject.Find("Rock Lines").GetComponent<TimeLineRockFall>().Initialize();
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
                }
                break;
            case 3:
                if (hp <= phase4Threshold * baseHP)
                {
                    actualPhase++;
                    Debug.Log("Passage phase 4");
                    probabilityTable = phase4;
                    nextAttack = Time.time + Random.Range(minWaitTime, maxWaitTime);
                    transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                    //fall to ground / level shrink / rock fall activation
                }
                break;
            case 4:
                if (hp <= 0.0f)
                {
                    Debug.Log("DED");
                    //ded
                }
                break;
        }
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
                StartCoroutine(ShrinkMysticLinesCoroutine());
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
        Debug.Log("Mystic Line");

        //canalisation + feedbacks
        anim.SetTrigger("LineFireBallShrink");

        yield return new WaitForSeconds(2.8f);

        Vector3 raycastPosition = new Vector3(transform.position.x, 0, transform.position.z);
        RaycastHit hit;
        Vector3 direction = (new Vector3(aimedPlayer.transform.position.x, raycastPosition.y, aimedPlayer.transform.position.z) - raycastPosition).normalized;

        //show feedback
        //instanciate the circle indicator
        GameObject mysticLineIndicator = Instantiate(mysticLineProjector, transform.position, Quaternion.identity) as GameObject;

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

        //if (!isMysticLineCreated)
        //{
        //}

        if (Physics.Raycast(raycastPosition, direction, out hit, 50, LayerMask.GetMask("Wall")))
        {
            StartCoroutine(CreateMysticLineCoroutine(raycastPosition, hit.transform.position, hit.distance));
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

        yield return new WaitForSeconds(lifeTime);

        Destroy(mysticLine);
        isMysticLineCreated = false;
    }

    //======================================================================================== SHRINK MYSTIC LINES

    public IEnumerator ShrinkMysticLinesCoroutine()
    {

        isAttacking = true;
        Debug.Log("Shrink MysticLines");

        //canalisation + feedbacks
        anim.SetTrigger("LineFireBallShrink");
        yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length);

        if (!isShrinkMysticLineCreated)
        {
            Vector3 raycastPosition = new Vector3(transform.position.x, 0, transform.position.z);
            RaycastHit hit;

            if (Physics.Raycast(raycastPosition, pivotLeft.transform.forward, out hit, 50, LayerMask.GetMask("Wall")))
            {
                //Debug
                //print("Distance : " + hit.distance);

                shrinkLeft = Instantiate(mysticLinePrefab, pivotLeft.transform.position, Quaternion.identity, pivotLeft.transform);
                shrinkLeft.transform.LookAt(new Vector3(hit.transform.position.x, shrinkLeft.transform.position.y, hit.transform.position.z));

                shrinkRight = Instantiate(mysticLinePrefab, pivotRight.transform.position, Quaternion.identity, pivotRight.transform);
                shrinkRight.transform.LookAt(new Vector3(-hit.transform.position.x, shrinkRight.transform.position.y, -hit.transform.position.z));

            }
            isShrinkMysticLineCreated = true;
        }

        StartCoroutine(Shrink());
    }

    public void UpdateScaleShrinkMysticLine()
    {
        Vector3 raycastPosition = new Vector3(transform.position.x, 0, transform.position.z);
        RaycastHit hit;

        Physics.Raycast(raycastPosition, pivotLeft.transform.forward, out hit, 50, LayerMask.GetMask("Wall"));
        //Debug.DrawRay(raycastPosition, pivotLeft.transform.forward * 50, Color.blue, 2);
        //print("shrinkLeft Length : " + hit.distance);
        shrinkLeft.transform.localScale = new Vector3(mysticLineWidth / transform.localScale.x, mysticLineHeight / transform.localScale.y, hit.distance / transform.localScale.z);

        Physics.Raycast(raycastPosition, pivotRight.transform.forward, out hit, 50, LayerMask.GetMask("Wall"));
        //print("shrinkRight Length : " + hit.distance);
        //Debug.DrawRay(raycastPosition, pivotRight.transform.forward * 50, Color.red, 2);
        shrinkRight.transform.localScale = new Vector3(mysticLineWidth / transform.localScale.x, mysticLineHeight / transform.localScale.y, hit.distance / transform.localScale.z);

    }

    public IEnumerator Shrink()
    {
        yield return new WaitForSeconds(1);
        Vector3 newDirLeft;
        Vector3 newDirRight;
        float step = shrinkSpeed * Time.deltaTime;

        int rand = Random.Range(0, 2);
        print("Rand : " + rand);

        //Forward
        if (rand == 0)
        {
            while (Vector3.Angle(pivotLeft.transform.forward, Quaternion.Euler(0, -limitAngle, 0) * transform.forward - pivotLeft.transform.position) > 0.4 || Vector3.Angle(pivotRight.transform.forward, Quaternion.Euler(0, limitAngle, 0) * transform.forward - pivotRight.transform.position) > 0.4)
            {
                Vector3 vectorLeft = Quaternion.Euler(0, -limitAngle, 0) * transform.forward;
                Vector3 vectorRight = Quaternion.Euler(0, limitAngle, 0) * transform.forward;
                newDirLeft = Vector3.RotateTowards(pivotLeft.transform.forward, vectorLeft, step, 0.0f);
                newDirRight = Vector3.RotateTowards(pivotRight.transform.forward, vectorRight, step, 0.0f);

                pivotLeft.transform.rotation = Quaternion.LookRotation(newDirLeft);
                pivotRight.transform.rotation = Quaternion.LookRotation(newDirRight);
                yield return new WaitForEndOfFrame();
            }
            yield return new WaitForSeconds(shrinkDuration);

            while (Vector3.Angle(pivotLeft.transform.forward, Quaternion.Euler(0, -90, 0) * transform.forward - pivotLeft.transform.position) > 0.4 || Vector3.Angle(pivotRight.transform.forward, Quaternion.Euler(0, 90, 0) * transform.forward - pivotRight.transform.position) > 0.4)
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
            while (Vector3.Angle(pivotLeft.transform.forward, Quaternion.Euler(0, limitAngle, 0) * -transform.forward - pivotLeft.transform.position) > 0.4 || Vector3.Angle(pivotRight.transform.forward, Quaternion.Euler(0, -limitAngle, 0) * -transform.forward - pivotRight.transform.position) > 0.4)
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

            while (Vector3.Angle(pivotLeft.transform.forward, Quaternion.Euler(0, 90, 0) * -transform.forward - pivotLeft.transform.position) > 0.4 || Vector3.Angle(pivotRight.transform.forward, Quaternion.Euler(0, -90, 0) * -transform.forward - pivotRight.transform.position) > 0.4)
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
        isAttacking = false;

    }


    //======================================================================================== FIREBALL

    public IEnumerator FireBallCoroutine()
    {
        isAttacking = true;

        anim.SetTrigger("LineFireBallShrink");
        yield return new WaitForSeconds(4.2f);


        //yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length);

        Vector3 target = aimedPlayer.transform.position;
        /*Vector3 dir = target - transform.position;//direction of the aimed player when the Fireball is creating
        dir = dir.normalized;
        dir.y = 0;*/

        Vector3 fireBallStartingPoint = transform.position + new Vector3(0f, 8.5f, 0f);// + 2.8f * dir;

        GameObject projectileFireBall = Instantiate(fireBallPrefab, fireBallStartingPoint, Quaternion.identity);
        FireBall fireBall = projectileFireBall.GetComponent<FireBall>();

        float travelTime = 0;
        if (fireBall != null)
        {
            fireBall.Init(fireBallDamage, fireBallDamageExplosion, fireBallRangeExplosion, fireBallSpeed);
            travelTime = fireBall.Launch(target + new Vector3(0f, 1.5f, 0f), fireBallStartingPoint);//offset so the fireball aims for the body of the player and not his/her feet
        }

        //show indicator feedback
        //instanciate the fireball indicator
        GameObject fireBallIndicator = Instantiate(fireBallProjector, target + new Vector3(0f, 1.5f, 0f), Quaternion.identity) as GameObject;
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


        yield return new WaitUntil(() => fireBall.isDestroyed);
        Destroy(fireBallIndicator);

        nextAttack = Time.time + Random.Range(minWaitTime, maxWaitTime);
        isAttacking = false;
    }

    //======================================================================================== ELECTRIC ZONE

    public IEnumerator ElectricZoneCoroutine()
    {
        isAttacking = true;

        //start chaneling anim
        Debug.Log("channeling electric zone");
        anim.SetTrigger("Electricity");
        yield return new WaitForSeconds(2.6f);

        Vector3 electricZoneLocation = aimedPlayer.transform.position;

        //show feedback
        //instanciate the circle indicator
        GameObject circleIndicator = Instantiate(circleProjector, electricZoneLocation, Quaternion.identity) as GameObject;
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

        Debug.Log("casting electric zone");

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
        Debug.Log("channeling electric cone");
        anim.SetTrigger("Electricity");
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
        //the instanciated circle indicator is a child of the boss
        coneIndicator.transform.parent = transform;
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

        Debug.Log("casting electric cone");

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

        anim.SetBool("IsDashing", true);

        while (Time.time - timeStamp < chargeCastingTime)
        {
            //alpha starting from 0.25 finishing to 0.75
            tempColor.a = 0.25f + ((Time.time - timeStamp) / electricAoeTimeBetweenFeedbackAndCast) / 2;
            chargeIndicator.transform.GetChild(0).gameObject.GetComponent<Projector>().material.color = tempColor;
            yield return new WaitForEndOfFrame();
        }

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

        yield return new WaitForSeconds(0.7f);//wait for the end animation

        nextAttack = Time.time + Random.Range(minWaitTime, maxWaitTime);
        isAttacking = false;
    }

    //======================================================================================== ELECTRIC AOE

    public IEnumerator ElectricAOECoroutine()
    {
        isAttacking = true;

        //start chaneling anim
        Debug.Log("channeling AOE zone");
        anim.SetTrigger("Electricity");
        //wait 75% of the cast time
        yield return new WaitForSeconds(2.8f);

        //show indicator feedback
        //instanciate the circle indicator
        GameObject circleIndicator = Instantiate(aoeCircleProjector, transform.position, Quaternion.identity) as GameObject;
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

    #endregion

    public void TakeDamage(int damage)
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
            }
            StopAllCoroutines();
            Destroy(this.gameObject);
        }

    }

    public IEnumerator Stun()
    {
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
