using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlaceholderBehavior : MonoBehaviour
{
    public CinemachineTargetGroup targetGroup;
    public float waitTime = 3f;
    

    // Update is called once per frame
    void Update()
    {
        if (waitTime <= 0) {
            foreach (CinemachineTargetGroup.Target target in targetGroup.m_Targets) {
                Debug.Log(target);
            }
        }    
    }
}
