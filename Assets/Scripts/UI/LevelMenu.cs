using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class LevelMenu : MonoBehaviour
{
	public Animation fader;
	bool active;

	public List<GameObject> pos = new List<GameObject>();
    List<int> indexPI = new List<int>();
    int curentPosIndex;
    int curentPIIndex;
    int destPosIndex;
    int destPIIndex;

    public bool isMoving;

	public SoundEmitter soundEmitter;

	public void Awake()
	{
		active = false;
		StartCoroutine(FadeIn());
	}

	// Start is called before the first frame update
	void Start()
    {
        curentPIIndex = 0;
        destPIIndex = 0;
        curentPosIndex = 0;
        destPosIndex = 0;
        isMoving = false;

		indexPI.Add(0);
		indexPI.Add(3);
		indexPI.Add(4);
		indexPI.Add(5);
		indexPI.Add(7);
        indexPI.Add(8);
        indexPI.Add(9);

		switch(GameData.previousScene)
		{
			case 2:
				curentPIIndex = 0;
				destPIIndex = 0;
				curentPosIndex = 0;
				destPosIndex = 0;
				transform.position = pos[0].transform.position;
				break;
			case 6:
				curentPIIndex = 0;
				destPIIndex = 0;
				curentPosIndex = 0;
				destPosIndex = 0;
				transform.position = pos[0].transform.position;
				break;
			case 7:
				curentPIIndex = 4;
				destPIIndex = 4;
				curentPosIndex = 7;
				destPosIndex = 7;
				transform.position = pos[7].transform.position;
				break;
			case 8:
				curentPIIndex = 5;
				destPIIndex = 5;
				curentPosIndex = 8;
				destPosIndex = 8;
				transform.position = pos[8].transform.position;
				break;
			case 9:
				curentPIIndex = 6;
				destPIIndex = 6;
				curentPosIndex = 9;
				destPosIndex = 9;
				transform.position = pos[9].transform.position;
				break;
			default:
				curentPIIndex = 0;
				destPIIndex = 0;
				curentPosIndex = 0;
				destPosIndex = 0;
				transform.position = pos[0].transform.position;
				break;
		}


    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(transform.position);
        if (!isMoving && active)
        {
            if (curentPosIndex == destPosIndex)
            {
				pos[curentPosIndex].transform.GetChild(0).gameObject.SetActive(true);
                if ((Input.GetAxis("HorizontalP1") > Input.GetAxis("VerticalP1")) && destPIIndex < indexPI.Count - 1)
				{
					pos[curentPosIndex].transform.GetChild(0).gameObject.SetActive(false);
					destPIIndex++;
                    destPosIndex = indexPI[destPIIndex];
                }
				else if ((Input.GetAxis("HorizontalP1") > Input.GetAxis("VerticalP1")) && destPIIndex >= indexPI.Count - 1)
				{
					soundEmitter.PlaySound(1);
				}

				if ((Input.GetAxis("HorizontalP1") < Input.GetAxis("VerticalP1")) && destPIIndex > 0)
				{
					pos[curentPosIndex].transform.GetChild(0).gameObject.SetActive(false);
					destPIIndex--;
                    destPosIndex = indexPI[destPIIndex];
				}
				else if ((Input.GetAxis("HorizontalP1") < Input.GetAxis("VerticalP1")) && destPIIndex <= 0)
				{
					soundEmitter.PlaySound(1);
				}

				if (Input.GetKeyDown(KeyCode.Joystick1Button0) || Input.GetKeyDown(KeyCode.Space))
				{
					switch (curentPIIndex)
                    {
                        case 0:
							soundEmitter.PlaySound(0);
							GameData.previousScene = 5;
							GameData.nextSceneToLoad = 5;
							StartCoroutine(FadeOut(4));
							break;
						case 1:
							soundEmitter.PlaySound(1);
							break;
						case 2:
							soundEmitter.PlaySound(1);
							break;
						case 3:
							soundEmitter.PlaySound(1);
							break;
						case 4:
							soundEmitter.PlaySound(0);
							GameData.previousScene = 7;
							GameData.nextSceneToLoad = 7;
							StartCoroutine(FadeOut(4));
							break;
                        case 5:
							soundEmitter.PlaySound(0);
							GameData.previousScene = 8;
							GameData.nextSceneToLoad = 8;
							StartCoroutine(FadeOut(4));
							break;
                        case 6:
							soundEmitter.PlaySound(0);
							GameData.previousScene = 9;
							GameData.nextSceneToLoad = 9;
							StartCoroutine(FadeOut(4));
							break;
                    }
                }

                if (Input.GetKeyDown(KeyCode.Joystick1Button1) || Input.GetKeyDown(KeyCode.Escape))
				{
					soundEmitter.PlaySound(0);
					StartCoroutine(FadeOut(2));
				}

            }

            if (curentPosIndex < destPosIndex)
            {
                StartCoroutine(GoToNextPos());
                curentPIIndex = destPIIndex;
            }

            if (curentPosIndex > destPosIndex)
            {
                StartCoroutine(GoToPreviousPos());
                curentPIIndex = destPIIndex;
            }
            
        }
    }

    
    IEnumerator GoToNextPos()
    {
        isMoving = true;
        float t = 0f;

        while(t<1)
        {
			transform.position = Vector3.Lerp(pos[curentPosIndex].transform.position, pos[curentPosIndex + 1].transform.position, t);
            t += 0.02f;
            yield return new WaitForSecondsRealtime(0.01f);
        }

        curentPosIndex++;
        isMoving = false;
    }


    IEnumerator GoToPreviousPos()
    {
        isMoving = true;
        float t = 0f;

        while (t < 1)
        {
			transform.position = Vector3.Lerp(pos[curentPosIndex].transform.position, pos[curentPosIndex - 1].transform.position, t);

			t += 0.02f;
            yield return new WaitForSecondsRealtime(0.01f);
        }

        curentPosIndex--;
        isMoving = false;
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
		yield return new WaitForSeconds(0.1f);
		yield return new WaitUntil(() => fader.isPlaying == false);
		SceneManager.LoadScene(sceneToLoad);
	}
}
