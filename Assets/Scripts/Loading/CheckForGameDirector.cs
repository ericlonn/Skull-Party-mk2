using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckForGameDirector : MonoBehaviour
{
    public GameObject gameDirectorPrefab;
    bool hasLoadedDirector = false;

    // Start is called before the first frame update
    void Start()
    {
        if (GameObject.FindGameObjectWithTag("GameDirector") == null && !hasLoadedDirector) {
            GameObject newDirector = Instantiate(gameDirectorPrefab, transform.position, Quaternion.identity);
            newDirector.GetComponent<GameDirector>().TriggerLoadingScreen();

            hasLoadedDirector = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
