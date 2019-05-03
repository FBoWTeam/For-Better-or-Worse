﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Playables;
using UnityEngine.UI;

public class DialogSystem : MonoBehaviour
{
	private Dialog dialog;

	public GameObject mia;
    public Material miaTalkingMat;
    public Material miaNotTalkingMat;

    public GameObject raka;
    public Material rakaTalkingMat;
    public Material rakaNotTalkingMat;

	public GameObject entity;

	public GameObject alan;
	public Material alanTalkingMat;
	public Material alanNotTalkingMat;

	public Image arrowButton;
	public Sprite arrowButtonNormal;
	public Sprite arrowButtonPressed;

	public TextMeshProUGUI displayedText;
	public float writeDelay = 0.1f;

	private bool speededUp = false;
    private bool canMoveToNext = false;

    /// <summary>
    /// Handle Full dialog between Characters
    /// </summary>
    /// <returns></returns>
    public IEnumerator StartDialog(Dialog dialogToDisplay, PlayableDirector director)
	{
		GameManager.gameManager.isPaused = true;
		dialog = dialogToDisplay;

		DisplayBonusCharacter(dialog.bonusCharacter);

        foreach (Dialog.DialogElement element in dialogToDisplay.dialogElementList)
		{
			DisplayTalker(element.characterTalking);
			StartCoroutine(DisplayReplica(I18n.Translate(element.replicaKey)));
			//wait for player input
			yield return new WaitUntil(() => MoveToNextReplica() && canMoveToNext);   
        }

		gameObject.SetActive(false);
		director.Resume();
		GameManager.gameManager.isPaused = false;

		yield return null;
    }

    /// <summary>
    /// return true if player can move to next dialog
    /// </summary>
    /// <returns></returns>
    private bool MoveToNextReplica()
	{
		if ((Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.Keypad0) || Input.GetKey(KeyCode.Joystick1Button0) || Input.GetKey(KeyCode.Joystick2Button0)) && !speededUp)
		{
			speededUp = true;
			arrowButton.sprite = arrowButtonPressed;
		}
		if((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Keypad0) || Input.GetKeyDown(KeyCode.Joystick1Button0) || Input.GetKeyDown(KeyCode.Joystick2Button0)) && canMoveToNext)
		{
			speededUp = false;
			arrowButton.sprite = arrowButtonNormal;
			return true;
		}
		if ((Input.GetKeyUp(KeyCode.Space) || Input.GetKeyUp(KeyCode.Keypad0) || Input.GetKeyUp(KeyCode.Joystick1Button0) || Input.GetKeyUp(KeyCode.Joystick2Button0)) && speededUp)
		{
			speededUp = false;
			arrowButton.sprite = arrowButtonNormal;
		}
        return false;
    }

   /* /// <summary>
    /// Restrain player from moving to next Dialog for delay
    /// </summary>
    /// <param name="delay"></param>
    /// <returns></returns>
    private IEnumerator InputCoolDown(float delay) {
        canPassReplica = false;
        yield return new WaitForSeconds(delay);
		canPassReplica = true;
    }*/

	private void DisplayBonusCharacter(Dialog.BonusCharacter bonusCharacter)
	{
		switch(bonusCharacter)
		{
			case Dialog.BonusCharacter.None:
				entity.SetActive(false);
				alan.SetActive(false);
				break;
			case Dialog.BonusCharacter.Entity:
				entity.SetActive(false);
				alan.SetActive(false);
				break;
			case Dialog.BonusCharacter.Alan:
				entity.SetActive(false);
				alan.SetActive(true);
				setTexture(alan, alanNotTalkingMat);
				break;
		}
	}

	/// <summary>
	/// Set UI element and text according to isFoxTalking
	/// </summary>
	/// <param name="isFoxTalking"></param>
	/// <param name="text"></param>
	private void DisplayTalker(Dialog.Character characterTalking) {
		switch(characterTalking)
		{
			case Dialog.Character.Mia:
				setTexture(mia, miaTalkingMat);
				setTexture(raka, rakaNotTalkingMat);
				entity.SetActive(false);
				setTexture(alan, alanNotTalkingMat);
				break;
			case Dialog.Character.Raka:
				setTexture(mia, miaNotTalkingMat);
				setTexture(raka, rakaTalkingMat);
				entity.SetActive(false);
				setTexture(alan, alanNotTalkingMat);
				break;
			case Dialog.Character.Entity:
				setTexture(mia, miaNotTalkingMat);
				setTexture(raka, rakaNotTalkingMat);
				entity.SetActive(true);
				setTexture(alan, alanNotTalkingMat);
				break;
			case Dialog.Character.Alan:
				setTexture(mia, miaNotTalkingMat);
				setTexture(raka, rakaNotTalkingMat);
				entity.SetActive(false);
				setTexture(alan, alanTalkingMat);
				break;
		}
    }

    /// <summary>
    /// Animate shared text element
    /// </summary>
    /// <param name="replica"></param>
    /// <returns></returns>
    IEnumerator DisplayReplica(string replica)
	{
		bool pass = false;
		displayedText.text = "";
        canMoveToNext = false;
        foreach (char c in replica)
		{
			if(c == '\\')
				displayedText.text += '\n';
			else
				displayedText.text += c;

			if (speededUp)
				pass = !pass;
			else
				pass = false;

			if (!pass)
				yield return new WaitForSeconds(writeDelay);
        }
        canMoveToNext = true;
        yield return null;
    }

    private void setTexture(GameObject foxOrRacoon,Material toAply) {
        foxOrRacoon.transform.GetChild(0).GetComponent<SkinnedMeshRenderer>().material = toAply;
    }
}
