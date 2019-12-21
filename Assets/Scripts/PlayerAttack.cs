﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{

    public List<AnimationClip> meleeAttackAnimations = new List<AnimationClip>();
    public Transform meleeAttackOrigin;
    public LayerMask playerLayer;

    public float meleeAttack1MoveForce = 10f;

    public float meleeAttackRaycastDistance = .2f;

    public float meleeAttack1StunForceX;
    public float meleeAttack1StunForceY;

    public bool attackCanLand = false;

    Player _player;
    MovementController _controller;
    Animator _animator;
    ApplyAnimation _applyAnimation;

    float meleeAttackTimer = 0f;

    bool attackAgain = false;
    bool isAttacking = false;

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
        EvaluateAttack();
    }

    public void Attack()
    {
        if (!isAttacking)
        {
            isAttacking = true;
            _applyAnimation.AttackAnimation(meleeAttackPhase);
            meleeAttackTimer = meleeAttackAnimations[0].length;
            _controller.SetHorizontalVelocity(0f);
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
            float smoothAttackMove = Mathf.SmoothStep(0, meleeAttack1MoveForce, meleeAttackTimer / meleeAttackAnimations[0].length);

            if (isAttacking && _player._isFacingRight)
            {
                _controller.SetHorizontalVelocity(smoothAttackMove);
            }
            else if (isAttacking && !_player._isFacingRight)
            {
                _controller.SetHorizontalVelocity(-smoothAttackMove);
            }

            Vector2 raycastDirection = transform.right;
            RaycastHit2D[] meleeAttackCollider = Physics2D.RaycastAll(meleeAttackOrigin.position, raycastDirection, meleeAttackRaycastDistance);

            foreach (RaycastHit2D attackRayCast in meleeAttackCollider)
            {
                if (attackRayCast.collider.gameObject.GetInstanceID() != this.gameObject.GetInstanceID())
                {
                    Vector2 launchForce;
                    if (!_player._isFacingRight) { launchForce = new Vector2(-meleeAttack1StunForceX, meleeAttack1StunForceY); }
                    else { launchForce = new Vector2(meleeAttack1StunForceX, meleeAttack1StunForceY); }
                    attackRayCast.collider.gameObject.GetComponent<Player>().TriggerStun(launchForce);
                }
            }
        }

        if (meleeAttackTimer > 0) { meleeAttackTimer -= Time.deltaTime; }
        else { 
            isAttacking = false; 
            _player.disablePlayerInput = false;
        }




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
