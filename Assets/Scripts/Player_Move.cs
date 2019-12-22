using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Move : MonoBehaviour
{   
    [SerializeField] private float speed = 10;
    [SerializeField] private string horizontal_axis = "Horizontal", vertical_axis = "Vertical";

    private Vector2 velocity = Vector2.zero;
    private Rigidbody2D rigid_body;

    private void Start()
    {
        rigid_body = GetComponent<Rigidbody2D>();
    }

    void Update()
    {   
        // process horizontal movement
        velocity.x = Input.GetAxis(horizontal_axis);
        // process vertical movement
        velocity.y = Input.GetAxis(vertical_axis);
    }

    private void FixedUpdate()
    {
        // apply velocity
        rigid_body.MovePosition(rigid_body.position + (velocity * speed * Time.deltaTime));
    }
}
