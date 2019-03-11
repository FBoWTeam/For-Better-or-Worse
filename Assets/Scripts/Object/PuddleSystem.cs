using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuddleSystem : MonoBehaviour
{
    [Header("[Puddle Effect Editing]")]
    public GameManager.PuddleType editingPuddleType;


    [Header("[Puddle common param]")]
    public float size;


    [DrawIf(new string[] { "editingPuddleType" }, GameManager.PuddleType.Slug)]
    public float slugLifeTime;
    [Tooltip("Amount in percentage of the slow")]
    [Range(1, 100)]
    public float slowAmount;


    [DrawIf(new string[] { "editingPuddleType" }, GameManager.PuddleType.Poison)]
    public float poisonDamage;


    [DrawIf(new string[] { "editingPuddleType" }, GameManager.PuddleType.Mud)]
    public float radius;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
