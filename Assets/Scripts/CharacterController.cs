using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniGlow;

[SelectionBase]
public class CharacterController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] float movementSpeed = 10f;
    [SerializeField] float jumpForce = 10f;

    [Header("Chain Movement")]
    [SerializeField] float attachMoveSpeed = 30f;

    [Header("Tweaks")]
    [SerializeField] float fallDownGravityMultiplier = 2f;
    [SerializeField] float lowJumpGravityMultiplier = 2f;
    [SerializeField] LayerMask layerMask;

    new Rigidbody2D rigidbody;
    float horizontal;
    [HideInInspector]
    bool jumpPressed;
    bool jumpBeingPressed;
    bool grounded;
    bool swinging;
    Vector3 swingTarget;
    bool facingRight = true;

    public bool FacingRight { get { return facingRight; } }



    void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        rigidbody.gravityScale = 1f;
    }

    void Update()
    {
        horizontal = Input.GetAxis(Constants.INPUT_HORIZONTAL);
        if (facingRight && horizontal < 0) Flip();
        else if (!facingRight && horizontal > 0) Flip();

        if (Input.GetButtonDown(Constants.INPUT_JUMP)) jumpPressed = true;

        if (Input.GetButton(Constants.INPUT_JUMP)) jumpBeingPressed = true;
        else jumpBeingPressed = false;
    }

    private void FixedUpdate()
    {
        rigidbody.gravityScale = 1f;

        if (swinging)
        {
            if (transform.position.x > swingTarget.x - 1f) swinging = false;
            else return;
        }

        rigidbody.velocity = new Vector2(horizontal * movementSpeed, rigidbody.velocity.y);

        grounded = GroundCheck();

        if (jumpPressed && grounded)
        {
            Jump();
        }

        // Better jumping with 4 lines of code
        if (!grounded && rigidbody.velocity.y < 0)
        {
            rigidbody.gravityScale = fallDownGravityMultiplier;
        }
        else if (!grounded && rigidbody.velocity.y > 0 && !jumpBeingPressed)
        {
            rigidbody.gravityScale = lowJumpGravityMultiplier;
        }
    }



    public void MoveToAttachmentPoint(Vector3 position)
    {
        rigidbody.AddForce((position - transform.position).normalized * attachMoveSpeed, ForceMode2D.Impulse);
        swingTarget = position;
        swinging = true;
    }

    void Jump()
    {
        rigidbody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        jumpPressed = false;
    }

    bool GroundCheck()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 0.1f, layerMask);

        if (hit.point != Vector2.zero)
        {
            Debug.DrawLine(transform.position, transform.position + Vector3.down * 0.1f, Color.green);
            return true;
        }
        else
        {
            Debug.DrawLine(transform.position, transform.position + Vector3.down * 0.1f, Color.red);
            return false;
        }
    }

    void Flip()
    {
        facingRight = !facingRight;
        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
    }
}
