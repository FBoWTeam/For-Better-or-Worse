using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    #region All Variables

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

    [Header("Wave")]
    public GameObject wave;

    [Header("Subave")]
    public GameObject subwave;

    [Header("WaveAnnouncer")]
    public GameObject waveAnnouncer;

    [Header("Fox Powers")]
    public GameObject elementalPowerFox;
    public GameObject behaviouralPowerFox;
    public GameObject elementalReadyFox;
    public GameObject behaviouralReadyFox;

    [Header("Raccoon Powers")]
    public GameObject elementalPowerRaccoon;
    public GameObject behaviouralPowerRaccoon;
    public GameObject elementalReadyRaccoon;
    public GameObject behaviouralReadyRaccoon;

    [Header("Taunt")]
    public GameObject tauntCooldownFox;
    public GameObject tauntCooldownRaccoon;
    public GameObject tauntReadyFox;
    public GameObject tauntReadyRaccoon;


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

    [Header("Drop GO")]
    public Camera camera;
    public GameObject drop;
    public float dropSpeed;
    private bool isDropActive = false;

    [Header("Text Quote")]
    public float displayTime;
    public int pourcentageQuote;

    public List<string> player1TextsGetHit;//quotes of the player 1 when hit by an enemy
    public List<string> player1TextsOtherGetHit;//quotes of the player 1 when player 2 is hit by an enemy

    public List<string> player2TextsGetHit;//quotes of the player 2 when hit by an enemy
    public List<string> player2TextsOtherGetHit;//quotes of the player 2 when player 1 is hit by an enemy

    public List<string> player1TextsOrbHit;//quotes of the player 1 when hit by an enemy
    public List<string> player1TextsOtherOrbHit;//quotes of the player 1 when player 2 is hit by the orb

    public List<string> player2TextsOrbHit;//quotes of the player 2 when hit by an enemy
    public List<string> player2TextsOtherOrbHit;//quotes of the player 2 when player 1 is hit by the orb

    [HideInInspector]
    Dictionary<int, GameManager.PowerType> busySlot;

    #endregion

    #region All Methods

    public void InitDictionary()
    {
        busySlot = new Dictionary<int, GameManager.PowerType>(2);
        busySlot.Add(1, GameManager.PowerType.None);
        busySlot.Add(2, GameManager.PowerType.None);
    }

    public IEnumerator TauntCooldownSystem(bool player1, float tauntCooldown)
    {
        if (player1)
        {
            yield return StartCoroutine(startCooldown(tauntCooldown, tauntCooldownFox.GetComponent<Image>()));
            tauntReadyFox.GetComponent<ParticleSystem>().Play();
        }
        else
        {
            yield return StartCoroutine(startCooldown(tauntCooldown, tauntCooldownRaccoon.GetComponent<Image>()));
            tauntReadyRaccoon.GetComponent<ParticleSystem>().Play();
        }

        yield return null;
    }

    #region Other Methods

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
    /// <param name="playerText">text display in the player 1 dialog box</param>
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
    /// <param name="playerText">text display in the player 1 dialog box</param>
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
                {
                    GetImage(elementalPowerFox).sprite = ImageAssignment(powerSlot);
                }
                else
                {
                    GetImage(elementalPowerRaccoon).sprite = ImageAssignment(powerSlot);
                }
                break;
            case 2:
                if (player1)
                {
                    GetImage(behaviouralPowerFox).sprite = ImageAssignment(powerSlot);
                }
                else
                {
                    GetImage(behaviouralPowerRaccoon).sprite = ImageAssignment(powerSlot);
                }
                break;
        }
    }

    public void UpdateDroppedPower(GameManager.PowerType droppedPower)
    {
        orbPower.sprite = ImageAssignment(droppedPower);
        StartCoroutine(DropFeedback(drop, GameManager.gameManager.orb.transform, orbPower.transform));
        //StartCoroutine(DropFeedback(drop, orbPower.transform, elementalPowerFox.transform));

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


    #region CoolDown
    /// <summary>
    /// Launch cooldown visualisation based on power
    /// </summary>
    /// <param name="power"></param>
    /// <param name="cd"></param>
    public IEnumerator Cooldown(GameManager.PowerType power, float cd, bool player1)
    {
        // lancer start cooldown sur les ( p1 et p2) slot assigner au power

        int slot = getSlotByPower(power);
        if (slot > -1)
        {
            switch (slot)
            {
                case 1:
                    if (player1)
                    {
                        yield return StartCoroutine(startCooldown(cd, GetCdImage(elementalPowerFox)));
                        elementalReadyFox.GetComponent<ParticleSystem>().Play();
                    }
                    else
                    {
                        yield return StartCoroutine(startCooldown(cd, GetCdImage(elementalPowerRaccoon)));
                        elementalReadyRaccoon.GetComponent<ParticleSystem>().Play();
                    }
                    break;
                case 2:
                    if (player1)
                    {
                        yield return StartCoroutine(startCooldown(cd, GetCdImage(behaviouralPowerFox)));
                        behaviouralReadyFox.GetComponent<ParticleSystem>().Play();
                    }
                    else
                    {
                        yield return StartCoroutine(startCooldown(cd, GetCdImage(behaviouralPowerRaccoon)));
                        behaviouralReadyRaccoon.GetComponent<ParticleSystem>().Play();
                    }
                    break;
                default:
                    break;
            }
        }
        else
        {
            Debug.LogError("NO SLOT FOUND FOR THIS POWER");
        }

        yield return null;
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
    #endregion

    #region Combo

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

    #endregion

    #region Arena
    public void UpdateWave(int nb)
    {
        wave.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = nb.ToString();
    }

    public void UpdateSubWave(int nb)
    {
        subwave.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = nb.ToString();
    }

    public IEnumerator AnnouceWave(int nb)
    {
        waveAnnouncer.SetActive(true);
        waveAnnouncer.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = nb.ToString();
        yield return new WaitForSeconds(2f);
        waveAnnouncer.SetActive(false);
    }

    #endregion


    #region Getters

    /// <summary>
    /// return 'Sprite' Image coponent from Ui gameObject
    /// </summary>
    /// <param name="go"></param>
    /// <returns></returns>
    Image GetImage(GameObject go)
    {
        return go.transform.GetChild(0).GetComponent<Image>();
    }
    /// <summary>
    /// return Cooldown Image coponent from Ui gameObject
    /// </summary>
    /// <param name="go"></param>
    /// <returns></returns>
    Image GetCdImage(GameObject go)
    {
        return go.transform.GetChild(1).GetComponent<Image>();
    }

    #endregion



    /// <summary>
    /// Call the UpdateDialogBox randomly when a player get hit by anything
    /// </summary>
    public void QuoteOnDamage(string damageDealer, GameObject targetPlayer)
    {
        if (Random.Range(1, 101)/*return an integer between 1(inclusive) and 101(exclusive)*/ <= pourcentageQuote)
        {
            if (damageDealer == "enemy")
            {
                if (targetPlayer == GameManager.gameManager.player1)
                {
                    if (Random.Range(1, 3) == 1)//50%
                    {
                        UpdateDialogBox1(player1TextsGetHit[Random.Range(0, player1TextsGetHit.Count)], displayTime);
                    }
                    else
                    {
                        UpdateDialogBox2(player2TextsOtherGetHit[Random.Range(0, player2TextsOtherGetHit.Count)], displayTime);
                    }
                }
                if (targetPlayer == GameManager.gameManager.player2)
                {
                    if (Random.Range(1, 3) == 1)//50%
                    {
                        UpdateDialogBox1(player1TextsOtherGetHit[Random.Range(0, player1TextsOtherGetHit.Count)], displayTime);
                    }
                    else
                    {
                        UpdateDialogBox2(player2TextsGetHit[Random.Range(0, player2TextsGetHit.Count)], displayTime);
                    }
                }
            }
            else if (damageDealer == "orb")
            {
                if (targetPlayer == GameManager.gameManager.player1)
                {
                    if (Random.Range(1, 3) == 1)//50%
                    {
                        UpdateDialogBox1(player1TextsOrbHit[Random.Range(0, player1TextsOrbHit.Count)], displayTime);
                    }
                    else
                    {
                        UpdateDialogBox2(player2TextsOtherOrbHit[Random.Range(0, player2TextsOtherOrbHit.Count)], displayTime);
                    }
                }
                if (targetPlayer == GameManager.gameManager.player2)
                {
                    if (Random.Range(1, 3) == 1)//50%
                    {
                        UpdateDialogBox1(player1TextsOtherOrbHit[Random.Range(0, player1TextsOtherOrbHit.Count)], displayTime);
                    }
                    else
                    {
                        UpdateDialogBox2(player2TextsOrbHit[Random.Range(0, player2TextsOrbHit.Count)], displayTime);
                    }
                }
            }
        }
    }

    public void RespawnReset()
    {
        GetCdImage(elementalPowerFox).fillAmount = 0;
        GetCdImage(behaviouralPowerFox).fillAmount = 0;
        tauntCooldownFox.GetComponent<Image>().fillAmount = 0;

        GetCdImage(elementalPowerRaccoon).fillAmount = 0;
        GetCdImage(behaviouralPowerRaccoon).fillAmount = 0;
        tauntCooldownRaccoon.GetComponent<Image>().fillAmount = 0;
    }

    public void SceneToUI(GameObject UIElement, Vector3 target)
    {
        //first you need the RectTransform component of your canvas
        RectTransform CanvasRect = this.GetComponent<RectTransform>();

        //then you calculate the position of the UI element
        //0,0 for the canvas is at the center of the screen, whereas WorldToViewPortPoint treats the lower left corner as 0,0. Because of this, you need to subtract the height / width of the canvas * 0.5 to get the correct position.

        Vector2 ViewportPosition = camera.WorldToViewportPoint(target);
        Vector2 WorldObject_ScreenPosition = new Vector2(
        ((ViewportPosition.x * CanvasRect.sizeDelta.x) - (CanvasRect.sizeDelta.x * 0.5f)),
        ((ViewportPosition.y * CanvasRect.sizeDelta.y) - (CanvasRect.sizeDelta.y * 0.5f)));

        //now you can set the position of the ui element
        UIElement.GetComponent<RectTransform>().anchoredPosition = WorldObject_ScreenPosition;
    }

    public IEnumerator DropFeedback(GameObject UIElement, Transform start, Transform end)
    {
        if (!isDropActive)
        {
            print("DROP");
            drop.SetActive(true);
            isDropActive = true;
            SceneToUI(UIElement, start.position);

            //print(Vector3.Distance(UIElement.transform.position, end));

            while (Vector3.Distance(UIElement.transform.position, end.position) > 0.01)
            {
                UIElement.transform.position = Vector3.Lerp(UIElement.transform.position, end.transform.position, dropSpeed);
                yield return new WaitForEndOfFrame();
            }
            isDropActive = false;
        }
        yield return null;
    }

    public void OrbToPowerSlotFeedback(bool isPlayer1, bool isElemental)
    {
        if (isPlayer1)
        {
            if (isElemental)
            {
                StartCoroutine(DropFeedback(drop, orbPower.transform, elementalPowerFox.transform));
            }
            else
            {
                StartCoroutine(DropFeedback(drop, orbPower.transform, behaviouralPowerFox.transform));
            }
        }
        else
        {
            if (isElemental)
            {
                StartCoroutine(DropFeedback(drop, orbPower.transform, elementalPowerRaccoon.transform));
            }
            else
            {
                StartCoroutine(DropFeedback(drop, orbPower.transform, behaviouralPowerRaccoon.transform));
            }
        }
    }

    //public enfant FaireLAmour(Individu ind1, ind2)
    //{
    //    float enceinte = Random.Range(0, 2);
    //    if (!enceinte)
    //    {
    //        print("tranquille");
    //    }
    //    else
    //    {
    //        return enfant;
    //    }
    //}

    //private void Update()
    //{
    //    SceneToUI(drop, GameManager.gameManager.orb.transform.position);
    //    SceneToUI(drop1, GameManager.gameManager.player2.transform.position + new Vector3(0, 1, 0));
    //}

    #endregion
}
