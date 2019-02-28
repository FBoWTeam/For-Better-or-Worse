using System.Collections;
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

    #endregion

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
