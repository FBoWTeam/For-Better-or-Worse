using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;


public class EnemySkill : MonoBehaviour
{
    #region Skill Variables
    public enum Skill
    {
        Impact,
        AOE,
        Ranged,
        Bloc,
        MudThrowing,
        Vortex,
        Inversion,
        Mentalist,
        Shield,
        HolyWater,
        Root,
        Silence,
        Magnet,
        None,
    };

    public Skill skillOne;
    //public Skill skillTwo;

    #region ImpactFields
    //[DrawIf(new string[] { "skillOne"}, Skill.Impact)]
    //public int impactDamage = 5;
    [DrawIf(new string[] { "skillOne" }, Skill.Impact)]
    public float impactCooldown = 3f;
    [DrawIf(new string[] { "skillOne" }, Skill.Impact)]
    public float impactSpeed = 1.5f;
    #endregion

    #region AoeFields
    [DrawIf(new string[] { "skillOne" }, Skill.AOE)]
    public float aoeCooldown = 2f;
    #endregion

    #region RangedFields

    //[DrawIf(new string[] { "skillOne" }, Skill.Distance)]
    //public int distanceDamage = 3;

    [DrawIf(new string[] { "skillOne" }, Skill.Ranged)]
    public float fireRate = 1f;

    //[DrawIf(new string[] { "skillOne" }, Skill.Ranged)]
    //private float fireCountdown = 0f;
    //public float turnSpeed = 6.5f;// Servira a tourner le joueur en direction de la target plus tard 
    //public Transform partToRotate;

    [DrawIf(new string[] { "skillOne" }, Skill.Ranged)]
    public float bulletSpeed = 70f;
    [DrawIf(new string[] { "skillOne" }, Skill.Ranged)]
    public GameObject bulletPrefab;

    //[DrawIf(new string[] { "skillOne" }, Skill.Ranged)]
    //public Transform firePoint; set to the transform postion for now , changed later
    #endregion

    public float range = 4f;
    public int damage = 5;

    //evenement avec aucun type de retour mais 1 parametre 
    public event Action<GameObject, Skill> inRangeEvent;

    Material myMat;
    float nextAttack = 0f;
    SphereCollider rangeCollider;

    #endregion
   

    private void Awake()
    {
        //set the sphereCollider to trigger just to be sure
        rangeCollider = transform.GetChild(1).GetComponent<SphereCollider>();
        if (!rangeCollider.isTrigger)
        {
            rangeCollider.isTrigger = true;
        }
        rangeCollider.radius = range;

        inRangeEvent += onPlayerinRange;
        myMat = GetComponent<Renderer>().material;
    }

    private void Update()
    {
        rangeCollider.radius = range;
    }

    //Dammage player on collision
    private void OnCollisionEnter(Collision collision)
    {
        if (skillOne == Skill.Impact)
        {
            if (collision.gameObject.CompareTag("Player") && collision.gameObject == Enemy.aimPlayer)
            {
                GameManager.gameManager.TakeDamage(collision.gameObject, damage, transform.position);
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (other.gameObject == Enemy.aimPlayer || skillOne == Skill.AOE)
            {
                inRangeEvent(other.gameObject, skillOne);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (myMat.color != Color.white)
        {
            myMat.color = Color.white;
        }
    }


    void onPlayerinRange(GameObject target, Skill skill)
    {
        //SET target = target
        switch (skill)
        {
            case Skill.Impact:

                if (Time.time > nextAttack)
                {
                    myMat.color = Color.red;
                    StartCoroutine("Impact", target.transform);

                    nextAttack = Time.time + impactCooldown;
                }

                break;
            case Skill.AOE:
                //DOT while in range
                myMat.color = Color.red;
                if (Time.time > nextAttack)
                {
                    GameManager.gameManager.TakeDamage(target, damage, transform.position);
                    nextAttack = Time.time + aoeCooldown;
                }
                break;
            case Skill.Ranged:
                myMat.color = Color.red;
              
                if (Time.time > nextAttack && isVisible(transform.position, target.transform.position))
                {
                    Shoot(bulletPrefab, transform, target.transform, damage);
                    nextAttack = Time.time + fireRate;
                }
                break;
            case Skill.None:
                break;
            default:
                break;
        }
    }




    IEnumerator Impact(Transform target)
    {

        Vector3 originalPosition = transform.position;
		Vector3 targetPos = new Vector3(target.position.x, target.position.y + 1, target.position.z);
        Vector3 dirToTarget = (targetPos - transform.position).normalized;
        Vector3 attackPosition = targetPos + dirToTarget;
        //Debug.Log(attackPosition);
        float percent = 0;



        while (percent <= 1)
        {

            percent += Time.deltaTime * impactSpeed;
            float interpolation = (-Mathf.Pow(percent, 2) + percent) * 4;
            //Debug.Log(interpolation);
            transform.position = Vector3.Lerp(originalPosition, attackPosition, interpolation);

            yield return null;
        }

        myMat.color = Color.white;

    }




    /// <summary>
    /// return true if a Player is "visible"
    /// Use if a player is in a range zone but potentially behind a obstacle/wall
    /// </summary>
    /// <param name="start"></param>
    /// <param name="end"></param>
    /// <returns></returns>
    bool isVisible(Vector3 start, Vector3 end)
    {

        // This would cast rays only against colliders in Player layer .
        // But instead we want to collide against everything except layer 8. The ~ operator does this, it inverts a bitmask.
        int playerLayer = 1 << LayerMask.NameToLayer("Players");
       
        if (!Physics.Linecast(start, end, ~playerLayer)){
            return true;
            
        }
        return false;
    }

    /// <summary>
    /// Shoot a projectilePrefab from firePoint to target in order to do damage onHit
    /// </summary>
    /// <param name="projectilePrefab"></param>
    /// <param name="firePoint"></param>
    /// <param name="target"></param>
    /// <param name="damage"></param>
    void Shoot(GameObject projectilePrefab, Transform firePoint, Transform target, int damage)
    {
        GameObject projectile = (GameObject)Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
        //Bullet bullet = bulletGO.GetComponent<Bullet>();
        EnemyShot enemyShot = projectile.GetComponent<EnemyShot>();

        //if (bullet != null)
        //{
        //    bullet.Seek(target, damage, bulletSpeed);
        //}

        if (enemyShot != null)
        {
            enemyShot.Initialise(target, damage, bulletSpeed);
        }
    }



}
