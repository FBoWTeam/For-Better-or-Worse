using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProtectionWall : MonoBehaviour
{
    public enum WallElement
    {
        Aquatic,
        Plant
    }

    public WallElement element;
    public int wallDamage;



    private int pylonDown;


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            GameManager.gameManager.TakeDamage(collision.gameObject, wallDamage, collision.contacts[0].point, true);
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Orb"))
        {
            switch (element)
            {
                case WallElement.Aquatic:
                    if (other.gameObject.GetComponent<PowerController>().elementalPower == GameManager.PowerType.Fire)
                    {
                        other.gameObject.GetComponent<PowerController>().DeactivatePower(GameManager.PowerType.Fire);
                    }
                    break;
                case WallElement.Plant:
                    if (other.gameObject.GetComponent<PowerController>().elementalPower == GameManager.PowerType.Electric)
                    {
                        other.gameObject.GetComponent<PowerController>().DeactivatePower(GameManager.PowerType.Electric);
                    }
                    break;
                default:
                    break;
            }
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
