using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScoreController : MonoBehaviour
{
	public Animation fader;
	public Animation bgm;
	bool active;

	public GameObject buttonA;
	public GameObject buttonB;
	public float timeBeforeActiveControl;
    private float timer;

	public void Awake()
	{
		active = false;
		StartCoroutine(FadeIn());
	}

	// Start is called before the first frame update
	void Start()
    {
        timer = timeBeforeActiveControl;
    }

    // Update is called once per frame
    void Update()
    {
		if(active)
		{
			if (timer < 0)
			{
				buttonA.SetActive(true);
				if (ScoreManager.scoreManager.gameMode == ScoreManager.GameMode.Arena)
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
						if(GameData.previousScene == 9)
						{
							StartCoroutine(FadeOut(11));
						}
						else
						{
							StartCoroutine(FadeOut(3));
						}
					}
					else
					{
						Destroy(ScoreManager.scoreManager.gameObject);
						Destroy(ScoreManager.scoreManager);
						StartCoroutine(FadeOut(12));
					}
				}

				//Button B
				else if (Input.GetKeyDown(KeyCode.Joystick1Button1) || Input.GetKeyDown(KeyCode.Escape))
				{
					if (ScoreManager.scoreManager.gameMode == ScoreManager.GameMode.Arena)
					{
						Destroy(ScoreManager.scoreManager.gameObject);
						Destroy(ScoreManager.scoreManager);
						StartCoroutine(FadeOut(2));
					}
				}
			}
			else
			{
				timer -= Time.fixedDeltaTime;
			}
		}
	}

	IEnumerator FadeIn()
	{
		fader.Play();
		yield return new WaitForSeconds(0.1f);
		yield return new WaitUntil(() => fader.isPlaying == false);
		active = true;
	}

	IEnumerator FadeOut(int sceneToLoad)
	{
		active = false;
		fader.clip = fader.GetClip("FadeOut");
		fader.Play();
		bgm.Play();
		yield return new WaitForSeconds(0.1f);
		yield return new WaitUntil(() => fader.isPlaying == false);
		SceneManager.LoadScene(sceneToLoad);
	}
}
