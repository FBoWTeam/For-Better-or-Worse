using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public static GameManager gameManager;
    public int baseHP;
    public int hp;

    [HideInInspector]
    public GameObject player1;
    [HideInInspector]
    public GameObject player2;

    public GameObject linkDeformation;

	public bool player1HasTaunt, player2HasTaunt;
    public int tauntRange = 10;

	public enum PowerType
    {
        None,

        LargeOrb,
        Vortex,
        LeechLife,
        Slug,
        Shield,

        Ice,
        Fire,
        Electric,
        Weakness,

        Elemental = Ice | Fire | Electric | Weakness,
        Behavioral = LargeOrb | Vortex | LeechLife | Slug | Shield
    }


    // Start is called before the first frame update
    void Awake()
    {
        if (gameManager == null)
        {
            gameManager = this;
        }
        else if (gameManager != this)
        {
            Destroy(this);
        }
        DontDestroyOnLoad(gameManager);

        player1 = GameObject.Find("Player1");
        player2 = GameObject.Find("Player2");
        linkDeformation = GameObject.Find("Deformation");

        hp = baseHP;
    }

    // Update is called once per frame
    void Update()
    {

    }
    /// <summary>
    /// Handle taking damage from an Ennemy or other things
    /// </summary>
    /// <param name="impactDamage"></param>
    public void TakeDamage(int damage)
    {
        hp -= damage;
        if (hp <= 0)
        {
            Debug.Log("DED");
        }
    }
}
