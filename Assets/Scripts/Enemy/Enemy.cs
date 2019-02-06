using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Enemy : MonoBehaviour
{
    public enum Movement { Immobile, Classic };
    public Movement movement;

    public enum Skill {
        Impact,
        AOE,
        Distance,
        Bloc,
        MudThrow,
        Vortex,
        Inverse,
        Mentaliste,
        Shield,
        PreciousWater,
        Rooting,
        Silence,
        Magnet
    };

    // Est-ce que les skill font des damages différentes
    // si oui...
    //TODO Cacher/Afficher les variable de dam
    public int impactDamage = 5;

    public Skill skillOne;
    public Skill skillTwo;

    public enum Bonus { Mirror}
    public Bonus bonusOne;
    public Bonus bonusTwo;


    private bool coliding = false;
    




    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision) {
        if (CompareTag("Player")) {
            coliding = true;
        }
    }

    #region Skill
    /// <summary>
    /// Activate the skill passed in parameters
    /// </summary>
    /// <param name="skill"></param>
    void doSkill(Skill skill) {
        switch (skill) {
            case Skill.Impact:
                //dégat a l'impact
                if (coliding) {
                    GameManager.gameManager.takeDamage(impactDamage);
                }
                break;
            case Skill.AOE:
                //Colliders d'un rayon parametrable
                break;
            case Skill.Distance:
                break;
            case Skill.Bloc:
                break;
            case Skill.MudThrow:
                break;
            case Skill.Vortex:
                break;
            case Skill.Inverse:
                break;
            case Skill.Mentaliste:
                break;
            case Skill.Shield:
                break;
            case Skill.PreciousWater:
                break;
            case Skill.Rooting:
                break;
            case Skill.Silence:
                break;
            case Skill.Magnet:
                break;
            default:
                break;
        }
    }
    #endregion 

}
