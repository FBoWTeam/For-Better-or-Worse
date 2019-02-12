using UnityEngine;

public class Bullet : MonoBehaviour {

    private Transform target;
    public float speed = 70f;
    Vector3 shootDir; 
    int turretDamage;

    public void Seek(Transform _target, int damage)
    {
        
        turretDamage = damage;
        target = _target;
        shootDir = target.position - transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
        if (target == null)
        {
            Destroy(gameObject);//si la balle n'a plus de cible, elle est détruite
            return;
        }

       
        float distanceThisFrame = speed * Time.deltaTime;

        if (shootDir.magnitude <= distanceThisFrame)//lorsque la distance avec la cible est inférieure ou égale à 0 lance la fonction HitTarget
        {
            HitTarget();
            return;
        }

        transform.Translate(shootDir.normalized * distanceThisFrame, Space.World);

    }

    void HitTarget()
    {
        GameManager.gameManager.takeDamage(turretDamage);
        //Destroy(target.gameObject);//détruit la cible
        Destroy(gameObject);//détruit la balle
    }
    
    private void OnBecameInvisible() {
        Destroy(gameObject);
    }
}
