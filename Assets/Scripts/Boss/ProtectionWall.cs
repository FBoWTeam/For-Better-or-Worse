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

    public GameObject protectionSystem;

    public WallElement element;
    public int wallDamage;

    private int pylonDown;

	public SoundEmitter soundEmitter;

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
		soundEmitter.PlaySound(0);
		if (pylonDown >= 3)
        {
            if (element == WallElement.Aquatic)
            {
                protectionSystem.GetComponent<ProtectionSystem>().waterWallsDestroyed = true;
                protectionSystem.GetComponent<ProtectionSystem>().checkProtectionSystemStatus();
            }
            else if (element == WallElement.Plant)
            {
                protectionSystem.GetComponent<ProtectionSystem>().plantWallsDestroyed = true;
                protectionSystem.GetComponent<ProtectionSystem>().checkProtectionSystemStatus();
            }
            
            Destroy(gameObject);
        }
    }
}
