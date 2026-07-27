using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour
{
    private float speed = 10f;

    private PlayerInput playerInput;

    private Vector2 moveInput;
    private Vector2 lookInput;

    public Transform cam;
    public Transform playerMesh;
    public Transform cameraPivot;

    public CharacterController controller;
    private float gravity = -20f;
    private float jumpForce = 12f;

    float yVelocity;

    private bool jumpPressed;
    int jumpCount = 0;
    private int maxJumps = 2;

    // ---- DASH ----
    private float dashForce = 15f;
    private float dashDuration = 0.4f;
    private float dashCooldown = 1f;
    bool dashPressed;
    bool isDashing;
    float dashTimer;
    float dashCooldownTimer;
    Vector3 dashDirection;

    [SerializeField] private Animator animator;

    void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
    }

    void OnEnable()
    {
        playerInput.actions["Move"].performed += OnMove;
        playerInput.actions["Move"].canceled += OnMove;

        playerInput.actions["Look"].performed += OnLook;
        playerInput.actions["Look"].canceled += OnLook;

        playerInput.actions["Jump"].performed += OnJump;
        playerInput.actions["Dash"].performed += OnDash;
    }

    void OnDisable()
    {
        playerInput.actions["Move"].performed -= OnMove;
        playerInput.actions["Move"].canceled -= OnMove;

        playerInput.actions["Look"].performed -= OnLook;
        playerInput.actions["Look"].canceled -= OnLook;

        playerInput.actions["Jump"].performed -= OnJump;
        playerInput.actions["Dash"].performed -= OnDash;
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    private void OnLook(InputAction.CallbackContext context)
    {
        lookInput = context.ReadValue<Vector2>();
    }

    private void OnJump(InputAction.CallbackContext context)
    {
        jumpPressed = true;
    }

    private void OnDash(InputAction.CallbackContext context)
    {
        dashPressed = true;
    }

    void Update()
    {
        HandleDash();
        Move();
        UpdateAnimator();
    }

    void Move()
    {
        Vector3 camForward = cam.forward;
        Vector3 camRight = cam.right;

        camForward.y = 0;
        camRight.y = 0;

        Vector3 dir = camForward * moveInput.y + camRight * moveInput.x;

        // ---- SOL ----
        if (controller.isGrounded)
        {
            //if (yVelocity < 0)
            //    yVelocity = -2f;
            jumpCount = 0;
        }

        // ---- JUMP ----
        if (jumpPressed && jumpCount < maxJumps)
        {
            yVelocity = jumpForce;
            jumpCount++;

            animator.SetTrigger("Jump");
        }
        jumpPressed = false;

        //---- GRAVITE ----
        if (controller.isGrounded && yVelocity < 0)
            yVelocity = -2f; // colle au sol

        yVelocity += gravity * Time.deltaTime;

        //Vector3 velocity = dir.normalized * speed;
        Vector3 velocity = dir.magnitude > 0.01f ? dir.normalized * speed : Vector3.zero;
        velocity.y = yVelocity;

        controller.Move(velocity * Time.deltaTime);

        //---- ROTATION MESH ----
        if (dir.magnitude > 0.01f)
        {
            // rotation du mesh seulement
            Quaternion targetRot = Quaternion.LookRotation(dir);
            playerMesh.rotation = Quaternion.Slerp(
                playerMesh.rotation,
                targetRot,
                12f * Time.deltaTime
            );
        }
    }

    void HandleDash()
    {
        dashCooldownTimer -= Time.deltaTime;

        if (dashPressed && !isDashing && dashCooldownTimer <= 0f)
        {
            dashDirection = playerMesh.forward.normalized;

            isDashing = true;
            dashTimer = dashDuration;
            dashCooldownTimer = dashCooldown;
        }
        dashPressed = false;

        if (isDashing)
        {
            dashTimer -= Time.deltaTime;
            controller.Move(dashDirection * dashForce * Time.deltaTime);

            if (dashTimer <= 0f)
                isDashing = false;
        }
    }

    private void UpdateAnimator()
    {
        // Si le joueur est en l'air, on ne joue plus l'animation de course
        float animationSpeed = controller.isGrounded
            ? (isDashing ? 1f : moveInput.magnitude)
            : 0f;

        animator.SetFloat(
            "Speed",
            animationSpeed,
            0.1f,
            Time.deltaTime
        );

        animator.SetBool("Grounded", controller.isGrounded);

        animator.SetFloat("VerticalSpeed", yVelocity);

        animator.SetBool("Dashing", isDashing);
    }
}