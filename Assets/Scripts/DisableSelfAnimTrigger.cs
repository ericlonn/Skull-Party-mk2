using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableSelfAnimTrigger : MonoBehaviour
{
    public void DisableSelf() {
        gameObject.SetActive(false);
    }
}
