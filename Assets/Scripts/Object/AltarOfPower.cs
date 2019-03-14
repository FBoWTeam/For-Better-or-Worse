using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AltarOfPower : MonoBehaviour, IActivable
{
    public bool isActive { get; set; }

    public GameManager.PowerType powerToGive;

    public Material standardMaterial;
    public Material electricMaterial;
    public Material fireMaterial;
    public Material iceMaterial;
    public Material leechLifeMaterial;
    public Material shieldMaterial;
    public Material slugMaterial;
    public Material vortexMaterial;
    public Material darknessMaterial;

    PowerController controller;

    private void Start()
    {
        controller = GameManager.gameManager.orb.GetComponent<PowerController>();
        switch (powerToGive)
        {
            case GameManager.PowerType.None:
                GetComponent<MeshRenderer>().material = standardMaterial;
                break;
            case GameManager.PowerType.Vortex:
                GetComponent<MeshRenderer>().material = vortexMaterial;
                break;
            case GameManager.PowerType.LeechLife:
                GetComponent<MeshRenderer>().material = leechLifeMaterial;
                break;
            case GameManager.PowerType.Slug:
                GetComponent<MeshRenderer>().material = slugMaterial;
                break;
            case GameManager.PowerType.Shield:
                GetComponent<MeshRenderer>().material = shieldMaterial;
                break;
            case GameManager.PowerType.Ice:
                GetComponent<MeshRenderer>().material = iceMaterial;
                break;
            case GameManager.PowerType.Fire:
                GetComponent<MeshRenderer>().material = fireMaterial;
                break;
            case GameManager.PowerType.Electric:
                GetComponent<MeshRenderer>().material = electricMaterial;
                break;
            case GameManager.PowerType.Darkness:
                GetComponent<MeshRenderer>().material = darknessMaterial;
                break;
            default:
                break;
        }
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
