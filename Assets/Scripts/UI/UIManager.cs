﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
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
    public Sprite weakness;
    public Sprite none;

    #endregion


    #region All Functions

    private void Start()
    {
    }


    #region PowerSlot Functions


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

    public Sprite ImageAssignment(GameManager.PowerType powerType)
    {
        switch (powerType)
        {
            case GameManager.PowerType.None:
                return none;
                break;
            case GameManager.PowerType.LargeOrb:
                return largeOrb;
                break;
            case GameManager.PowerType.Vortex:
                return vortex;
                break;
            case GameManager.PowerType.LeechLife:
                return leechLife;
                break;
            case GameManager.PowerType.Slug:
                return slug;
                break;
            case GameManager.PowerType.Shield:
                return shield;
                break;
            case GameManager.PowerType.Ice:
                return ice;
                break;
            case GameManager.PowerType.Fire:
                return fire;
                break;
            case GameManager.PowerType.Electric:
                return electric;
                break;
            case GameManager.PowerType.Weakness:
                return weakness;
                break;
            default:
                return none;
                break;
        }


    }

    #endregion

}
