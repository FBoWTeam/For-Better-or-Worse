using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(Rigidbody))]
public class EnemyAOEShot: MonoBehaviour {

    private float effectRadius;
    private int damagePerS;
    private Rigidbody body;
    private bool isGrounded = false;
    private float effectDuration;
    private GameObject[] Players;


    private void Awake() {
        body = GetComponent<Rigidbody>();
        body.useGravity = false;

        Players = new GameObject[2] {GameManager.gameManager.player1,GameManager.gameManager.player2 };
       
    }


    public void Launch(Vector3 velocity) {
        body.useGravity = true;
        body.velocity = velocity;
   
    }

    public void Init(float radius,float duration,int _damage) {
        effectRadius = radius;
        effectDuration = duration;
        damagePerS = _damage;
    }

    private void OnCollisionEnter(Collision collision) {
        if (collision.transform.CompareTag("Floor")) {
            // start aplying damge in the zone
            isGrounded = true;
            body.constraints = RigidbodyConstraints.FreezeAll;
            gameObject.GetComponent<Renderer>().material.color = Color.red;
            StartCoroutine(Duration());
            StartCoroutine(AOE());


        }
    }

    IEnumerator AOE() {
        //Apply damage each seconds
        while (true) {
            foreach (GameObject item in Players) {
                float distToPlayer = Vector3.Distance(transform.position, item.transform.position); //Optimisable               
                if (distToPlayer < effectRadius) {
                    
                    GameManager.gameManager.TakeDamage(item, damagePerS, transform.position);
                    GameManager.gameManager.UIManager.QuoteOnDamage("enemy", item);
                }
            }
            yield return new WaitForSeconds(1f);
        }
    }

    IEnumerator Duration() {
        yield return new WaitForSeconds(effectDuration);
        StopAllCoroutines();
        Destroy(this);
    }

    private void OnDrawGizmos() {
        if (isGrounded) {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, effectRadius);
        }
        
    }
}

