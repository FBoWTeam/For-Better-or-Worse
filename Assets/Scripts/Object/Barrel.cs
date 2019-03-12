using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrel : MonoBehaviour, IActivable
{
    public bool isActive { get; set; }

    public float rangeAOE;//range de l'explosion
    public int damage;//dégat de l'explosion

    
    // Start is called before the first frame update
    void Start()
    {
        isActive = false;
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Orb") && !isActive && GameManager.gameManager.orb.GetComponent<PowerController>().elementalPower == GameManager.PowerType.Fire)
        {
            this.Activate();
        }
    }

    public void Activate()
    {
        isActive = true;

        Vector3 explosionPos = transform.position;
        Collider[] colliders = Physics.OverlapSphere(explosionPos, rangeAOE);
        foreach (Collider hit in colliders)
        {
            if(hit.CompareTag("Player"))
            {
                Rigidbody rb = hit.GetComponent<Rigidbody>();

                if (rb != null)
                {
                    rb.AddExplosionForce(0, explosionPos, rangeAOE, 0.0F, ForceMode.Impulse);//pas de force tant que le knock back est forcé dans TakeDamage
                    GameManager.gameManager.TakeDamage(hit.gameObject, damage, transform.position);
                }
                  
            }
        }

        Destroy(gameObject);//le baril explose et est donc détruit

    }
}
