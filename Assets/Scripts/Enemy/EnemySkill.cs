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
	public float impactCooldown = 3f;
	[DrawIf(new string[] { "skill" }, Skill.Impact)]
	public float impactSpeed = 1.5f;
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
    //damage = damage/S
    [DrawIf(new string[] { "skill" }, Skill.RangedAOE)]
    public bool drawPath = false;
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
	#endregion

	public float range = 4f;
	public bool isInRange;
	public int damage = 5;
	float nextAttack = 0f;
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
		switch (skill)
		{
			case Skill.Impact:
				if (Time.time > nextAttack)
				{
					StartCoroutine("Impact", target.transform);
					GetComponent<Animator>().SetTrigger("Attack");
					nextAttack = Time.time + impactCooldown;
				}
				break;
			case Skill.AOE:
				//DOT while in range
				if (Time.time > nextAttack)
				{
					GameManager.gameManager.TakeDamage(target, damage, transform.position, true);
					GameManager.gameManager.UIManager.QuoteOnDamage("enemy", target);
					GetComponent<Animator>().SetTrigger("Attack");
					nextAttack = Time.time + aoeCooldown;
				}
				break;
			case Skill.Ranged:
				if (Time.time > nextAttack && isVisible(transform.position, target.transform.position))
				{
                    print("TARGET :" + target.transform.position);
                    Shoot(bulletPrefab, transform, target.transform, damage);
					GetComponent<Animator>().SetTrigger("Attack");
					nextAttack = Time.time + fireRate;
				}
				break;
			case Skill.RangedAOE:

                if (drawPath) {
                    DrawThrowPath(ComputeThrowVelocity(target.transform.position));
                }
				if (Time.time > nextAttack)
				{				
					Throw(aoeProjectilePrefab, transform, target.transform, damage,puddle);
					GetComponent<Animator>().SetTrigger("Attack");
					nextAttack = Time.time + throwRate;
				}
				break;
			case Skill.Root:
				if (Time.time > nextAttack)
				{
                    target.GetComponent<PlayerController>().StartRoot(GetComponent<EnemyMovement>(), castingTime, target, damage, transform.position, rootTime, rootBranchPrefab);
					GetComponent<Animator>().SetTrigger("Attack");
					nextAttack = Time.time + rootCooldown;
				}
				break;
			case Skill.None:
				break;
			default:
				break;
		}
	}

	void Throw(GameObject aoeProjectile, Transform firePoint, Transform target, int damage, GameObject puddleprefab)
	{
        Vector3 pos = new Vector3(firePoint.position.x, firePoint.position.y + 1, firePoint.position.z); // fix pivot
		GameObject projectile = Instantiate(aoeProjectile, pos, firePoint.rotation);
		EnemyAOEShot enemyShot = projectile.GetComponent<EnemyAOEShot>();
		
		if (enemyShot != null)
		{
			enemyShot.Init(projectileAoeRadius, projectileAoeDuration, damage,puddleprefab);
            
			enemyShot.Launch(ComputeThrowVelocity(target.position).Item1);
		}
	}

	/// <summary>
	/// Compute at wich velocity a gameobjet should go , in order to hit a target using Kinematic equation
	/// MaxHeight should always be > dirY
	/// </summary>
	/// <param name="target"></param>
	/// <returns></returns>
	Tuple<Vector3,float> ComputeThrowVelocity(Vector3 target)
	{
        
		float dirY = target.y - transform.position.y;

        Vector3 dirXZ = new Vector3(target.x - transform.position.x, 0, target.z - transform.position.z);
		float time = Mathf.Sqrt(-2 * maxHeight / Physics.gravity.y) + Mathf.Sqrt(2 * (dirY - maxHeight) / Physics.gravity.y);
		Vector3 velocityY = Vector3.up * Mathf.Sqrt(-2 * Physics.gravity.y * maxHeight);
		Vector3 velocityXZ = dirXZ / time;

        Vector3 velocity = velocityXZ + velocityY * -Mathf.Sign(Physics.gravity.y);       
		return new Tuple<Vector3, float>( velocity,time);
	}

    /// <summary>
    /// Draw throw path on scene View
    /// </summary>
    /// <param name="info"></param>
    void DrawThrowPath(Tuple<Vector3, float> info) {
        int res = 30;
        Vector3 prev = transform.position;
        for (int i = 0; i < res; i++) {
            float s = i / (float)res * info.Item2;
            Vector3 disp = (info.Item1 * s) + Vector3.up * (Physics.gravity.y * s * s / 2f);
            Vector3 drawp = transform.position + disp;
            Debug.DrawLine(prev, drawp, Color.green);
            prev = drawp;
        }
    }


    IEnumerator Impact(Transform target)
	{

		Vector3 originalPosition = transform.position;
		Vector3 targetPos = new Vector3(target.position.x, target.position.y, target.position.z);// PIVOT DE ....
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
	void Shoot(GameObject projectilePrefab, Transform firePoint, Transform target, int damage)
	{
		GameObject projectile = (GameObject)Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
		EnemyShot enemyShot = projectile.GetComponent<EnemyShot>();

		if (enemyShot != null)
		{
			enemyShot.Initialise(target, damage, bulletSpeed);
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
