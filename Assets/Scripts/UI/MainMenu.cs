using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    Text currentText;

    #region Resolution variable
    int indexResText;
    //int nbRes = 4;
    public string[] listResText = new string[4];

    #endregion

    private void Start()
    {
        currentText = GetComponent<Text>();
        indexResText = 3;
        listResText[0] = "640x480";
        listResText[1] = "720x480";
        listResText[2] = "1366x768";
        listResText[3] = "1920x1080";
    }

    public void PlayGame ()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void QuitGame ()
    {
        print("QUIT");
        Application.Quit();
    }


    public void UpdateTextSlider(float value)
    {
        currentText.text = Mathf.RoundToInt(100 * value) + "%";
    }


    public void NextRes()
    {
        if (indexResText < 3)
        {
            indexResText++;
            currentText.text = listResText[indexResText];
        }
    }

    public void PreviousRes()
    {
        if (indexResText > 0)
        { 
            indexResText--;
            currentText.text = listResText[indexResText];
        }
    }


}
