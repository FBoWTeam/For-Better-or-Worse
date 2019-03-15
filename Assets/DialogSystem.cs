using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogSystem : MonoBehaviour
{
    public GameObject fox;
    public Material foxTalkingTexture;
    public Material foxNotTalkingTexture;

    public GameObject racoon;
    public Material racoonTalking;
    public Material racoonNotTalking;

    public GameObject textGameobject;
    public float writeDelay = 0.1f;
    private TextMeshProUGUI sharedText;
   

    public bool foxTalking = true;

    private string[] foxDialog;
    private string[] racoonDialog;

    private bool canMoveToNext = true;


    //Dictionaire <bool,String> true = fox , false = racoon string = text to print
    private List<Tuple<bool, string>> textOrder = new List<Tuple<bool, string>>();

    // Start is called before the first frame update
    void Start()
    {
        sharedText = textGameobject.GetComponent<TextMeshProUGUI>();
    }

    public IEnumerator StartDialog(string[] foxDialogScenario, string[] racoonDialogScenario,bool foxFirst) {
        foxTalking = foxFirst;
        SetDialogs(foxDialogScenario,racoonDialogScenario);
        //Afficher l'ui
        fox.transform.parent.GetComponent<RectTransform>().localScale = Vector3.one;
        foreach (Tuple<bool, string> entry in textOrder) {           
            SetTalker(entry.Item1, entry.Item2);
            //wait for player input
            yield return new WaitUntil(() => MoveNext());   
        }
        yield return null;
    }

  
    private void SetDialogs(string[] foxDialogScenario, string[] racoonDialogScenario) {
        foxDialog = foxDialogScenario;
        racoonDialog = racoonDialogScenario;
        InitDictionary();
    }

    private bool MoveNext() {
        if ( Input.GetKeyUp(KeyCode.A) && canMoveToNext) {

            StartCoroutine(InputCoolDown());
            return true;
        }
        return false;
    }

    private IEnumerator InputCoolDown() {
        canMoveToNext = false;
        yield return new WaitForSeconds(0.5f);
        canMoveToNext = true;
    }

    private void SetTalker(bool isFoxTalking, string text) {
        if (isFoxTalking) {
            setTexture(fox, foxTalkingTexture);
            setTexture(racoon, racoonNotTalking);
            //foxTalking = false;
        } else {
            setTexture(racoon, racoonTalking);
            setTexture(fox, foxNotTalkingTexture);
            //foxTalking = true;
        }
        StartCoroutine(setText(text));
    }

    IEnumerator setText(string text) {
        sharedText.text = "";
        canMoveToNext = false;
        foreach (char c in text) {            
            sharedText.text += c;
            yield return new WaitForSeconds(writeDelay);
        }
        canMoveToNext = true;
        yield return null;
    }

    private void setTexture(GameObject foxOrRacoon,Material toAply) {
        foxOrRacoon.GetComponent<MeshRenderer>().material = toAply;
    }

    void InitDictionary() {
        int max = (foxDialog.Length > racoonDialog.Length) ? foxDialog.Length : racoonDialog.Length;
        for (int i = 0; i < max; i++) {
            if (foxTalking) {
                if (i< foxDialog.Length) {
                    textOrder.Add(new Tuple<bool, string>(true, foxDialog[i]));
                }
                if (i < racoonDialog.Length) {
                    textOrder.Add(new Tuple<bool, string>(false, racoonDialog[i]));
                }
               
            } else {
                if (i < racoonDialog.Length) {
                    textOrder.Add(new Tuple<bool, string>(false, racoonDialog[i]));
                }
                if (i < foxDialog.Length) {
                    textOrder.Add(new Tuple<bool, string>(true, foxDialog[i]));
                }
                    
                
            }
        }
    }
}
