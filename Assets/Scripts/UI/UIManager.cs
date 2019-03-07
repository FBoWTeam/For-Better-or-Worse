using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{

    #region All Variables

    [Header("Fader")]
    public Canvas fader;

    [Header("Health Bar")]
    public Image damageTakenP1;
    public Image damageTakenP2;

    [Header("Dialog Box")]
    public GameObject dialogBoxFox;
    public GameObject dialogBoxRaccoon;

    [Header("Orb Power")]
    public Image orbPower;

    [Header("Combo")]
    public GameObject combo;

    [Header("Fox Powers")]
    public GameObject elementalPowerFox;
	public GameObject behaviouralPowerFox;

	[Header("Raccoon Powers")]
    public GameObject elementalPowerRaccoon;
    public GameObject behaviouralPowerRaccoon;

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

    [Header("Text Quote")]
    public float displayTime;
    public int pourcentageQuote;
    [TextArea]
    public string player1TextEnemy;
    [TextArea]
    public string player2TextEnemy;
    [TextArea]
    public string player1TextOrb;
    [TextArea]
    public string player2TextOrb;

    [HideInInspector]
    Dictionary<int, GameManager.PowerType> busySlot = new Dictionary<int, GameManager.PowerType>(2);

    #endregion



    #region All Methods

    private void Awake()
    {
        //init dico
        busySlot.Add(1, GameManager.PowerType.None);
        busySlot.Add(2, GameManager.PowerType.None);


    }


    private void Update()
    {
        if (GameManager.gameManager.player1HasTaunt)
        {
            StartCoroutine(startCooldown(GameManager.gameManager.tauntCooldown, tauntCooldownFox.GetComponent<Image>()));
        }
        if (GameManager.gameManager.player2HasTaunt)
        {
            StartCoroutine(startCooldown(GameManager.gameManager.tauntCooldown, tauntCooldownRaccoon.GetComponent<Image>()));
        }
    }

    #region Other Methods

    public IEnumerator FadeCoroutine(string fadeName)
    {
        GameManager.gameManager.isPaused = true;
        Animation anim = fader.GetComponent<Animation>();
        anim.clip = anim.GetClip(fadeName);
        anim.Play();
        yield return new WaitForSeconds(1.2f);
        GameManager.gameManager.isPaused = false;
    }

    /// <summary>
    /// Update Health Bar 
    /// </summary>
    public void UpdateHealthBar()
    {
        damageTakenP1.fillAmount = (float)GameManager.gameManager.damageTakenP1 / (float)GameManager.gameManager.hp;
        damageTakenP2.fillAmount = (float)GameManager.gameManager.damageTakenP2 / (float)GameManager.gameManager.hp;
    }

    #endregion

    #region Dialog Methods

    /// <summary>
    /// In this case, the method check if dialog are display and removed them for the player 1
    /// </summary>
    public void UpdateDialogBox1()
    {
        if (dialogBoxFox.activeSelf || dialogBoxRaccoon.activeSelf)
        {
            dialogBoxFox.GetComponentInChildren<TextMeshProUGUI>().SetText("");
            dialogBoxFox.SetActive(false);
        }
    }

    /// <summary>
    /// In this case, the method check which dialog box display and update the text for the player 1
    /// </summary>
    /// <param name="player1Text">text display in the player 1 dialog box</param>
    /// <param name="player2Text">text display in the player 2 dialog box</param>
    /// <param name="displayTime">use for the invoke, to determine how much time display the text</param>
    public void UpdateDialogBox1(string playerText, float displayTime)
    {
        CancelInvoke("UpdateDialogBox1");
        if (playerText != "")
        {
            dialogBoxFox.GetComponentInChildren<TextMeshProUGUI>().SetText(playerText);
            dialogBoxFox.SetActive(true);
        }
        Invoke("UpdateDialogBox1", displayTime);
    }


    /// <summary>
	/// In this case, the method check if dialog are display and removed them for the player 2
	/// </summary>
	public void UpdateDialogBox2()
    {
        if (dialogBoxFox.activeSelf || dialogBoxRaccoon.activeSelf)
        {
            dialogBoxRaccoon.GetComponentInChildren<TextMeshProUGUI>().SetText("");
            dialogBoxRaccoon.SetActive(false);
        }
    }

    /// <summary>
    /// In this case, the method check which dialog box display and update the text for the player 2
    /// </summary>
    /// <param name="player1Text">text display in the player 1 dialog box</param>
    /// <param name="player2Text">text display in the player 2 dialog box</param>
    /// <param name="displayTime">use for the invoke, to determine how much time display the text</param>
    public void UpdateDialogBox2(string playerText, float displayTime)
    {
        CancelInvoke("UpdateDialogBox2");
        if (playerText != "")
        {
            dialogBoxRaccoon.GetComponentInChildren<TextMeshProUGUI>().SetText(playerText);
            dialogBoxRaccoon.SetActive(true);
        }
        Invoke("UpdateDialogBox2", displayTime);
    }

    #endregion

    #region PowerSlot Methods

    /// <summary>
    /// Update the UI power slot according to the new power obtained
    /// </summary>
    /// <param name="slot">use to know the slot to update</param>
    /// <param name="player1">use to know the player affected</param>
    /// <param name="powerSlot">refers to the new powerType obtained</param>
    public void UpdatePowerSlot(int slot, bool player1, GameManager.PowerType powerSlot)
    {
        busySlot[slot] = powerSlot;
        switch (slot)
        {
            case 1:

                if (player1)
                    GetImage(elementalPowerFox).sprite = ImageAssignment(powerSlot);
                else
                    GetImage(elementalPowerRaccoon).sprite = ImageAssignment(powerSlot);
                break;
            case 2:
                if (player1)
                    GetImage(behaviouralPowerFox).sprite = ImageAssignment(powerSlot);
                else
                    GetImage(behaviouralPowerRaccoon).sprite = ImageAssignment(powerSlot);
                break;
        }
    }

    public void UpdateDroppedPower(GameManager.PowerType droppedPower)
    {
        orbPower.sprite = ImageAssignment(droppedPower);
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

    /// <summary>
    /// Launch cooldown visualisation based on power
    /// </summary>
    /// <param name="power"></param>
    /// <param name="cd"></param>
    public void Cooldown(GameManager.PowerType power, float cd)
    {
        // lancer start cooldown sur les ( p1 et p2) slot assigner au power
        
        int slot = getSlotByPower(power);
        if (slot > -1)
        {
            switch (slot)
            {
                case 1:
                    StartCoroutine(startCooldown(cd, GetCdImage(elementalPowerFox)));
                    StartCoroutine(startCooldown(cd, GetCdImage(elementalPowerRaccoon)));
                    break;
                case 2:
                    StartCoroutine(startCooldown(cd, GetCdImage(behaviouralPowerFox)));
                    StartCoroutine(startCooldown(cd, GetCdImage(behaviouralPowerRaccoon)));
                    break;
                default:
                    break;
            }
        }
        else
        {
            Debug.LogError("NO SLOT FOUND FOR THIS POWER");
        }
    }

    int getSlotByPower(GameManager.PowerType power)
    {

        if (busySlot.ContainsValue(power))
        {
            foreach (KeyValuePair<int, GameManager.PowerType> item in busySlot)
            {
                if (item.Value == power)
                {
                    return item.Key;
                }
            }
        }

        return -1;
    }

    IEnumerator startCooldown(float cd, Image image)
    {

        image.fillAmount = 0.999f;
        while (image.fillAmount != 1)
        {
            image.fillAmount -= 1 / cd * Time.deltaTime;
            if (image.fillAmount <= 0.001f)
            {
                image.fillAmount = 1;
            }
            yield return new WaitForEndOfFrame();
        }
        image.fillAmount = 0f;
        yield return null;
    }

    /// <summary>
    /// Update Combo UI
    /// </summary>
    /// <param name="nb"></param>
    public void UpdateCombo(int nb)
    {
        if (nb < 2)
        {
            combo.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = string.Empty;
            combo.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = string.Empty;
        }
        else if (nb >= 2)
        {
            combo.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Combo !";
            combo.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "x" + nb;
        }

    }
    
    /// <summary>
    /// Call the UpdateDialogBox randomly
    /// </summary>
    public void QuoteOnDamage(string damageDealer, GameObject targetPlayer)
    {
        if (Random.Range(1, 101)/*return an integer between 1(in) and 101(out)*/ <= pourcentageQuote)
        {
            if(damageDealer == "enemy")
            { 
                if (targetPlayer == GameManager.gameManager.player1)
                {
                    UpdateDialogBox1(player1TextEnemy, displayTime);
                }
                if (targetPlayer == GameManager.gameManager.player2)
                {
                    UpdateDialogBox2(player2TextEnemy, displayTime);
                }
            }
            else if(damageDealer == "orb")
            {
                if (targetPlayer == GameManager.gameManager.player1)
                {
                    UpdateDialogBox1(player1TextOrb, displayTime);
                }
                if (targetPlayer == GameManager.gameManager.player2)
                {
                    UpdateDialogBox2(player2TextOrb, displayTime);
                }
            }
        }
    }

    /// <summary>
    /// return 'Sprite' Image coponent from Ui gameObject
    /// </summary>
    /// <param name="go"></param>
    /// <returns></returns>
    Image GetImage(GameObject go) {
        return go.transform.GetChild(0).GetComponent<Image>();
    }
    /// <summary>
    /// return Cooldown Image coponent from Ui gameObject
    /// </summary>
    /// <param name="go"></param>
    /// <returns></returns>
    Image GetCdImage(GameObject go) {
        return go.transform.GetChild(1).GetComponent<Image>();
    }
}
