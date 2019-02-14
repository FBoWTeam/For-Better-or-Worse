using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(SphereCollider))]
public class EnemySkill : MonoBehaviour
{
    public enum Skill {
        Impact,
        AOE,
        Distance,
        /*Bloc,
        MudThrow,
        Vortex,
        Inverse,
        Mentaliste,
        Shield,
        PreciousWater,
        Rooting,
        Silence,
        Magnet,*/
        None,
    };

    public Skill skillOne;
    //public Skill skillTwo;

    
    
    #region ImpactFields
    [DrawIf(new string[] { "skillOne"}, Skill.Impact)]
    public int impactDamage = 5;
    [DrawIf(new string[] { "skillOne"}, Skill.Impact)]
    public float impactCooldown = 3f;

    bool canAttack = true;
    #endregion

    //evenement avec aucun type de retour mais 1 parametre 
    public event Action<GameObject,Skill> inRangeEvent;


    Material myMat;
    float nextAttack = 0f;
    SphereCollider rangeCollider;

    private void Awake() {
        //set the sphereCollider to trigger just to be sure
        rangeCollider = GetComponent<SphereCollider>();
        rangeCollider.isTrigger = true;

        inRangeEvent += onPlayerinRange;
        myMat = GetComponent<Renderer>().material;
    }

    private void Update() {
        //si in range attacker

    }

    //Dammge player on coll
    private void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.CompareTag("Player")) {
            GameManager.gameManager.takeDamage(impactDamage);
        }
    }


    //Triiger inRangeEvent
    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            inRangeEvent(other.gameObject,skillOne);
        }
    }

    private void OnTriggerStay(Collider other) {
        if (other.CompareTag("Player")) {
            inRangeEvent(other.gameObject, skillOne);
        }
    }


    void onPlayerinRange(GameObject target,Skill skill) {
        //myMat.color = Color.red;
        //SET target = target

        switch (skill) {
            case Skill.Impact:
                
                if (Time.time > nextAttack) {
                    myMat.color = Color.red;
                    StartCoroutine("Impact",target.transform);
                    Debug.Log("Lancer animation d'attaque "+target.tag);
                    nextAttack = Time.time + impactCooldown;
                }
                
                break;
            case Skill.AOE:
                break;
            case Skill.Distance:
                break;
            case Skill.None:
                break;
            default:
                break;
        }
    }


    IEnumerator Impact(Transform target) {
      
        Vector3 originalPosition = transform.position;
        Vector3 dirToTarget = (target.position - transform.position).normalized;
        Vector3 attackPosition = target.position + dirToTarget ;
        Debug.Log(attackPosition);
        float percent = 0;

        

        while (percent <= 1) {

            percent += Time.deltaTime * impactCooldown;
            float interpolation = (-Mathf.Pow(percent, 2) + percent) * 4;
            transform.position = Vector3.Lerp(originalPosition, attackPosition, interpolation);

            yield return null;
        }

        myMat.color = Color.white ;
        
    }

}
