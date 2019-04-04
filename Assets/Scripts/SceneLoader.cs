using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour, IActivable
{
	public int sceneToLoad;
	public bool player1, player2;
	bool loading;

	[Header("[UI References]")]
	public Canvas charactersHead;

	public Image fox, racoon;

	public Sprite foxSprite, foxGreySprite;
	public Sprite racoonSprite, racoonGreySprite;

    public bool isActive { get; set; }

    // Update is called once per frame
    void Update()
    {
		charactersHead.transform.LookAt(Camera.main.transform);
		if (player1 && player2 && !loading)
		{
			StartCoroutine(loadSceneCoroutine());
		}
		else if (player1 || player2)
		{
			charactersHead.enabled = true;
		}
		else
		{
			charactersHead.enabled = false;
		}
    }

	private void OnTriggerEnter(Collider other)
	{
		if(other.CompareTag("Player"))
		{
			if (other.GetComponent<PlayerController>().player1)
			{
				player1 = true;
				fox.sprite = foxSprite;
			}
			else
			{
				player2 = true;
				racoon.sprite = racoonSprite;
			}
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			if (other.GetComponent<PlayerController>().player1)
			{
				player1 = false;
				fox.sprite = foxGreySprite;
			}
			else
			{
				player2 = false;
				racoon.sprite = racoonGreySprite;
			}
		}
	}

	IEnumerator loadSceneCoroutine()
	{
		loading = true;
		yield return new WaitForSeconds(0.25f);
		StartCoroutine(GameManager.gameManager.FadeCoroutine("FadeOut"));
		yield return new WaitUntil(() => GameManager.gameManager.isPaused == false);
		SceneManager.LoadScene(sceneToLoad);
	}

    public void Activate()
    {
        gameObject.GetComponent<SphereCollider>().enabled = true;
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(true);
        }
    }
}
