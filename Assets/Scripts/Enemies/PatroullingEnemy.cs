using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatroullingEnemy : Enemy
{
    [SerializeField] float movementSpeed = 10f;
    [SerializeField] LayerMask groundLayerMask;



    void FixedUpdate()
    {
        RaycastHit2D hitDown;
        RaycastHit2D hitFront;

        if (movingRight)
        {
            hitDown = Physics2D.Raycast(transform.position + Vector3.right * 0.5f, Vector2.down, 0.3f, groundLayerMask);
            hitFront = Physics2D.Raycast(transform.position + Vector3.up * 0.3f + Vector3.right * 0.7f, Vector2.right, 0.5f, groundLayerMask);

            Debug.DrawLine(transform.position + Vector3.up * 0.3f + Vector3.right * 0.7f, transform.position + Vector3.up * 0.3f + Vector3.right * 1.2f, Color.cyan);
        }
        else
        {
            hitDown = Physics2D.Raycast(transform.position + Vector3.left * 0.5f, Vector2.down, 0.3f, groundLayerMask);
            hitFront = Physics2D.Raycast(transform.position + Vector3.up * 0.3f + Vector3.left * 0.7f, Vector2.left, 0.5f, groundLayerMask);

            Debug.DrawLine(transform.position + Vector3.up * 0.3f + Vector3.left * 0.7f, transform.position + Vector3.up * 0.3f + Vector3.left * 1.2f, Color.cyan);
        }

        if (hitDown.point == Vector2.zero || hitFront.point != Vector2.zero) Flip();

        Move();
    }



    void Move()
    {
        if (movingRight) rigidbody.velocity = Vector2.right * movementSpeed;
        else rigidbody.velocity = Vector2.left * movementSpeed;
    }
}
