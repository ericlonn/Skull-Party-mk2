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

    // Start is called before the first frame update
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        loadingSkull.transform.localScale = Vector3.zero;
        loadingSkull.SetActive(false);
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TriggerLoadingScreen() {
        StartCoroutine("LoadingScreenSequence");
    }

    public void TriggerTransitionIn() {
        loadingSkull.SetActive(false);
        sceneTransitionIn.SetActive(true);
    }


    IEnumerator LoadingScreenSequence() {
        sceneTransitionOut.SetActive(true);
        yield return new WaitForSeconds(transOutAnimClip.length);

        loadingSkull.SetActive(true);
        LeanTween.scale(loadingSkull, Vector3.one * 6, 1f).setEase(LeanTweenType.easeOutElastic).setDelay(.2f);

        AsyncOperation loadLoadingScreen = SceneManager.LoadSceneAsync(0);
        while (!loadLoadingScreen.isDone) {
            yield return null;
        }
        sceneTransitionOut.SetActive(false);

    }
}
