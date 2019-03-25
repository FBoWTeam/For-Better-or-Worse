using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RootBranch : MonoBehaviour
{
    LineRenderer branch;
    public int nbPoint = 10;
    public GameObject targetPlayer;
    public Vector3[] testPoints = new Vector3[10];
    [HideInInspector]
    public float rootTime;

    Vector3[] points;

    // Start is called before the first frame update
    void Start()
    {
        branch = GetComponent<LineRenderer>();
        points = new Vector3[nbPoint];
        StartCoroutine(CreateRootBranchCoroutine(targetPlayer, nbPoint));
    }

    // Update is called once per frame
    void Update()
    {

    }
    
    AnimationCurve CurveForBranch(int nbPointCurve)
    {
        AnimationCurve curve = new AnimationCurve();
        for (int i = 0; i < nbPointCurve; i++)
        {
            if (i % 2 == 0)
                curve.AddKey(i / (nbPointCurve - 1), 0.3f * Mathf.Sin((Mathf.PI / 3) * (1 + i / (nbPointCurve - 1))));
            else
                curve.AddKey(i / (nbPointCurve - 1), 0.01f * Mathf.Sin((Mathf.PI / 3) * (1 + i / (nbPointCurve - 1))));
        }
        return curve;
    }


    IEnumerator CreateRootBranchCoroutine(GameObject targetPlayer, int nbPoint)
    {
        Vector3 posPlayer = targetPlayer.transform.position;
        List<Vector3> pointsToUseList = new List<Vector3>();

        AnimationCurve curveToUse;

        for (int i = 0; i < nbPoint; i++)
        {
            points[i] = new Vector3(posPlayer.x + Mathf.Cos(Mathf.PI * ((float)i / ((float)nbPoint - 1f))) * Mathf.Cos(Mathf.PI * (targetPlayer.transform.localEulerAngles.y / 180f)) / 2f,
                                    Mathf.Sin(Mathf.PI * i / (nbPoint - 1)) / 2f,
                                    posPlayer.z + Mathf.Cos(Mathf.PI * ((float)i / ((float)nbPoint - 1f))) * Mathf.Cos(Mathf.PI * (targetPlayer.transform.localEulerAngles.y / 180f) + Mathf.PI / 2f) / 2);

            pointsToUseList.Add(points[i]);            
            Vector3[] pointsToUse = pointsToUseList.ToArray();
            pointsToUse.CopyTo(testPoints, 0);
            branch.positionCount = i + 1;
            branch.SetPositions(pointsToUse);
            if (i > 1)
            {
                //print(pointsToUse);
                curveToUse = CurveForBranch(2 * i - 1);
                branch.widthCurve = curveToUse;
                yield return new WaitForSecondsRealtime(1f / (float)nbPoint);
            }                
            
        }

        yield return new WaitForSecondsRealtime(rootTime - 1.0f);

        Destroy(gameObject);
    }

}