using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Torch : MonoBehaviour, IActivable
{
    public bool isActive { get; set; }

    [Tooltip("list of objects to activate to activate the brazier")]
    public List<GameObject> objectToActivate;

    [Tooltip("list of activated objects needed to activate the brazier")]
    public List<GameObject> objectsConditions;

    public bool activated;

    // Start is called before the first frame update
    void Start()
    {
        if (activated)
        {
            this.Activate();
        }
    }
    
    public void Activate()
    {
        if (CheckValidObjects())
        {
            ActivateFireParticles();
            gameObject.transform.GetChild(0).gameObject.SetActive(true);
            isActive = true;
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
        DeactivateFireParticles();
        gameObject.transform.GetChild(0).gameObject.SetActive(false);
        isActive = false;
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

    void ActivateFireParticles()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(true);
        }
    }

    void DeactivateFireParticles()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }
    }

}
