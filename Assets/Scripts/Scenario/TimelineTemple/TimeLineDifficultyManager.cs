using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeLineDifficultyManager : MonoBehaviour
{

    //save the chosen difficulty from the menu and set it to the 'better' mode for the timeline
    void OnEnable()
    {
        GameManager.gameManager.orb.GetComponent<OrbController>().canHitPlayer = false;
    }

    //on the disable event of the gameObject, reset the difficulty to the original one
    private void OnDisable()
    {
        if (GameData.worseModeActivated)
        {
            GameManager.gameManager.orb.GetComponent<OrbController>().canHitPlayer = true;
        }
    }
}
