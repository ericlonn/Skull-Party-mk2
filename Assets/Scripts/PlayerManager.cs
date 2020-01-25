using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayerManager : MonoBehaviour
{
    public GameObject playerPrefab;
    public GameObject deathStandIn;
    public CinemachineTargetGroup camTargetGroup;

    public int startingPowerskulls = 1;

    public List<GameObject> playerObjects = new List<GameObject>();
    public List<Color> playerColors = new List<Color>();

    // Start is called before the first frame update
    void Awake()
    {
        for (int i = 0; i < playerObjects.Count; i++)
        {
            playerObjects[i].GetComponent<Player>().playerNumber = i + 1;
            playerObjects[i].GetComponent<Player>().powerskullCount = startingPowerskulls;
        }

        foreach (GameObject playerObject in playerObjects)
        {
            Player playerScript = playerObject.GetComponent<Player>();
            playerScript.playerColor = playerColors[playerScript.playerNumber - 1];
            camTargetGroup.AddMember(playerObject.transform, 1f, 0f);
            Debug.Log("player loop");
        }
    }

    // Update is called once per frame
    void Update()
    {
        foreach (GameObject playerObject in playerObjects)
        {
            if (playerObject != null)
            {
                if (playerObject.GetComponent<Player>().health <= 0)
                {
                    Transform deadPlayerTranform = playerObject.transform;
                    GameObject playerStandIn = Instantiate(deathStandIn, playerObject.transform.position, Quaternion.identity);
                    playerStandIn.GetComponent<PlayerDeathSpriteBehavior>().targetGroup = camTargetGroup;

                    playerStandIn.GetComponent<SpriteRenderer>().color = playerObject.GetComponent<Player>().playerColor;

                    ParticleSystem ps = playerStandIn.transform.Find("Player Death Particles").GetComponent<ParticleSystem>();
                    ParticleSystem.MainModule main = ps.main;
                    Color playerObjectColor = playerObject.GetComponent<Player>().playerColor;

                    main.startColor = playerObjectColor;

                    playerStandIn.GetComponent<PlayerDeathSpriteBehavior>().playerColor = playerObject.GetComponent<Player>().playerColor;

                    camTargetGroup.AddMember(playerStandIn.transform,1f, 0f);

                    Destroy(playerObject);
                }

                
            }
        }
    }
}
