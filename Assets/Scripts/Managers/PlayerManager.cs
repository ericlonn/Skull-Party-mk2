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
    public GameDirector _director;
    public GameObject victoryScreenUIPrompts;

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
    bool hasCalledDirector = false;

    // Start is called before the first frame update
    void Awake()
    {
        playerNames.Add("bone mommy");
        playerNames.Add("pelvis wrestley");
        playerNames.Add("duke skullington");
        playerNames.Add("frasier cranium");

        _director = GameObject.FindGameObjectWithTag("GameDirector").GetComponent<GameDirector>();

        for (int i = 0; i <= 3; i++)
        {
            isPlayerActive[i] = _director.activePlayers[i];
        }

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
                playerObjects[i].gameObject.SetActive(false);

                GameObject playerToSpawn = playerObjects[i];
                var spawnDelaySeq = LeanTween.sequence();
                spawnDelaySeq.append(.5f);
                spawnDelaySeq.append(() =>
                {
                    playerToSpawn.SetActive(true);

                    GameObject newSpawnBurst = Instantiate(spawnBurst, playerToSpawn.transform.position, Quaternion.identity);
                    newSpawnBurst.GetComponent<SpriteRenderer>().color = playerToSpawn.GetComponent<Player>().playerColor;
                } );



                playerCount++;
            }
            else
            {
                playerObjects[i].gameObject.SetActive(false);
                playerObjects[i].GetComponent<Player>().powerskullCount = 0;
                playerObjects[i].GetComponent<Player>().health = 0;

            }

        }

        var fightAudioSeq = LeanTween.sequence();
        fightAudioSeq.append(.5f);
        fightAudioSeq.append(() => { GameObject.Find("Sound Manager").GetComponent<PlaySound>().PlayClip(6, false); });


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

                    GameObject.Find("Sound Manager").GetComponent<PlaySound>().PlayClip(5, false, transform.position);

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

            if ((highScoreUI.activeInHierarchy && highScoreUI.GetComponent<HighScoreManager>().entryComplete) || (!gotHighScore && playerHasWon))
            {
                if (!victoryScreenUIPrompts.activeInHierarchy)
                {
                    victoryScreenUIPrompts.SetActive(true);
                }
                bool playerPressedGreen = Input.GetButtonDown("Jump1") ||
                                          Input.GetButtonDown("Jump2") ||
                                          Input.GetButtonDown("Jump3") ||
                                          Input.GetButtonDown("Jump4");

                bool playerPressedYellow = Input.GetButtonDown("Attack1") ||
                                           Input.GetButtonDown("Attack2") ||
                                           Input.GetButtonDown("Attack3") ||
                                           Input.GetButtonDown("Attack4");

                if (playerPressedGreen && !hasCalledDirector)
                {
                    for (int i = 0; i <= 3; i++)
                    {
                        _director.activePlayers[i] = false;
                    }

                    _director.nextLevelToLoad = 1;
                    _director.TriggerLoadingScreen();
                    hasCalledDirector = true;
                }

                if (playerPressedYellow && !hasCalledDirector)
                {
                    _director.nextLevelToLoad = 2;
                    _director.TriggerLoadingScreen();
                    hasCalledDirector = true;
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
                _director.GetComponent<PlayMusic>().PlayClip(2, .25f);

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
