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
    [SerializeField] float midairMovementSpeed = 40f;
    [SerializeField] float maxVelocity = 10f;
    [Range(0f, 0.5f)]
    [SerializeField] float airFriction = 0.05f;

    [Header("Chain Movement")]
    [SerializeField] float attachedMoveForce = 5f;

    [Header("Damage")]
    [SerializeField] Vector2 damageRecoil = new Vector2();
    [SerializeField] float invincibilityDuration = 1f;

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
    bool takingDamage;
    float invincibilityTimer;

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

        if (takingDamage) invincibilityTimer += Time.deltaTime;
        if (invincibilityTimer >= invincibilityDuration) takingDamage = false;
    }

    private void FixedUpdate()
    {
        rigidbody.gravityScale = 1f;

        // Movement controls on ground
        if (grounded && !takingDamage) rigidbody.velocity = new Vector2(horizontal * movementSpeed, rigidbody.velocity.y);
        // Movement controls mid-air
        else if (!grounded && !(chain && chain.Attached))
        {
            // Apply air friction
            rigidbody.velocity = new Vector2(rigidbody.velocity.x * (1- airFriction), rigidbody.velocity.y);

            Vector2 newVelocity = rigidbody.velocity;
            // This ensures the player can control himself in midair, but not over the movementSpeed limit
            // External sources however (hook) can push the player up to his maxVelocity
            if ((rigidbody.velocity.x >= movementSpeed && horizontal < 0)
                || (rigidbody.velocity.x <= -movementSpeed && horizontal > 0)
                || (rigidbody.velocity.x >= 0 && rigidbody.velocity.x <= movementSpeed)
                || (rigidbody.velocity.x < 0 && rigidbody.velocity.x >= -movementSpeed))
            {
                newVelocity.x = Mathf.Clamp(rigidbody.velocity.x + horizontal * midairMovementSpeed, -movementSpeed, movementSpeed);
                rigidbody.velocity = newVelocity;
            }
        }
        // Attached movement controls
        else if (chain && chain.Attached && !chain.ChainButtonBeingPressed) rigidbody.AddForce(Vector2.right * horizontal * attachedMoveForce, ForceMode2D.Force);

        grounded = GroundCheck();

        if (jumpPressed && (grounded || (chain && chain.Attached)))
        {
            Jump();
            if (chain && chain.Attached) GameEvents.DetachPlayer(Vector3.zero);
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



    public void TakeDamage(Vector3 enemyPosition)
    {
        // Calculate recoil direction
        Vector2 recoilDirection = new Vector2();
        if (enemyPosition.x >= transform.position.x) recoilDirection.x = -damageRecoil.x;
        else recoilDirection.x = damageRecoil.x;
        recoilDirection.y = damageRecoil.y;
        
        rigidbody.AddForce(recoilDirection, ForceMode2D.Impulse);
        takingDamage = true;
        invincibilityTimer = 0f;
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

    void HandlePlayerAttach(Vector2 position)
    {
        rigidbody.freezeRotation = false;
    }

    void HandlePlayerDetach(Vector2 detachForce)
    {
        rigidbody.freezeRotation = true;
        transform.rotation = Quaternion.identity;
        rigidbody.AddForce(detachForce, ForceMode2D.Impulse);
    }
}
