using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlammableObjects : MonoBehaviour, IActivable
{
    [Tooltip("time needed for the object to be destroyed when burning")]
    public float burnTime;

    private bool isBurning;

    [HideInInspector]
    public bool isDestroyedByFire = false;

    public bool isActive { get; set; }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Orb"))
        {
            PowerController orbPower = other.gameObject.GetComponent<PowerController>();
            if (orbPower.elementalPower == GameManager.PowerType.Fire)
            {
                if (!isActive)
                {
                    this.Activate();
                }
            }
            else if (isBurning)
            {
                orbPower.ActivatePower(GameManager.PowerType.Fire, "forced");
            }
        }
    }

    public void Activate()
    {
        isActive = true;
        isBurning = true;
		transform.GetChild(0).gameObject.SetActive(true);
        //gameObject.GetComponent<Renderer>().material.color = Color.red;
        StartCoroutine(FireCoroutine());
        Debug.Log("coroutine");
        Destroy(gameObject, burnTime);
    }

    IEnumerator FireCoroutine()
    {
        Debug.Log(burnTime - 0.01f);
        yield return new WaitForSeconds(burnTime - 1f);
        isDestroyedByFire = true;
        Debug.Log("detruit par le feu");
    }
}

