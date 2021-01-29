using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
public class Enemy : MonoBehaviour
{
    [SerializeField] protected GameObject deathParticlesPrefab = null;

    protected bool movingRight;
    protected new Rigidbody2D rigidbody;



    void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }



    public void ReleaseSoul()
    {
        Instantiate(deathParticlesPrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    protected void Flip()
    {
        movingRight = !movingRight;
    }
}
