using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class LevelMenu : MonoBehaviour
{

    public List<GameObject> pos = new List<GameObject>();
    List<int> indexPI = new List<int>();
    int curentPosIndex;
    int curentPIIndex;
    int destPosIndex;
    int destPIIndex;

    public bool isMoving;

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

		transform.position = pos[0].transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(transform.position);
        if (!isMoving)
        {
            if (curentPosIndex == destPosIndex)
            {
                if ((Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.DownArrow)
                     || Input.GetAxis("DeformP1X") > Input.GetAxis("DeformP1Z")
                     || Input.GetAxis("HorizontalP1") > Input.GetAxis("VerticalP1"))
                    && destPIIndex < indexPI.Count - 1)
                {
                    destPIIndex++;
                    destPosIndex = indexPI[destPIIndex];
                }
                
                if ((Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.UpArrow)
                     || Input.GetAxis("DeformP1X") < Input.GetAxis("DeformP1Z")
                     || Input.GetAxis("HorizontalP1") < Input.GetAxis("VerticalP1"))
                    && destPIIndex > 0)
                {
                    destPIIndex--;
                    destPosIndex = indexPI[destPIIndex];
                }

                if (Input.GetKeyDown(KeyCode.Joystick1Button0) || Input.GetKeyDown(KeyCode.Joystick2Button0) || Input.GetKeyDown(KeyCode.Return))
                {
                    switch (curentPIIndex)
                    {
                        case 0:
							GameData.nextSceneToLoad = 5;
                            SceneManager.LoadScene(4);
                            break;
                        case 1:
							GameData.nextSceneToLoad = 7;
							SceneManager.LoadScene(4);
                            break;
                        case 2:
							GameData.nextSceneToLoad = 8;
							SceneManager.LoadScene(4);
                            break;
                        case 3:
							GameData.nextSceneToLoad = 9;
							SceneManager.LoadScene(4);
                            break;
                    }
                }

                if (Input.GetKeyDown(KeyCode.Joystick1Button1) || Input.GetKeyDown(KeyCode.Joystick2Button1) || Input.GetKeyDown(KeyCode.Backspace) || Input.GetKeyDown(KeyCode.Escape))
                {
                    SceneManager.LoadScene(2);
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


}
