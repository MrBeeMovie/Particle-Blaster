using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Shoot : MonoBehaviour
{
    public GameObject bullet_prefab;
    public Transform fire_position;

    [SerializeField] private float bullet_force = 20f;
    [SerializeField] private float fire_rate = .25f;

    private float elapsed_time = 0f;

    void Update()
    {
        // shoot if we are above fire_rate delay
        if (Input.GetButton("Fire1") && fire_rate <= elapsed_time)
        {
            Shoot();
            elapsed_time = 0;
        }
        // otherwise increase delay
        else
            elapsed_time += Time.deltaTime;
    }

    private void Shoot()
    {
        // create bullet object and add force
        GameObject bullet = Instantiate(bullet_prefab, fire_position.position, fire_position.rotation);
        Rigidbody2D bullet_rb = bullet.GetComponent<Rigidbody2D>();
        bullet_rb.AddForce(fire_position.up * bullet_force, ForceMode2D.Impulse);
    }
}
