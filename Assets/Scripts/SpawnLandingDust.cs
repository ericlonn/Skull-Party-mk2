using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnLandingDust : MonoBehaviour
{

    public GameObject _dustPrefab;

    bool previousFrameGrounded;

    Player _player;

    // Start is called before the first frame update
    void Start()
    {
        _player = GetComponent<Player>();
        previousFrameGrounded = _player.IsGrounded;
    }

    // Update is called once per frame
    void Update()
    {
        if (!previousFrameGrounded && _player.IsGrounded) {
            GameObject newDust = Instantiate(_dustPrefab, transform);
            newDust.transform.SetParent(null);
            Debug.Log("dust");
        }

        previousFrameGrounded = _player.IsGrounded;
    }
}
