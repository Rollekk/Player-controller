using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerController : MonoBehaviour
{
    [Header("Components")]
    private Rigidbody Rigidbody;
    private PlayerStats playerStats;
    private Combat combat;
    public CapsuleCollider CapsuleCollider;
    public ParticleSystem WindEffect;
    public Transform groundCheck;
    public Transform headCheck;
    public Animator animator;

    [Header("Player movement")]
    private float Forward;
    private float Sideways;
    public Vector3 moveVelocity;
    public float moveSpeed = 10f;
    private float defaultMoveSpeed;

    [Header("Checks")]
    public float groundDistance = 0.2f;
    public float headDistance = 0.6f;
    public LayerMask groundMask;
    private bool shouldUncrouch;
    private bool isUnder;

    [Header("Gravity")]
    [SerializeField] Vector3 gravity;
    public float gravityStrength = 10f;
    public float holdDownForce = 3f;
    private float defaultHoldDownForce;
    private bool shouldGravity = true;
    public bool isOnGround;
    public bool isOnWall;
    public int wallSide = 0;
    RaycastHit GroundHit;
    RaycastHit WallHit;

    [Header("Slopes")]
    public float MaxSlopeAngle = 45f;
    private float ForwardSlopeAngle;
    private float SlopeAngle;
    [SerializeField] private Vector3 slopeDirection;

    [Header("Jump")]
    public KeyCode jumpKey;
    public float jumpForce = 160f;
    public bool isInAir;
    public bool canDoubleJump;
    public bool canJump;

    [Header("Sprint")]
    public KeyCode sprintKey;
    public bool isSprinting;
    public float sprintSpeed;
    public float slowDownSpeed;
    public float maxMoveSpeed;

    [Header("Crouch && Slide")]
    public KeyCode crouchKey;
    public bool isCrouching;
    public bool isSliding;
    public float crouchSpeed;
    public float slideSlowDownSpeed;
    public float reducedCrouch;
    private float defaultCrouch;

    [Header("Ledge")]
    public Transform ledgeCheck;
    public LayerMask ledgeMask;
    public Vector3 ledgeBox;
    public bool isOnLedge;
    public GameObject Hand;

    // Start is called before the first frame update
    void Start()
    {
        Rigidbody = GetComponent<Rigidbody>();
        Rigidbody.useGravity = false;

        playerStats = GetComponent<PlayerStats>();
        combat = GetComponent<Combat>();

        defaultMoveSpeed = moveSpeed;
        defaultCrouch = CapsuleCollider.height;
        defaultHoldDownForce = holdDownForce;
    }

    // Update is called once per frame
    void Update()
    {
        PlayerInput();

        isUnder = Physics.CheckBox(headCheck.position, new Vector3(0.2f, headDistance, 0.2f), headCheck.rotation, groundMask);
        groundCheck.position = transform.position - new Vector3(0f, CapsuleCollider.height / 2f) + CapsuleCollider.center;

        if (shouldUncrouch) UnCrouch();

        if (slopeDirection.y > 0f) if (moveSpeed < maxMoveSpeed) moveSpeed += sprintSpeed * Time.deltaTime;

        if (moveSpeed >= (maxMoveSpeed - 3f) && !WindEffect.isEmitting)
        {
            WindEffect.Play();
        }
        else if(moveSpeed < (maxMoveSpeed - 5f) && WindEffect.isEmitting)
        {
            WindEffect.Stop();
            animator.SetBool("bIsSprinting", false);
        }

        if (Forward != 0 || Sideways != 0 && !isSprinting && isOnGround && !isOnWall) animator.SetBool("bIsWalking", true);
        else
        {
            WindEffect.Stop();
            animator.SetBool("bIsWalking", false);
            animator.SetBool("bIsSprinting", false);
        }

        animator.SetBool("bIsOnWall", isOnWall);
        animator.SetBool("bIsCrouching", isCrouching);
        animator.SetBool("bIsOnGround", isOnGround);
        animator.SetBool("hasWeapon", combat.bHasWeapon);
        animator.SetBool("bIsInAir", isInAir);
        animator.SetInteger("wallSide", wallSide);
    }

    // Update used for Physics
    void FixedUpdate()
    {
        PlayerPhysics();
        AddExtraGravity();
        WallRunCheck();
    }

    void PlayerInput()
    {
        if (Input.GetKeyDown(jumpKey)) Jump();

        if (Input.GetKey(sprintKey) && !isCrouching && isOnGround && !isOnWall && Forward != 0) Sprint();
        if (!Input.GetKey(sprintKey) && !isCrouching && isOnGround && !isOnWall) StopSprint();

        if (Input.GetKey(crouchKey) && !isSprinting && isOnGround && !isOnWall) Crouch();
        if (Input.GetKeyUp(crouchKey) && !isSprinting && !isOnWall) UnCrouch();
    }

    void Jump()
    {
        float jump = jumpForce * gravityStrength;

        if (isOnGround)
        {
            Rigidbody.AddForce(new Vector3(0f, jump, 0f));
            canJump = false;
            canDoubleJump = true;
        }
        else if (canDoubleJump)
        {
            canDoubleJump = false;
            canJump = false;
            Rigidbody.velocity = new Vector3(Rigidbody.velocity.x, 0f, Rigidbody.velocity.z);
            Rigidbody.AddForce(new Vector3(0f, jump, 0f));
        }
        else if (canJump)
        {
            canJump = false;
            canDoubleJump = true;
            Rigidbody.velocity = new Vector3(Rigidbody.velocity.x, 0f, Rigidbody.velocity.z);
            Rigidbody.AddForce(new Vector3(0f, jump, 0f));
        }
    }

    void Sprint()
    {
        animator.SetBool("bIsSprinting", true);
        isSprinting = true;
        if (moveSpeed < maxMoveSpeed) moveSpeed += sprintSpeed * Time.deltaTime;
    }

    void StopSprint()
    {
        isSprinting = false;
        if (moveSpeed > defaultMoveSpeed) moveSpeed -= slowDownSpeed * Time.deltaTime;
    }

    void Crouch()
    {
        if (moveSpeed > defaultMoveSpeed)
        {
            isSliding = true;
            isSprinting = false;
            isCrouching = true;
            if (moveSpeed > defaultMoveSpeed) moveSpeed -= slideSlowDownSpeed * Time.deltaTime;
            CapsuleCollider.height = reducedCrouch;
        }
        else
        {
            isCrouching = true;
            moveSpeed = crouchSpeed;
            CapsuleCollider.height = reducedCrouch;
        }
        if(Vector3.Cross(transform.forward, Vector3.Cross(Vector3.up, GroundHit.normal)).y > 0f)
        {
            //Rigidbody.AddForce(new Vector3(moveSpeed,-moveSpeed, transform.forward.z), );
            print("slide down");
        }
    }

    void UnCrouch()
    {
        if (!isUnder)
        {
            isCrouching = false;
            isSliding = false;
            shouldUncrouch = false;
            CapsuleCollider.height = defaultCrouch;
            moveSpeed = defaultMoveSpeed;
        }
        if (isUnder) shouldUncrouch = true;
    }

    //Player Physics and movement
    void PlayerPhysics()
    {
        Sideways = Input.GetAxisRaw("Horizontal");
        Forward = Input.GetAxisRaw("Vertical");

        moveVelocity = ((Sideways * transform.right + Forward * transform.forward) * moveSpeed) + Vector3.up * Rigidbody.velocity.y;
        Rigidbody.velocity = moveVelocity;

    }

    //Extra gravity for Unity Engine with slope Gravity
    void AddExtraGravity()
    {
        Physics.Raycast(groundCheck.position, -groundCheck.up, out GroundHit, groundMask);

        Ledge();
        Ground();
        Slopes();
    }

    void Ground()
    {
        isOnGround = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        if (!isOnGround && shouldGravity)
        {
            gravity = gravityStrength * -holdDownForce * Vector3.up;
            Rigidbody.AddForce(gravity);
            isInAir = true;
        }
        else isInAir = false;
    }

    void Slopes()
    {
        ForwardSlopeAngle = Vector3.Angle(GroundHit.normal, transform.up);
        slopeDirection = Vector3.Cross(moveVelocity, Vector3.Cross(Vector3.up, GroundHit.normal));
        if (ForwardSlopeAngle > 0f && slopeDirection.y <= 0f)
        {
            if (!isInAir)
            {
                Rigidbody.velocity = new Vector3(Rigidbody.velocity.x, 0f, Rigidbody.velocity.z);
            }
        }

        SlopeAngle = Mathf.Abs(Vector3.Angle(GroundHit.normal, transform.up));
        if (SlopeAngle > MaxSlopeAngle)
        {
            isOnGround = false;
            Rigidbody.velocity = new Vector3(Rigidbody.velocity.x, -10f, Rigidbody.velocity.z);
        }
    }

    void WallRunCheck()
    {
        if (isInAir && moveVelocity.y <= 0)
        {
            if (Physics.Raycast(transform.position, transform.right, out WallHit, CapsuleCollider.radius + 0.3f))
            {
                StickToWall();
                wallSide = 1;
            }
            else if(Physics.Raycast(transform.position, -transform.right, out WallHit, CapsuleCollider.radius + 0.3f))
            {
                StickToWall();
                wallSide = -1;
            }
            else
            {
                isOnWall = false;
                wallSide = 0;
            }
        }
        else
        {
            isOnWall = false;
            wallSide = 0;
        }
    }

    void StickToWall()
    {
        Rigidbody.velocity = new Vector3(Rigidbody.velocity.x, -1f, Rigidbody.velocity.z);
        if (moveSpeed > defaultMoveSpeed) moveSpeed -= 4f * Time.deltaTime;

        canJump = true;
        isOnWall = true;
    }

    void Ledge()
    {
        isOnLedge = Physics.CheckBox(ledgeCheck.position, ledgeBox, ledgeCheck.rotation, ledgeMask);
        if (isOnLedge) StickToLedge();
        else
        {
            shouldGravity = true;
            Hand.SetActive(true);
        } 
    }

    void StickToLedge()
    {
        StopSprint();
        Rigidbody.velocity = new Vector3(Rigidbody.velocity.x, 0f, Rigidbody.velocity.z);
        shouldGravity = false;
        Hand.SetActive(false);
        canJump = true;
    }

    //void OnDrawGizmosSelected()
    //{
    //    Gizmos.color = Color.red;
    //    Gizmos.DrawCube(ledgeCheck.position, ledgeBox);

    //    //Gizmos.color = Color.blue;
    //    //Gizmos.DrawSphere(headCheck.position, headDistance);

    //}
}
