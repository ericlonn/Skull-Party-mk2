using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{

    public AnimationClip meleeAttackAnimation;
    public Transform meleeAttackOrigin;
    public Transform kickAttackOrigin;
    public LayerMask playerLayer;
    public GameObject bulletPrefab;

    public float meleeAttack1MoveForce = 10f;
    public Vector2 kickAttackMoveForce = new Vector2(5f, 5f);

    public float meleeAttackRaycastDistance = .2f;

    public float meleeAttack1StunForceX;
    public float meleeAttack1StunForceY;

    public bool attackCanLand = true;
    public bool attackLanded = false;
    public bool isAttacking = false;

    public int ammoCount = 0;

    Player _player;
    MovementController _controller;
    Animator _animator;
    ApplyAnimation _applyAnimation;
    PlaySound _soundPlayer;

    float meleeAttackTimer = 0f;


    int attackType = 1;

    void Start()
    {
        _player = GetComponent<Player>();
        _controller = GetComponent<MovementController>();
        _animator = transform.Find("Squash and Stretch").transform.Find("player_sprite").GetComponent<Animator>();
        _applyAnimation = GetComponent<ApplyAnimation>();
        _soundPlayer = GetComponent<PlaySound>();
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
        if (!isAttacking && !_player.isPoweredUp && !_player.isStunned)
        {
            isAttacking = true;
            attackType = 1;
            _applyAnimation.AttackAnimation(attackType);
            meleeAttackTimer = meleeAttackAnimation.length;

            // if (_player.IsGrounded)
            // {
            //     _controller.SetHorizontalVelocity(0f);
            // }
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
        if (attackType == 1)
        {
            if (attackCanLand && isAttacking)
            {
                float smoothAttackMove = Mathf.SmoothStep(0, meleeAttack1MoveForce, meleeAttackTimer / meleeAttackAnimation.length);

                if (isAttacking && _player._isFacingRight && !attackLanded)
                {
                    _controller.SetHorizontalVelocity(smoothAttackMove);
                }
                else if (isAttacking && !_player._isFacingRight && !attackLanded)
                {
                    _controller.SetHorizontalVelocity(-smoothAttackMove);
                }
                Debug.Log(_controller.Velocity.x);

                Vector2 raycastDirection;

                if (_player._isFacingRight)
                {
                    raycastDirection = transform.right;
                }
                else
                {
                    raycastDirection = -transform.right;
                }

                RaycastHit2D[] meleeAttackCollider = Physics2D.RaycastAll(meleeAttackOrigin.position, raycastDirection, meleeAttackRaycastDistance);
                // Vector3 debugPunchOffsetX = new Vector3(meleeAttackOrigin.position.x + (Mathf.Sign() meleeAttackRaycastDistance, 
                //                                         meleeAttackOrigin.position.y, 
                //                                         meleeAttackOrigin.position.z);
                // Debug.DrawLine(meleeAttackOrigin.position, debugPunchOffsetX, Color.yellow); 



                foreach (RaycastHit2D attackRayCast in meleeAttackCollider)
                {
                    if (attackRayCast.collider.gameObject.GetInstanceID() != this.gameObject.GetInstanceID() && attackRayCast.collider.gameObject.CompareTag("Player"))
                    {
                        Vector2 launchForce;

                        if (!_player._isFacingRight) { launchForce = new Vector2(-meleeAttack1StunForceX, meleeAttack1StunForceY); }
                        else { launchForce = new Vector2(meleeAttack1StunForceX, meleeAttack1StunForceY); }

                        if (!attackRayCast.collider.gameObject.GetComponent<Player>().isStunned)
                        {
                            attackRayCast.collider.gameObject.GetComponent<Player>().TriggerStun(launchForce, false);
                            _player.score += 30;
                            _soundPlayer.PlayClip(6, false);

                            if (attackRayCast.collider.gameObject.GetComponent<Player>().isPoweredUp)
                            {
                                attackRayCast.collider.gameObject.GetComponent<PlayerAttack>().ammoCount--;
                            }
                            attackRayCast.collider.gameObject.GetComponent<Player>().EjectPowerskull();
                        }

                        attackLanded = true;
                        _controller.SetHorizontalVelocity(0f);
                    }
                    else if (attackRayCast.collider.CompareTag("Tossable"))
                    {
                        attackRayCast.collider.GetComponent<TossableObject>().TriggerHit(Mathf.Sign(attackRayCast.collider.gameObject.transform.position.x - transform.position.x), gameObject);
                        if (!attackRayCast.collider.gameObject.GetComponent<TossableObject>().isTossed)
                        {
                            _player.score += 30;
                        }
                    }
                }
            }

            if (meleeAttackTimer > 0) { meleeAttackTimer -= Time.deltaTime; }
            else
            {
                isAttacking = false;
                attackLanded = false;
                _player.disablePlayerInput = false;
                attackType = 0;
            }
        }
    }

    private void FireBullet()
    {
        GameObject firedBullet = Instantiate(bulletPrefab, meleeAttackOrigin.position, Quaternion.identity);
        firedBullet.GetComponent<BulletBehavior>().playerColor = _player.playerColor;
        firedBullet.GetComponent<BulletBehavior>().isMovingRight = _player._isFacingRight;
        firedBullet.GetComponent<BulletBehavior>().playerID = _player.gameObject.GetInstanceID();
        firedBullet.GetComponent<BulletBehavior>().playerResponsible = gameObject;
        transform.Find("bullet flash0").GetComponent<Animator>().SetTrigger("fire");
        ammoCount -= 1;
        if (ammoCount == 0)
        {
            _player.powerskullCount = 0;
        }

        _soundPlayer.PlayClip(2, false);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawLine(meleeAttackOrigin.position,
        new Vector3(meleeAttackOrigin.position.x + meleeAttackRaycastDistance,
        meleeAttackOrigin.position.y,
        meleeAttackOrigin.position.z));
    }

}
