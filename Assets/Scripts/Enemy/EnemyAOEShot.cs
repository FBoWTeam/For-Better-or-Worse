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
    private Collider collider;
    private GameObject puddle;

    private void Awake() {
        body = GetComponent<Rigidbody>();
        body.useGravity = false;
        collider = GetComponent<SphereCollider>();
        Players = new GameObject[2] {GameManager.gameManager.player1,GameManager.gameManager.player2 };
       
    }


    public void Launch(Vector3 velocity) {
        body.useGravity = true;
        body.velocity = velocity;
        collider.isTrigger = false;
   
    }

    public void Init(float radius,float duration,int _damage, GameObject puddleprefab) {
        effectRadius = radius;
        effectDuration = duration;
        damagePerS = _damage;
        puddle = puddleprefab;     
    }

    private void OnCollisionEnter(Collision collision) {
        if (collision.transform.CompareTag("Floor")) {
            // start aplying damge in the zone
            isGrounded = true;        
            body.constraints = RigidbodyConstraints.FreezeAll;
            GetComponent<MeshRenderer>().enabled = false;

            GameObject pdle = Instantiate(puddle,transform.position,Quaternion.identity,transform);
            pdle.transform.localScale = new Vector3(effectRadius*2, pdle.transform.localScale.y, 2*effectRadius);
           
            StartCoroutine(Duration());         

        }
    }

    

    IEnumerator Duration() {
        yield return new WaitForSeconds(effectDuration);
        StopAllCoroutines();
        Destroy(gameObject);
    }

    
}

