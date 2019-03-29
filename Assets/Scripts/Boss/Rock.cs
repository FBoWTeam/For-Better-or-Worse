using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Rock : MonoBehaviour
{
    public float fallSpeed;
    public float spinSpeed;

    
    private void Start()
    {
        StartCoroutine(Fall());
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Boss"))
        {
            collision.gameObject.GetComponent<BossSystem>().StartCoroutine(collision.gameObject.GetComponent<BossSystem>().Stun());
            Destroy(gameObject);
        }
    }


    IEnumerator Fall()
    {
        while (transform.position.y > 1)
        {
            transform.Translate(Vector3.down * fallSpeed * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }
        yield return new WaitForSeconds(1.0f);
    }

}
