using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Chase : MonoBehaviour
{
    [SerializeField] private float speed = 5f, cc_radius = 1, move_delay = 1;
    [SerializeField] private int damage = 1;

    private Vector2 velocity = Vector2.zero, direction;
    private Rigidbody2D rb;
    private CircleCollider2D cc;
    private bool in_range = false;
    private float time_d_move;

    public Transform player;
    public bool keep_distance = false;

    private void Start()
    {
        // get rigid body component
        rb = GetComponent<Rigidbody2D>();

        // if keep_distance is true get circle collider, set radius, and set time_d_move to move delay
        if (keep_distance)
        {
            cc = GetComponent<CircleCollider2D>();
            cc.radius = cc_radius;
            time_d_move = move_delay;
        }       
    }

    void Update()
    {
        // get direction from enemy to player
        direction = player.position - transform.position;
    }

    private void FixedUpdate()
    {
        if ((keep_distance & !in_range & time_d_move >= move_delay) || !keep_distance)
        {
            // apply velocity to rigid body
            rb.MovePosition(rb.position + (direction * speed * Time.deltaTime));
        }
        // if we are keeping distance and not in range
        else if (keep_distance & !in_range)
        {
            rb.velocity = Vector2.zero;
            time_d_move += Time.deltaTime;
        } 

        // change rotation
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
        rb.rotation = angle;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        // if we collide with player apply damage to the player
        if(collision.gameObject.CompareTag("Player"))
            collision.gameObject.GetComponent<Player>().ApplyDamage(damage);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // if trigger circle collides with player
        if (collision.gameObject.CompareTag("Player"))
        {
            in_range = true;
            time_d_move = 0;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // if player steps out of trigger circle
        if (collision.gameObject.CompareTag("Player"))
            in_range = false;
    }
}
