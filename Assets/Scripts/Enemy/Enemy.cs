using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Enemy : MonoBehaviour
{
    public enum Movement { Immobile, Classic };
    public Movement movement;

    public enum Skill { Impact, Distance };
    public Skill skillOne;
    public Skill skillTwo;

    public enum Bonus { Mirror}
    public Bonus bonusOne;
    public Bonus bonusTwo;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
