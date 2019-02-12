using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbHitter : MonoBehaviour
{
    GameObject orb;

    public float hitZone;

    bool canHit;

    void Start()
    {
		orb = GameObject.Find("Orb");
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
                if (Input.GetKeyDown(KeyCode.E) || Input.GetAxisRaw("OrbHitterP1") != 0)
                {
                    if (!orb.GetComponent<OrbController>().toPlayer2)
                    {
                        orb.GetComponent<OrbController>().toPlayer2 = true;
                    }
                }
            }
        }
        else
        {
            if (canHit)
            {
                if (Input.GetKeyDown(KeyCode.Keypad0) || Input.GetAxisRaw("OrbHitterP2") != 0)
                {
                    if (orb.GetComponent<OrbController>().toPlayer2)
                    {
                        orb.GetComponent<OrbController>().toPlayer2 = false;
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
