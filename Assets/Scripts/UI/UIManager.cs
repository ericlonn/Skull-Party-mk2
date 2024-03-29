﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public List<GameObject> uiStatusContainers = new List<GameObject>();
    public PlayerManager _playerManager;

    // Start is called before the first frame update
    void Start()
    {

        foreach (GameObject uiStatusContainer in uiStatusContainers)
        {
            uiStatusContainer.transform.Find("Heart 1").GetComponent<UIIconStatus>().isEnabled = true;
            uiStatusContainer.transform.Find("Heart 2").GetComponent<UIIconStatus>().isEnabled = true;
            uiStatusContainer.transform.Find("Heart 3").GetComponent<UIIconStatus>().isEnabled = true;

            uiStatusContainer.transform.Find("Powerskull 1").GetComponent<UIIconStatus>().isEnabled = false;
            uiStatusContainer.transform.Find("Powerskull 2").GetComponent<UIIconStatus>().isEnabled = false;
            uiStatusContainer.transform.Find("Powerskull 3").GetComponent<UIIconStatus>().isEnabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        int playerNumber = 0;

        foreach (GameObject player in _playerManager.playerObjects)
        {

            if (player == null)
            {
                uiStatusContainers[playerNumber].transform.Find("Heart 1").GetComponent<UIIconStatus>().isEnabled = false;
                uiStatusContainers[playerNumber].transform.Find("Heart 2").GetComponent<UIIconStatus>().isEnabled = false;
                uiStatusContainers[playerNumber].transform.Find("Heart 3").GetComponent<UIIconStatus>().isEnabled = false;

                uiStatusContainers[playerNumber].transform.Find("Powerskull 1").GetComponent<UIIconStatus>().isEnabled = false;
                uiStatusContainers[playerNumber].transform.Find("Powerskull 2").GetComponent<UIIconStatus>().isEnabled = false;
                uiStatusContainers[playerNumber].transform.Find("Powerskull 3").GetComponent<UIIconStatus>().isEnabled = false;

                uiStatusContainers[playerNumber].GetComponent<CanvasGroup>().alpha = .25f;
            }
            else
            {


                switch (player.GetComponent<Player>().health)
                {
                    case 0:
                        uiStatusContainers[playerNumber].transform.Find("Heart 1").GetComponent<UIIconStatus>().isEnabled = false;
                        uiStatusContainers[playerNumber].transform.Find("Heart 2").GetComponent<UIIconStatus>().isEnabled = false;
                        uiStatusContainers[playerNumber].transform.Find("Heart 3").GetComponent<UIIconStatus>().isEnabled = false;

                        uiStatusContainers[playerNumber].GetComponent<CanvasGroup>().alpha = .25f;
                        break;
                    case 1:
                        uiStatusContainers[playerNumber].transform.Find("Heart 1").GetComponent<UIIconStatus>().isEnabled = true;
                        uiStatusContainers[playerNumber].transform.Find("Heart 2").GetComponent<UIIconStatus>().isEnabled = false;
                        uiStatusContainers[playerNumber].transform.Find("Heart 3").GetComponent<UIIconStatus>().isEnabled = false;
                        break;
                    case 2:
                        uiStatusContainers[playerNumber].transform.Find("Heart 1").GetComponent<UIIconStatus>().isEnabled = true;
                        uiStatusContainers[playerNumber].transform.Find("Heart 2").GetComponent<UIIconStatus>().isEnabled = true;
                        uiStatusContainers[playerNumber].transform.Find("Heart 3").GetComponent<UIIconStatus>().isEnabled = false;
                        break;
                    case 3:
                        uiStatusContainers[playerNumber].transform.Find("Heart 1").GetComponent<UIIconStatus>().isEnabled = true;
                        uiStatusContainers[playerNumber].transform.Find("Heart 2").GetComponent<UIIconStatus>().isEnabled = true;
                        uiStatusContainers[playerNumber].transform.Find("Heart 3").GetComponent<UIIconStatus>().isEnabled = true;
                        break;
                    default:
                        break;
                }

                if (player.gameObject.GetComponent<Player>().isPoweredUp)
                {

                    switch (player.GetComponent<PlayerAttack>().ammoCount)
                    {
                        case 0:
                            uiStatusContainers[playerNumber].transform.Find("Powerskull 1").GetComponent<UIIconStatus>().isEnabled = false;
                            uiStatusContainers[playerNumber].transform.Find("Powerskull 2").GetComponent<UIIconStatus>().isEnabled = false;
                            uiStatusContainers[playerNumber].transform.Find("Powerskull 3").GetComponent<UIIconStatus>().isEnabled = false;
                            break;
                        case 1:
                            uiStatusContainers[playerNumber].transform.Find("Powerskull 1").GetComponent<UIIconStatus>().isEnabled = true;
                            uiStatusContainers[playerNumber].transform.Find("Powerskull 2").GetComponent<UIIconStatus>().isEnabled = false;
                            uiStatusContainers[playerNumber].transform.Find("Powerskull 3").GetComponent<UIIconStatus>().isEnabled = false;
                            break;
                        case 2:
                            uiStatusContainers[playerNumber].transform.Find("Powerskull 1").GetComponent<UIIconStatus>().isEnabled = true;
                            uiStatusContainers[playerNumber].transform.Find("Powerskull 2").GetComponent<UIIconStatus>().isEnabled = true;
                            uiStatusContainers[playerNumber].transform.Find("Powerskull 3").GetComponent<UIIconStatus>().isEnabled = false;
                            break;
                        case 3:
                            uiStatusContainers[playerNumber].transform.Find("Powerskull 1").GetComponent<UIIconStatus>().isEnabled = true;
                            uiStatusContainers[playerNumber].transform.Find("Powerskull 2").GetComponent<UIIconStatus>().isEnabled = true;
                            uiStatusContainers[playerNumber].transform.Find("Powerskull 3").GetComponent<UIIconStatus>().isEnabled = true;
                            break;
                        default:
                            break;
                    }
                }
                else
                {
                    switch (player.GetComponent<Player>().powerskullCount)
                    {
                        case 0:
                            uiStatusContainers[playerNumber].transform.Find("Powerskull 1").GetComponent<UIIconStatus>().isEnabled = false;
                            uiStatusContainers[playerNumber].transform.Find("Powerskull 2").GetComponent<UIIconStatus>().isEnabled = false;
                            uiStatusContainers[playerNumber].transform.Find("Powerskull 3").GetComponent<UIIconStatus>().isEnabled = false;
                            break;
                        case 1:
                            uiStatusContainers[playerNumber].transform.Find("Powerskull 1").GetComponent<UIIconStatus>().isEnabled = true;
                            uiStatusContainers[playerNumber].transform.Find("Powerskull 2").GetComponent<UIIconStatus>().isEnabled = false;
                            uiStatusContainers[playerNumber].transform.Find("Powerskull 3").GetComponent<UIIconStatus>().isEnabled = false;
                            break;
                        case 2:
                            uiStatusContainers[playerNumber].transform.Find("Powerskull 1").GetComponent<UIIconStatus>().isEnabled = true;
                            uiStatusContainers[playerNumber].transform.Find("Powerskull 2").GetComponent<UIIconStatus>().isEnabled = true;
                            uiStatusContainers[playerNumber].transform.Find("Powerskull 3").GetComponent<UIIconStatus>().isEnabled = false;
                            break;
                        case 3:
                            uiStatusContainers[playerNumber].transform.Find("Powerskull 1").GetComponent<UIIconStatus>().isEnabled = true;
                            uiStatusContainers[playerNumber].transform.Find("Powerskull 2").GetComponent<UIIconStatus>().isEnabled = true;
                            uiStatusContainers[playerNumber].transform.Find("Powerskull 3").GetComponent<UIIconStatus>().isEnabled = true;
                            break;
                        default:
                            break;
                    }
                }

            }

            playerNumber++;


        }
    }
}
