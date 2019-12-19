using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public AnimationClip _meleeAttack1Anim;

    public float meleeAttack1MoveForce = 100f;

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
        if (!isAttacking)
        {
            StartCoroutine("AttackPause");
        }
        else if (isAttacking && meleeAttackPhase == 1)
        {
            attackAgain = true;
        }

    }

    public IEnumerator AttackPause()
    {
        meleeAttackPhase = 1;
        _applyAnimation.AttackAnimation(meleeAttackPhase);
        isAttacking = true;

        _player.disableXInput = true;
        _player.disableJump = true;

        yield return new WaitForSeconds(_meleeAttack1Anim.length);

        _player.disableXInput = false;
        _player.disableJump = false;
        if (!attackAgain)
        {
            isAttacking = false;
        }
        else
        {
            _player.disableXInput = true;
            _player.disableJump = true;

            Debug.Log("SECOND PHASE");
            meleeAttackPhase = 2;
            _applyAnimation.AttackAnimation(meleeAttackPhase);
            isAttacking = true;

            yield return new WaitForSeconds(_meleeAttack1Anim.length);


        }

        attackAgain = false;
        meleeAttackPhase = 1;
        _player.disableXInput = false;
        _player.disableJump = false;
        isAttacking = false;

    }


}
