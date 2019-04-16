using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class LevelMenu : MonoBehaviour
{

    List<Vector3> pos = new List<Vector3>();
    List<int> indexPI = new List<int>();
    public int curentPosIndex;
    public int curentPIIndex;
    public int destPosIndex;
    public int destPIIndex;

    public bool isMoving;

    // Start is called before the first frame update
    void Start()
    {
        curentPIIndex = 0;
        destPIIndex = 0;
        curentPosIndex = 0;
        destPosIndex = 0;
        isMoving = false;

        /*pos.Add(new Vector3(202, 445, 0));//0-tower
        pos.Add(new Vector3(265, 445, 0));
        pos.Add(new Vector3(300, 390, 0));
        pos.Add(new Vector3(370, 390, 0));//3-fire
        pos.Add(new Vector3(450, 375, 0));//4-lake
        pos.Add(new Vector3(500, 360, 0));//5-croco
        pos.Add(new Vector3(540, 355, 0));
        pos.Add(new Vector3(610, 410, 0));//7-cave
        pos.Add(new Vector3(665, 405, 0));//8-rune
        pos.Add(new Vector3(650, 360, 0));//9-jag*/

        pos.Add(new Vector3(255, 570, 0));//0-tower
        pos.Add(new Vector3(340, 570, 0));
        pos.Add(new Vector3(390, 500, 0));
        pos.Add(new Vector3(470, 500, 0));//3-fire
        pos.Add(new Vector3(580, 480, 0));//4-lake
        pos.Add(new Vector3(640, 465, 0));//5-croco
        pos.Add(new Vector3(680, 455, 0));
        pos.Add(new Vector3(780, 530, 0));//7-cave
        pos.Add(new Vector3(850, 520, 0));//8-rune
        pos.Add(new Vector3(835, 455, 0));//9-jag
        
        indexPI.Add(0);
        indexPI.Add(7);
        indexPI.Add(8);
        indexPI.Add(9);

        transform.position = pos[0];
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
            transform.position = Vector3.Lerp(pos[curentPosIndex], pos[curentPosIndex + 1], t);
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
            transform.position = Vector3.Lerp(pos[curentPosIndex], pos[curentPosIndex - 1], t);
            t += 0.02f;
            yield return new WaitForSecondsRealtime(0.01f);
        }

        curentPosIndex--;
        isMoving = false;
    }


}
