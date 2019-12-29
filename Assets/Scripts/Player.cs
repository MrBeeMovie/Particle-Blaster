using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public GameObject bullet_prefab;
    public Transform fire_position;
    public Text health_text;

    [SerializeField] private float bullet_force = 20f, fire_rate = .25f, speed = 10,
        hurt_delay = 1, hurt_alpha = .25f;
    [SerializeField] private int health = 5;
    [SerializeField] private string horizontal_axis = "Horizontal", vertical_axis = "Vertical",
        HEALTH_STRING = "Health:";

    private Rigidbody2D rb;
    private SpriteRenderer spriter;
    private Vector2 mouse_pos, velocity = Vector2.zero;
    private float time_d_shooting = 0f, time_d_hurting = 0f;
    private bool hurting = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriter = GetComponent<SpriteRenderer>();

        // set health_text to initial health value
        health_text.text = HEALTH_STRING + health;
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
        if (Input.GetButton("Fire1") && fire_rate <= time_d_shooting)
        {
            Shoot();
            time_d_shooting = 0;
        }
        // otherwise increase delay
        else
            time_d_shooting += Time.deltaTime;

        // if hurting add delta time to hurting delta time
        if (hurting)
            time_d_hurting += Time.deltaTime;
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

    public void ApplyDamage(int damage)
    {
        if (!hurting)
            StartCoroutine(TakeDamage(damage));
    }

    private IEnumerator TakeDamage(int damage)
    {
        // set hurting to true
        hurting = true;

        // apply damage to health
        health -= damage;

        // redraw health gui text
        health_text.text = HEALTH_STRING + health;

        // set alpha of player sprite to hurt alpha%
        Color old_color = spriter.color;
        Color new_color = old_color;
        new_color.a = hurt_alpha;
        spriter.color = new_color;

        // wait until delta time on hurting is past hurt delay
        yield return new WaitUntil(() => (time_d_hurting >= hurt_delay));

        // reset alpha
        spriter.color = old_color;

        // reset delta time on hurting
        time_d_hurting = 0f;

        // set hurting to false
        hurting = false;
    }
}
