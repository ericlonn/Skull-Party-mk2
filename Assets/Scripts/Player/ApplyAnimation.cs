using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class ApplyAnimation : MonoBehaviour
{

    Animator playerAnim;
    Player _player;
    MovementController _controller;
    SquashAndStretch _sAndSController;
    PlaySound _soundPlayer;
    Rewired.Player playerInput;


    // Start is called before the first frame update
    void Start()
    {
        playerAnim = transform.Find("Squash and Stretch").transform.Find("player_sprite").GetComponent<Animator>();
        _player = GetComponent<Player>();
        _controller = GetComponent<MovementController>();
        _soundPlayer = GetComponent<PlaySound>();
        playerInput = ReInput.players.GetPlayer(_player.playerNumber - 1);
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 joystickInputVector = new Vector2(playerInput.GetAxisRaw("Move X"), playerInput.GetAxisRaw("Move Y"));

        float combinedXInput = Mathf.Round(joystickInputVector.x);

        if (_player.xInput != null)
        {
            if (_player.IsGrounded && combinedXInput == 0)
            {
                playerAnim.SetBool("isRunning", false);
                _soundPlayer.StopClip(8);
            }
            else
            {
                playerAnim.SetBool("isRunning", true);
                if (_player.IsGrounded)
                {
                    _soundPlayer.PlayClip(8, true);
                }
                else
                {
                    _soundPlayer.StopClip(8);
                }
            }
        }

        if (_player.IsGrounded)
        {
            playerAnim.SetBool("isGrounded", true);
        }
        else
        {
            playerAnim.SetBool("isGrounded", false);
        }

        if (_player.isWallSliding)
        {
            playerAnim.SetBool("isWallSliding", true);
        }
        else
        {
            playerAnim.SetBool("isWallSliding", false);
        }

        if (_player.isStunned)
        {
            playerAnim.SetBool("isStunned", true);
        }
        else
        {
            playerAnim.SetBool("isStunned", false);
        }


    }

    public void AttackAnimation(int attackPhase)
    {
        if (attackPhase == 1)
        {
            playerAnim.SetTrigger("meleeAttack1");
            _soundPlayer.PlayClip(7, false);
        }
        else if (attackPhase == 2)
        {
            playerAnim.SetBool("kick", true);
        }
        else if (attackPhase == 0)
        {
            playerAnim.SetBool("kick", false);
        }

    }
}