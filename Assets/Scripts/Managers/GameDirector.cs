using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameDirector : MonoBehaviour
{
    public int nextLevelToLoad;
    public GameObject sceneTransitionIn, sceneTransitionOut;
    public GameObject loadingSkull;
    public AnimationClip transInAnimClip, transOutAnimClip;

    public List<bool> activePlayers = new List<bool>();

    bool firstLoad = true;

    // Start is called before the first frame update
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        loadingSkull.transform.localScale = Vector3.zero;
        loadingSkull.SetActive(false);

        activePlayers[0] = false;
        activePlayers[1] = false;
        activePlayers[2] = false;
        activePlayers[3] = false;

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void TriggerLoadingScreen()
    {
        StartCoroutine("LoadingScreenSequence");
    }

    public void TriggerTransitionIn()
    {

    }


    IEnumerator LoadingScreenSequence()
    {
        if (firstLoad)
        {
            firstLoad = false;
        }
        else
        {
            sceneTransitionOut.SetActive(true);
        }

        yield return new WaitForSeconds(transOutAnimClip.length);

        loadingSkull.SetActive(true);
        LeanTween.scale(loadingSkull, Vector3.one * 6, 1f).setEase(LeanTweenType.easeOutElastic).setDelay(.2f);

        AsyncOperation loadLoadingScreen = SceneManager.LoadSceneAsync(0);
        while (!loadLoadingScreen.isDone)
        {
            yield return null;
        }
        sceneTransitionOut.SetActive(false);
        StartCoroutine("LevelLoadSequence");
    }

    IEnumerator LevelLoadSequence()
    {
        float minLoadTime = 2.5f;

        while (minLoadTime > 0)
        {
            minLoadTime -= Time.deltaTime;
            yield return null;
        }

        AsyncOperation operation = SceneManager.LoadSceneAsync(nextLevelToLoad);

        while (!operation.isDone)
        {
            yield return null;
        }

        loadingSkull.SetActive(false);
        sceneTransitionIn.SetActive(true);
    }
}
