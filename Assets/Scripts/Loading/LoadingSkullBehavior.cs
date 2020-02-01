using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingSkullBehavior : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        transform.localScale = Vector3.zero;
        LeanTween.scale(gameObject, Vector3.one, 1f).setEase(LeanTweenType.easeOutElastic);
    }

}
