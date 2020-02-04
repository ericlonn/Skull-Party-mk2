using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VictoryInputButtons : MonoBehaviour
{
    public CanvasGroup _canvasGroup;

    SpriteRenderer _sprite;
    // Start is called before the first frame update
    void Start()
    {
        _sprite = GetComponent<SpriteRenderer>();

        SetAlpha();
    }

    // Update is called once per frame
    void Update()
    {
        SetAlpha();
    }

    void SetAlpha() {
        Color tmpColor = _sprite.color;
        tmpColor.a = _canvasGroup.alpha;
        _sprite.color = tmpColor;
    }
}
