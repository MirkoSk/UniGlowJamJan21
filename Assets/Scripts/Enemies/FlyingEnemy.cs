using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEnemy : Enemy
{
    [SerializeField] float movementSpeed = 3f;
    [SerializeField] BoxCollider2D movementArea = null;

    Vector3 currentDestination;



    private void Start()
    {
        currentDestination = GetNextDestination();
    }

    private void FixedUpdate()
    {
        if (Vector3.Distance(transform.position, currentDestination) <= 0.5f) currentDestination = GetNextDestination();

        rigidbody.velocity = (currentDestination - transform.position).normalized * movementSpeed;
    }



    Vector3 GetNextDestination()
    {
        Vector3 newDestination = new Vector3();

        newDestination.x = Random.Range(movementArea.bounds.min.x, movementArea.bounds.max.x);
        newDestination.y = Random.Range(movementArea.bounds.min.y, movementArea.bounds.max.y);
        newDestination.z = 0f;

        return newDestination;
    }
}
