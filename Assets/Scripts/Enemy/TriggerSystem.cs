using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TriggerSystem : MonoBehaviour
{
    #region All Variables

    public enum TriggerActivation { Enter, Exit, Stay };
    public TriggerActivation triggerActivation;

    public enum TriggerMode { Enemy, Dialog, EnemyAndDialog };
    public TriggerMode triggerMode;

    public GameObject[] enemies;

    [Tooltip("Display time of dialogs")]
    public int displayTime;
    [TextArea]
    public string player1Text;
    [TextArea]
    public string player2Text;

    #endregion

    #region All Methods

    #region OnTrigger Methods
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

    #endregion

    private void TriggerEnemy()
    {
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
    }

    private void TriggerDialog()
    {
        if (player1Text != "")
        {
            GameManager.gameManager.UIManager.UpdateDialogBox1(player1Text, displayTime);
        }

        if (player2Text != "")
        {
            GameManager.gameManager.UIManager.UpdateDialogBox2(player2Text, displayTime);
        }

        if (player1Text == "" && player2Text == "")
        {
            Debug.LogError("Where are the fucking dialogs!", this);
        }
    }

    private void TriggerManager(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            switch (triggerMode)
            {
                case TriggerMode.Enemy:
                    TriggerEnemy();
                    break;
                case TriggerMode.Dialog:
                    TriggerDialog();
                    break;
                case TriggerMode.EnemyAndDialog:
                    TriggerEnemy();
                    TriggerDialog();
                    break;
                default:
                    break;
            }
            Destroy(this.gameObject);
        }
    }


    void OnDrawGizmosSelected()
    {

        switch (triggerMode)
        {
            case TriggerMode.Enemy:
                Gizmos.color = new Color(0.7f, 0.05f, 0.05f, 0.7f);
                break;
            case TriggerMode.Dialog:
                Gizmos.color = new Color(0.25f, 0.5f, 0.95f, 0.7f);
                break;
            case TriggerMode.EnemyAndDialog:
                Gizmos.color = new Color(0.6f, 0.05f, 0.9f, 0.7f);
                break;
            default:
                break;
        }
        Gizmos.DrawCube(transform.position, GetComponent<Collider>().bounds.size);
    }

    #endregion
}