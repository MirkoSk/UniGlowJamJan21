using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniGlow.Utility;

[SelectionBase]
public class WombatController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] float movementSpeed = 10f;
    [SerializeField] float jumpForce = 10f;
    [SerializeField] float maxVelocity = 10f;

    [Header("Chain Movement")]
    [SerializeField] float attachedMoveForce = 5f;

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
    bool facingRight = true;
    Chain chain;

    public Rigidbody2D Rigidbody { get { return rigidbody; } }
    public bool FacingRight { get { return facingRight; } }
    public Chain Chain { get { return chain; } set { chain = value; } }



    void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        rigidbody.gravityScale = 1f;
    }

    private void OnEnable()
    {
        GameEvents.PlayerAttached += HandlePlayerAttach;
        GameEvents.PlayerDetached += HandlePlayerDetach;
    }

    private void OnDisable()
    {
        GameEvents.PlayerAttached -= HandlePlayerAttach;
        GameEvents.PlayerDetached -= HandlePlayerDetach;
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

        // Movement controls on ground
        if (grounded) rigidbody.velocity = new Vector2(horizontal * movementSpeed, rigidbody.velocity.y);
        // TODO: Movement controls in air
        else if (false) ;
        // Attached movement controls
        else if (chain && chain.Attached && !chain.ChainButtonBeingPressed) rigidbody.AddForce(Vector2.right * horizontal * attachedMoveForce, ForceMode2D.Force);

        grounded = GroundCheck();

        if (jumpPressed && (grounded || (chain && chain.Attached)))
        {
            Jump();
            if (chain && chain.Attached) GameEvents.DetachPlayer();
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

        // Limit max velocity
        if (rigidbody.velocity.magnitude > maxVelocity) rigidbody.velocity = rigidbody.velocity.normalized * maxVelocity;

        jumpPressed = false;
    }



    void Jump()
    {
        rigidbody.velocity = new Vector2(rigidbody.velocity.x, rigidbody.velocity.y + jumpForce);
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

    void HandlePlayerAttach(Vector3 position)
    {
        rigidbody.freezeRotation = false;
    }

    void HandlePlayerDetach()
    {
        rigidbody.freezeRotation = true;
        transform.rotation = Quaternion.identity;
    }
}
