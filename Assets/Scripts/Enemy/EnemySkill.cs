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
		RangedAOE,
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

	public Skill skill;

	#region ImpactFields
	[DrawIf(new string[] { "skill" }, Skill.Impact)]
	public float impactCooldown;
	[DrawIf(new string[] { "skill" }, Skill.Impact)]
	public GameObject hitter;
	[DrawIf(new string[] { "skill" }, Skill.Impact)]
	public GameObject SwordEffect;
	//private float timeAnimation = 1f;
	//[DrawIf(new string[] { "stopAfterHit" }, true)]
	//public float timeImoAfterHit;
	#endregion

	#region AoeFields
	[DrawIf(new string[] { "skill" }, Skill.AOE)]
	public float aoeCooldown = 2f;
	#endregion

	#region RangedFields
	[DrawIf(new string[] { "skill" }, Skill.Ranged)]
	public float fireRate = 1f;
	//public float turnSpeed = 6.5f;// Servira a tourner le joueur en direction de la target plus tard 
	//public Transform partToRotate;
	[DrawIf(new string[] { "skill" }, Skill.Ranged)]
	public float bulletSpeed = 70f;
	[DrawIf(new string[] { "skill" }, Skill.Ranged)]
	public GameObject bulletPrefab;
    //[DrawIf(new string[] { "skill" }, Skill.Ranged)]
    //public Transform firePoint; set to the transform postion for now , changed later
    #endregion

    #region RangedAOE
    [DrawIf(new string[] { "skill" }, Skill.RangedAOE)]
    public bool drawPath = false;
    [DrawIf(new string[] { "skill" }, Skill.RangedAOE)]
    public bool anticipateTarget = false;
    [DrawIf(new string[] { "anticipateTarget" },true)]
    public float anticpationValue = 5f; // (>0 = l'ennemy visera DEVANT || < 0 l'enemy visera DERRIERE) ,  la target avec une anticpation de "anticpationValue" unit 
    [DrawIf(new string[] { "skill" }, Skill.RangedAOE)]
	public GameObject aoeProjectilePrefab;
	[DrawIf(new string[] { "skill" }, Skill.RangedAOE)]
	public float projectileAoeRadius = 5f;
	[DrawIf(new string[] { "skill" }, Skill.RangedAOE)]
	public float projectileAoeDuration = 5f;
    //like fire rate but the name is already used
    [DrawIf(new string[] { "skill" }, Skill.RangedAOE)]
	public float throwRate = 1f;
	[DrawIf(new string[] { "skill" }, Skill.RangedAOE)]
    public float maxHeight = 10; //[Range(0.1f,100f)]
    [DrawIf(new string[] { "skill" }, Skill.RangedAOE)]
    public GameObject puddle;
    #endregion

    #region Root
    [DrawIf(new string[] { "skill" }, Skill.Root)]
	public float rootCooldown;
	[DrawIf(new string[] { "skill" }, Skill.Root)]
	public float castingTime;
	[DrawIf(new string[] { "skill" }, Skill.Root)]
	public float rootTime;
	[DrawIf(new string[] { "skill" }, Skill.Root)]
	public GameObject rootBranchPrefab;
    [DrawIf(new string[] { "skill" }, Skill.Root)]
    public Coroutine rootCoroutine;
    [DrawIf(new string[] { "skill" }, Skill.Root)]
    public bool isCasting;
    #endregion

    public float range = 4f;
	public bool isInRange;
	public int damage = 5;
	float nextAttack = 0f;
    float gravity = -9.81f;
    public Transform shotPosition;

    #endregion


    //Dammage player on collision
    private void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.CompareTag("Player"))
		{
			GameManager.gameManager.TakeDamage(collision.gameObject, damage, transform.position, true);
			GameManager.gameManager.UIManager.QuoteOnDamage("enemy", collision.gameObject);
		}
	}

	/// <summary>
	/// Return trun if a target enter in range
	/// </summary>
	/// <param name="target"></param>
	/// <returns></returns>
	public bool InRange(GameObject target)
	{
		float dist = Vector3.Distance(transform.position, target.transform.position);
		isInRange = false;
		if (dist < range)
		{
			isInRange = true;
		}
		return isInRange;
	}

	public void DoSkill(GameObject target)
	{
        //SET target = target
        if (Time.time > nextAttack)
		{
			switch (skill)
			{
				case Skill.Impact:
					StartCoroutine(Impact());
					nextAttack = Time.time + impactCooldown;
					break;
				case Skill.AOE:
					//DOT while in range
					GameManager.gameManager.TakeDamage(target, damage, transform.position, true);
					GameManager.gameManager.UIManager.QuoteOnDamage("enemy", target);
					nextAttack = Time.time + aoeCooldown;
				break;
			    case Skill.Ranged:
				    if (isVisible(transform.position, target.transform.position))
				    {
                        StartCoroutine(Shoot(bulletPrefab, shotPosition, target.transform, damage));

                        nextAttack = Time.time + fireRate;
				    }
				    break;
			    case Skill.RangedAOE:
                    Vector3 pos = new Vector3(transform.position.x, transform.position.y + 1, transform.position.z); // fix pivot
                    Vector3 newTarget = target.transform.position;
                    if (anticipateTarget) {
                        newTarget = newTarget + (target.transform.forward * anticpationValue);
                    }
                    if (drawPath) {
                        DrawThrowPath(ComputeThrowVelocity(newTarget, pos), pos, Color.green);
                    }

                    Throw(aoeProjectilePrefab, pos, newTarget, damage, puddle);
                    nextAttack = Time.time + throwRate;
                    break;
				case Skill.Root:
                    rootCoroutine = target.GetComponent<PlayerController>().StartCoroutine(target.GetComponent<PlayerController>().RootCoroutine(GetComponent<EnemySkill>(), GetComponent<EnemyMovement>(), castingTime, target, damage, transform.position, rootTime, rootBranchPrefab));
                    nextAttack = Time.time + rootCooldown;
					break;
				case Skill.None:
					break;
				default:
					break;
			}
			GetComponent<Animator>().SetTrigger("Attack");
		}
	}

	void Throw(GameObject aoeProjectile, Vector3 firePoint, Vector3 target, int damage, GameObject puddleprefab)
	{
       
		GameObject projectile = Instantiate(aoeProjectile, firePoint, Quaternion.identity);
		EnemyAOEShot enemyShot = projectile.GetComponent<EnemyAOEShot>();
		
		if (enemyShot != null)
		{
			enemyShot.Init(projectileAoeRadius, projectileAoeDuration, damage,puddleprefab);
            // !! si les LD veulent changer la vitesse passer la variable gravity a public 
            Physics.gravity = Vector3.up * gravity;
			enemyShot.Launch(ComputeThrowVelocity(target,firePoint).Item1);
		}
	}
    
    /// <summary>
    /// Compute at wich velocity a gameobjet should go , in order to hit a target using Kinematic equation
    /// MaxHeight should always be > dirY
    /// </summary>
    /// <param name="target"></param>
    /// <returns></returns>
    Tuple<Vector3,float> ComputeThrowVelocity(Vector3 target, Vector3 initialPos)	{
		float dirY = target.y - initialPos.y;
        Vector3 dirXZ = new Vector3(target.x - initialPos.x, 0, target.z - initialPos.z);
		float time = Mathf.Sqrt(-2 * maxHeight / gravity) + Mathf.Sqrt(2 * (dirY - maxHeight) / gravity);
		Vector3 velocityY = Vector3.up * Mathf.Sqrt(-2 * gravity * maxHeight);
		Vector3 velocityXZ = dirXZ / time ;
        Vector3 velocity = velocityXZ + velocityY * -Mathf.Sign(gravity);
        return new Tuple<Vector3, float>( velocity,time);
	}

    /// <summary>
    /// Draw throw path on scene View
    /// </summary>
    /// <param name="info"></param>
    void DrawThrowPath(Tuple<Vector3, float> info,Vector3 init,Color color) {
        int res = 60;
        Vector3 prev = init;
        for (int i = 0; i < res; i++) {
            float s = i / (float)res * info.Item2;
            Vector3 disp = (info.Item1 * s) + Vector3.up * (gravity * s * s / 2f);
            Vector3 drawp = init + disp;
            Debug.DrawLine(prev, drawp, color,throwRate);
            prev = drawp;
        }
    }

    IEnumerator Impact()
	{
		GetComponent<EnemyMovement>().agent.isStopped = true;
		hitter.SetActive(true);
		hitter.GetComponent<BoxCollider>().size = new Vector3(range, 0.1f, 0.1f);
		hitter.GetComponent<BoxCollider>().center = new Vector3((-range)/2.0f, 0.0f, 0.0f);
		SwordEffect.SetActive(true);
		yield return new WaitForSeconds(GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length);
		hitter.SetActive(false);
		SwordEffect.SetActive(false);
		GetComponent<EnemyMovement>().agent.isStopped = false;
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
		end = new Vector3(end.x, end.y, end.z);

		if (!Physics.Linecast(start, end, ~playerLayer))
		{
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
	IEnumerator Shoot(GameObject projectilePrefab, Transform firePoint, Transform target, int damage)
	{
        yield return new WaitForSeconds(GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length/4);

        GameObject projectile = (GameObject)Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
		EnemyShot enemyShot = projectile.GetComponent<EnemyShot>();

		if (enemyShot != null)
		{
			enemyShot.Initialise(target.position, damage, bulletSpeed);
		}
	}

/*
	/// <summary>
	/// Coroutine for the root skill of the enemy
	/// </summary>
	/// <param name="targetPlayer"></param>
	/// <returns></returns>
	IEnumerator RootCoroutine(GameObject targetPlayer)
	{
		//the enemy doesn't move during the casting time of the root
		GetComponent<EnemyMovement>().agent.isStopped = true;
		yield return new WaitForSecondsRealtime(castingTime);
		GetComponent<EnemyMovement>().agent.isStopped = false;

        StartCoroutine(targetPlayer.GetComponent<PlayerController>().RootCoroutine(GetComponent<EnemyMovement>(), castingTime, targetPlayer, damage, transform.position, rootTime, rootBranchPrefab));        
	}*/
	
	private void OnDrawGizmos()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(transform.position + Vector3.up, range);
	}
	
}
