using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelMenu : MonoBehaviour
{

    List<Vector3> pos = new List<Vector3>();
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

        pos.Add(new Vector3(260 , 555, 0));//0-tower
        pos.Add(new Vector3(345 , 555, 0));
        pos.Add(new Vector3(385 , 495, 0));
        pos.Add(new Vector3(470 , 495, 0));//3-fire
        pos.Add(new Vector3(580, 485, 0));//4-lake
        pos.Add(new Vector3(642 , 455, 0));//5-croco
        pos.Add(new Vector3(700 , 460, 0));
        pos.Add(new Vector3(780 , 525, 0));//7-cave
        pos.Add(new Vector3(855 , 525, 0));//8-rune
        pos.Add(new Vector3(825 , 455, 0));//9-jag

        /*pos.Add(new Vector3(320 , 425, 0));
        pos.Add(new Vector3(375 , 355, 0));
        pos.Add(new Vector3(450 , 255, 0));//12-tree
        pos.Add(new Vector3(490 , 155, 0));
        pos.Add(new Vector3(425 , 135, 0));//14-rock
        pos.Add(new Vector3(370 , 65, 0));
        pos.Add(new Vector3(300 , 55, 0));//16-eagle
        pos.Add(new Vector3(205 , 65, 0));
        pos.Add(new Vector3(160 , 105, 0));//18-moutain
        pos.Add(new Vector3(200 , 160, 0));
        pos.Add(new Vector3(160 , 195, 0));
        pos.Add(new Vector3(50 , -90, 0));//21-
        pos.Add(new Vector3(80 , -145, 0));
        pos.Add(new Vector3(30 , -135, 0));//23-bear
        pos.Add(new Vector3(-30 , -180, 0));
        pos.Add(new Vector3(0 , -250, 0));
        pos.Add(new Vector3(-60 ,-265, 0));//26-houses
        pos.Add(new Vector3(-135 ,-260 , 0));
        pos.Add(new Vector3(-135 ,-215 , 0));
        pos.Add(new Vector3(-115 ,-165 , 0));
        pos.Add(new Vector3(-170 ,-100 , 0));//30-fishing rod
        pos.Add(new Vector3(-215 ,-185 , 0));//31-dog
        pos.Add(new Vector3(-320 ,-190 , 0));
        pos.Add(new Vector3(-330 ,-155 , 0));//33-palm tree
        pos.Add(new Vector3(-335 ,-120 , 0));
        pos.Add(new Vector3(-390 ,-125 , 0));
        pos.Add(new Vector3(-425 ,-165 , 0));
        pos.Add(new Vector3(-460 ,-135 , 0));//37-temple
        pos.Add(new Vector3(-495 ,-85 , 0));//38-round*/

        indexPI.Add(0);
        //indexPI.Add(3);
        //indexPI.Add(4);
        //indexPI.Add(5);
        indexPI.Add(7);
        //indexPI.Add(8);
        indexPI.Add(9);
        /*indexPI.Add(12);
        indexPI.Add(14);
        indexPI.Add(16);
        indexPI.Add(18);
        indexPI.Add(21);
        indexPI.Add(23);
        indexPI.Add(26);
        indexPI.Add(30);
        indexPI.Add(31);
        indexPI.Add(33);
        indexPI.Add(37);
        indexPI.Add(38);*/

        transform.position = pos[0];
    }

    // Update is called once per frame
    void Update()
    {
        if (!isMoving)
        {
            if ( (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.DownArrow)) && destPIIndex < indexPI.Capacity)
            {
                destPIIndex++;
                destPosIndex = indexPI[destPIIndex];
            }

            if (curentPosIndex < destPosIndex)
            {
                StartCoroutine(GoToNextPos());
            }

            if ( (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.UpArrow)) && destPIIndex > 0)
            {
                destPIIndex--;
                destPosIndex = indexPI[destPIIndex];
            }

            if (curentPosIndex > destPosIndex)
            {
                StartCoroutine(GoToPreviousPos());
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
