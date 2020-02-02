using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HighScoreManager : MonoBehaviour
{
    public List<TextMeshProUGUI> textObjects = new List<TextMeshProUGUI>();
    public PlayerManager _playerManager;
    public bool scoreBeingEnter = true;
    public int highScorePlace = 0;
    public int highScorePoints;

    List<string> letters = new List<string>();
    int curLetter = 0;
    int curPosition = 0;
    int winningPlayerNumber;
    List<string> setLetters = new List<string>();

    float letterDelayTime = .2f;
    float letterDelayTimer = 0;


    bool entryComplete = false;
    bool lastFrameUp = false;
    bool lastFrameDown = false;
    void Start()
    {

        letters.Add("A");
        letters.Add("B");
        letters.Add("C");
        letters.Add("D");
        letters.Add("E");
        letters.Add("F");
        letters.Add("G");
        letters.Add("H");
        letters.Add("I");
        letters.Add("J");
        letters.Add("K");
        letters.Add("L");
        letters.Add("M");
        letters.Add("N");
        letters.Add("O");
        letters.Add("P");
        letters.Add("Q");
        letters.Add("R");
        letters.Add("S");
        letters.Add("T");
        letters.Add("U");
        letters.Add("V");
        letters.Add("W");
        letters.Add("X");
        letters.Add("Y");
        letters.Add("Z");

        textObjects[0].text = letters[0];
        textObjects[1].text = letters[0];
        textObjects[2].text = letters[0];
        textObjects[3].text = letters[0];

        textObjects[curPosition].color = Color.yellow;

        winningPlayerNumber = _playerManager.winner.GetComponent<Player>().playerNumber;

        letterDelayTimer = letterDelayTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (highScorePlace == 0)
        {
            highScorePlace = _playerManager.highScoreRank;
        }

        if (curPosition <= 3)
        {
            letterDelayTimer -= Time.deltaTime;

            bool upPressed = Input.GetAxis("Vertical" + winningPlayerNumber) == -1f;
            bool downPressed = Input.GetAxis("Vertical" + winningPlayerNumber) == 1f;
            bool greenButtonPressed = Input.GetButtonDown("Jump" + winningPlayerNumber);

            if ((upPressed && letterDelayTimer <= 0f) || (!lastFrameUp && upPressed))
            {
                curLetter++;
                if (curLetter > 25) curLetter = 0;
                textObjects[curPosition].text = letters[curLetter];
            }

            if ((downPressed && letterDelayTimer <= 0f) || (!lastFrameDown && downPressed))
            {
                curLetter--;
                if (curLetter < 0) curLetter = 25;
                textObjects[curPosition].text = letters[curLetter];
            }

            if (greenButtonPressed)
            {
                textObjects[curPosition].color = Color.white;
                setLetters.Add(letters[curLetter]);
                curPosition++;
                curLetter = 0;
                textObjects[curPosition].color = Color.yellow;
            }

            if ((!lastFrameUp && upPressed) || (!lastFrameDown && downPressed))
            {
                letterDelayTimer = letterDelayTime;
            }

            if (letterDelayTimer <= 0f)
            {
                letterDelayTimer = letterDelayTime;
            }

            if (curPosition <= 3)
            {
                textObjects[curPosition].color = Color.yellow;
                lastFrameUp = upPressed;
                lastFrameDown = downPressed;
            }
        }
        else if (!entryComplete && curPosition > 3)
        {
            for (int i = 9; i > 0; i--)
            {
                if (PlayerPrefs.HasKey("highScoreName" + i))
                {
                    string higherName = PlayerPrefs.GetString("highScoreName" + (i - 1));
                    int higherScore = PlayerPrefs.GetInt("highScorePoints" + (i - 1));

                    PlayerPrefs.SetString("highScoreName" + i, higherName);
                    PlayerPrefs.SetInt("highScorePoints" + i, higherScore);
                    PlayerPrefs.Save();
                }



                if (i == highScorePlace)
                {
                    PlayerPrefs.SetString("highScoreName" + (i - 1), setLetters[0] + setLetters[1] + setLetters[2] + setLetters[3]);
                    PlayerPrefs.SetInt("highScorePoints" + (i - 1), _playerManager.winner.GetComponent<Player>().score);
                    Debug.Log(highScorePlace);
                    break;
                }

            }

            for (int i = 1; i <= 9; i++)
            {
                Debug.Log(i + ". " + PlayerPrefs.GetString("highScoreName" + i) + ": " + PlayerPrefs.GetInt("highScorePoints" + i));
            }

            entryComplete = true;
        }
    }
}
