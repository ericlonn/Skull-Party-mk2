using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameDirector : MonoBehaviour
{
    public int nextLevelToLoad;
    public GameObject sceneTransitionIn, sceneTransitionOut;
    public GameObject loadingSkull;
    public GameObject controlsExplainer;
    public AnimationClip transInAnimClip, transOutAnimClip;
    public PlayMusic _musicPlayer;
    public float timeOutTime = 180f;

    public List<bool> activePlayers = new List<bool>();

    bool firstLoad = true;
    bool anyInput;
    float inputTimer;


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

        _musicPlayer = GetComponent<PlayMusic>();

        Cursor.visible = false;
    }

    private void Start()
    {
        _musicPlayer.PlayClip(0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        InputWatcher();

    }

    public void TriggerLoadingScreen()
    {
        StartCoroutine("LoadingScreenSequence");
    }

    void InputWatcher()
    {
        if (Input.GetButtonDown("Jump1") ||
            Input.GetButtonDown("Jump2") ||
            Input.GetButtonDown("Jump3") ||
            Input.GetButtonDown("Jump4") ||
            Input.GetButtonDown("Attack1") ||
            Input.GetButtonDown("Attack2") ||
            Input.GetButtonDown("Attack3") ||
            Input.GetButtonDown("Attack4") ||
            Input.GetAxis("Horizontal1") > 0 || Input.GetAxis("Horizontal1") < 0 ||
            Input.GetAxis("Horizontal2") > 0 || Input.GetAxis("Horizontal2") < 0 ||
            Input.GetAxis("Horizontal3") > 0 || Input.GetAxis("Horizontal3") < 0 ||
            Input.GetAxis("Horizontal4") > 0 || Input.GetAxis("Horizontal4") < 0 ||
            Input.GetAxis("Vertical1") > 0 || Input.GetAxis("Vertical1") < 0 ||
            Input.GetAxis("Vertical2") > 0 || Input.GetAxis("Vertical2") < 0 ||
            Input.GetAxis("Vertical3") > 0 || Input.GetAxis("Vertical3") < 0 ||
            Input.GetAxis("Vertical4") > 0 || Input.GetAxis("Vertical4") < 0)
        {
            inputTimer = 0f;
        }

        inputTimer += Time.deltaTime;

        if (SceneManager.GetActiveScene().buildIndex == 2 && inputTimer >= timeOutTime)
        {
            inputTimer = 0f;
            nextLevelToLoad = 1;
            TriggerLoadingScreen();
        }
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
        if (nextLevelToLoad == 2) {
            controlsExplainer.SetActive(true);
        }

        LeanTween.scale(loadingSkull, Vector3.one * 6, 1f).setEase(LeanTweenType.easeOutElastic).setDelay(.05f);

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
        float minLoadTime = 5f;

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
        if (nextLevelToLoad == 2) {
            controlsExplainer.SetActive(false);
        }
        if (nextLevelToLoad == 2)
        {
            _musicPlayer.PlayClip(1, .25f);
        }
        else if (nextLevelToLoad == 1)
        {
            _musicPlayer.PlayClip(0, .25f);
        }
        sceneTransitionIn.SetActive(true);
    }
}
