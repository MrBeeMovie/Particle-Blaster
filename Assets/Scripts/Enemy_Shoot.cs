using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Shoot : MonoBehaviour
{
    [SerializeField] private float shoot_delay = .75f, bullet_force = 10f;
    [SerializeField] private string ENEMY_BULLET_TAG = "Enemy_Bullet";

    private float time_d_shoot = 0;

    public GameObject bullet_prefab;
    public Transform player, fire_position;

    void Update()
    {
        // if time_d_shoot is less than the shoot delay
        if (time_d_shoot <= shoot_delay)
            time_d_shoot += Time.deltaTime;
        else
        {
            // shoot at target
            ShootAtTarget();
        }
    }

    private void ShootAtTarget()
    {
        // calculate direction vector
        Vector2 direction = transform.position - player.position;
        direction.Normalize();

        // shoot bullet at player
        GameObject bullet = Instantiate(bullet_prefab, fire_position.position, fire_position.rotation);
        bullet.tag = ENEMY_BULLET_TAG;
        Rigidbody2D bullet_rb = bullet.GetComponent<Rigidbody2D>();
        bullet_rb.AddForce(fire_position.up * bullet_force, ForceMode2D.Impulse);

        // reset time_d_shoot
        time_d_shoot = 0;
    }
}
