using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{ 
    Text percentageText;
    Text resText;
    int indexResText;
    //int nbRes = 4;

    Text[] listResText = new Text[4];

    private void Start()
    {
        percentageText = GetComponent<Text>();
        listResText = { "640x480", "720x480", "1366x768", "1920x1080"};
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


    public void UpdateText(float value)
    {
        percentageText.text = Mathf.RoundToInt(100 * value) + "%";
    }


    public void NextRes()
    {
        resText = 
    }


}
