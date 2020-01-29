using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIIconStatus : MonoBehaviour
{
    public Sprite enabledSprite;
    public Sprite disabledSprite;

    public bool isEnabled = false;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (isEnabled)
        {
            GetComponent<Image>().sprite = enabledSprite;
        }
        else
        {
            GetComponent<Image>().sprite = disabledSprite;

        }
    }
}
