using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public int loadSceneID;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) {
            LoadLevel();
        }
    }

    void LoadLevel() {
        StartCoroutine("LoadLevelAsync");
    }

    IEnumerator LoadLevelAsync(){
        AsyncOperation operation = SceneManager.LoadSceneAsync(loadSceneID);

        while (!operation.isDone) {
            Debug.Log(operation.progress);
            yield return null;
        }


    }
}
