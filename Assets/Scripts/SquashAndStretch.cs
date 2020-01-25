using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquashAndStretch : MonoBehaviour
{
    public Transform squashPoint;
    public float scaleFactor = 0f;
    public Player _player;

    public float launchBounceSpeed = 1f;
    public float launchBounceAmount = -.1f;

    public float landBounceSpeed = .75f;
    public float landBounceAmount = .5f;

    bool previousIsGrounded;

    private void Start() {
        previousIsGrounded = _player.IsGrounded;

    }

    private void Update()
    {
        squashPoint.localScale = new Vector3(1 + scaleFactor, 1 - scaleFactor, squashPoint.localScale.z);

        if (Input.GetButtonDown("Jump" + _player.playerNumber) && _player.IsGrounded)
        {
            LaunchSandS();
        }

        if (previousIsGrounded == false && _player.IsGrounded == true)
        {
            LandSandS();
        }

        previousIsGrounded = _player.IsGrounded;
    }

    public void LaunchSandS()
    {
        LeanTween.cancel(this.gameObject);
        LeanTween.value(this.gameObject, UpdateScaleFactor, 0f, launchBounceAmount, launchBounceSpeed).setEase(LeanTweenType.punch);
    }

    public void LandSandS()
    {
        LeanTween.cancel(this.gameObject);
        LeanTween.value(this.gameObject, UpdateScaleFactor, 0f, landBounceAmount, landBounceSpeed).setEase(LeanTweenType.punch);
    }

    public void UpdateScaleFactor(float newScaleFactor)
    {
        scaleFactor = newScaleFactor;
    }
}
