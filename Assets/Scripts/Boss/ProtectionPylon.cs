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
                    pylonHealth--;
                    if (other.gameObject.GetComponent<PowerController>().elementalPower == GameManager.PowerType.Electric)
                    {
                        pylonHealth--;
                    }
                    break;
                case PylonElement.Plant:
                    pylonHealth--;
                    if (other.gameObject.GetComponent<PowerController>().elementalPower == GameManager.PowerType.Fire)
                    {
                        pylonHealth--;
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
