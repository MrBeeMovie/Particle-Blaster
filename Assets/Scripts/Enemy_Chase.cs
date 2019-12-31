using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Chase : MonoBehaviour
{
    [SerializeField] private float speed = 5f, ccRadius = 1, moveDelay = 1;
    [SerializeField] private int damage = 1;

    private Vector2 velocity = Vector2.zero, direction;
    private Rigidbody2D rb;
    private bool inRange = false;
    private float deltaMove;

    public Transform player;
    public bool keepDistance = false;

    private void Start()
    {
        // get rigid body component
        rb = GetComponent<Rigidbody2D>();

        // if keep_distance is true get circle collider, set radius, and set time_d_move to move delay
        if (keepDistance)
        {
            GetComponent<CircleCollider2D>().radius = ccRadius;
            deltaMove = moveDelay;
        }       
    }

    void Update()
    {
        // get direction from enemy to player
        direction = player.position - transform.position;
    }

    private void FixedUpdate()
    {
        if ((keepDistance & !inRange & deltaMove >= moveDelay) || !keepDistance)
        {
            // apply velocity to rigid body
            rb.MovePosition(rb.position + (direction * speed * Time.deltaTime));
        }
        // if we are keeping distance and not in range
        else if (keepDistance & !inRange)
        {
            rb.velocity = Vector2.zero;
            deltaMove += Time.deltaTime;
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
            inRange = true;
            deltaMove = 0;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // if player steps out of trigger circle
        if (collision.gameObject.CompareTag("Player"))
            inRange = false;
    }
}
