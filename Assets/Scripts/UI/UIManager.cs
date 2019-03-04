using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{

    #region All Variables

    [Header("Dialog Box")]
    public GameObject dialogBoxFox;
    public GameObject dialogBoxRaccoon;

    [Header("Orb Power")]
    public Image orbPower;

    [Header("Combo")]
    public GameObject combo;

    [Header("Main Powers of Fox")]
    public Image mainPower1Fox;
    public Image mainPower2Fox;
    [Header("Main Powers of Raccoon")]
    public Image mainPower1Raccoon;
    public Image mainPower2Raccoon;

    [Header("Secondary Powers of Fox")]
    public Image secondaryPower1Fox;
    public Image secondaryPower2Fox;
    [Header("Secondary Powers of Raccoon")]
    public Image secondaryPower1Raccoon;
    public Image secondaryPower2Raccoon;

    [Header("Taunt")]
    public GameObject tauntCooldownFox;
    public GameObject tauntCooldownRaccoon;

    [Header("Powers Image")]
    public Sprite largeOrb;
    public Sprite vortex;
    public Sprite leechLife;
    public Sprite slug;
    public Sprite shield;
    public Sprite ice;
    public Sprite fire;
    public Sprite electric;
    public Sprite darkness;
    public Sprite none;

    #endregion


    #region All Functions

    #region Dialog 


    /// <summary>
    /// In this case, the method check if dialog are display and removed them
    /// </summary>
    public void UpdateDialogBox()
    {
        if (dialogBoxFox.activeSelf || dialogBoxRaccoon.activeSelf)
        {
            dialogBoxFox.GetComponentInChildren<TextMeshProUGUI>().SetText("");
            dialogBoxRaccoon.GetComponentInChildren<TextMeshProUGUI>().SetText("");
            dialogBoxFox.SetActive(false);
            dialogBoxRaccoon.SetActive(false);
        }
    }

    /// <summary>
    /// In this case, the method check which dialog box display and update the text
    /// </summary>
    /// <param name="player1Text">text display in the player 1 dialog box</param>
    /// <param name="player2Text">text display in the player 2 dialog box</param>
    /// <param name="displayTime">use for the invoke, to determine how much time display the text</param>
    public void UpdateDialogBox(string player1Text, string player2Text, int displayTime)
    {
        if (player1Text != "")
        {
            dialogBoxFox.GetComponentInChildren<TextMeshProUGUI>().SetText(player1Text);
            dialogBoxFox.SetActive(true);
        }
        if (player2Text != "")
        {
            dialogBoxRaccoon.GetComponentInChildren<TextMeshProUGUI>().SetText(player2Text);
            dialogBoxRaccoon.SetActive(true);
        }

        Invoke("UpdateDialogBox", displayTime);
    }

    #endregion

    #region PowerSlot 

    /// <summary>
    /// Update the UI power slot according to the new power obtained
    /// </summary>
    /// <param name="slot">use to know the slot to update</param>
    /// <param name="player1">use to know the player affected</param>
    /// <param name="powerSlot">refers to the new powerType obtained</param>
    public void UpdatePowerSlot(int slot, bool player1, GameManager.PowerType powerSlot)
    {
        switch (slot)
        {
            case 1:
                if (player1)
                    mainPower1Fox.sprite = ImageAssignment(powerSlot);
                else
                    mainPower1Raccoon.sprite = ImageAssignment(powerSlot);
                break;
            case 2:
                if (player1)
                    mainPower2Fox.sprite = ImageAssignment(powerSlot);
                else
                    mainPower2Raccoon.sprite = ImageAssignment(powerSlot);
                break;
            case 3:
                if (player1)
                    secondaryPower1Fox.sprite = ImageAssignment(powerSlot);
                else
                    secondaryPower1Raccoon.sprite = ImageAssignment(powerSlot);
                break;
            case 4:
                if (player1)
                    secondaryPower2Fox.sprite = ImageAssignment(powerSlot);
                else
                    secondaryPower2Raccoon.sprite = ImageAssignment(powerSlot);
                break;
        }
    }

    #endregion

    /// <summary>
    /// Assign the image corresponding to the power
    /// </summary>
    /// <param name="powerType">refers to the new powerType obtained</param>
    /// <returns></returns>
    public Sprite ImageAssignment(GameManager.PowerType powerType)
    {
        switch (powerType)
        {
            case GameManager.PowerType.None:
                return none;
            case GameManager.PowerType.LargeOrb:
                return largeOrb;
            case GameManager.PowerType.Vortex:
                return vortex;
            case GameManager.PowerType.LeechLife:
                return leechLife;
            case GameManager.PowerType.Slug:
                return slug;
            case GameManager.PowerType.Shield:
                return shield;
            case GameManager.PowerType.Ice:
                return ice;
            case GameManager.PowerType.Fire:
                return fire;
            case GameManager.PowerType.Electric:
                return electric;
            case GameManager.PowerType.Darkness:
                return darkness;
            default:
                return none;
        }
    }

    #endregion

}
