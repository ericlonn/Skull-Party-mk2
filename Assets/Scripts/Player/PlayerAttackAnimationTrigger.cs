using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackAnimationTrigger : MonoBehaviour
{
    public GameObject _player;

    public void ToggleAttackCanLand(){
        bool currentState = _player.GetComponent<PlayerAttack>().attackCanLand;
        currentState = !currentState;
        _player.GetComponent<PlayerAttack>().attackCanLand = currentState;

    }
}
