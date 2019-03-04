using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TriggerSystem : MonoBehaviour
{
    public enum TriggerActivation { Enter, Exit, Stay };
    public TriggerActivation triggerActivation;

    public enum TriggerMode { Enemy, Dialog };
    public TriggerMode triggerMode;

    public GameObject[] enemies;

    [DrawIf(new string[] { "triggerMode" },TriggerMode.Dialog)]
    public int displayTime;
    [TextArea]
    public string player1Text;
    [TextArea]
    public string player2Text;

    private void OnTriggerEnter(Collider other)
    {
        if (triggerActivation == TriggerActivation.Enter)
        {
            TriggerManager(other);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (triggerActivation == TriggerActivation.Exit)
        {
            TriggerManager(other);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (triggerActivation == TriggerActivation.Stay)
        {
            TriggerManager(other);
        }
    }

    private void TriggerManager(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            switch (triggerMode)
            {
                case TriggerMode.Enemy:
                    if (enemies.Length > 0)
                    {
                        foreach (GameObject enemy in enemies)
                        {
                            enemy.GetComponent<Enemy>().enabled = true;
                        }
                    }
                    else
                    {
                        Debug.LogError("Enemies list is empty", this);
                    }
                    break;
                case TriggerMode.Dialog:
                    if (player1Text != "" ||   player2Text != "")
                    {
                        //UpdateUI
                        GameManager.gameManager.UIManager.UpdateDialogBox(player1Text, player2Text, displayTime);
                    }
                    else
                    {
                        Debug.LogError("Where are the fucking dialogs!", this);
                    }
                    break;
                default:
                    break;
            }
            Destroy(this.gameObject);
        }
    }
}