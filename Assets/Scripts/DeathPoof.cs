using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathPoof : MonoBehaviour
{
	public void OnEnable()
	{
		Destroy(gameObject, 1.0f);
	}
}
