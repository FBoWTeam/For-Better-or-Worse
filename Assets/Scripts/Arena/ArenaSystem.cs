using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ArenaSystem : MonoBehaviour
{
    [System.Serializable]
    public class Wave
    {
        public List<SubWave> subWaveList;
        public int timeBeforeNextWave;
    }

    [System.Serializable]
    public class SubWave
    {
        //index used in the spawList to know where to instanciate the enemies
        public int spawnNumber;
        public int timeBeforeNextSubWave;
        public GameObject subWavePrefab;
    }

    [Header("Wave List")]
    public List<Wave> waveList;

    [Header("Boss")]
    public GameObject bonusWave;
    public int spawnNumer;
    //the wave index from which we will begin to have a chance to encounter the boss
    public int threshold;
    //the bonus chance that will increase the chance to encounter the boss
    //this variable will be increased as we go through the waves
    private float bonusChance;
    //the more we go through the waves without encountering the boss, the more we have a chance to encounter him
    //the chance is increased by increasedChanceValue
    private float increaseChanceValue;
    //if we already spawned the boss, it can't be instanciated in the remaining waves
    private bool bonusWaveAlreadySpawned;

    [Header("Wave Param")]
    public bool arenaCleared;

    [Header("Wave Param")]
    //list of transform containing all the spawn position
    //enemies will be instanciated in funtion of an integer coresponding to the position in the list at the index of the integer
    public List<Transform> spawnList;
    
    //used to keep track of the current position in our lists and the right wave to instanciate
    private int waveIndex;
    private int subWaveIndex;
    //timer used to spawn enemies which requires a delay between 2 instanciations of enemies
    private float timer;

    //to go to the next scene once the arena is cleared
    public GameObject sceneLoader;

    //enemies currently alive in the arena
    private List<GameObject> remainingEnemiesList;
    

    public List<GameObject> arenaCanvas;
    public GameObject countdownArena;


    private void Start()
    {
        waveIndex = 0;
        subWaveIndex = 0;
        timer = 0f;
        remainingEnemiesList = new List<GameObject>();
        increaseChanceValue = 100f / ((waveList.Count - threshold));
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(CountDown());
            GetComponent<BoxCollider>().enabled = false;
        }
    }


    IEnumerator CountDown()
    {
        countdownArena.SetActive(true);
        countdownArena.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "5";
        yield return new WaitForSeconds(1f);
        countdownArena.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "4";
        yield return new WaitForSeconds(1f);
        countdownArena.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "3";
        yield return new WaitForSeconds(1f);
        countdownArena.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "2";
        yield return new WaitForSeconds(1f);
        countdownArena.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "1";
        yield return new WaitForSeconds(1f);
        countdownArena.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "";
        StartArena();
    }

    void StartArena()
    {
        for (int i = 0; i < arenaCanvas.Count; i++)
        {
            arenaCanvas[i].SetActive(true);
        }
        StartCoroutine(WaveSystem());
        GameManager.gameManager.UIManager.UpdateWave(waveIndex + 1);
    }
    
    IEnumerator WaveSystem()
    {
        if (waveList.Count <= 0)
        {
            yield break;
        }

        GameManager.gameManager.UIManager.StartCoroutine(GameManager.gameManager.UIManager.AnnouceWave(waveIndex + 1));
        // ==== ARENA
        while (!arenaCleared)
        {
            // ==== WAVE
            while (waveIndex < waveList.Count)
            {
                // ==== SUBWAVE
                while (subWaveIndex < waveList[waveIndex].subWaveList.Count)
                {
                    GameObject subwave = Instantiate(waveList[waveIndex].subWaveList[subWaveIndex].subWavePrefab, spawnList[waveList[waveIndex].subWaveList[subWaveIndex].spawnNumber].position, Quaternion.identity);

                    foreach (Enemy enemy in subwave.GetComponentsInChildren<Enemy>())
                    {
                        remainingEnemiesList.Add(enemy.gameObject);
                    }

                    while (timer < waveList[waveIndex].subWaveList[subWaveIndex].timeBeforeNextSubWave)
                    {
                        timer += Time.deltaTime;
                        yield return new WaitForEndOfFrame();
                    }
                    timer = 0;
                    GameManager.gameManager.UIManager.UpdateSubWave(subWaveIndex + 1);
                    subWaveIndex++;
                    
                }

                SpawnBoss();

                if (waveCleared())
                {
                    //Debug.Log("Next Wave");
                    while (timer < waveList[waveIndex].timeBeforeNextWave)
                    {
                        timer += Time.deltaTime;
                        yield return new WaitForEndOfFrame();
                    }
                    waveIndex++;
                    if (waveIndex < waveList.Count)
                    {
                        GameManager.gameManager.UIManager.UpdateWave(ScoreManager.scoreManager.totalWave + waveIndex + 1);
                        GameManager.gameManager.UIManager.StartCoroutine(GameManager.gameManager.UIManager.AnnouceWave(waveIndex + 1));
                    }
                    subWaveIndex = 0;
                    GameManager.gameManager.UIManager.UpdateSubWave(subWaveIndex + 1);
                    bonusChance = 0;
                }
                yield return new WaitForEndOfFrame();
            }
            if (waveIndex >= waveList.Count)
            {
                arenaCleared = true;

                //update total wave cleared
                ScoreManager.scoreManager.totalWave += waveIndex + 1;
            }
        }
        ScoreManager.scoreManager.Save();
        sceneLoader.GetComponent<IActivable>().Activate();
    }

    void SpawnBoss()
    {
        if (!bonusWaveAlreadySpawned && subWaveIndex >= threshold && (Random.Range(0f, 100f) - bonusChance) <= increaseChanceValue)
        {
            while (timer < 10.0f)
            {
                timer += Time.deltaTime;
            }
            timer = 0;
            GameObject boss = Instantiate(bonusWave, spawnList[spawnNumer].position, Quaternion.identity).gameObject;
            foreach (Enemy enemy in boss.GetComponentsInChildren<Enemy>())
            {
                remainingEnemiesList.Add(enemy.gameObject);
            }
            bonusWaveAlreadySpawned = true;
        }
        else if (!bonusWaveAlreadySpawned)
        {
            bonusChance += increaseChanceValue;
        }
    }




    //check if the wave given in parameter is cleared by checking if the list in empty
    private bool waveCleared()
    {
        CleanNullInEnemyList(remainingEnemiesList);
        if (remainingEnemiesList.Count <= 0)
        {
            return true;
        }
        return false;
    }

    //function that removes the destroyed enemies from the list
    public void CleanNullInEnemyList(List<GameObject> wave)
    {
        if (wave.Exists(x => x.Equals(null)))
        {
            wave.RemoveAll(x => x.Equals(null));
        }
    }

}
