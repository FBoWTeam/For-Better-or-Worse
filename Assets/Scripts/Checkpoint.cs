using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
	GameObject contentToLoad;
	GameObject actualContent;


	// Start is called before the first frame update
	void Start()
    {
		contentToLoad = transform.GetChild(1).gameObject;
		actualContent = Instantiate(contentToLoad, transform);
		Destroy(contentToLoad);

		actualContent = Instantiate(contentToLoad, transform);
	}

	// Update is called once per frame
	void Update()
    {
        
    }

	private void OnTriggerEnter(Collider other)
	{
		if(other.CompareTag("Player"))
			Debug.Log("oui");
	}
}
