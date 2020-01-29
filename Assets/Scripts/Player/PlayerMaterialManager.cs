using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMaterialManager : MonoBehaviour
{
    public Player _playerScript;

    public List<Material> playerMaterials = new List<Material>();

    Material _playerMat;
    bool lastFrameStunned = false;
    float hitFlashTime = .075f;
    float hitFlashTimer;

    private void Start()
    {
        _playerScript = transform.parent.transform.parent.GetComponent<Player>();

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
            _playerMat.EnableKeyword("HOLOGRAM_ON");
            _playerMat.SetFloat("_Alpha", .8f);
        }
        else
        {
            _playerMat.DisableKeyword("HOLOGRAM_ON");
            _playerMat.SetFloat("_Alpha", 1f);
        }

        if (!lastFrameStunned && _playerScript.isStunned)
        {
            _playerMat.EnableKeyword("GRADIENT_ON");
            hitFlashTimer = 0f;
        }
        else if (hitFlashTimer < hitFlashTime){
            _playerMat.EnableKeyword("GRADIENT_ON");
        }
        else
        {
            _playerMat.DisableKeyword("GRADIENT_ON");
        }

        hitFlashTimer += Time.deltaTime;
        lastFrameStunned = _playerScript.isStunned;
    }
}
