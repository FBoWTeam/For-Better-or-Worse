using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrel : MonoBehaviour, IActivable
{
    public bool isActive { get; set; }

    public float rangeAOE;//explosion range
    public int damage;//explosion damage
    public float powerForce;
    public float TimeBeforeActivation;

    public GameObject explosionEffect;

	public SoundEmitter soundEmitter;

    // Start is called before the first frame update
    void Start()
    {
        isActive = false;        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Orb") && !isActive && GameManager.gameManager.orb.GetComponent<PowerController>().elementalPower == GameManager.PowerType.Fire)
        {
            StartCoroutine(TimeBeforeActivationCoroutine());
        }
    }

    IEnumerator TimeBeforeActivationCoroutine()
	{
		soundEmitter.PlaySound(0);
		transform.GetChild(0).gameObject.SetActive(true);
        yield return new WaitForSeconds(TimeBeforeActivation);

        this.Activate();
    }

    public void Activate()
    {
		soundEmitter.PlaySound(1);
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
                    rb.AddExplosionForce(powerForce, explosionPos, rangeAOE, 0.0F, ForceMode.Impulse);
                    GameManager.gameManager.TakeDamage(hit.gameObject, damage, transform.position, false);
                }
                  
            }

            if(hit.CompareTag("Enemy"))
            {
                Rigidbody rb = hit.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.AddExplosionForce(powerForce, explosionPos, rangeAOE, 0.0F, ForceMode.Impulse );
                    hit.GetComponent<Enemy>().TakeDamage(damage);
                }
            }
        }
        Instantiate(explosionEffect, transform.position + Vector3.up, Quaternion.Euler(new Vector3(90, 0, 0)));
        Destroy(gameObject);

    }
}