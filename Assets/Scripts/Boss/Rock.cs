using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Rock : MonoBehaviour
{
    public float fallSpeed;
    public GameObject circleProjector;
    public float rockRadius;
    public int rockDamage;
    public GameObject rockPoof;

	public SoundEmitter soundEmitter;

    private void Start()
    {
        StartCoroutine(Fall());
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            GameManager.gameManager.TakeDamage(collision.gameObject, rockDamage, collision.contacts[0].point, true);
        }
    }


    IEnumerator Fall()
    {
        //instanciate the circle indicator
        GameObject circleIndicator = Instantiate(circleProjector, transform.position, Quaternion.identity) as GameObject;
        //rescale the sprite
        circleIndicator.transform.GetChild(0).gameObject.GetComponent<Projector>().orthographicSize = rockRadius;
        //save the original instanciation position
        Vector3 instanciationPos = transform.position;
        //create a copy of the material to avoid the shared material problem. This way each instanciation can have it's own sprite for the projector
        Material tempMat = new Material(circleProjector.transform.GetChild(0).gameObject.GetComponent<Projector>().material);
        //define the color of the sprite of the projector
        Color tempColor = Color.black;

        while (transform.position.y > 1)
        {
            //alpha starting from 0 finishing to 0.666666
            tempColor.a = (Vector3.Distance(instanciationPos, transform.position) / instanciationPos.y) / 1.5f;
            tempMat.color = tempColor;
            //set the color / alpha of the sprite acording to the progression of the rock falling
            circleIndicator.transform.GetChild(0).gameObject.GetComponent<Projector>().material = tempMat;
            //rock going down
            transform.Translate(Vector3.down * fallSpeed * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }

		soundEmitter.PlaySound(0, true);
        Instantiate(rockPoof, transform.position, Quaternion.identity);

        Destroy(circleIndicator);
        Destroy(gameObject);
    }

}
