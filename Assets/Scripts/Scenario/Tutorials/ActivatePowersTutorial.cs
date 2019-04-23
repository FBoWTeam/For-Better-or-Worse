using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivatePowersTutorial : MonoBehaviour
{

    public GameObject dropedPowersTutorial;
    public GameObject usePowersTutorial;

    private void OnEnable()
    {
        StartCoroutine(PowersTutorialCoroutine());
    }

    IEnumerator PowersTutorialCoroutine()
    {
        while (GameManager.gameManager.orb.GetComponent<PowerController>().droppedPower == GameManager.PowerType.None)
        {
            Debug.Log("oui1");
            yield return new WaitForEndOfFrame();
        }
        dropedPowersTutorial.SetActive(true);
        

        while (GameManager.gameManager.player1.GetComponent<PlayerController>().behaviouralPowerSlot == GameManager.PowerType.None && GameManager.gameManager.player2.GetComponent<PlayerController>().behaviouralPowerSlot == GameManager.PowerType.None)
        {
            Debug.Log("oui2");
            yield return new WaitForEndOfFrame();
        }
        usePowersTutorial.SetActive(true);
    }
}
