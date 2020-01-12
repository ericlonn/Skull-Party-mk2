using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerskullManager : MonoBehaviour
{
    public List<Transform> spawnPoints = new List<Transform>();
    public List<GameObject> powerskullObjects = new List<GameObject>();

    private PlayerManager _playerManager;
    // Start is called before the first frame update
    void Start()
    {
        _playerManager = FindObjectOfType<PlayerManager>();

        FindSkullsInScene();

    }
    // Update is called once per frame
    void Update()
    {
        FindSkullsInScene();
    }

    void FindSkullsInScene()
    {
        GameObject[] taggedObjects = GameObject.FindGameObjectsWithTag("Powerskull");
        foreach (GameObject taggedObject in taggedObjects)
        {
            powerskullObjects.Add(taggedObject);
        }
    }
}
