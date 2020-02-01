﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharSelectSpriteBehavior : MonoBehaviour
{
    public bool isActivated = false;
    public float inactiveAlpha = .5f;
    public GameObject activatedBurst;
    public Transform activatedBurstTransform;
    public Color activatedBurstColor;

    Animator _animator;
    SpriteRenderer _sprite;
    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
        _sprite = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        Color tmpColor = _sprite.color;

        if (isActivated)
        {
            _animator.SetBool("isActivated", true);
            tmpColor.a = 1f;

        }
        else
        {
            _animator.SetBool("isActivated", false);
            tmpColor.a = inactiveAlpha;
        }

        _sprite.color = tmpColor;
    }

    public void ToggleActive()
    {
        if (isActivated)
        {
            isActivated = false;
        }
        else
        {
            isActivated = true;
            GameObject newBurst = Instantiate(activatedBurst, activatedBurstTransform.transform.position, Quaternion.identity);
            newBurst.transform.localScale = activatedBurstTransform.localScale;
            newBurst.GetComponent<SpriteRenderer>().color = activatedBurstColor;
        }
    }
}