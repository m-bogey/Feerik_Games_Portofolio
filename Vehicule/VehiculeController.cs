using UnityEngine;
using UnityEngine.InputSystem;

public class VehicleController : MonoBehaviour
{
    [Header("Mouvement")]
    private float forwardSpeed = 40f;

    [Header("Virage")]
    private float turnSpeed = 80f;
    private float steerLerpSpeed = 10f;

    [Header("Saut")]
    private float jumpForce = 12f;

    // ── State direction ───────────────────────────────────────────────────
    private float _currentSteer = 0f;
    private bool _p1Hold = false;
    private bool _p2Hold = false;

    // ── State saut ────────────────────────────────────────────────────────
    private bool _p1CanJump = true;
    private bool _p2CanJump = true;
    private bool _p1JumpPressed = false;
    private bool _p2JumpPressed = false;

    // ── Composants ────────────────────────────────────────────────────────
    private Rigidbody _rb;

    // ── Input actions ─────────────────────────────────────────────────────
    private InputAction _p1MoveAction;
    private InputAction _p2MoveAction;
    private InputAction _p1JumpAction;
    private InputAction _p2JumpAction;

    // ─────────────────────────────────────────────────────────────────────
    [SerializeField] private MoveMode moveMode = MoveMode.Ground;
    // -- Water ------------------------------------------------------------
    public enum MoveMode
    {
        Ground,
        Water
    }
    
    [Header("Water")]

    [SerializeField] private float paddleForce = 2f;
    [SerializeField] private float paddleTurnImpulse = 15f;

    [SerializeField] private float waterMaxSpeed = 20f;
    [SerializeField] private float waterSlowDown = 2f;
    [SerializeField] private float turnSlowDown = 10f;

    private float waterSpeed = 0f;
    private float turnVelocity = 0f;

    private bool _p1Paddle;
    private bool _p2Paddle;

    // ---------------------------------------------------------------------

    void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    void OnEnable()
    {
        foreach (var pi in FindObjectsByType<PlayerInput>(FindObjectsSortMode.None))
            BindPlayer(pi);
    }

    void OnDisable()
    {
        if (_p1MoveAction != null) { _p1MoveAction.performed -= OnP1Performed; _p1MoveAction.canceled -= OnP1Canceled; }
        if (_p2MoveAction != null) { _p2MoveAction.performed -= OnP2Performed; _p2MoveAction.canceled -= OnP2Canceled; }
        if (_p1JumpAction != null) _p1JumpAction.performed -= OnP1Jump;
        if (_p2JumpAction != null) _p2JumpAction.performed -= OnP2Jump;
    }

    void BindPlayer(PlayerInput pi)
    {
        InputAction move = pi.actions["Move"];
        InputAction jump = pi.actions["Jump"];

        if (pi.playerIndex == 0)
        {
            if (move != null) { _p1MoveAction = move; move.performed += OnP1Performed; move.canceled += OnP1Canceled; }
            if (jump != null) { _p1JumpAction = jump; jump.performed += OnP1Jump; }
            Debug.Log("[VehicleController] Joueur 1 bindé");
        }
        else if (pi.playerIndex == 1)
        {
            if (move != null) { _p2MoveAction = move; move.performed += OnP2Performed; move.canceled += OnP2Canceled; }
            if (jump != null) { _p2JumpAction = jump; jump.performed += OnP2Jump; }
            Debug.Log("[VehicleController] Joueur 2 bindé");
        }
    }

    // ── Callbacks direction ───────────────────────────────────────────────
    private void OnP1Performed(InputAction.CallbackContext ctx)
    {
        _p1Hold = true;
        _p1Paddle = true;
    }

    private void OnP2Performed(InputAction.CallbackContext ctx)
    {
        _p2Hold = true;
        _p2Paddle = true;
    }

    private void OnP1Canceled(InputAction.CallbackContext ctx)
    {
        _p1Hold = false;
    }

