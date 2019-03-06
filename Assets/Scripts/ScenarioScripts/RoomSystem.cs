using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomSystem : MonoBehaviour, IActivable
{
    [Header("Private variables  => Can't touch this")]
    [SerializeField]
    [Tooltip("number of player present in the current room")]
    private int numberPlayerPresent;

    private int numberEnemiesPresent;

    [Tooltip("if the room is cleared or not")]
    [SerializeField]
    private bool roomCleared;
    [SerializeField]
    [Tooltip("indicates if the players left the room or not")]
    private bool playerLeft;

    private bool enemiesLeft;

    [Header("Public variables  => Can touch this")]
    public GameObject[] doorsToClose;

    [Tooltip("game object coontaining the enemies of the room")]
    public List<GameObject> enemies;
    [Tooltip("objects to activate when the room is cleared")]
    public GameObject[] objectsToActivate;

    public bool isActive { get; set; }

    void Update()
    {
        CleanNullInEnemyList();
        if (!roomCleared && enemies.Count == 0)
        {
            this.Activate();
        }
        else if (numberPlayerPresent == 0 && !playerLeft && numberEnemiesPresent == 0 && !enemiesLeft)
        {
            this.Deactivate();
        }
    }


    public void Activate()
    {
        isActive = true;
        roomCleared = true;
        for (int i = 0; i < objectsToActivate.Length; i++)
        {
            objectsToActivate[i].GetComponent<IActivable>().Activate();
        }
    }

    public void Deactivate()
    {
        enemiesLeft = true;
        playerLeft = true;
        CloseDoors();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerLeft = false;
            numberPlayerPresent++;
        }
        if (other.CompareTag("Enemy"))
        {
            enemiesLeft = false;
            numberEnemiesPresent++;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            numberPlayerPresent--;
        }
        if (other.CompareTag("Enemy"))
        {
            numberEnemiesPresent--;
        }
    }
    
    void CloseDoors()
    {
        for (int i = 0; i < doorsToClose.Length; i++)
        {
            doorsToClose[i].GetComponent<Door>().Deactivate();
        }
    }

    public void CleanNullInEnemyList()
    {
        if (enemies.Exists(x => x.Equals(null)))
        {
            enemies.RemoveAll(x => x.Equals(null));
        }
    }
    
}
