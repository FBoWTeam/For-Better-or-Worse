using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProtectionPylon : MonoBehaviour
{
    public enum PylonElement
    {
        Aquatic,
        Plant
    }

    public PylonElement pylonType;
    public int pylonHealth;

    public GameObject walls;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Orb"))
        {
            switch (pylonType)
            {
                case PylonElement.Aquatic:
                    if (other.gameObject.GetComponent<PowerController>().elementalPower == GameManager.PowerType.Electric)
                    {
                        pylonHealth--;
                    }
                    else if (other.gameObject.GetComponent<PowerController>().elementalPower == GameManager.PowerType.Fire)
                    {
                        other.gameObject.GetComponent<PowerController>().DeactivatePower(GameManager.PowerType.Fire);
                    }
                    break;
                case PylonElement.Plant:
                    if (other.gameObject.GetComponent<PowerController>().elementalPower == GameManager.PowerType.Fire)
                    {
                        pylonHealth--;
                    }
                    else if (other.gameObject.GetComponent<PowerController>().elementalPower == GameManager.PowerType.Electric)
                    {
                        other.gameObject.GetComponent<PowerController>().DeactivatePower(GameManager.PowerType.Electric);
                    }
                    break;
            }

        }
        checkHealth();
    }

    void checkHealth()
    {
        if (pylonHealth <= 0)
        {
            walls.GetComponent<ProtectionWall>().DestroyWalls();
            Destroy(gameObject);
        }
    }


}
