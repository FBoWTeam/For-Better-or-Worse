using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricPylon : MonoBehaviour, IActivable
{
    public bool isActive { get; set; }

    public bool electrified;

    public enum ElectricPylonType
    {
        Classic,
        ArenaElectricPylon
    }

    public ElectricPylonType type;

    [DrawIf(new string[] { "type" }, ElectricPylonType.ArenaElectricPylon)]
    public float reActivationTime;

    [Tooltip("list of objects to activate to activate the brazier")]
    public List<GameObject> objectToActivate;

    [Tooltip("list of activated objects needed to activate the brazier")]
    public List<GameObject> objectsConditions;

    private Animator anim;
    

    // Start is called before the first frame update
    void Start()
    {
        //anim = GetComponentInParent<Animator>();
        if (electrified)
        {
            //anim.SetBool("isActive", true);
            this.Activate();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Orb"))
        {
            PowerController powerController = other.GetComponent<PowerController>();

            if (type == ElectricPylonType.Classic)
            {
                //if the electric pylon is not active and the orb is on fire, set the brazier on and activates the object if not null
                if (!isActive && powerController.elementalPower == GameManager.PowerType.Electric)
                {
                    this.Activate();
                }
                //if the electric pylon is active and the orb isn't (else refrech the duration of the power)
                else if (isActive)
                {
                    powerController.ActivatePower(GameManager.PowerType.Electric, "forced");
                }
            }

            if (type == ElectricPylonType.ArenaElectricPylon)
            {
                powerController.ActivatePower(GameManager.PowerType.Electric, "forced");
                Deactivate();
                StartCoroutine(ReActivateArenaElectricPylon());
            }
        }
    }

    IEnumerator ReActivateArenaElectricPylon()
    {
        yield return new WaitForSeconds(reActivationTime);
        this.Activate();
    }


    public void Activate()
    {
        if (CheckValidObjects())
        {
            gameObject.GetComponent<Renderer>().material.color = Color.yellow;
            isActive = true;
            if (type == ElectricPylonType.ArenaElectricPylon)
            {
                //anim.SetBool("isActive", true);
            }
            electrified = true;
            if (objectToActivate.Count != 0)
            {
                for (int i = 0; i < objectToActivate.Count; i++)
                {
                    objectToActivate[i].GetComponent<IActivable>().Activate();
                }
            }
        }
    }

    public void Deactivate()
    {
        if (type == ElectricPylonType.ArenaElectricPylon)
        {
            //anim.SetBool("isActive", false);
        }
        gameObject.GetComponent<Renderer>().material.color = Color.grey;
        isActive = false;
        electrified = false;
    }

    bool CheckValidObjects()
    {
        for (int i = 0; i < objectsConditions.Count; i++)
        {
            if (objectsConditions[i].GetComponent<IActivable>().isActive != true)
            {
                return false;
            }
        }
        return true;
    }
}
