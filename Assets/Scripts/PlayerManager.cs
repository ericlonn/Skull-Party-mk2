using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public List<GameObject> playerObjects = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
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
