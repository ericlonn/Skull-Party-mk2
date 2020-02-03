using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using TMPro;

public class PlayerManager : MonoBehaviour
{
    public GameObject playerPrefab;
    public GameObject deathStandIn;
    public CinemachineTargetGroup camTargetGroup;
    public GameObject spawnBurst;
    public GameObject playerVictoryStandin;
    public GameObject gameCamera, victoryCamera;
    public CanvasGroup gameUI, victoryUI;
    public GameObject winner = null;

    public TextMeshProUGUI victoryPlayerNameText, victoryPlayerPointsText;

    public GameObject highScoreUI;

    public int highScoreRank;

    public int startingPowerskulls = 1;
    public int playerCount = 0;
    
    

    public List<bool> isPlayerActive = new List<bool>();
    public List<GameObject> playerObjects = new List<GameObject>();
    public List<Color> playerColors = new List<Color>();

    List<string> playerNames = new List<string>();
    bool playerHasWon = false;
    bool gotHighScore = false;

    // Start is called before the first frame update
    void Awake()
    {
        playerNames.Add("bone mommy");
        playerNames.Add("pelvis wrestley");
        playerNames.Add("duke skullington");
        playerNames.Add("frasier cranium");

        for (int i = 0; i < playerObjects.Count; i++)
        {
            playerObjects[i].GetComponent<Player>().playerNumber = i + 1;
            playerObjects[i].GetComponent<Player>().playerName = playerNames[i];
            if (isPlayerActive[i])
            {
                Player playerScript = playerObjects[i].GetComponent<Player>();

                playerScript.playerColor = playerColors[playerScript.playerNumber - 1];
                camTargetGroup.AddMember(playerObjects[i].transform, 1f, 0f);
                playerObjects[i].GetComponent<Player>().powerskullCount = startingPowerskulls;
                playerObjects[i].gameObject.SetActive(true);

                GameObject newSpawnBurst = Instantiate(spawnBurst, playerObjects[i].transform.position, Quaternion.identity);
                newSpawnBurst.GetComponent<SpriteRenderer>().color = playerObjects[i].GetComponent<Player>().playerColor;

                playerCount++;
            }
            else
            {
                playerObjects[i].gameObject.SetActive(false);
                playerObjects[i].GetComponent<Player>().powerskullCount = 0;
                playerObjects[i].GetComponent<Player>().health = 0;

            }

        }


    }

    // Update is called once per frame
    void Update()
    {
        foreach (GameObject playerObject in playerObjects)
        {
            if (playerObject != null)
            {
                if (playerObject.GetComponent<Player>().health <= 0 && playerObject.activeInHierarchy)
                {
                    Transform deadPlayerTranform = playerObject.transform;
                    GameObject playerStandIn = Instantiate(deathStandIn, playerObject.transform.position, Quaternion.identity);
                    playerStandIn.GetComponent<PlayerDeathSpriteBehavior>().targetGroup = camTargetGroup;

                    playerStandIn.GetComponent<SpriteRenderer>().color = playerObject.GetComponent<Player>().playerColor;

                    playerObject.GetComponent<Player>().killedBy.GetComponent<Player>().score += 300;

                    ParticleSystem ps = playerStandIn.transform.Find("Player Death Particles").GetComponent<ParticleSystem>();
                    ParticleSystem.MainModule main = ps.main;
                    Color playerObjectColor = playerObject.GetComponent<Player>().playerColor;

                    main.startColor = playerObjectColor;

                    playerStandIn.GetComponent<PlayerDeathSpriteBehavior>().playerColor = playerObject.GetComponent<Player>().playerColor;

                    camTargetGroup.AddMember(playerStandIn.transform, 1f, 0f);

                    GameObject.Find("Sound Manager").GetComponent<PlaySound>().PlayClip(5, false);

                    Destroy(playerObject);

                    playerCount--;
                }
            }

            if (playerCount == 1 && !playerHasWon)
            {
                playerHasWon = true;
                winner = playerObject.GetComponent<Player>().killedBy;
                TriggerVictory();
            }

            if ((highScoreUI.activeInHierarchy && highScoreUI.GetComponent<HighScoreManager>().entryComplete) || (!gotHighScore && playerHasWon) ) {
                bool playerPressedGreen = Input.GetButtonDown("Jump1") || 
                                          Input.GetButtonDown("Jump2") || 
                                          Input.GetButtonDown("Jump3") || 
                                          Input.GetButtonDown("Jump4");

                if (playerPressedGreen) {
                    GameObject.Find("GameDirector").GetComponent<GameDirector>().nextLevelToLoad = 1;
                    GameObject.Find("GameDirector").GetComponent<GameDirector>().TriggerLoadingScreen();
                }
            }
        }

        void TriggerVictory()
        {


            

            GameObject newVictoryStandin = Instantiate(playerVictoryStandin, winner.transform.position, Quaternion.identity);

            camTargetGroup.AddMember(newVictoryStandin.transform, 1f, 0f);

            newVictoryStandin.GetComponent<SpriteRenderer>().material = winner.GetComponent<Player>()._playerSprite.GetComponent<SpriteRenderer>().material;
            winner.transform.localScale = Vector3.zero;
            winner.GetComponent<Player>().disablePlayerInput = true;


            var ltCamSeq = LeanTween.sequence();
            ltCamSeq.append(3f);
            ltCamSeq.append(() =>
            {
                gameCamera.SetActive(false);
                victoryCamera.SetActive(true);

                victoryPlayerNameText.text = winner.GetComponent<Player>().playerName;
                victoryPlayerNameText.color = winner.GetComponent<Player>().playerColor;
                victoryPlayerPointsText.text = winner.GetComponent<Player>().score.ToString() + " POINTS";

                victoryCamera.GetComponent<CinemachineVirtualCamera>().m_Follow = newVictoryStandin.transform;
                LeanTween.alphaCanvas(gameUI, 0f, .25f);
            });
            ltCamSeq.append(LeanTween.alphaCanvas(victoryUI, 1f, .25f));

            for (int i = 1; i <= 9; i++)
            {
                if (PlayerPrefs.GetInt("highScorePoints" + i) < winner.GetComponent<Player>().score)
                {
                    highScoreRank = i;
                    highScoreUI.SetActive(true);
                    gotHighScore = true;
                    break;
                }
            }

        }
    }
}