    private void OnP2Canceled(InputAction.CallbackContext ctx)
    {
        _p2Hold = false;
    }

    // ── Callbacks saut ────────────────────────────────────────────────────
    private void OnP1Jump(InputAction.CallbackContext ctx)
    {
        if (_p1CanJump) { _p1JumpPressed = true; _p1CanJump = false; }
    }
    private void OnP2Jump(InputAction.CallbackContext ctx)
    {
        if (_p2CanJump) { _p2JumpPressed = true; _p2CanJump = false; }
    }

    // ─────────────────────────────────────────────────────────────────────

    void FixedUpdate()
    {
        HandleSteering();
        if (moveMode == MoveMode.Ground)
            MoveVehicle();
        else
            WaterMove();
        upSpeedWithTime();
    }

    void HandleSteering()
    {
        float targetSteer = 0f;
        if (_p1Hold && !_p2Hold) targetSteer = -1f;
        else if (_p2Hold && !_p1Hold) targetSteer = 1f;

        _currentSteer = Mathf.MoveTowards(_currentSteer, targetSteer, steerLerpSpeed * Time.deltaTime);
    }

    void MoveVehicle()
    {
        transform.Rotate(0f, _currentSteer * turnSpeed * Time.deltaTime, 0f);

        // ── Saut via AddForce sur le Rigidbody ────────────────────────────
        if (_rb != null)
        {
            if (_p1JumpPressed)
            {
                _rb.linearVelocity = new Vector3(_rb.linearVelocity.x, 0f, _rb.linearVelocity.z);
                _rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
                _p1JumpPressed = false;
            }
            else if (_p2JumpPressed)
            {
                _rb.linearVelocity = new Vector3(_rb.linearVelocity.x, 0f, _rb.linearVelocity.z);
                _rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
                _p2JumpPressed = false;
            }

            // Avance — laisse le RB gérer la gravité Y tout seul
            _rb.linearVelocity = new Vector3(
                transform.forward.x * forwardSpeed,
                _rb.linearVelocity.y,   //  ne touche pas au Y
                transform.forward.z * forwardSpeed
            );
        }
    }

    void upSpeedWithTime()
    {
        if (forwardSpeed < 50)
            forwardSpeed += (Time.deltaTime * 0.1f);
        Debug.Log(forwardSpeed);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Water"))
        {
            moveMode = MoveMode.Water;
            waterSpeed = forwardSpeed;
        }

        if (other.CompareTag("Ground"))
        {
            moveMode = MoveMode.Ground;
        }

        if (other.CompareTag("KillZone"))
            forwardSpeed = 40;
    }

    private void OnCollisionEnter(Collision collision)
    {
        _p1CanJump = true;
        _p2CanJump = true;
    }

    void WaterMove()
    {
        // ----- Coup de rame -----

        if (_p1Paddle)
        {
            waterSpeed += paddleForce;
            turnVelocity += paddleTurnImpulse;

            _p1Paddle = false;
        }

        if (_p2Paddle)
        {
            waterSpeed += paddleForce;
            turnVelocity -= paddleTurnImpulse;

            _p2Paddle = false;
        }

        // ----- Inertie -----

        waterSpeed = Mathf.MoveTowards(
            waterSpeed,
            0f,
            waterSlowDown * Time.deltaTime
        );

        turnVelocity = Mathf.MoveTowards(
            turnVelocity,
            0f,
            turnSlowDown * Time.deltaTime
        );

        waterSpeed = Mathf.Clamp(waterSpeed, 0f, waterMaxSpeed);

        // Rotation
        transform.Rotate(
            0f,
            turnVelocity * Time.deltaTime,
            0f
        );

        // Déplacement
        _rb.linearVelocity = new Vector3(
            transform.forward.x * waterSpeed,
            _rb.linearVelocity.y,
            transform.forward.z * waterSpeed
        );
    }
}