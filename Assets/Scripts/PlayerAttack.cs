using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{

    public AnimationClip meleeAttackAnimation;
    public Transform meleeAttackOrigin;
    public LayerMask playerLayer;
    public GameObject bulletPrefab;

    public float meleeAttack1MoveForce = 10f;

    public float meleeAttackRaycastDistance = .2f;

    public float meleeAttack1StunForceX;
    public float meleeAttack1StunForceY;

    public bool attackCanLand = false;
    public bool attackLanded = false;
    public bool isAttacking = false;

    Player _player;
    MovementController _controller;
    Animator _animator;
    ApplyAnimation _applyAnimation;

    float meleeAttackTimer = 0f;

    bool attackAgain = false;


    int meleeAttackPhase = 1;

    void Start()
    {
        _player = GetComponent<Player>();
        _controller = GetComponent<MovementController>();
        _animator = transform.Find("Squash and Stretch").transform.Find("player_sprite").GetComponent<Animator>();
        _applyAnimation = GetComponent<ApplyAnimation>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!_player.isPoweredUp)
        {
            EvaluateAttack();
        }
    }

    public void Attack()
    {
        if (!isAttacking && !_player.isPoweredUp)
        {
            isAttacking = true;
            _applyAnimation.AttackAnimation(meleeAttackPhase);
            meleeAttackTimer = meleeAttackAnimation.length;

            if (_player.IsGrounded)
            {
                _controller.SetHorizontalVelocity(0f);
            }
        }
        else if (_player.isPoweredUp)
        {
            FireBullet();
        }
    }

    public void EvaluateAttack()
    {
        if (isAttacking)
        {
            _player.disablePlayerInput = true;
        }

        if (attackCanLand && isAttacking)
        {
            float smoothAttackMove = Mathf.SmoothStep(0, meleeAttack1MoveForce, meleeAttackTimer / meleeAttackAnimation.length);

            if (isAttacking && _player._isFacingRight && !attackLanded && _player.IsGrounded)
            {
                _controller.SetHorizontalVelocity(smoothAttackMove);
            }
            else if (isAttacking && !_player._isFacingRight && !attackLanded && _player.IsGrounded)
            {
                _controller.SetHorizontalVelocity(-smoothAttackMove);
            }

            Vector2 raycastDirection = transform.right;
            RaycastHit2D[] meleeAttackCollider = Physics2D.RaycastAll(meleeAttackOrigin.position, raycastDirection, meleeAttackRaycastDistance);

            foreach (RaycastHit2D attackRayCast in meleeAttackCollider)
            {
                if (attackRayCast.collider.gameObject.GetInstanceID() != this.gameObject.GetInstanceID() && attackRayCast.collider.gameObject.CompareTag("Player"))
                {
                    Vector2 launchForce;
                    if (!_player._isFacingRight) { launchForce = new Vector2(-meleeAttack1StunForceX, meleeAttack1StunForceY); }
                    else { launchForce = new Vector2(meleeAttack1StunForceX, meleeAttack1StunForceY); }
                    attackRayCast.collider.gameObject.GetComponent<Player>().TriggerStun(launchForce);
                    attackLanded = true;
                    _controller.SetHorizontalVelocity(0f);
                }
                else if (attackRayCast.collider.CompareTag("Tossable"))
                {
                    attackRayCast.collider.GetComponent<TossableObject>().TriggerHit(Mathf.Sign(attackRayCast.collider.gameObject.transform.position.x - transform.position.x), gameObject);
                }
            }
        }

        if (meleeAttackTimer > 0) { meleeAttackTimer -= Time.deltaTime; }
        else
        {
            isAttacking = false;
            attackLanded = false;
            _player.disablePlayerInput = false;
        }
    }

    private void FireBullet() {
       GameObject firedBullet = Instantiate(bulletPrefab, meleeAttackOrigin.position, Quaternion.identity);
       firedBullet.GetComponent<BulletBehavior>().playerColor = _player.playerColor;
       firedBullet.GetComponent<BulletBehavior>().isMovingRight = _player._isFacingRight;
       firedBullet.GetComponent<BulletBehavior>().playerID = _player.gameObject.GetInstanceID();
       transform.Find("bullet flash0").GetComponent<Animator>().SetTrigger("fire");
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;

        Gizmos.DrawLine(meleeAttackOrigin.position,
        new Vector3(meleeAttackOrigin.position.x + meleeAttackRaycastDistance,
        meleeAttackOrigin.position.y,
        meleeAttackOrigin.position.z));
    }

}
