using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Chase : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private int damage = 1;
    private Vector2 velocity = Vector2.zero, direction;
    private Rigidbody2D rb;

    public Transform player;

    private void Start()
    {
        // get rigid body component
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // get direction from enemy to player
        direction = player.position - transform.position;
    }

    private void FixedUpdate()
    {
        // apply velocity to rigid body
        rb.MovePosition(rb.position + (direction * speed * Time.deltaTime));
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
}
