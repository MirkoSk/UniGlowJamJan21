using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChainProjectile : MonoBehaviour
{
    public enum Direction
    {
        Up,
        UpDiagonal,
        Straight,
        DownDiagonal,
        Down
    }

    [SerializeField] float movementSpeed = 20f;
    [Tooltip("Max length of the chain in unity units.")]
    [SerializeField] float chainLength = 5f;

    [Header("References")]
    [SerializeField] LineRenderer lineRenderer = null;

    new Rigidbody2D rigidbody;
    CharacterController player;
    float lifeTime;


    
    void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        player.MoveToAttachmentPoint(transform.position);
        Destroy(gameObject);
    }

    private void Update()
    {
        lifeTime += Time.deltaTime;
        lineRenderer.SetPosition(1, transform.position);

        if (movementSpeed * lifeTime >= chainLength)
        {
            Destroy(gameObject);
        }
    }



    public void Initialize(CharacterController player, Direction direction)
    {
        this.player = player;
        lifeTime = 0f;

        // Set direction vector
        Vector2 directionVector = new Vector2();
        if (direction == Direction.Up) directionVector = new Vector2(0f, 1f);
        else if (direction == Direction.UpDiagonal) directionVector = new Vector2(0.75f, 0.75f);
        else if (direction == Direction.Straight) directionVector = new Vector2(1f, 0f);
        else if (direction == Direction.DownDiagonal) directionVector = new Vector2(0.75f, -0.75f);
        else if (direction == Direction.Down) directionVector = new Vector2(0f, -1f);

        // Apply velocity
        if (player.FacingRight) rigidbody.velocity = directionVector * movementSpeed;
        else rigidbody.velocity = new Vector2(-directionVector.x, directionVector.y) * movementSpeed;

        // Setup LineRenderer
        lineRenderer.positionCount = 2;
        lineRenderer.SetPosition(0, transform.position);
    }
}
