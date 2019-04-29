using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AltarOfPower : MonoBehaviour, IActivable
{
    public bool isActive { get; set; }

    public GameManager.PowerType powerToGive;
    
    PowerController controller;

    public enum AltarType
    {
        Classic,
        AltarBoss
    }

    public AltarType type;

    Animator anim;

    private void Start()
    {
        controller = GameManager.gameManager.orb.GetComponent<PowerController>();
        if (type == AltarType.AltarBoss)
        {
            anim = GetComponentInParent<Animator>();
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
            if (type == AltarType.AltarBoss)
            {
                anim.SetTrigger("MoveDown");
            }
            

            controller.droppedPower = powerToGive;
            controller.reflectedDrop = false;
            GameManager.gameManager.UIManager.UpdateDroppedPower(powerToGive);
            if (transform.childCount > 0)
            {
                for (int i = 0; i < transform.childCount; i++)
                {
                    transform.GetChild(i).gameObject.SetActive(false);
                }
            }
        }
    }

}
