using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MysticLine : MonoBehaviour
{
    public int damage;
    // Start is called before the first frame update
    void Start()
    {
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            print("Collision");

            //Vector3 knockDirection = transform.localEulerAngles + new Vector3(0, 90, 0); 
            if (collision.gameObject.CompareTag("Player"))
            {

                Instantiate(new GameObject(), collision.transform.position, Quaternion.identity);
                GameManager.gameManager.TakeDamage(collision.gameObject, damage, collision.transform.position, true);
            }
        }

    }
}
