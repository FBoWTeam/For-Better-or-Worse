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
				curentPIIndex = 1;
				destPIIndex = 1;
				curentPosIndex = 7;
				destPosIndex = 7;
				transform.position = pos[7].transform.position;
				break;
			case 8:
				curentPIIndex = 2;
				destPIIndex = 2;
				curentPosIndex = 8;
				destPosIndex = 8;
				transform.position = pos[8].transform.position;
				break;
			case 9:
				curentPIIndex = 3;
				destPIIndex = 3;
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
                if ((Input.GetAxis("HorizontalP1") > Input.GetAxis("VerticalP1")) && destPIIndex < indexPI.Count - 1)
                {
                    destPIIndex++;
                    destPosIndex = indexPI[destPIIndex];
                }
                
                if ((Input.GetAxis("HorizontalP1") < Input.GetAxis("VerticalP1")) && destPIIndex > 0)
                {
                    destPIIndex--;
                    destPosIndex = indexPI[destPIIndex];
                }

                if (Input.GetKeyDown(KeyCode.Joystick1Button0) || Input.GetKeyDown(KeyCode.Space))
				{
					soundEmitter.PlaySound(0);
					switch (curentPIIndex)
                    {
                        case 0:
							GameData.previousScene = 5;
							GameData.nextSceneToLoad = 5;
							StartCoroutine(FadeOut(4));
							break;
                        case 1:
							GameData.previousScene = 7;
							GameData.nextSceneToLoad = 7;
							StartCoroutine(FadeOut(4));
							break;
                        case 2:
							GameData.previousScene = 8;
							GameData.nextSceneToLoad = 8;
							StartCoroutine(FadeOut(4));
							break;
                        case 3:
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
