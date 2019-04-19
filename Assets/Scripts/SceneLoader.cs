using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour, IActivable
{
    public enum LoaderType{
        SceneLoader,
        Teleporter
    }

    public LoaderType type;

    [DrawIf(new string[] { "type" }, LoaderType.SceneLoader)]
    public int sceneToLoad;
    
    [DrawIf(new string[] { "type" }, LoaderType.Teleporter)]
    public GameObject nextLocation;


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
            if (type == LoaderType.SceneLoader)
            {
                StartCoroutine(loadSceneCoroutine());
            }
            else if (type == LoaderType.Teleporter)
            {
                StartCoroutine(PlayerTeleportation());
            }
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
		GameData.previousScene = SceneManager.GetActiveScene().buildIndex;
		SceneManager.LoadScene(sceneToLoad);
	}

    IEnumerator PlayerTeleportation()
    {
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(GameManager.gameManager.FadeCoroutine("FadeIn"));
        yield return new WaitUntil(() => GameManager.gameManager.isPaused == false);
        GameManager.gameManager.player1.transform.position = nextLocation.transform.position + new Vector3(-5, 0, 0);
        GameManager.gameManager.player2.transform.position = nextLocation.transform.position + new Vector3(5, 0, 0);
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
