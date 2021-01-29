using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniGlow.Utility;

public class Chain : MonoBehaviour
{
    public enum Direction
    {
        Up,
        UpDiagonal,
        Straight,
        DownDiagonal,
        Down
    }

    [Header("Projectile")]
    [SerializeField] float movementSpeed = 20f;
    [Tooltip("Max length of the chain in unity units.")]
    [SerializeField] float chainLength = 5f;
    [SerializeField] GameObject impactEffectPrefab = null;

    [Header("Chain Controls")]
    [SerializeField] float attachmentPullSpeed = 2f;
    [SerializeField] float attachmentPushSpeed = 2f;
    [SerializeField] float attachSpeedBoost = 5f;

    [Space]
    [SerializeField] float chainBreakDistance = 0.5f;

    [Space]
    [SerializeField] bool breakChainOnStandstill = true;
    [SerializeField] float chainBreakSpeed = 1f;
    [SerializeField] float chainBreakSpeedDuration = 1f;

    [Header("References")]
    [SerializeField] LineRenderer lineRenderer = null;
    [SerializeField] new HingeJoint2D hingeJoint = null;

    new Rigidbody2D rigidbody;
    WombatController player;
    float lifeTime;
    bool attached;
    Vector3 attachmentPoint;
    ChainThrower chainOrigin;
    bool chainInButtonBeingPressed;
    bool chainOutButtonBeingPressed;
    bool playerStandingStill;
    float timeStandingStill;
    JumpUpTrigger jumpUpTrigger;
    float chainOriginOffset;
    AudioSource audioSource;

    public bool Attached { get { return attached; } }
    public Vector3 AttachmentPoint { get { return attachmentPoint; } }
    public bool ChainButtonBeingPressed { get { return chainInButtonBeingPressed; } }
    public JumpUpTrigger JumpUpTrigger { get => jumpUpTrigger; set => jumpUpTrigger = value; }


    void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        GameEvents.PlayerDetached += Detach;
    }

    private void OnDisable()
    {
        GameEvents.PlayerDetached -= Detach;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Attach(collision.ClosestPoint(transform.position));
        
        // Give the swinging an initial boost
        player.Rigidbody.AddForce((transform.position - player.transform.position).normalized * attachSpeedBoost, ForceMode2D.Impulse);

        // TODO: Activate Stun Trigger
    }

    private void Update()
    {
        lifeTime += Time.deltaTime;
        lineRenderer.SetPosition(1, transform.position);
        lineRenderer.SetPosition(0, chainOrigin.transform.position);

        if (!attached && movementSpeed * lifeTime >= chainLength)
        {
            Destroy(gameObject);
        }

        if (Input.GetButton(Constants.INPUT_CHAIN_IN)) chainInButtonBeingPressed = true;
        else chainInButtonBeingPressed = false;

        if (Input.GetButton(Constants.INPUT_CHAIN_OUT)) chainOutButtonBeingPressed = true;
        else chainOutButtonBeingPressed = false;
    }

    private void FixedUpdate()
    {
        if (attached)
        {
            // Detach conditions
            if (breakChainOnStandstill && player.Rigidbody.velocity.magnitude <= chainBreakSpeed)
            {
                timeStandingStill += Time.deltaTime;
                if (timeStandingStill >= chainBreakSpeedDuration) playerStandingStill = true;
            }
            else timeStandingStill = 0f;

            if (Vector2.Distance(chainOrigin.transform.position, transform.position) <= chainBreakDistance)
            {
                if (jumpUpTrigger) GameEvents.DetachPlayer(jumpUpTrigger.DetachForce);
                else GameEvents.DetachPlayer(Vector2.zero);
                Detach(Vector2.zero);
            }
            else if (breakChainOnStandstill && playerStandingStill)
            {
                GameEvents.DetachPlayer(Vector2.zero);
                Detach(Vector2.zero);
            }

            // Move to attachment point
            if (chainInButtonBeingPressed)
            {
                hingeJoint.connectedAnchor = new Vector2(hingeJoint.connectedAnchor.x, Mathf.Clamp(hingeJoint.connectedAnchor.y - attachmentPullSpeed * Time.deltaTime, chainOriginOffset, chainLength));
                player.Animator.SetInteger("hookPull", 1);
            }
            else if (chainOutButtonBeingPressed)
            {
                hingeJoint.connectedAnchor = new Vector2(hingeJoint.connectedAnchor.x, Mathf.Clamp(hingeJoint.connectedAnchor.y + attachmentPushSpeed * Time.deltaTime, chainOriginOffset, chainLength));
                player.Animator.SetInteger("hookPull", -1);
            }
            else
            {
                player.Animator.SetInteger("hookPull", 0);
            }
        }
    }



    void Attach(Vector2 point)
    {
        attached = true;
        attachmentPoint = point;

        // Rotate GameObject to player and set up HingeJoint
        transform.rotation = Quaternion.LookRotation(Vector3.forward, chainOrigin.transform.position - transform.position);
        hingeJoint.connectedBody = player.Rigidbody;
        hingeJoint.connectedAnchor = new Vector2(0f, Vector2.Distance(player.transform.position, transform.position));
        hingeJoint.enabled = true;

        rigidbody.constraints = RigidbodyConstraints2D.FreezePosition;

        chainOriginOffset = Vector2.Distance(chainOrigin.transform.position, player.transform.position);

        GameObject impactGO = Instantiate(impactEffectPrefab, attachmentPoint, Quaternion.LookRotation(Vector3.forward, (Vector2)transform.position - point));
        Destroy(impactGO, 1f);
        audioSource.Play();

        GameEvents.AttachPlayer(transform.position);
    }

    void Detach(Vector2 detachForce)
    {
        if (!attached) Debug.LogError("Tried to detach the player while not attached.");

        player.Chain = null;
        Destroy(gameObject);
    }

    public void Initialize(WombatController player, ChainThrower chainOrigin, Direction direction)
    {
        this.chainOrigin = chainOrigin;
        this.player = player;
        player.Chain = this;
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
        lineRenderer.SetPosition(0, chainOrigin.transform.position);
        lineRenderer.SetPosition(1, transform.position);
    }

    public void Initialize(WombatController player, ChainThrower chainOrigin, Vector2 direction)
    {
        this.chainOrigin = chainOrigin;
        this.player = player;
        player.Chain = this;
        lifeTime = 0f;

        // Apply velocity
        rigidbody.velocity = direction * movementSpeed;

        // Setup LineRenderer
        lineRenderer.positionCount = 2;
        lineRenderer.SetPosition(0, chainOrigin.transform.position);
        lineRenderer.SetPosition(1, transform.position);
    }
}
