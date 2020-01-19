﻿using UnityEngine;

public class Player : MonoBehaviour
{
    public int playerNumber;
    public int powerskullCount = 0;
    public int health = 3;
    public int poweredUpAmmo = 3;

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

    public GameObject _playerSprite;

    public bool isStunned = false;
    public bool stunnedAir = false;

    public float bounceForceScaleX = 20f;
    public float bounceForceScaleY = 20f;
    public float stunnedTime = 2f;

    public Color playerColor;

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

    public ParticleSystem stunnedParticleSystem;

    public bool AnticipateJump { get { return !IsGrounded && GroundIsNear && _controller.Velocity.y < 0; } }
    public bool IsTouchingWall { get { return _controller.State.IsCollidingLeft || _controller.State.IsCollidingRight; } }
    public bool CanWallJump { get { return WallJump && (IsTouchingWall || _wallLingerTime < WallLinger); } }
    public bool _isFacingRight;
    public bool disablePlayerInput = false;

    public bool isPoweredUp = false;
    public float poweredUpTime = 10f;
    public float poweredUpTimer = 0f;

    private enum Walls { left, rigth };

    private float _normalizedHorizontalSpeed;
    private float _groundLingerTime;
    private float _wallLingerTime;
    private float stunnedTimer = 0f;
    
    

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

        stunnedParticleSystem.Stop();


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

        if (powerskullCount == 3)
        {
            TriggerPoweredUp();
        }

        HandleInput();

        if (isPoweredUp)
        {
            ApplyPoweredUp();
        }

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

            _controller.SetVelocity(new Vector2(0f, 0f));
            _controller.SetVelocity(launchDirection);

        }
    }

    public void TriggerPoweredUp()
    {
        isPoweredUp = true;
        poweredUpTimer = poweredUpTime;
        _playerSprite.GetComponent<SpriteRenderer>().color = Color.green;
        GetComponent<PlayerAttack>().ammoCount = poweredUpAmmo;

    }

    void DisablePoweredUp()
    {
        isPoweredUp = false;
        poweredUpTimer = 0f;
        _playerSprite.GetComponent<SpriteRenderer>().color = Color.white;
    }

    void ApplyPoweredUp()
    {
        poweredUpTimer -= Time.deltaTime;

        if (poweredUpTimer <= 0)
        {
            DisablePoweredUp();
        }

    }

    void ApplyStun()
    {
        if (!stunnedAir && !IsGrounded) stunnedAir = true;

        if (stunnedAir && IsGrounded)
        {
            stunnedTimer = 0;
            stunnedAir = false;
            isStunned = false;
        }

        if (isStunned)
        {
            stunnedTimer -= Time.deltaTime;
            disablePlayerInput = true;

            if (_controller.State.IsCollidingRight || _controller.State.IsCollidingLeft)
            {
                _controller.SetHorizontalVelocity(-_controller.Velocity.x);
            }

            if (!stunnedParticleSystem.isPlaying) stunnedParticleSystem.Play();

            if (Mathf.Sign(_controller.Velocity.x) == 1 && Mathf.Sign(transform.localScale.x) == -1)
            {
                Flip();
            }
            else if (Mathf.Sign(_controller.Velocity.x) == -1 && Mathf.Sign(transform.localScale.x) == 1)
            {
                Flip();
            }
        }
        else
        {
            isStunned = false;
            disablePlayerInput = false;
            stunnedTimer = 0;
            stunnedParticleSystem.Stop();
        }
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
                TriggerPoweredUp();
            }

            _controller.State.DropThroughPlatform = Input.GetAxisRaw(yInput) < 0;
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

    private void OnCollisionEnter2D(Collision2D other)
    {
        bool otherTagIsPlayer = other.gameObject.CompareTag("Player");
        bool otherIsNotSelf = other.gameObject.GetInstanceID() != gameObject.GetInstanceID();
        bool otherIsStunned;

        if (otherTagIsPlayer)
        {
            otherIsStunned = other.gameObject.GetComponent<Player>().isStunned;
        }
        else
        {
            otherIsStunned = false;
        }


        if (otherTagIsPlayer && otherIsNotSelf)
        {
            ApplyBounce(transform.position - other.collider.gameObject.transform.position);
            // Debug.Log(gameObject.name + " " + (transform.position - other.collider.gameObject.transform.position));
        }
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player") && other.gameObject.GetInstanceID() != gameObject.GetInstanceID())
        {
            ApplyBounce(transform.position - other.collider.gameObject.transform.position);
            // Debug.Log(gameObject.name + " " + (transform.position - other.collider.gameObject.transform.position));
        }
    }

    public void ApplyBounce(Vector2 bounceDirection)
    {
        float bounceForceX = bounceDirection.normalized.x * bounceForceScaleX;
        float bounceForceY;

        if (IsGrounded)
        {
            bounceForceY = bounceForceScaleY;
        }
        else
        {
            bounceForceY = bounceDirection.normalized.y * bounceForceScaleY;
        }

        if (!isStunned && !GetComponent<PlayerAttack>().isAttacking)
        {

            _controller.SetHorizontalVelocity(bounceForceX);
            _controller.SetVerticalVelocity(bounceForceY);
        }

        if (isStunned)
        {
            if (Mathf.Sign(bounceDirection.x) == -1)
            {
                transform.Translate(-Vector2.right * _playerCollider.bounds.extents.x);
            }
            else
            {
                transform.Translate(Vector2.right * _playerCollider.bounds.extents.x);
            }
            _controller.SetHorizontalVelocity(-_controller.Velocity.x);
        }
    }

}