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
        public Transform[] spawn_points;
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

    [SerializeField] private float wave_delay = 5f;
    [SerializeField] private string WAVE_DELAY_TEXT = "Next Wave Starting In ", 
        WAVE_NUM_TEXT = "Wave:", SECONDS_STRING = " Seconds";

    private enum State { WAITING, SPAWNING };
    // default state is waiting
    private State state = State.WAITING;
    private int wave_indx = 0, wave_num = 1;
    private float elapsed_time;

    public Wave[] waves;
    public Transform player;
    public Text wave_num_text, wave_delay_text;

    private void Start()
    {
        // allow first wave to spawn upon starting up game
        elapsed_time = wave_delay;
    }

    void Update()
    {
        if (state == State.WAITING)
        {
            if(elapsed_time >= wave_delay)
            {
                wave_delay_text.gameObject.SetActive(false);

                // check if wave_indx is valid
                if (wave_indx >= waves.Length)
                    wave_indx = 0;
                //spawn wave at spawn index
                StartCoroutine(SpawnWave(waves[wave_indx++]));
            }
            // otherwise add to elapsed time and show countdown
            else
            {
                wave_delay_text.gameObject.SetActive(true);

                // update elapsed time and text
                elapsed_time += Time.deltaTime;
                wave_delay_text.text = WAVE_DELAY_TEXT + (int)(wave_delay - elapsed_time) + SECONDS_STRING;
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
        wave_num_text.text = WAVE_NUM_TEXT + (++wave_num);

        // set to waiting state and set elapsed time to zero
        state = State.WAITING;
        elapsed_time = 0;

        yield return null;
    }

    private IEnumerator SpawnEnemy(EnemyInfo enemy)
    {
        int count = 0;

        // spawn the enemy for amount
        while(count++ < enemy.amount)
        {
            // generate random spawn point index
            int spawn_indx = Random.Range(0, enemy.spawn_points.Length);

            GameObject new_enemy = Instantiate(enemy.type, enemy.spawn_points[spawn_indx]);
            Enemy_Chase enemy_chase_script = new_enemy.GetComponent<Enemy_Chase>();
            enemy_chase_script.player = player;

            Enemy_Shoot enemy_shoot_script = new_enemy.GetComponent<Enemy_Shoot>();

            // if this is a shooter enemy
            if (enemy_shoot_script != null)
                enemy_shoot_script.player = player;

            // wait for spawn delay
            yield return new WaitForSecondsRealtime(enemy.delay);
        }

        // set spawned to true
        enemy.spawned = true;

        yield return null;
    }

    private bool CheckWaveSpawned(Wave wave)
    {
        bool all_spawned = true;

        // and all spawned variables in wave
        foreach (EnemyInfo enemy in wave.enemies)
            all_spawned &= enemy.spawned;

        return all_spawned;
    }

    private void SetWaveSpawnedFalse(Wave wave)
    {
        // set each spawned value to false in the wave
        foreach (EnemyInfo enemy in wave.enemies)
            enemy.spawned = false;
    }
}
