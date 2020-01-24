using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerskullManager : MonoBehaviour
{
    public List<Transform> spawnPoints = new List<Transform>();
    // public List<GameObject> powerskullObjects = new List<GameObject>();
    public int maxPowerskullsInPlay = 6;
    public GameObject powerskullPrefab;
    public GameObject treasureChestPrefab;
    public float treasureChestChance = .25f;
    public float spawnDelayTime = 20f;
    
    float lastSkullSpawnedTime = 0f;
    int currentPowerskullsInPlay = 0;
    Vector2 averagePlayerLocation = Vector2.zero;
    bool playerIsPoweredUp;
    GameObject[] powerskullObjects;
    GameObject[] treasureChests;

    private PlayerManager _playerManager;
    // Start is called before the first frame update
    void Start()
    {
        _playerManager = FindObjectOfType<PlayerManager>();

    }
    // Update is called once per frame
    void Update()
    {
        averagePlayerLocation = Vector2.zero;
        currentPowerskullsInPlay = 0;
        playerIsPoweredUp = false;
        lastSkullSpawnedTime -= Time.deltaTime;

        FindPlayersInScene();
        FindSkullsInScene();

        if (powerskullObjects.Length + treasureChests.Length < 3 &&
            currentPowerskullsInPlay < maxPowerskullsInPlay &&
            playerIsPoweredUp == false &&
            lastSkullSpawnedTime <= 0)
        {
            SpawnSkull();
            lastSkullSpawnedTime = spawnDelayTime;
        }
    }

    void FindPlayersInScene()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        foreach (GameObject player in players)
        {
            averagePlayerLocation += new Vector2(player.transform.position.x, player.transform.position.y);
            currentPowerskullsInPlay += player.GetComponent<Player>().powerskullCount;

            if (player.GetComponent<Player>().isPoweredUp)
            {
                playerIsPoweredUp = true;
            }
        }

        averagePlayerLocation = new Vector2(averagePlayerLocation.x / players.Length, averagePlayerLocation.y / players.Length);
    }

    void FindSkullsInScene()
    {
        powerskullObjects = GameObject.FindGameObjectsWithTag("Powerskull");
        treasureChests = GameObject.FindGameObjectsWithTag("Tossable");
        currentPowerskullsInPlay += powerskullObjects.Length;
        currentPowerskullsInPlay += treasureChests.Length;
    }

    void SpawnSkull()
    {
        Transform furthestSpawnPoint = null;

        foreach (Transform spawnPoint in spawnPoints)
        {
            if (furthestSpawnPoint == null)
            {
                furthestSpawnPoint = spawnPoint;
            }

            if (Vector2.Distance(spawnPoint.transform.position, averagePlayerLocation) > Vector2.Distance(furthestSpawnPoint.transform.position, averagePlayerLocation))
            {
                furthestSpawnPoint = spawnPoint;
            }
        }

        if (Random.Range(0f, 1f) > treasureChestChance)
        {
            Instantiate(powerskullPrefab, furthestSpawnPoint.position, Quaternion.identity);
        }
        else
        {
            Instantiate(treasureChestPrefab, furthestSpawnPoint.position, Quaternion.identity);
        }

    }

    // private void OnDrawGizmos() {
    //     Gizmos.color = Color.red;
    //     Gizmos.DrawSphere(new Vector3(averagePlayerLocation.x, averagePlayerLocation.y, 0), 1f);
    // }
}
