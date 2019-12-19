using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{

    public List<AnimationClip> meleeAttackAnimations = new List<AnimationClip>();
    public Transform meleeAttackOrigin;
    public LayerMask playerLayer;

    public float meleeAttack1MoveForce = 100f;

    public float meleeAttackRaycastDistance = .2f;

    Player _player;
    MovementController _controller;
    Animator _animator;
    ApplyAnimation _applyAnimation;

    bool attackAgain = false;
    bool isAttacking = false;

    int meleeAttackPhase = 1;
    int totalAttacks = 3;

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
        if (isAttacking && _player._isFacingRight)
        {
            _controller.SetHorizontalVelocity(meleeAttack1MoveForce);
        }
        else if (isAttacking && !_player._isFacingRight)
        {
            _controller.SetHorizontalVelocity(-meleeAttack1MoveForce);
        }


    }

    public void Attack()
    {
        if (!isAttacking && !attackAgain)
        {
            StartCoroutine("AttackPause");
        }
        else if (isAttacking)
        {
            attackAgain = true;
        }

    }

    public IEnumerator AttackPause()
    {
        isAttacking = true;

        while (isAttacking || attackAgain)
        {
            _player.disablePlayerInput = true;
            attackAgain = false;

            _applyAnimation.AttackAnimation(meleeAttackPhase);

            Vector2 raycastDirection = _player._isFacingRight ? transform.right : -transform.right;
            RaycastHit2D[] meleeAttackCollider = Physics2D.RaycastAll(meleeAttackOrigin.position, raycastDirection, meleeAttackRaycastDistance);

            // if (meleeRaycastResult.collider.gameObject.GetInstanceID() != this.gameObject.GetInstanceID())


            yield return new WaitForSeconds(meleeAttackAnimations[meleeAttackPhase - 1].length);

            isAttacking = false;
            meleeAttackPhase = meleeAttackPhase == 1 ? 2 : 1;

        }

        attackAgain = false;
        meleeAttackPhase = 1;
        _player.disablePlayerInput = false;
        meleeAttackPhase = 1;
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
