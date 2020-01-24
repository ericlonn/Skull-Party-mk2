using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerskullFire : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Vector3 currentScale = transform.localScale;
        transform.localScale = Vector3.zero;
        LeanTween.scale(gameObject, currentScale, .35f);
        GetComponent<SpriteRenderer>().color = transform.parent.GetComponent<Player>().playerColor;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void PowerDown()
    {
        gameObject.SetActive(false);
    }
}
