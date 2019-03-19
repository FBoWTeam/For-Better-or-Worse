using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{ 
    Text percentageText;

    private void Start()
    {
        percentageText = GetComponent<Text>();
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



}
