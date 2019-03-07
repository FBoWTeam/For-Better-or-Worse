using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWaveSystem : MonoBehaviour,IActivable
{
    [Tooltip("time between the spawn of the different waves")]
    public float timeBetweenWaves;
    private float timer;
    private bool enemiesDefeated;

    [Tooltip("list of the prefabs that contain the enemies to spawn")]
    public GameObject[] enemyWaves;
    private int currentWave;

    public bool isActive { get; set; }


    /// <summary>
    /// coroutine that handles the wave system
    /// instanciate the prefabs in the array enemyWaves
    /// the prefabs are gameObjects that contain the enemies to spawn
    /// </summary>
    /// <returns></returns>
    IEnumerator InstanciateEnemyWave()
    {
        isActive = true;
        //if no waves to instanciate, exit the coroutine
        if (enemyWaves.Length == 0)
        {
            yield break;
        }

        //while the list of enemies to instanciate is not empty
        while (!enemiesDefeated)
        {
            //instanciate the wave at the index currentWave 
            GameObject wave = Instantiate(enemyWaves[currentWave], transform.position, Quaternion.identity);


            //if the enemies of the current wave is not defeated or if the timer is still under the limit timeBetweenWaves => doesn't instanciate the next wave
            while (wave.transform.childCount != 0 && timer < timeBetweenWaves)
            {
                timer += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }

            //increase the index to instanciate the next wave
            currentWave++;
            //reset of the timer
            timer = 0;

            //if all the waves are not defeated, instanciate the next wave
            if (enemyWaves.Length <= currentWave)
            {
                enemiesDefeated = true;
                yield break;
            }

        }

    }

    public void Activate()
    {
        if (!isActive)
        {
            StartCoroutine(InstanciateEnemyWave());
        }
    }
}
