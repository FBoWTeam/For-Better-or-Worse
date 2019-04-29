using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateBoss : MonoBehaviour
{
	public RuntimeAnimatorController RAC;
	public AudioClip bossTheme;
    public GameObject boss;
	public GameObject protectionPylon;

	public void Awake()
	{
		boss.GetComponent<Animator>().runtimeAnimatorController = null;
	}

	private void OnDestroy()
	{
		AudioSource bgm = GameObject.Find("BGM").GetComponent<AudioSource>();
		if(bgm.clip != bossTheme)
		{
			bgm.clip = bossTheme;
			bgm.Play();
			bgm.volume = 0.4f;
		}
		boss.GetComponent<Animator>().runtimeAnimatorController = RAC;
		boss.GetComponent<BossSystem>().enabled = true;
		boss.transform.position = new Vector3(1.5f, 0.0f, -20.5f);
		boss.transform.localScale = new Vector3(1, 1, 1);
		protectionPylon.transform.position = new Vector3(1.5f, 0.0f, -20.5f);
    }
}
