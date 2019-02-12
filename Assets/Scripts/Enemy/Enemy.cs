using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(NavMeshAgent))]
public class Enemy : MonoBehaviour
{
	#region All Variables

	public int hp;

    #region Debug Variables
    public bool debug;
    [DrawIf(new string[] { "debug" }, true)]
    public bool drawPath = false;
    #endregion

    #region Movement Variables
    public enum Movement
    {
        Immobile,
        Classic,
    };
    [Header("[Movement]")]
    public Movement movement;

    [DrawIf(new string[] { "movement" }, Movement.Classic)]
    public float speed = 2f;

    [DrawIf(new string[] { "movement" }, Movement.Classic)]
    [Tooltip("represents the time of the attack animation")]
    public float stopTime = 2f;

    [DrawIf(new string[] { "movement" }, Movement.Classic)]
    [Tooltip("represents the remaining distance between the enemy and the player")]
    public float distanceBetweenPlayer = 5f;

    private LineRenderer line;
    #endregion

    #region Skills Variables
    public enum Skill
    {
        Impact,
        AOE,
        Distance,
        Bloc,
        MudThrow,
        Vortex,
        Inverse,
        Mentaliste,
        Shield,
        PreciousWater,
        Rooting,
        Silence,
        Magnet,
        None,
    };
    [Header("[Skills]")]
    public Skill skillOne;
    public Skill skillTwo;

    // Est-ce que les skill font des damages différentes
    // si oui...
    //TODO Cacher/Afficher les variable de dam
    #region ImpactFields
    [DrawIf(new string[] { "skillOne", "skillTwo" }, Skill.Impact)]
    public int impactDamage = 5;
    #endregion

    #region AoeFields
    [DrawIf(new string[] { "skillOne", "skillTwo" }, Skill.AOE)]
    public float aoeRange = 5f;
    [DrawIf(new string[] { "skillOne", "skillTwo" }, Skill.AOE)]
    public int aeoDamage = 5;
    #endregion

    #region DistanceFields
    [DrawIf(new string[] { "skillOne", "skillTwo" }, Skill.Distance)]
    public float distanceRange = 5f;

    [DrawIf(new string[] { "skillOne", "skillTwo" }, Skill.Distance)]
    public int distanceDamage = 3;

    [DrawIf(new string[] { "skillOne", "skillTwo" }, Skill.Distance)]
    public float fireRate = 1f;

    [DrawIf(new string[] { "skillOne", "skillTwo" }, Skill.Distance)]
    public float turnSpeed = 6.5f;// Rotation Speed


    private float fireCountdown = 0f;

    //public Transform partToRotate;

    [DrawIf(new string[] { "skillOne", "skillTwo" }, Skill.Distance)]
    public GameObject bulletPrefab;

    [DrawIf(new string[] { "skillOne", "skillTwo" }, Skill.Distance)]
    public Transform firePoint;
    #endregion
    #endregion

    #region Bonus Variables
    public enum Bonus
    {
        Mirror,
        None,
    };
    [Header("[Bonus]")]
    public Bonus bonusOne;
    public Bonus bonusTwo;
    #endregion


    private bool coliding = false;
    private NavMeshAgent agent;
    private GameObject[] players;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        agent = this.GetComponent<NavMeshAgent>();
        agent.speed = speed;
        players = GameObject.FindGameObjectsWithTag("Player");
        line = this.GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
		if (hp <= 0)
		{
			StopAllCoroutines();
			Destroy(this.gameObject);
		}

        DoSkill(skillOne);
        DoSkill(skillTwo);

