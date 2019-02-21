using UnityEngine;

public class Bullet : MonoBehaviour {

    Transform target;
    float speed = 70f;
    Vector3 shootDir; 
    int turretDamage;

    public void Seek(Transform _target, int damage, float _speed)
    {
        speed = _speed;
        turretDamage = damage;
        target = _target;
        shootDir = target.position - transform.position;
    }

    
    // Update is called once per frame
    void FixedUpdate()
    {
        
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }
 
        float distanceThisFrame = speed * Time.deltaTime;

        if (CheckCollisions(target.position - transform.position, distanceThisFrame)) {
            HitTarget();
            return;
        }

        transform.Translate(shootDir.normalized * distanceThisFrame, Space.World);

    }

    void HitTarget()
    {
        
        Destroy(gameObject);
        //GameManager.gameManager.TakeDamage(turretDamage);
        
    }
    
    private void OnBecameInvisible() {
        Destroy(gameObject);
    }

    //Askip la meilleur solution pour check les collisiosn avec des projectils ce sont des raycast
    // mais le temps calculer , la balle traverse le joueurs ..
    //du coup old style
    bool CheckCollisions(Vector3 dir,float dist) {
        /*Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, moveDistance)) {
            if (hit.collider.CompareTag("Player")) {
                return true;
            }
            
            
        }
        return false;
        */

        if (dir.magnitude <= dist) {
            return true;
        }
        return false;
    }
}
