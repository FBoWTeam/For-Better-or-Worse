using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class FireBall : MonoBehaviour
{
    private Rigidbody body;
    int damage;
    int damageExplosion;
    float rangeExplosion;
    public int fireBallHeight;
    public GameObject explosion;

    public bool isDestroyed;
    public bool willBeDestroyed;

    float gravity = 35f;//-9.81f;




    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Awake()
    {
        body = GetComponent<Rigidbody>();
        isDestroyed = false;
        willBeDestroyed = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.position.y <= 1f && !willBeDestroyed)//1f
        {
            if ((transform.position - GameManager.gameManager.player1.transform.position).magnitude < rangeExplosion)
            {
                GameManager.gameManager.TakeDamage(GameManager.gameManager.player1, damageExplosion, transform.position, true);
            }

            if ((transform.position - GameManager.gameManager.player2.transform.position).magnitude < rangeExplosion)
            {
                GameManager.gameManager.TakeDamage(GameManager.gameManager.player2, damageExplosion, transform.position, true);
            }

            StartCoroutine(DestroyFireBall());
        }

        if (transform.position.y <= -2f)//1f
        {
            body.velocity = Vector3.zero;
        }
    }

    public void Init(int damageParam, int damageExplosionParam, float rangeExplosionParam, float velocityParam)
    {
        damage = damageParam;
        damageExplosion = damageExplosionParam;
        rangeExplosion = rangeExplosionParam;
        gravity = velocityParam;
    }

    public float Launch(Vector3 target, Vector3 fireBallStartingPoint)
    {
        /*Physics.gravity = Vector3.up * -gravity;
        body.useGravity = true;*/
        body.velocity = ComputeThrowVelocity(target, fireBallStartingPoint).Item1;
        return ComputeThrowVelocity(target, fireBallStartingPoint).Item2;
    }

    /// <summary>
    /// Compute at wich velocity a gameobjet should go , in order to hit a target using Kinematic equation
    /// MaxHeight should always be > dirY
    /// </summary>
    /// <param name="target"></param>
    /// <returns></returns>
    Tuple<Vector3, float> ComputeThrowVelocity(Vector3 target, Vector3 initialPos)
    {
        float dirY = target.y - initialPos.y;
        Vector3 dirXZ = new Vector3(target.x - initialPos.x, 0, target.z - initialPos.z);
        float time = Mathf.Sqrt(2 * fireBallHeight / gravity) + Mathf.Sqrt(-2 * (dirY - fireBallHeight) / gravity);
        Vector3 velocityY = Vector3.up * Mathf.Sqrt(2 * gravity * fireBallHeight);
        Vector3 velocityXZ = dirXZ / time;
        Vector3 velocity = velocityXZ + velocityY * Mathf.Sign(gravity);
        velocity += Vector3.up * -13.5f;
        return new Tuple<Vector3, float>(velocity, time);
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            GameManager.gameManager.TakeDamage(other.gameObject, damage, transform.position, true);
        }
    }
    
    IEnumerator DestroyFireBall()
    {
        willBeDestroyed = true;
        Instantiate(explosion, transform.position, Quaternion.identity);

        yield return new WaitForSeconds(1f);
        isDestroyed = true;
        Destroy(gameObject);
    }
}
