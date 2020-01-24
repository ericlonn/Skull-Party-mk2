using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayerManager : MonoBehaviour
{
    public GameObject playerPrefab;
    public CinemachineTargetGroup camTargetGroup;

    public List<GameObject> playerObjects = new List<GameObject>();
    public List<Color> playerColors = new List<Color>();

    // Start is called before the first frame update
    void Awake()
    {
        int playerNumber = 1;

        foreach (GameObject playerObject in playerObjects) {
            playerObject.GetComponent<Player>().playerNumber = playerNumber;
            playerNumber++;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
