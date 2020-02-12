using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PointsTextPosition : MonoBehaviour
{
    public GameObject highScoreUI;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!highScoreUI.activeSelf) {
            GetComponent<TextMeshProUGUI>().fontSize = 176f;
            transform.position = new Vector2(transform.position.x, -57.3f);
        }
    }
}
