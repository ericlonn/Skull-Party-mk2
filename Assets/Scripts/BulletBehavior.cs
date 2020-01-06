using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehavior : MonoBehaviour
{
    public float speedScale = 2f;
    // Start is called before the first frame update
    void Start()
    {
        gameObject.transform.localScale = new Vector2(.5f, 1);
        LeanTween.scaleX(gameObject, 1f, .2f);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector2.right * speedScale);
    }
}
