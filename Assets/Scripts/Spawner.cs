using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private float spawn_delay = 1f; 

    private float cam_width, cam_height, elapsed_time;

    public Transform player;
    public GameObject enemy;

    private void Start()
    {
        elapsed_time = spawn_delay;

        Vector2 enemy_size = enemy.GetComponent<BoxCollider2D>().size;
        cam_height = Camera.main.orthographicSize * 2f;
        cam_width = cam_height + (Screen.width/Screen.height);
    }

    void Update()
    {
        // check elapsed time
        if (elapsed_time >= spawn_delay)
        {
            // spawn enemy
            GameObject new_enemy = Instantiate(enemy, new Vector3(Random.Range(-cam_width, cam_width), cam_height, 0), Quaternion.identity);
            new_enemy.GetComponent<Enemy_Chase>().player = player;

            // reset elapsed_time
            elapsed_time = 0f;
        }

        // otherwise
        else
            elapsed_time += Time.deltaTime;
    }
}
