using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
public class Enemy : MonoBehaviour
{
    protected bool movingRight;
    protected new Rigidbody2D rigidbody;



    void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }



    public void ReleaseSoul()
    {
        Destroy(gameObject);
    }

    protected void Flip()
    {
        movingRight = !movingRight;
    }
}
