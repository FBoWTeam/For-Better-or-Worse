using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbHitter : MonoBehaviour
{
    GameObject orb;

    [Header("[Parameters]")]
    [Tooltip("represents the orb hitting range")]
    public float hitZone;
    public float accelerationFactor;

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
                if (!orb.GetComponent<OrbController>().toPlayer2)
                {
                    if (Input.GetKeyDown(KeyCode.E) || Input.GetAxisRaw("OrbHitterP1") != 0)
                    {
                        orb.GetComponent<OrbController>().toPlayer2 = true;
                        orb.GetComponent<OrbController>().speed += accelerationFactor;
                    }
                }
            }
        }
        else
        {
            if (canHit)
            {
                if (orb.GetComponent<OrbController>().toPlayer2)
                {
                    if (Input.GetKeyDown(KeyCode.Keypad0) || Input.GetAxisRaw("OrbHitterP2") != 0)
                    {
                        orb.GetComponent<OrbController>().toPlayer2 = false;
                        orb.GetComponent<OrbController>().speed += accelerationFactor;
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
