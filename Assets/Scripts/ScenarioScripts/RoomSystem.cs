using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomSystem : MonoBehaviour
{
    [SerializeField]
    [Tooltip("number of player present in the current room")]
    private int numberPlayerPresent;

    [Tooltip("if the room is cleared or not")]
    [SerializeField]
    private bool roomCleared;
    [SerializeField]
    private bool playerLeft;

    public GameObject[] doorsToClose;
    public GameObject[] doorsToOpen;

    [Tooltip("game object coontaining the enemies of the room")]
    public GameObject[] enemies;
    
    void Update()
    {
        if (!roomCleared)
        {
            if (enemies.Length == 0)
            {
                roomCleared = true;
                OpenDoors();
            }
        }
        else if (numberPlayerPresent == 0 && !playerLeft)
        {
            playerLeft = true;
            CloseDoors();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        numberPlayerPresent++;
    }

    private void OnTriggerExit(Collider other)
    {
        numberPlayerPresent--;
    }

    void CloseDoors()
    {
        for (int i = 0; i < doorsToClose.Length; i++)
        {
            doorsToClose[i].GetComponent<Door>().Deactivate();
        }
    }

    void OpenDoors()
    {
        for (int i = 0; i < doorsToOpen.Length; i++)
        {
            doorsToOpen[i].GetComponent<Door>().Activate();
        }
    }

}
