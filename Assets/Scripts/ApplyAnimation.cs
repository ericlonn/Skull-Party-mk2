using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplyAnimation : MonoBehaviour
{

    Animator playerAnim;
    Player _player;
    MovementController _controller;
    SquashAndStretch _sAndSController;


    // Start is called before the first frame update
    void Start()
    {
        playerAnim = transform.Find("Squash and Stretch").transform.Find("player_sprite").GetComponent<Animator>();
        _player = GetComponent<Player>();
        _controller = GetComponent<MovementController>();

    }

        // Update is called once per frame
        void Update()
        {
            if (_player.IsGrounded && Mathf.Round(Input.GetAxisRaw(_player.xInput)) == 0)
            {
                playerAnim.SetBool("isRunning", false);
            }
            else
            {
                playerAnim.SetBool("isRunning", true);
            }

            if (_player.IsGrounded) {
                playerAnim.SetBool("isGrounded", true);
            } else {
                playerAnim.SetBool("isGrounded", false);
            }

            if (_player.IsTouchingWall) {
                playerAnim.SetBool("isWallSliding", true);
            } else {
                playerAnim.SetBool("isWallSliding", false);
            } 

            if (_player.isStunned) {
                playerAnim.SetBool("isStunned", true);
            } else {
                playerAnim.SetBool("isStunned", false);
            }


        }

        public void AttackAnimation(int attackPhase) {
            if (attackPhase == 1){
                playerAnim.SetTrigger("meleeAttack1");    
            }
            else if (attackPhase == 2) {
                playerAnim.SetBool("kick", true);
            }
            else if (attackPhase == 0) {
                playerAnim.SetBool("kick", false);
            }
            
        }
    }