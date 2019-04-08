using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Audio;
using TMPro;
using UnityEngine.EventSystems;


public class MainMenu : MonoBehaviour
{
    TextMeshProUGUI currentTextPro;

    #region Resolution variable
    int currentIndexRes;
    List<string> resOptions = new List<string>();
    Resolution[] resolutions;

    #endregion

    #region Volume variable
    AudioMixer mixer;

    #endregion

    #region Scene variable
    public int sceneToLoad;

    #endregion

    EventSystem eS;

    private void Start()
    {
        currentTextPro = GetComponent<TextMeshProUGUI>();

        eS = EventSystem.current;

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
        {
            currentTextPro.SetText(resOptions[currentIndexRes]);
        }
    }

    public void LoadScene ()
    {
        SceneManager.LoadScene(sceneToLoad);
        //eS.firstSelectedGameObject = ;
    }

    public void QuitGame ()
    {
        Debug.Log("QUIT");
        Application.Quit();
    }


    public void UpdateTextSlider(float value)
    {
        currentTextPro.SetText(Mathf.RoundToInt(100 * value) + "%");
    }

    #region Resolution

    public void NextRes()
    {
        if (currentIndexRes < resOptions.Capacity - 1)
        {
            currentIndexRes++;
            currentTextPro.SetText(resOptions[currentIndexRes]);
            SetResolution(currentIndexRes);
        }
    }

    public void PreviousRes()
    {
        if (currentIndexRes > 0)
        {
            currentIndexRes--;
            currentTextPro.SetText(resOptions[currentIndexRes]);
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
        Debug.Log(isFullscreen);
        Screen.fullScreen = isFullscreen; 
    }

    
}
