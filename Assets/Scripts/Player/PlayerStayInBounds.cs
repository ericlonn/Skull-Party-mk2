using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStayInBounds : MonoBehaviour
{


    // Update is called once per frame
    void Update()
    {
        if (transform.position.x > 21 || transform.position.x < -21 ||
            transform.position.y > 28 || transform.position.y < -1) {
                transform.position = new Vector2(0, 24);
            }        
    }
}
