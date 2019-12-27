using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    private enum State { WAITING, SPAWNING};
    // default state is waiting
    private State state = State.WAITING;
    private int wave_indx = 0;

    public Wave[] waves;
    public Transform player;    

    void Update()
    {
        if(state == State.WAITING)
        {
            // check if wave_indx is valid
            if (wave_indx >= waves.Length)
                wave_indx = 0;
            //spawn wave at spawn index
            StartCoroutine(SpawnWave(waves[wave_indx++]));
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

        state = State.WAITING;

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
            new_enemy.GetComponent<Enemy_Chase>().player = player;

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
}
