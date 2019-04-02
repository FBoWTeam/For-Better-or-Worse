using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TauntTutorial : Tutorial
{
	bool launched = false;

	private void OnTriggerEnter(Collider other)
	{
		if(other.CompareTag("Player") && !launched)
		{
			launched = true;
			StartCoroutine(TauntTutorialCoroutine());
		}
	}

	public IEnumerator TauntTutorialCoroutine()
	{
		GetsIn();

		bool playersHasTaunt = false;

		while (!playersHasTaunt)
		{
			if (Input.GetKey(KeyCode.Joystick1Button4) || Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.Joystick2Button4) || Input.GetKey(KeyCode.Keypad0))
			{
				playersHasTaunt = true;
			}
			yield return new WaitForEndOfFrame();
		}

		StartCoroutine(GetsOut());
		yield return new WaitForSeconds(4.0f);

		Destroy(this.gameObject);
	}
}
