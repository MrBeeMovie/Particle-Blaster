using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaveManager : MonoBehaviour
{
    // info for enemy spawned during a wave
    [System.Serializable]
    public class EnemyInfo
    {
        public GameObject type;
        public Transform[] spawnPoints;
        [Min(1)] public int amount;
        [Min(0)] public float delay;
        [HideInInspector] public bool spawned;
    }

    // wave info
    [System.Serializable]
    public struct Wave
    {
        public EnemyInfo[] enemies;
    }

    [SerializeField] private float waveDelay = 5f;
    [SerializeField] private string WAVE_DELAY_TEXT = "Next Wave Starting In ", 
        WAVE_NUM_TEXT = "Wave:", SECONDS_STRING = " Seconds";

    private enum State { WAITING, SPAWNING };
    // default state is waiting
    private State state = State.WAITING;
    private int waveIndx = 0, waveNum = 1;
    private float deltaTime;

    public Wave[] waves;
    public Transform player;
    public Text waveNumText, waveDelayText;

    private void Start()
    {
        // allow first wave to spawn upon starting up game
        deltaTime = waveDelay;
    }

    void Update()
    {
        if (state == State.WAITING)
        {
            if(deltaTime >= waveDelay)
            {
                waveDelayText.gameObject.SetActive(false);

                // check if wave_indx is valid
                if (waveIndx >= waves.Length)
                    waveIndx = 0;
                //spawn wave at spawn index
                StartCoroutine(SpawnWave(waves[waveIndx++]));
            }
            // otherwise add to elapsed time and show countdown
            else
            {
                waveDelayText.gameObject.SetActive(true);

                // update elapsed time and text
                deltaTime += Time.deltaTime;
                waveDelayText.text = WAVE_DELAY_TEXT + (int)(waveDelay - deltaTime) + SECONDS_STRING;
            }
        }
    }

    private IEnumerator SpawnWave(Wave wave)
    {
        state = State.SPAWNING;

        // spawn the wave
        for(int i=0; i<wave.enemies.Length; i++)
        {
            // start coroutine for spawning individual enemies
            StartCoroutine(SpawnEnemy(wave.enemies[i]));
        }        

        // wait until all enemies have been spawned
        yield return new WaitUntil(() => CheckWaveSpawned(wave));

        // reset wave spawned values to false
        SetWaveSpawnedFalse(wave);

        // update wave_num
        waveNumText.text = WAVE_NUM_TEXT + (++waveNum);

        // set to waiting state and set elapsed time to zero
        state = State.WAITING;
        deltaTime = 0;

        yield return null;
    }

    private IEnumerator SpawnEnemy(EnemyInfo enemy)
    {
        int count = 0;

        // spawn the enemy for amount
        while(count++ < enemy.amount)
        {
            // generate random spawn point index
            int spawnIndx = Random.Range(0, enemy.spawnPoints.Length);

            GameObject newEnemy = Instantiate(enemy.type, enemy.spawnPoints[spawnIndx]);
            newEnemy.GetComponent<Enemy_Chase>().player = player;

            Enemy_Shoot enemyShootScript = newEnemy.GetComponent<Enemy_Shoot>();

            // if this is a shooter enemy
            if (enemyShootScript != null)
                enemyShootScript.player = player;

            // wait for spawn delay
            yield return new WaitForSecondsRealtime(enemy.delay);
        }

        // set spawned to true
        enemy.spawned = true;

        yield return null;
    }

    private bool CheckWaveSpawned(Wave wave)
    {
        bool allSpawned = true;

        // and all spawned variables in wave
        foreach (EnemyInfo enemy in wave.enemies)
            allSpawned &= enemy.spawned;

        return allSpawned;
    }

    private void SetWaveSpawnedFalse(Wave wave)
    {
        // set each spawned value to false in the wave
        foreach (EnemyInfo enemy in wave.enemies)
            enemy.spawned = false;
    }
}
