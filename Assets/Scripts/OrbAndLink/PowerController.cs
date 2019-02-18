using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerController : MonoBehaviour
{
    public GameManager.PowerType elementalPower;
    public GameManager.PowerType behavioralPower;

	/// <summary>
	/// Activate the powerToActivate, deactivate the power of the same type if there's already an active one
	/// </summary>
	/// <param name="powerToActivate"></param>
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
				ActivateLargeOrb();
                break;
            case GameManager.PowerType.Vortex:
				ActivateVortex();
                break;
            case GameManager.PowerType.LeechLife:
				ActivateLeechLife();
                break;
            case GameManager.PowerType.Slug:
				ActivateSlug();
                break;
            case GameManager.PowerType.Shield:
				ActivateShield();
                break;
            case GameManager.PowerType.Ice:
				ActivateIce();
                break;
            case GameManager.PowerType.Fire:
				ActivateFire();
                break;
            case GameManager.PowerType.Water:
				ActivateWater();
                break;
            case GameManager.PowerType.Electric:
				ActivateElectric();
                break;
            case GameManager.PowerType.Weakness:
				ActivateWeakness();
                break;
        }
    }

	/// <summary>
	/// Deactivate the powerToDeactivate
	/// </summary>
	/// <param name="powerToDeactivate"></param>
    public void DeactivatePower(GameManager.PowerType powerToDeactivate)
    {
        switch (powerToDeactivate)
        {
            case GameManager.PowerType.LargeOrb:
				DeactivateLargeOrb();
                break;
            case GameManager.PowerType.Vortex:
				DeactivateVortex();
                break;
            case GameManager.PowerType.LeechLife:
				DeactivateLeechLife();
                break;
            case GameManager.PowerType.Slug:
				DeactivateSlug();
                break;
            case GameManager.PowerType.Shield:
				DeactivateShield();
                break;
            case GameManager.PowerType.Ice:
				DeactivateIce();
                break;
            case GameManager.PowerType.Fire:
				DeactivateFire();
                break;
            case GameManager.PowerType.Water:
				DeactivateWater();
                break;
            case GameManager.PowerType.Electric:
				DeactivateElectric();
                break;
            case GameManager.PowerType.Weakness:
				DeactivateWeakness();
                break;
            default:
                break;
        }
    }

	//==========BEHAVIORAL POWERS FUNCTIONS==========

	#region LargeOrb
	//==========LARGEORB==========

	void ActivateLargeOrb()
	{
		behavioralPower = GameManager.PowerType.LargeOrb;
	}

	void DeactivateLargeOrb()
	{
		behavioralPower = GameManager.PowerType.None;
	}

	#endregion

	#region Vortex
	//==========VORTEX==========

	void ActivateVortex()
	{
		behavioralPower = GameManager.PowerType.Vortex;
	}

	void DeactivateVortex()
	{
		behavioralPower = GameManager.PowerType.None;
	}

	#endregion

	#region LeechLife
	//==========LEACHLIFE==========

	void ActivateLeechLife()
	{
		behavioralPower = GameManager.PowerType.LeechLife;
	}

	void DeactivateLeechLife()
	{
		behavioralPower = GameManager.PowerType.None;
	}

	#endregion

	#region Slug
	//==========SLUG==========

	void ActivateSlug()
	{
		behavioralPower = GameManager.PowerType.Slug;
	}

	void DeactivateSlug()
	{
		behavioralPower = GameManager.PowerType.None;
	}

	#endregion

	#region Shield
	//==========SHIELD==========

	void ActivateShield()
	{
		behavioralPower = GameManager.PowerType.Shield;
	}

	void DeactivateShield()
	{
		behavioralPower = GameManager.PowerType.None;
	}

	#endregion

	//==========ELEMENTAL POWERS FUNCTIONS==========

	#region Ice
	//==========ICE==========

	void ActivateIce()
	{
		elementalPower = GameManager.PowerType.Ice;
	}

	void DeactivateIce()
	{
		elementalPower = GameManager.PowerType.None;
	}

	#endregion

	#region Fire
	//==========FIRE==========

	void ActivateFire()
	{
		elementalPower = GameManager.PowerType.Fire;
	}

	void DeactivateFire()
	{
		elementalPower = GameManager.PowerType.None;
	}

	#endregion

	#region Water
	//==========WATER==========

	void ActivateWater()
	{
		elementalPower = GameManager.PowerType.Water;
	}

	void DeactivateWater()
	{
		elementalPower = GameManager.PowerType.None;
	}

	#endregion

	#region Electric
	//==========ELECTRIC==========

	void ActivateElectric()
	{
		elementalPower = GameManager.PowerType.Electric;
	}

	void DeactivateElectric()
	{
		elementalPower = GameManager.PowerType.None;
	}

	#endregion

	#region Weakness
	//==========WEAKNESS==========

	void ActivateWeakness()
	{
		elementalPower = GameManager.PowerType.Weakness;
	}

	void DeactivateWeakness()
	{
		elementalPower = GameManager.PowerType.None;
	}

	#endregion
}
