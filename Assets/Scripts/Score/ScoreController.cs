using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScoreController : MonoBehaviour
{
    public GameObject buttonA;
    public float timeBeforeActiveControl;
    private float timer;
    // Start is called before the first frame update
    void Start()
    {
        timer = timeBeforeActiveControl;
    }

    // Update is called once per frame
    void Update()
    {
        if (timer < 0)
        {
            buttonA.SetActive(true);

            if (Input.GetKeyDown(KeyCode.Joystick1Button0) || Input.GetKeyDown(KeyCode.Joystick2Button0))
            {
                if (ScoreManager.scoreManager.gameMode == ScoreManager.GameMode.Story)
                {
                    //Button A
                    SceneManager.LoadScene(3);
                }
                else
                {
                    SceneManager.LoadScene(9);
                }
            }

            //Button B
            else if (Input.GetKeyDown(KeyCode.Joystick1Button1) || Input.GetKeyDown(KeyCode.Joystick2Button1))
            {
                SceneManager.LoadScene(2);
            }
        }
        else
        {
            timer -= Time.fixedDeltaTime;
        }

    }
}
