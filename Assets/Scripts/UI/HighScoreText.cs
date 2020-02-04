using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HighScoreText : MonoBehaviour
{
    public int rank;
    public TextMeshProUGUI hiScoreRankText, hiScoreNameText, hiScorePointsText;

    private void Awake()
    {
        if (PlayerPrefs.GetString("highScoreName" + rank).Length == 0) {
            gameObject.SetActive(false);
        }

        hiScoreRankText.text = rank + "";
        hiScoreNameText.text = PlayerPrefs.GetString("highScoreName" + rank) + "  " + PlayerPrefs.GetInt("highScorePoints" + rank).ToString();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
