using UnityEngine;

public class Player : MonoBehaviour
{
    public int playerNumber;

    [Tooltip("Jump strenght.")]
    public float JumpMagnitude = 12f;

    [Tooltip("The amount of force used to interrupt a jump.")]
    public float JumpInterruptStrength = 120f;

    [Tooltip("How long can the player still be considered grounded after leaving the ground?")]
    public float GroundedLinger = 0.05f;

    [Tooltip("The distance below the player where jump input is registered while falling.")]
    public float GroundCheckDistance = 0.5f;

    [Tooltip("Will the player be able to slide off walls?")]
    public bool WallSlide = true;

    [Tooltip("How fast does the player slide off walls?")]
    [Range(0, 1)]
    public float WallFriction = 0.5f;

    [Tooltip("Is wall jumping allowed?")]
    public bool WallJump = true;

    [Tooltip("Direction and strength of wall jump")]
    public Vector2 WallJumpForce = new Vector2(12, 12);

    [Tooltip("How long can wall jump still be performed after not touching a wall?")]
    public float WallLinger = 0.1f;

    public bool isStunned = false;

    public float bounceForceScaleX = 20f;
    public float bounceForceScaleY = 20f;
    public float stunnedTime = 2f;

    [System.NonSerialized]
    public string xInput, yInput, jumpInput, attackInput;

    public bool Jumpping { get; set; }
    public bool JumpWhenGrounded { get; set; }
    public bool IsGrounded
    {
        get
        {
            if (_controller.State.IsCollidingBelow)
            {
                _groundLingerTime = 0;
                return true;
            }
            if (_groundLingerTime < GroundedLinger)
                return true;

            return false;
        }
    }
    public bool GroundIsNear
    {
        get
        {
            var rayOrigin = new Vector2(_transform.position.x, _transform.position.y + _playerCollider.offset.y - _playerCollider.size.y / 2 - 0.01f);
            var rayHit = Physics2D.Raycast(rayOrigin, Vector2.down, GroundCheckDistance);
            Debug.DrawRay(rayOrigin, Vector2.down * GroundCheckDistance, Color.green);
            return rayHit;
        }
    }

    public PhysicsMaterial2D stunnedMaterial;

    public bool AnticipateJump { get { return !IsGrounded && GroundIsNear && _controller.Velocity.y < 0; } }
    public bool IsTouchingWall { get { return _controller.State.IsCollidingLeft || _controller.State.IsCollidingRight; } }
    public bool CanWallJump { get { return WallJump && (IsTouchingWall || _wallLingerTime < WallLinger); } }
    public bool _isFacingRight;

    public bool disablePlayerInput = false;

    private enum Walls { left, rigth };

    private float _normalizedHorizontalSpeed;
    private float _groundLingerTime;
    private float _wallLingerTime;
    private float stunnedTimer = 0;

    private Walls _lastWallTouched;

    private Transform _transform;
    private BoxCollider2D _playerCollider;
    private MovementController _controller;
    private Rigidbody2D _playerRB;

    void Awake()
    {
        _transform = transform;
        _playerCollider = GetComponent<BoxCollider2D>();
        _controller = GetComponent<MovementController>();
        _playerRB = GetComponent<Rigidbody2D>();
        _isFacingRight = _transform.localScale.x > 0;


    }

    void Update()
    {
        if (playerNumber != 0)
        {
            xInput = "Horizontal" + playerNumber;
            yInput = "Vertical" + playerNumber;
            jumpInput = "Jump" + playerNumber;
            attackInput = "Attack" + playerNumber;
        }

        _groundLingerTime += Time.deltaTime;
        if (IsTouchingWall)
        {
            if (_controller.State.IsCollidingLeft)
                _lastWallTouched = Walls.left;
            else _lastWallTouched = Walls.rigth;
            _wallLingerTime = 0;
        }
        else _wallLingerTime += Time.deltaTime;

        if (_controller.Velocity.y < 0)
            Jumpping = false;

        if (WallSlide && IsTouchingWall && _controller.Velocity.y <= 0)
        {
            if (WallFriction == 1)
                _controller.Parameters.Flying = true;
            _controller.SetVerticalVelocity(_controller.Velocity.y * (1 - WallFriction));
        }
        else _controller.Parameters.Flying = false;

        HandleInput();

        if (isStunned)
        {
            ApplyStun();
        }

        var acceleration = IsGrounded ? _controller.Parameters.AccelerationOnGround : _controller.Parameters.AccelerationInAir;

        if (!disablePlayerInput)
        {
            _controller.SetHorizontalVelocity(Mathf.Lerp(_controller.Velocity.x, _normalizedHorizontalSpeed * _controller.Parameters.MaxSpeed, Time.deltaTime * acceleration));
        }
    }

