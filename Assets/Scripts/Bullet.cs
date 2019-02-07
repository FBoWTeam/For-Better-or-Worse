﻿using UnityEngine;

public class Bullet : MonoBehaviour {

    private Transform target;
    public float speed = 70f;

    int turretDamage;

    public void Seek(Transform _target,int damage){
        turretDamage = damage;
        target = _target;
    }

	// Update is called once per frame
	void Update () {
		if(target == null){
            Destroy(gameObject);//si la balle n'a plus de cible, elle est détruite
            return;
        }

        Vector3 dir = target.position - transform.position;// dir correspond au vecteur allant de la balle jusqu'à la cible
        float distanceThisFrame = speed * Time.deltaTime;

        if(dir.magnitude <= distanceThisFrame)//lorsque la distance avec la cible est inférieure ou égale à 0 lance la fonction HitTarget
        {
            HitTarget();
            return;
        }

        transform.Translate(dir.normalized * distanceThisFrame, Space.World);//fait se dépalcer la balle vers sa cible

	}

    void HitTarget(){
        target.gameObject.GetComponent<Unit>().TakeDamage(turretDamage);
        //Destroy(target.gameObject);//détruit la cible
        Destroy(gameObject);//détruit la balle
    }
}
