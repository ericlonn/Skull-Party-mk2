using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;

public class HighScoreManager : MonoBehaviour
{
    public List<TextMeshProUGUI> textObjects = new List<TextMeshProUGUI>();
    public PlayerManager _playerManager;
    public bool scoreBeingEnter = true;
    public bool entryComplete = false;
    public int highScorePlace = 0;

    List<string> letters = new List<string>();
    int curLetter = 0;
    int curPosition = 0;
    int winningPlayerNumber;
    List<string> setLetters = new List<string>();

    float letterDelayTime = .2f;
    float letterDelayTimer = 0;


    
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
        textObjects[curPosition].fontStyle = FontStyles.Underline;

        

        letterDelayTimer = letterDelayTime;
    }

    // Update is called once per frame
    void Update()
    {

        winningPlayerNumber = _playerManager.winner.GetComponent<Player>().playerNumber;
        
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

            Debug.Log(winningPlayerNumber);

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
                textObjects[curPosition].fontStyle = FontStyles.Normal;
                setLetters.Add(letters[curLetter]);

                if (curPosition < 3) {
                    GameObject.Find("Sound Manager").GetComponent<PlaySound>().PlayClip(8, false);
                } else {
                    GameObject.Find("Sound Manager").GetComponent<PlaySound>().PlayClip(0, true);
                }

                curPosition++;
                curLetter = 0;
                textObjects[curPosition].color = Color.yellow;
                Debug.Log(curPosition);

                
                
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
                textObjects[curPosition].fontStyle = FontStyles.Underline;
                lastFrameUp = upPressed;
                lastFrameDown = downPressed;
            }
        }
        else if (!entryComplete && curPosition > 3)
        {
            HighScoreEntry newScore = new HighScoreEntry();
            newScore.name = setLetters[0] + setLetters[1] + setLetters[2] + setLetters[3];
            newScore.score = _playerManager.winner.GetComponent<Player>().score;

            List<HighScoreEntry> prevEntries = new List<HighScoreEntry>();
            List<HighScoreEntry> curEntries = new List<HighScoreEntry>();

            for (int i = 1; i <= 9; i++) 
            {
                HighScoreEntry holder = new HighScoreEntry();
                holder.name = PlayerPrefs.GetString("highScoreName" + i);
                holder.score = PlayerPrefs.GetInt("highScorePoints" + i);

                prevEntries.Add(holder);
            }

            prevEntries.Add(newScore);

            prevEntries = prevEntries.OrderByDescending(w => w.score).ToList();

            for (int i = 1; i <= 9; i++) {
                PlayerPrefs.SetString("highScoreName" + i, prevEntries[i-1].name);
                PlayerPrefs.SetInt("highScorePoints" + i, prevEntries[i - 1].score);
            }

            PlayerPrefs.Save();

            entryComplete = true;
        }
    }
}