        DoMovement(movement);

        Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 5, Color.yellow);
        //Debug.DrawRay(transform.position, transform.forward, Color.green);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            coliding = true;
        }
    }

    #region Skill
    /// <summary>
    /// Activate the skill passed in parameters
    /// </summary>
    /// <param name="skill"></param>
    void DoSkill(Skill skill)
    {

        switch (skill)
        {
            case Skill.Impact:

                //dégat a l'impact
                if (coliding)
                {
                    GameManager.gameManager.takeDamage(impactDamage);
                }
                break;
            case Skill.AOE:
                AoeDamage(transform.position, aoeRange, aeoDamage);
                break;
            case Skill.Distance:

                GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
                //recup le plus porhce
                GameObject nearest = GetNearestGO(players);
                float distanceto = Vector3.Distance(transform.position, nearest.transform.position);

                // if inRange && isVisible

                if (nearest != null && (distanceto <= distanceRange) && isVisible(transform.position, nearest.transform.position))
                {

                    Vector3 dir = nearest.transform.position - transform.position;
                    Quaternion lookRotation = Quaternion.LookRotation(dir);
                    Vector3 rotation = Quaternion.Lerp(transform.rotation, lookRotation, Time.deltaTime * turnSpeed).eulerAngles; //tourner l'ennemy vers le jouer

                    //Vector3 rotation = Quaternion.Lerp(partToRotate.rotation, lookRotation, Time.deltaTime * turnSpeed).eulerAngles; // tourne le gun/arc/arme de l'ennemi 
                    //partToRotate.rotation = Quaternion.Euler(0f, rotation.y, 0f); // idem


                    //1 Throw projectile
                    // 2 check if projectile hits
                    //   2.1 do domage

                    if (fireCountdown <= 0f)
                    {//active le tir si le countdown avant de tirer est inférieur ou égal à 0

                        Shoot(bulletPrefab, firePoint, nearest.transform, distanceDamage);
                        fireCountdown = 1 / fireRate;//réinitialise le countdown
                    }

                    fireCountdown -= Time.deltaTime;

                }
                break;
            case Skill.Bloc:
                break;
            case Skill.MudThrow:
                break;
            case Skill.Vortex:
                break;
            case Skill.Inverse:
                break;
            case Skill.Mentaliste:
                break;
            case Skill.Shield:
                break;
            case Skill.PreciousWater:
                break;
            case Skill.Rooting:
                break;
            case Skill.Silence:
                break;
            case Skill.Magnet:
                break;
            default:
                break;
        }
    }
    #endregion


    #region Movement

    void DoMovement(Movement movement)
    {
        switch (movement)
        {
            case Movement.Immobile:
                this.transform.LookAt(players[0].transform);
                break;
            case Movement.Classic:
                StartCoroutine(ClassicMovement());
                break;
            default:
                break;
        }

        if (drawPath)
        {
            line.enabled = true;
            DrawPath(agent.path);
        }
        else
        {
            line.enabled = false;
        }
    }

    IEnumerator ClassicMovement()
    {
        agent.isStopped = false;
        agent.destination = players[0].transform.position;

        if (agent.remainingDistance <= distanceBetweenPlayer)
        {
            agent.isStopped = true;
            yield return new WaitForSeconds(stopTime);
        }
    }

    void DrawPath(NavMeshPath path)
    {
        if (path.corners.Length < 2) //if the path has 1 or no corners, there is no need
        {
            return;
        }
        line.SetPositions(path.corners); //set the array of positions to the amount of corners
    }

    #endregion


    /// <summary>
    /// do damage to all gameObject inside a sphereCollider of center in radius
    /// </summary>
    /// <param name="center"></param>
    /// <param name="radius"></param>
    void AoeDamage(Vector3 center, float radius, int damage)
    {
        Collider[] hits = Physics.OverlapSphere(center, radius);
        foreach (Collider item in hits)
        {

            if (item.CompareTag("Player"))
            {
                GameManager.gameManager.takeDamage(damage);
            }
        }
    }

    GameObject GetNearestGO(GameObject[] gos)
    {
        float min = Mathf.Infinity;
        GameObject nearest = null;
        foreach (var item in gos)
        {
            float dist = Vector3.Distance(transform.position, item.transform.position);
            if (dist < min)
            {
                min = dist;
                nearest = item.gameObject;
            }
        }
        return nearest;
    }

    /// <summary>
    /// Shoot a bulletPrefab from firePoint to target in order to do damage onHit
    /// </summary>
    /// <param name="bulletPrefab"></param>
    /// <param name="firePoint"></param>
    /// <param name="target"></param>
    /// <param name="damage"></param>
    void Shoot(GameObject bulletPrefab, Transform firePoint, Transform target, int damage)
    {
        GameObject bulletGO = (GameObject)Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Bullet bullet = bulletGO.GetComponent<Bullet>();//instancie un objet bullet à partir du prefab défini

        if (bullet != null)
        {
            bullet.Seek(target, damage);//
        }
    }

    /// <summary>
    /// return true if a Player is "visible"
    /// </summary>
    /// <param name="start"></param>
    /// <param name="end"></param>
    /// <returns></returns>
    bool isVisible(Vector3 start, Vector3 end)
    {
        RaycastHit hitInfo;
        if (Physics.Linecast(start, end, out hitInfo))
        {
            if (hitInfo.transform.CompareTag("Player"))
            {
                return true;
            }
        }
        return false;
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        switch (skillTwo)
        {
            case Skill.Impact:
                break;
            case Skill.AOE:
                Gizmos.DrawWireSphere(transform.position, aoeRange);
                break;
            case Skill.Distance:
                Gizmos.DrawLine(transform.position, new Vector3(-17.3f, 1.0f, 0));
                Gizmos.DrawWireSphere(transform.position, distanceRange);
                break;
            case Skill.Bloc:
                break;
            case Skill.MudThrow:
                break;
            case Skill.Vortex:
                break;
            case Skill.Inverse:
                break;
            case Skill.Mentaliste:
                break;
            case Skill.Shield:
                break;
            case Skill.PreciousWater:
                break;
            case Skill.Rooting:
                break;
            case Skill.Silence:
                break;
            case Skill.Magnet:
                break;
            case Skill.None:
                break;
            default:
                break;
        }
    }

}
