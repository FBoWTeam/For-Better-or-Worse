using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MysticLine : MonoBehaviour
{
    [HideInInspector]
    public int damage;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                GameManager.gameManager.TakeDamage(collision.gameObject, damage, collision.contacts[0].point, true);
            }
        }

    }
}
