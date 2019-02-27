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
    public RawImage orbPower;

    [Header("Combo")]
    public GameObject combo;

    [Header("Main Powers of Fox")]
    public RawImage mainPower1Fox;
    public RawImage mainPower2Fox;
    [Header("Main Powers of Raccoon")]
    public RawImage mainPower1Raccoon;
    public RawImage mainPower2Raccoon;

    [Header("Secondary Powers of Fox")]
    public RawImage secondaryPower1Fox;
    public RawImage secondaryPower2Fox;
    [Header("Secondary Powers of Raccoon")]
    public RawImage secondaryPower1Raccoon;
    public RawImage secondaryPower2Raccoon;

    [Header("Taunt")]
    public GameObject tauntCooldownFox;
    public GameObject tauntCooldownRaccoon;


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
