using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Shoot : MonoBehaviour
{
    [SerializeField] private float shootDelay = .75f, bulletForce = 10f;
    [SerializeField] private string ENEMY_BULLET_TAG = "Enemy_Bullet";

    private float deltaShoot = 0;

    public GameObject bulletPrefab;
    public Transform player, firePosition;

    void Update()
    {
        // if time_d_shoot is less than the shoot delay
        if (deltaShoot <= shootDelay)
            deltaShoot += Time.deltaTime;
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
        GameObject bullet = Instantiate(bulletPrefab, firePosition.position, firePosition.rotation);
        bullet.tag = ENEMY_BULLET_TAG;
        Rigidbody2D bullet_rb = bullet.GetComponent<Rigidbody2D>();
        bullet_rb.AddForce(firePosition.up * bulletForce, ForceMode2D.Impulse);

        // reset time_d_shoot
        deltaShoot = 0;
    }
}
