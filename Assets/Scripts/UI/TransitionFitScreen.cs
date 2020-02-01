using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionFitScreen : MonoBehaviour
{
    Camera mainCamera;
    SpriteRenderer sprite;
    // Start is called before the first frame update
    void Awake()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        mainCamera = Camera.main;
        sprite = GetComponent<SpriteRenderer>();

        Debug.Log(sprite.bounds.size.y);
        Debug.Log(mainCamera.orthographicSize * 2);

        transform.localScale = new Vector2(1f, sprite.size.y / mainCamera.orthographicSize * 2);
    }
}
