using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerController : MonoBehaviour
{
    GameManager.PowerType elementalPower;
    GameManager.PowerType behavioralPower;


    public void ActivatePower(GameManager.PowerType powerToActivate)
    {
        if (powerToActivate == GameManager.PowerType.Elemental && elementalPower != GameManager.PowerType.None)
        {
            DeactivatePower(elementalPower);
        }
        else if (powerToActivate == GameManager.PowerType.Behavioral && behavioralPower != GameManager.PowerType.None)
        {
            DeactivatePower(behavioralPower);
        }

        switch (powerToActivate)
        {
            case GameManager.PowerType.LargeOrb:
                break;
            case GameManager.PowerType.Vortex:
                break;
            case GameManager.PowerType.LeechLife:
                break;
            case GameManager.PowerType.Slug:
                break;
            case GameManager.PowerType.Shield:
                break;
            case GameManager.PowerType.Ice:
                break;
            case GameManager.PowerType.Fire:
                break;
            case GameManager.PowerType.Water:
                break;
            case GameManager.PowerType.Electric:
                break;
            case GameManager.PowerType.Weakness:
                break;
            default:
                break;
        }



    }

    public void DeactivatePower(GameManager.PowerType powerToDeactivate)
    {
        switch (powerToDeactivate)
        {
            case GameManager.PowerType.LargeOrb:

                break;
            case GameManager.PowerType.Vortex:
                break;
            case GameManager.PowerType.LeechLife:
                break;
            case GameManager.PowerType.Slug:
                break;
            case GameManager.PowerType.Shield:
                break;
            case GameManager.PowerType.Ice:
                break;
            case GameManager.PowerType.Fire:
                break;
            case GameManager.PowerType.Water:
                break;
            case GameManager.PowerType.Electric:
                break;
            case GameManager.PowerType.Weakness:
                break;
            default:
                break;
        }
    }


}
