using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbHitter : MonoBehaviour
{
    public GameObject orb;

    public float hitZone;

    bool canHit;

    void Start()
    {
        canHit = false;
    }

    // Update is called once per frame
    void Update()
    {
        OrbHit();
    }

    /// <summary>
    /// function that let the players hit the ball
    /// </summary>
    void OrbHit()
    {
        checkHit();
        if (GetComponent<PlayerController>().player1)
        {
            if (canHit)
            {
                if (Input.GetKeyDown(KeyCode.E))
                {
                    if (!orb.GetComponent<OrbController2>().ascending)
                    {
                        orb.GetComponent<OrbController2>().ascending = true;
                    }
                }
            }
        }
        else
        {
            if (canHit)
            {
                if (Input.GetKeyDown(KeyCode.I))
                {
                    if (orb.GetComponent<OrbController2>().ascending)
                    {
                        orb.GetComponent<OrbController2>().ascending = false;
                    }
                }
            }
        }
    }

    /// <summary>
    /// function that check if the orb is close enough to let the player to hit the ball
    /// </summary>
    void checkHit()
    {
        if (Vector3.Distance(transform.position, orb.transform.position) < hitZone)
        {
            canHit = true;
        }
        else
        {
            canHit = false;
        }
    }


}
