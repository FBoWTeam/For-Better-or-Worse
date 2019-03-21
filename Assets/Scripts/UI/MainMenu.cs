using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Audio;

public class MainMenu : MonoBehaviour
{
    Text currentText;

    #region Resolution variable
    int currentIndexRes;
    //public string[] listResText = new string[4];
    List<string> resOptions = new List<string>();
    Resolution[] resolutions;

    #endregion

    #region Volume Variable
    AudioMixer mixer;

    #endregion

    private void Start()
    {
        currentText = GetComponent<Text>();

        resolutions = Screen.resolutions;

        for (int i = 0; i<resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            resOptions.Add(option);

            if(resolutions[i].width == Screen.currentResolution.width &&
               resolutions[i].height == Screen.currentResolution.height)
            {
                currentIndexRes = i;
            }
        }

        if(this.name == "ValueRes")
            currentText.text = resOptions[currentIndexRes];
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

    #region Resolution

    public void NextRes()
    {
        if (currentIndexRes < resOptions.Capacity - 1)
        {
            currentIndexRes++;
            currentText.text = resOptions[currentIndexRes];
            SetResolution(currentIndexRes);
        }
    }

    public void PreviousRes()
    {
        if (currentIndexRes > 0)
        {
            currentIndexRes--;
            currentText.text = resOptions[currentIndexRes];
            SetResolution(currentIndexRes);
        }
    }

    public void SetResolution(int resolutionindex)
    {
        Resolution resolution = resolutions[resolutionindex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    #endregion

    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }

}