    public void TriggerStun(Vector2 launchDirection)
    {
        if (!isStunned)
        {
            stunnedTimer = stunnedTime;
            isStunned = true;

            _playerRB.bodyType = RigidbodyType2D.Dynamic;
            _playerRB.sharedMaterial = stunnedMaterial;
            _playerRB.mass = 1;

            _playerRB.AddForce(launchDirection, ForceMode2D.Impulse);
            Debug.Log(launchDirection);

        }

        // _controller.SetHorizontalVelocity(0f);
        // _controller.AddForce(launchDirection * launchForce);
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        ApplyBounce(other);
    }

    void HandleInput()
    {


        if (!disablePlayerInput)
        {
            float horizontalInputRaw = Mathf.Round(Input.GetAxisRaw(xInput));
            _normalizedHorizontalSpeed = Input.GetAxis(xInput);

            if ((horizontalInputRaw < 0 && _isFacingRight) ||
                 (horizontalInputRaw > 0 && !_isFacingRight))
                Flip();

            if (AnticipateJump && Input.GetButtonDown(jumpInput))
                JumpWhenGrounded = true;

            if ((Input.GetButtonDown(jumpInput) && IsGrounded && !Jumpping) || (JumpWhenGrounded && IsGrounded))
                Jump(JumpMagnitude);

            else if (CanWallJump && Input.GetButtonDown(jumpInput))
                JumpOffWall(WallJumpForce);

            if (Jumpping && !Input.GetButton(jumpInput))
                _controller.AddVerticalForce(-JumpInterruptStrength);

            if (Input.GetButtonDown(attackInput))
            {
                gameObject.GetComponent<PlayerAttack>().Attack();
            }

            _controller.State.DropThroughPlatform = Input.GetAxisRaw(yInput) < 0;
        }
    }

    void ApplyStun()
    {
        if (stunnedTimer > 0)
        {
            stunnedTimer -= Time.deltaTime;
            disablePlayerInput = true;
        }
        else
        {
            isStunned = false;
            disablePlayerInput = false;

            _playerRB.sharedMaterial = null;
            _playerRB.bodyType = RigidbodyType2D.Kinematic;
            _playerRB.mass = .000001f;
        }
    }

    void Jump(float magnitude)
    {
        JumpWhenGrounded = false;
        Jumpping = true;
        _controller.SetVerticalVelocity(magnitude);
    }

    void JumpOffWall(Vector2 force)
    {
        JumpWhenGrounded = false;
        Jumpping = true;
        var jumpVector = new Vector2(_lastWallTouched == Walls.left ? force.x : -force.x, force.y);
        _controller.SetVelocity(jumpVector);
    }

    void Flip()
    {
        _transform.localScale = new Vector3(-_transform.localScale.x, _transform.localScale.y, _transform.localScale.z);
        _isFacingRight = !_isFacingRight;
    }

    private void ApplyBounce(Collision2D other)
    {
        float bounceForceX;
        float bounceForceY = bounceForceScaleY;

        if (other.gameObject.tag == "Player")
        {

            if (other.gameObject.transform.position.x > transform.position.x)
            {
                bounceForceX = -bounceForceScaleX;
            }
            else
            {
                bounceForceX = bounceForceScaleX;
            }

            _controller.SetHorizontalVelocity(bounceForceX);
            _controller.SetVerticalVelocity(bounceForceY);
        }
    }

}