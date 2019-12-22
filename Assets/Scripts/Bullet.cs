using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Score score;

    private void Start()
    {
        score = FindObjectOfType<Score>();
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // destroy what it hit if enemy
        if (collision.gameObject.CompareTag("Enemy"))
        {
            // increment score
            score.IncrementScore();
            Destroy(collision.gameObject);
        }

        // destroy itself
        Destroy(gameObject);
    }
}
