using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class EnemyShot : MonoBehaviour
{
    private Transform target;
    private float speed;
    private Vector3 shootDirection;
    private int damage;

    // Start is called before the first frame update
    void Start()
    {
    }

    void FixedUpdate()
    {
        //this.GetComponent<Rigidbody>().AddForce(shootDirection * speed);
        this.GetComponent<Rigidbody>().velocity = shootDirection * speed;
    }

    public void Initialise(Transform _target, int _damage, float _speed)
    {
        target = _target;
        damage = _damage;
        speed = _speed;
        shootDirection = (target.position - transform.position).normalized;
		transform.position += Vector3.up;
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
