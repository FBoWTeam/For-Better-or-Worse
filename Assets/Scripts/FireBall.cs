﻿using System.Collections;
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

    public bool isDestroyed;

    float gravity = 35f;//-9.81f;




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
        if(transform.position.y <= 0)
        {
            if ((transform.position - GameManager.gameManager.player1.transform.position).magnitude < rangeExplosion)
            {
                GameManager.gameManager.TakeDamage(GameManager.gameManager.player1, damageExplosion, transform.position, true);
            }

            if ((transform.position - GameManager.gameManager.player2.transform.position).magnitude < rangeExplosion)
            {
                GameManager.gameManager.TakeDamage(GameManager.gameManager.player2, damageExplosion, transform.position, true);
            }

            isDestroyed = true;
            Destroy(gameObject);
        }
    }

    public void Init(int damageParam, int damageExplosionParam, float rangeExplosionParam, float velocityParam)
    {
        damage = damageParam;
        damageExplosion = damageExplosionParam;
        rangeExplosion = rangeExplosionParam;
        gravity = velocityParam;
    }

    public void Launch(Vector3 target, Vector3 fireBallStartingPoint)
    {
        Physics.gravity = Vector3.up * -gravity;
        body.useGravity = true;
        body.velocity = ComputeThrowVelocity(target, fireBallStartingPoint);
    }

    /// <summary>
    /// Compute at wich velocity a gameobjet should go , in order to hit a target using Kinematic equation
    /// MaxHeight should always be > dirY
    /// </summary>
    /// <param name="target"></param>
    /// <returns></returns>
    Vector3 ComputeThrowVelocity(Vector3 target, Vector3 initialPos)
    {
        float dirY = target.y - initialPos.y;
        Vector3 dirXZ = new Vector3(target.x - initialPos.x, 0, target.z - initialPos.z);
        float time = Mathf.Sqrt(2 * fireBallHeight / gravity) + Mathf.Sqrt(-2 * (dirY - fireBallHeight) / gravity);
        Vector3 velocityY = Vector3.up * Mathf.Sqrt(2 * gravity * fireBallHeight);
        Vector3 velocityXZ = dirXZ / time;
        Vector3 velocity = velocityXZ + velocityY * Mathf.Sign(gravity);
        return velocity;
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
