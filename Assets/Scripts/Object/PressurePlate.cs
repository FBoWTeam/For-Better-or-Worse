using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlate : MonoBehaviour, IActivable
{
    //boolean indicating if the pressure plate is pressed or not
    public bool isActive { get; set; }

    public enum PressurePlateType
    {
        Classic,
        PowerGiver,
        Trial
    }

    public PressurePlateType type;

    //the pressure plate activates an other object
    [Tooltip("the object to activate id the current lever is active")]
    public GameObject[] objectToActivate;

    [DrawIf(new string[] { "type" }, PressurePlateType.PowerGiver)]
    public GameManager.PowerType powerToGive;
    [DrawIf(new string[] { "type" }, PressurePlateType.PowerGiver)]
    public GameObject otherPowerGiver;

    private GameObject playerWhoTookPower;

    private Animator anim;
    public bool powerGiven;

    [Header("Trial")]
    [DrawIf(new string[] { "type" }, PressurePlateType.Trial)]
    public bool trialTriggered;
    [DrawIf(new string[] { "type" }, PressurePlateType.Trial)]
    public GameObject otherPressurePlate;

	public SoundEmitter soundEmitter;

    private void Start()
    {
        anim = GetComponentInParent<Animator>();
    }

    /// <summary>
    /// activates the pressure plate if the orb enter and stays on it's collider
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isActive)
        {
            switch (type)
            {
                case PressurePlateType.Classic:
                    this.Activate();
                    break;
                case PressurePlateType.PowerGiver:
                    GivePower(other.gameObject);
                    this.Activate();
                    break;
                case PressurePlateType.Trial:
                    if (!trialTriggered)
                    {
                        this.Activate();
                    }
                    break;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && isActive)
        {
            this.Activate();
        }
    }

    private void GivePower(GameObject other)
    {
        if (!powerGiven)
        {
            other.gameObject.GetComponent<PlayerController>().AttributePower(powerToGive);
        }
    }

    public void Activate()
    {
        isActive = !isActive;
        //activates the other object
        if (objectToActivate.Length > 0)
        {
            for (int i = 0; i < objectToActivate.Length; i++)
            {
                objectToActivate[i].GetComponent<IActivable>().Activate();
            }
        }

        if (type == PressurePlateType.Trial)
        {
            if (otherPressurePlate.GetComponent<IActivable>().isActive)
            {
                trialTriggered = true;
            }
        }

        if (isActive)
        {
			soundEmitter.PlaySound(0);
            anim.SetBool("isActivated", true);
        }
        else
		{
			soundEmitter.PlaySound(0);
			anim.SetBool("isActivated", false);
        }

        if (type == PressurePlateType.PowerGiver && checkObjectActivated())
        {
            powerGiven = true;
        }

    }

    bool checkObjectActivated()
    {
        for (int i = 0; i < objectToActivate.Length; i++)
        {
            if (!objectToActivate[i].GetComponent<IActivable>().isActive)
            {
                return false;
            }
        }
        return true;
    }

}
