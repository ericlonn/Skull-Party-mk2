using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharSelectTextBehavior : MonoBehaviour
{
    public GameObject playerSprite;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        GetComponent<CanvasRenderer>().SetAlpha(playerSprite.GetComponent<SpriteRenderer>().color.a);
    }
}
