using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform firePosition;
    public Text healthText;

    [SerializeField] private float bulletForce = 20f, fireRate = .25f, speed = 10,
        hurtDelay = 1, hurtAlpha = .25f;
    [SerializeField] private int health = 5;
    [SerializeField] private string horizontalAxis = "Horizontal", verticalAxis = "Vertical",
        HEALTH_STRING = "Health:", PLAYER_BULLET_TAG = "Player_Bullet";

    private Rigidbody2D rb;
    private SpriteRenderer spriteRend;
    private Vector2 mousePos, velocity = Vector2.zero;
    private float deltaShoot = 0f, deltaHurt = 0f;
    private bool hurting = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRend = GetComponent<SpriteRenderer>();

        // set health_text to initial health value
        healthText.text = HEALTH_STRING + health;
    }

    void Update()
    {
        // check if health is zero or less
        if (health <= 0)
            SceneManager.LoadScene("GameOver");

        // process horizontal movement
        velocity.x = Input.GetAxis(horizontalAxis);
        // process vertical movement
        velocity.y = Input.GetAxis(verticalAxis);

        // get mouse position
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // shoot if we are above fire_rate delay
        if (Input.GetButton("Fire1") && fireRate <= deltaShoot)
        {
            Shoot();
            deltaShoot = 0;
        }
        // otherwise increase delay
        else
            deltaShoot += Time.deltaTime;

        // if hurting add delta time to hurting delta time
        if (hurting)
            deltaHurt += Time.deltaTime;
    }

    private void FixedUpdate()
    {
        // apply velocity
        rb.MovePosition(rb.position + (velocity * speed * Time.deltaTime));

        // apply rotation
        Vector2 direction = mousePos - rb.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
        rb.rotation = angle;
    }

    private void Shoot()
    {
        // create bullet object and add force
        GameObject bullet = Instantiate(bulletPrefab, firePosition.position, firePosition.rotation);
        bullet.tag = PLAYER_BULLET_TAG;
        Rigidbody2D bulletRB = bullet.GetComponent<Rigidbody2D>();
        bulletRB.AddForce(firePosition.up * bulletForce, ForceMode2D.Impulse);
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
        healthText.text = HEALTH_STRING + health;

        // set alpha of player sprite to hurt alpha%
        Color old_color = spriteRend.color;
        Color new_color = old_color;
        new_color.a = hurtAlpha;
        spriteRend.color = new_color;

        // wait until delta time on hurting is past hurt delay
        yield return new WaitUntil(() => (deltaHurt >= hurtDelay));

        // reset alpha
        spriteRend.color = old_color;

        // reset delta time on hurting
        deltaHurt = 0f;

        // set hurting to false
        hurting = false;
    }
}
