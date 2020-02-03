using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public int loadSceneID;

    void Start()
    {
        loadSceneID = GameObject.Find("GameDirector").GetComponent<GameDirector>().nextLevelToLoad;
        StartCoroutine("LoadLevelAsync");
    }

    IEnumerator LoadLevelAsync()
    {
        float minLoadTime = 4f;

        while (minLoadTime > 0)
        {
            minLoadTime -= Time.deltaTime;
            yield return null;
        }

        AsyncOperation operation = SceneManager.LoadSceneAsync(loadSceneID);
GameObject.Find("GameDirector").GetComponent<GameDirector>().TriggerTransitionIn();
        while (!operation.isDone)
        {
            yield return null;
        }

        

    }
}
