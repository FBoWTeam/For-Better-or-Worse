using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScoreController : MonoBehaviour
{
	public GameObject buttonA;
	public GameObject buttonB;
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
			if(ScoreManager.scoreManager.gameMode == ScoreManager.GameMode.Arena)
			{
				buttonB.SetActive(true);
			}

            if (Input.GetKeyDown(KeyCode.Joystick1Button0) || Input.GetKeyDown(KeyCode.Space))
            {
                if (ScoreManager.scoreManager.gameMode == ScoreManager.GameMode.Story)
                {
					//Button A
					Destroy(ScoreManager.scoreManager.gameObject);
					Destroy(ScoreManager.scoreManager);
					SceneManager.LoadScene(3);
                }
                else
				{
					Destroy(ScoreManager.scoreManager.gameObject);
					Destroy(ScoreManager.scoreManager);
					SceneManager.LoadScene(12);
                }
            }

            //Button B
            else if (Input.GetKeyDown(KeyCode.Joystick1Button1) || Input.GetKeyDown(KeyCode.Space))
            {
				if (ScoreManager.scoreManager.gameMode == ScoreManager.GameMode.Arena)
				{
					Destroy(ScoreManager.scoreManager.gameObject);
					Destroy(ScoreManager.scoreManager);
					SceneManager.LoadScene(2);
				}
			}
        }
        else
        {
            timer -= Time.fixedDeltaTime;
        }

    }
}
