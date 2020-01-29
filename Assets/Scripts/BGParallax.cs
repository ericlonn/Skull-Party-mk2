using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGParallax : MonoBehaviour
{
    public Transform levelCenter;
    public Camera _cam;
    public float parallaxAmt;

    // Update is called once per frame
    void Update()
    {
        Vector3 newPos = Vector3.Lerp(levelCenter.position, _cam.transform.position, parallaxAmt);
        transform.position = newPos;
    }
}
