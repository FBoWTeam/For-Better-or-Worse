using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class FireBall : MonoBehaviour
{
    private Rigidbody body;
    public int damage;
    public float rangeExplosion;
    public int fireBallHeight;

    public bool isDestroyed;

    float gravity = -9.81f;




    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Awake()
    {
        body = GetComponent<Rigidbody>();
        isDestroyed = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void Launch(Vector3 target, Vector3 fireBallStartingPoint)
    {
        body.useGravity = true;
        body.velocity = ComputeThrowVelocity(target, fireBallStartingPoint).Item1;
        StartCoroutine(TravelTimeCoroutine(ComputeThrowVelocity(target, fireBallStartingPoint).Item2));
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
        float time = Mathf.Sqrt(-2 * fireBallHeight / gravity) + Mathf.Sqrt(2 * (dirY - fireBallHeight) / gravity);
        Vector3 velocityY = Vector3.up * Mathf.Sqrt(-2 * gravity * fireBallHeight);
        Vector3 velocityXZ = dirXZ / time;
        Vector3 velocity = velocityXZ + velocityY * -Mathf.Sign(gravity);
        return new Tuple<Vector3, float>(velocity, time);
    }

    IEnumerator TravelTimeCoroutine(float travelTime)
    {
        yield return new WaitForSeconds(travelTime);

        if ((transform.position - GameManager.gameManager.player1.transform.position).magnitude < rangeExplosion)
        {
            GameManager.gameManager.TakeDamage(GameManager.gameManager.player1, damage, transform.position, true);
        }

        if ((transform.position - GameManager.gameManager.player2.transform.position).magnitude < rangeExplosion)
        {
            GameManager.gameManager.TakeDamage(GameManager.gameManager.player2, damage, transform.position, true);
        }

        isDestroyed = true;
        Destroy(gameObject);
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            GameManager.gameManager.TakeDamage(other.gameObject, damage, transform.position, true);
            StopAllCoroutines();
            isDestroyed = true;
            Destroy(gameObject);
        }
    }
}
