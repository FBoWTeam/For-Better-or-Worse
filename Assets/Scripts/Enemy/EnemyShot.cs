using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class EnemyShot : MonoBehaviour
{
    private Vector3 target;
    private float speed;
    private Vector3 shootDirection;
    private int damage;


    void FixedUpdate()
    {
        this.GetComponent<Rigidbody>().velocity = shootDirection * speed;
        gameObject.transform.LookAt(shootDirection * 1000000);
    }

    public void Initialise(Vector3 _target, int _damage, float _speed)
    {
        target = _target + Vector3.up;
        damage = _damage;
        speed = _speed;
        shootDirection = (new Vector3(target.x, transform.position.y, target.z) - transform.position).normalized;
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Player"))
        {
            GameManager.gameManager.TakeDamage(other.gameObject, damage, transform.position, true);
            GameManager.gameManager.UIManager.QuoteOnDamage("enemy", other.gameObject);
            Destroy(this.gameObject);
        }
        else if (other.CompareTag("Wall"))
        {
            Destroy(this.gameObject);
        }

    }
}
