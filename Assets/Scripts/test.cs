using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
	public bool launched;
	public Dialog d;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if(!launched)
		{
			launched = true;
			GameManager.gameManager.dialogSystem.gameObject.SetActive(true);
			StartCoroutine(GameManager.gameManager.dialogSystem.StartDialog(d));
		}
    }
}
