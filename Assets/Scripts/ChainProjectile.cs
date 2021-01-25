using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChainProjectile : MonoBehaviour
{
    [SerializeField] float movementSpeed = 20f;

    new Rigidbody2D rigidbody;
    CharacterController player;


    
    void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        player.MoveToAttachmentPoint(transform.position);
        Destroy(gameObject);
    }



    public void Initialize(CharacterController player)
    {
        this.player = player;
        rigidbody.velocity = new Vector2(movementSpeed, 0f);
    }
}
