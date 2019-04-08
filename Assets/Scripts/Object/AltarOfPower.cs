using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AltarOfPower : MonoBehaviour, IActivable
{
    public bool isActive { get; set; }

    public GameManager.PowerType powerToGive;
    
    PowerController controller;

    private void Start()
    {
        controller = GameManager.gameManager.orb.GetComponent<PowerController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Orb"))
        {
            this.Activate();
        }
    }

    public void Activate()
    {
        if (!isActive)
        {
            GivePower();
            isActive = true;
        }
    }

    void GivePower()
    {
        if (controller.droppedPower == GameManager.PowerType.None)
        {
            controller.droppedPower = powerToGive;
            controller.reflectedDrop = false;
            GameManager.gameManager.UIManager.UpdateDroppedPower(powerToGive);
            GetComponent<MeshRenderer>().material.color = Color.black;
        }
    }

}
