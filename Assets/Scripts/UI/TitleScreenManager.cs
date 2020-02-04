using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleScreenManager : MonoBehaviour
{
    public GameObject titleScreenObjects;
    public GameObject titleScreenCanvas;

    public GameObject charSelectObjects;
    public GameObject charSelectCanvas;

    public List<GameObject> playerSprites = new List<GameObject>();

    public float transTime = .3f;

    GameDirector _gameDirector;
    bool onTitleScreen = true;
    bool hasCalledDirector = false;

    // Start is called before the first frame update
    void Start()
    {
        charSelectObjects.SetActive(false);
        charSelectCanvas.SetActive(false);

        titleScreenObjects.SetActive(true);
        titleScreenCanvas.SetActive(true);

        _gameDirector = GameObject.FindGameObjectWithTag("GameDirector").GetComponent<GameDirector>();
    }

    // Update is called once per frame
    void Update()
    {
        bool anyPlayerPressGreen = Input.GetButtonDown("Jump1") ||
                                   Input.GetButtonDown("Jump2") ||
                                   Input.GetButtonDown("Jump3") ||
                                   Input.GetButtonDown("Jump4");


        if (!onTitleScreen)
        {
            if (Input.GetButtonDown("Jump1") || Input.GetKeyDown(KeyCode.Keypad1))
            {
                playerSprites[0].GetComponent<CharSelectSpriteBehavior>().ToggleActive();
            }

            if (Input.GetButtonDown("Jump2") || Input.GetKeyDown(KeyCode.Keypad2))
            {
                playerSprites[1].GetComponent<CharSelectSpriteBehavior>().ToggleActive();
            }

            if (Input.GetButtonDown("Jump3") || Input.GetKeyDown(KeyCode.Keypad3))
            {
                playerSprites[2].GetComponent<CharSelectSpriteBehavior>().ToggleActive();
            }

            if (Input.GetButtonDown("Jump4") || Input.GetKeyDown(KeyCode.Keypad4))
            {
                playerSprites[3].GetComponent<CharSelectSpriteBehavior>().ToggleActive();
            }
        }

        else if (anyPlayerPressGreen && onTitleScreen)
        {
            ToCharacterSelect();
        }

        int activePlayers = 0;
        for (int i = 0; i <= 3; i++)
        {
            if (_gameDirector.activePlayers[i])
            {
                activePlayers++;
            }
        }

        if (activePlayers > 1)
        {
            for (int i = 0; i <= 3; i++)
            {
                if (_gameDirector.activePlayers[i] && Input.GetButton("Attack" + (i + 1)) && !hasCalledDirector)
                {
                    _gameDirector.nextLevelToLoad = 2;
                    _gameDirector.TriggerLoadingScreen();
                    hasCalledDirector = true;
                }
            }
        }
    }

    void ToCharacterSelect()
    {
        onTitleScreen = false;
        LeanTweenType tweenType = LeanTweenType.easeOutQuad;

        titleScreenObjects.transform.localScale = Vector3.one;
        titleScreenCanvas.transform.localScale = Vector3.one;

        var titleObjSeq = LeanTween.sequence();
        titleObjSeq.append(LeanTween.scale(titleScreenObjects, Vector3.zero, transTime).setEase(tweenType));
        titleObjSeq.append(() => { titleScreenObjects.SetActive(false); });

        var titleCanSeq = LeanTween.sequence();
        titleCanSeq.append(LeanTween.scale(titleScreenCanvas, Vector3.zero, transTime).setEase(tweenType));
        titleCanSeq.append(() => { titleScreenCanvas.SetActive(false); });


        charSelectObjects.SetActive(true);
        charSelectCanvas.SetActive(true);
        charSelectObjects.transform.localScale = Vector3.zero;
        charSelectCanvas.transform.localScale = Vector3.zero;

        var charObjSeq = LeanTween.sequence();
        charObjSeq.append(LeanTween.scale(charSelectObjects, Vector3.one, transTime).setEase(tweenType));

        var charCanSeq = LeanTween.sequence();
        charCanSeq.append(LeanTween.scale(charSelectCanvas, Vector3.one, transTime).setEase(tweenType));

    }

    void ToTitleScreen()
    {
        onTitleScreen = true;
        LeanTweenType tweenType = LeanTweenType.easeOutQuad;

        charSelectObjects.transform.localScale = Vector3.one;
        charSelectCanvas.transform.localScale = Vector3.one;

        var charObjSeq = LeanTween.sequence();
        charObjSeq.append(LeanTween.scale(charSelectObjects, Vector3.zero, transTime).setEase(tweenType));
        charObjSeq.append(() => { charSelectObjects.SetActive(false); });

        var charCanSeq = LeanTween.sequence();
        charCanSeq.append(LeanTween.scale(charSelectCanvas, Vector3.zero, transTime).setEase(tweenType));
        charCanSeq.append(() => { charSelectCanvas.SetActive(false); });


        titleScreenObjects.SetActive(true);
        titleScreenCanvas.SetActive(true);
        titleScreenObjects.transform.localScale = Vector3.zero;
        titleScreenCanvas.transform.localScale = Vector3.zero;

        var titleObjSeq = LeanTween.sequence();
        titleObjSeq.append(LeanTween.scale(titleScreenObjects, Vector3.one, transTime).setEase(tweenType));

        var titleCanSeq = LeanTween.sequence();
        titleCanSeq.append(LeanTween.scale(titleScreenCanvas, Vector3.one, transTime).setEase(tweenType));
    }

    // void ToCharacterSelect()
    // {
    //     onTitleScreen = false;
    //     float transTime = .5f;

    //     // char select objects on
    //     foreach (Transform child in charSelectObjects.transform)
    //     {
    //         child.gameObject.SetActive(true);

    //         Color tmpColor = child.gameObject.GetComponent<SpriteRenderer>().color;
    //         tmpColor.a = 0f;
    //         child.gameObject.GetComponent<SpriteRenderer>().color = tmpColor;

    //         LeanTween.alpha(child.gameObject, 1, transTime).setEase(LeanTweenType.easeInOutQuad);
    //     }

    //     titleScreenCanvas.alpha = 0f;
    //     var canSeq1 = LeanTween.sequence();
    //     canSeq1.append( LeanTween.alphaCanvas(titleScreenCanvas, 1f, transTime).setEase(LeanTweenType.easeInOutQuad) );

    //     // title screen objects off
    //     foreach (Transform child in titleScreenObjects.transform)
    //     {
    //         Color tmpColor = child.gameObject.GetComponent<SpriteRenderer>().color;
    //         tmpColor.a = 1f;
    //         child.gameObject.GetComponent<SpriteRenderer>().color = tmpColor;

    //         var objSeq = LeanTween.sequence();
    //         objSeq.append(LeanTween.alpha(child.gameObject, 0, transTime).setEase(LeanTweenType.easeInOutQuad));
    //         objSeq.append(() => { child.gameObject.SetActive(false); });
    //     }

    //     titleScreenCanvas.alpha = 1f;
    //     var canSeq2 = LeanTween.sequence();
    //     canSeq2.append( LeanTween.alphaCanvas(titleScreenCanvas, 0f, transTime).setEase(LeanTweenType.easeInOutQuad) );
    // }

    // void ToTitleScreen()
    // {
    //     onTitleScreen = true;
    //     float transTime = .5f;

    //     // title screen objects on
    //     foreach (Transform child in titleScreenObjects.transform)
    //     {
    //         child.gameObject.SetActive(true);

    //         Color tmpColor = child.gameObject.GetComponent<SpriteRenderer>().color;
    //         tmpColor.a = 0f;
    //         child.gameObject.GetComponent<SpriteRenderer>().color = tmpColor;

    //         LeanTween.alpha(child.gameObject, 1, transTime).setEase(LeanTweenType.easeInOutQuad);
    //     }

    //     titleScreenCanvas.alpha = 0f;
    //     var canSeq1 = LeanTween.sequence();
    //     canSeq1.append( LeanTween.alphaCanvas(titleScreenCanvas, 1f, transTime).setEase(LeanTweenType.easeInOutQuad) );

    //     // char select objects off
    //     foreach (Transform child in charSelectObjects.transform)
    //     {
    //         Color tmpColor = child.gameObject.GetComponent<SpriteRenderer>().color;
    //         tmpColor.a = 1f;
    //         child.gameObject.GetComponent<SpriteRenderer>().color = tmpColor;

    //         var objSeq = LeanTween.sequence();
    //         objSeq.append(LeanTween.alpha(child.gameObject, 0, transTime).setEase(LeanTweenType.easeInOutQuad));
    //         objSeq.append(() => { child.gameObject.SetActive(false); });
    //     }

    //     titleScreenCanvas.alpha = 1f;
    //     var canSeq2 = LeanTween.sequence();
    //     canSeq2.append( LeanTween.alphaCanvas(titleScreenCanvas, 0f, transTime).setEase(LeanTweenType.easeInOutQuad) );
    // }
}
