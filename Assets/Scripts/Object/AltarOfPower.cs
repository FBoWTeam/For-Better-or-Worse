using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AltarOfPower : MonoBehaviour, IActivable
{
    public bool isActive { get; set; }

    

    public void Activate()
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Orb"))
        {
            GivePower();
        }
    }


    void GivePower()
    {

    }

}
