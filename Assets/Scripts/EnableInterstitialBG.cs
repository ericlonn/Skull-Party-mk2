using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableInterstitialBG : MonoBehaviour
{
    public GameObject bgPrefab;

    GameObject existingBg;

    public void EnableBG() {

            Instantiate(bgPrefab, Vector3.zero, Quaternion.identity);
    }
}
