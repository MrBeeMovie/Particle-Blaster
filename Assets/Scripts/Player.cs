using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public GameObject bullet_prefab;
    public Transform fire_position;

    [SerializeField] private float bullet_force = 20f;
    [SerializeField] private float fire_rate = .25f;
    [SerializeField] private float speed = 10;
    [SerializeField] private string horizontal_axis = "Horizontal", vertical_axis = "Vertical";

    private Rigidbody2D rb;
    private Vector2 mouse_pos, velocity = Vector2.zero;
    private float elapsed_time = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // process horizontal movement
        velocity.x = Input.GetAxis(horizontal_axis);
        // process vertical movement
        velocity.y = Input.GetAxis(vertical_axis);

        // get mouse position
        mouse_pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

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

    private void FixedUpdate()
    {
        // apply velocity
        rb.MovePosition(rb.position + (velocity * speed * Time.deltaTime));

        // apply rotation
        Vector2 direction = mouse_pos - rb.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
        rb.rotation = angle;
    }

    private void Shoot()
    {
        // create bullet object and add force
        GameObject bullet = Instantiate(bullet_prefab, fire_position.position, fire_position.rotation);
        Rigidbody2D bullet_rb = bullet.GetComponent<Rigidbody2D>();
        bullet_rb.AddForce(fire_position.up * bullet_force, ForceMode2D.Impulse);
    }
}
