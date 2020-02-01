using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVictoryStandingBehavior : MonoBehaviour
{
    public SpriteRenderer _fire;
    public ParticleSystem _particles;

    Color _winnerColor;

    // Start is called before the first frame update
    void Start()
    {
        _winnerColor = GetComponent<SpriteRenderer>().material.GetColor("_OutlineColor");
        _fire.color = _winnerColor;

        ParticleSystem.MainModule main = _particles.main;
        main.startColor = _winnerColor;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
