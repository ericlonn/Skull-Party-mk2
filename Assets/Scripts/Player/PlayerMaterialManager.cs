using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMaterialManager : MonoBehaviour
{
    public Player _playerScript;
    public List<Material> playerMaterials = new List<Material>();

    public float blinkTime = .2f;
    public float blinkTimer = 0f;
    public float blinkAlpha1 = 0;
    public float blinkAlpha2 = 1f;

    SpriteRenderer _sprite;
    Material _playerMat;
    bool lastFrameStunned = false;
    bool stunBlinkActive = false;
    float hitFlashTime = .075f;
    float hitFlashTimer;

    private void Start()
    {
        _playerScript = transform.parent.transform.parent.GetComponent<Player>();
        _sprite = GetComponent<SpriteRenderer>();

        GetComponent<SpriteRenderer>().material = playerMaterials[_playerScript.playerNumber - 1];
        _playerMat = GetComponent<SpriteRenderer>().material;

        _playerMat.EnableKeyword("OUTBASE_ON");
        _playerMat.SetColor("_outlineColor", _playerScript.playerColor);

        hitFlashTimer = hitFlashTime;

    }

    // Update is called once per frame
    void Update()
    {


        if (_playerScript.isStunned && _playerScript.bulletStunned)
        {
            if (!stunBlinkActive)
            {
                stunBlinkActive = true;
                blinkTimer = blinkTime;
            }
        }
        else
        {
            Color tmpColor = _sprite.color;
            tmpColor.a = 1f;
            _sprite.color = tmpColor;
            stunBlinkActive = false;
        }

        StunBlink();

        if (!lastFrameStunned && _playerScript.isStunned)
        {
            _playerMat.EnableKeyword("GRADIENT_ON");
            hitFlashTimer = 0f;
        }
        else if (hitFlashTimer < hitFlashTime)
        {
            _playerMat.EnableKeyword("GRADIENT_ON");
        }
        else
        {
            _playerMat.DisableKeyword("GRADIENT_ON");
        }

        hitFlashTimer += Time.deltaTime;
        lastFrameStunned = _playerScript.isStunned;
    }

    void StunBlink()
    {

        if (stunBlinkActive)
        {
            Color tmpColor = _sprite.color;
            float normalizedBlink = Mathf.Lerp(blinkAlpha1, blinkAlpha2, blinkTimer / blinkTime);

           tmpColor.a = normalizedBlink;
            
            blinkTimer -= Time.deltaTime;
            
            if (blinkTimer <= 0f) blinkTimer = blinkTime;

            _sprite.color = tmpColor;
        }
    }
}
