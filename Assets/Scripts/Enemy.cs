using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] float movementSpeed = 10f;
    [SerializeField] LayerMask groundLayerMask;

    bool movingRight;
    new Rigidbody2D rigidbody;



    void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        RaycastHit2D hit;

        if (movingRight)
        {
            rigidbody.velocity = Vector2.right * movementSpeed;

            hit = Physics2D.Raycast(transform.position + Vector3.right * 0.5f, Vector2.down, 0.2f, groundLayerMask);
        }
        else
        {
            rigidbody.velocity = Vector2.left * movementSpeed;

            hit = Physics2D.Raycast(transform.position + Vector3.left * 0.5f, Vector2.down, 0.2f, groundLayerMask);
        }

        if (hit.point != Vector2.zero)
        {
            Debug.DrawLine(transform.position, transform.position + Vector3.down * 0.1f, Color.green);
        }
        else
        {
            Debug.DrawLine(transform.position, transform.position + Vector3.down * 0.1f, Color.red);
            Flip();
        }
    }



    void Flip()
    {
        movingRight = !movingRight;
    }
}
