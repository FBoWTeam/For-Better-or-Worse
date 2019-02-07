using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public static GameManager gameManager;
    public int pv = 100;

    // Start is called before the first frame update
    void Awake()
    {
        if(gameManager == null) {
            
            gameManager = this;
        } else if (gameManager != this) {
            Destroy(this);
        }
        DontDestroyOnLoad(gameManager);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    /// <summary>
    /// Handle taking damage from an Ennemy or other things
    /// </summary>
    /// <param name="impactDamage"></param>
    public void takeDamage(int damage) {
        pv -= damage;
        if (pv <= 0) {
            throw new NotImplementedException();
        }
    }
}
