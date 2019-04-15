using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProtectionWall : MonoBehaviour
{
    public int wallDamage;

    private int pylonDown;


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            GameManager.gameManager.TakeDamage(collision.gameObject, wallDamage, collision.contacts[0].point, true);
        }
    }

    public void DestroyWalls()
    {
        pylonDown++;
        if (pylonDown >= 3)
        {
            Destroy(gameObject);
        }
    }
}
