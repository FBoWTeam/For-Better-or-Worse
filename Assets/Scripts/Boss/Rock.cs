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


    IEnumerator Fall()
    {
        while (transform.position.y > 1)
        {
            transform.Translate(Vector3.down * fallSpeed * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }
        Destroy(gameObject);
    }

}
