using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Panel : MonoBehaviour
{
    GameManager gameManager;
    Transform players;
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        players = GameObject.Find("Players").transform;
    }

    void Update()
    {

        for (int i = 0; i < players.childCount; i++)
        {
            PlayerManager playerManager = players.GetChild(i).GetComponent<PlayerManager>();
            if (playerManager.enabled == true)
            {
                for (int j = 0; j < transform.GetChild(1).childCount; j++)
                {
                    if (j < playerManager.heavyBurden)
                    {
                        transform.GetChild(1).GetChild(j).gameObject.SetActive(true);
                    }
                    else
                    {
                        transform.GetChild(1).GetChild(j).gameObject.SetActive(false);
                    }
                }

                transform.GetChild(0).GetComponent<Text>().text = "行動 : " + string.Format("{0:d3}", playerManager.action) + "\n" +
                                                                                                   "驚嚇 : " + string.Format("{0:d3}", playerManager.scared) + "\n" +
                                                                                                   "子彈 : " + string.Format("{0:d3}", playerManager.bullet) + "\n" +
                                                                                                   "火柴 : " + string.Format("{0:d3}", playerManager.match) + "\n" +
                                                                                                   "火把 : " + string.Format("{0:d3}", playerManager.torch) + "\n" +
                                                                                                   "技能 : " + string.Format("{0:d3}", playerManager.abilityTimes);

                Transform equipment = transform.GetChild(1);
                for (int j = 0; j < equipment.childCount; j++)
                {
                    if (playerManager.equipment.Count > j)
                    {
                        equipment.GetChild(j).GetChild(0).GetComponent<Text>().text = playerManager.equipment[j];
                    }
                    else
                    {
                        equipment.GetChild(j).GetChild(0).GetComponent<Text>().text = "";
                    }
                }
                return;
            }
        }
        players = GameObject.Find("Players").transform;

        for (int i = 0; i < players.childCount; i++)
        {
            PlayerManager playerManager = players.GetChild(i).GetComponent<PlayerManager>();
            if (playerManager.enabled == true)
            {
                print(transform.name);

            }
        }
        
    }

    public void ChangeScared(string AddORLess)
    {
        for (int i = 0; i < players.childCount; i++)
        {
            if (players.GetChild(i).GetComponent<PlayerManager>().enabled == true)
            {
                PlayerManager playerManager = players.GetChild(i).GetComponent<PlayerManager>();
                if(AddORLess == "+")
                {
                    playerManager.scared++;
                }
                else
                {
                    playerManager.scared--;
                }
            }
        }
    }

    public void ChangeAction(string AddORLess)
    {
        for (int i = 0; i < players.childCount; i++)
        {
            if (players.GetChild(i).GetComponent<PlayerManager>().enabled == true)
            {
                PlayerManager playerManager = players.GetChild(i).GetComponent<PlayerManager>();
                if (AddORLess == "+")
                {
                    playerManager.action++;
                }
                else
                {
                    playerManager.action--;
                }
            }
        }
    }

    public void ChangeBullet(string AddORLess)
    {
        for (int i = 0; i < players.childCount; i++)
        {
            if (players.GetChild(i).GetComponent<PlayerManager>().enabled == true)
            {
                PlayerManager playerManager = players.GetChild(i).GetComponent<PlayerManager>();
                if (AddORLess == "+")
                {
                    playerManager.bullet++;
                }
                else
                {
                    playerManager.bullet--;
                }
            }
        }
    }

    public void ChangeMatch(string AddORLess)
    {
        for (int i = 0; i < players.childCount; i++)
        {
            if (players.GetChild(i).GetComponent<PlayerManager>().enabled == true)
            {
                PlayerManager playerManager = players.GetChild(i).GetComponent<PlayerManager>();
                if (AddORLess == "+")
                {
                    playerManager.match++;
                }
                else
                {
                    playerManager.match--;
                }
            }
        }
    }

    public void ChangeTorch(string AddORLess)
    {
        for (int i = 0; i < players.childCount; i++)
        {
            if (players.GetChild(i).GetComponent<PlayerManager>().enabled == true)
            {
                PlayerManager playerManager = players.GetChild(i).GetComponent<PlayerManager>();
                if (AddORLess == "+")
                {
                    playerManager.torch++;
                }
                else
                {
                    playerManager.torch--;
                }
            }
        }
    }

    public void ChangeAbility(string AddORLess)
    {
        for (int i = 0; i < players.childCount; i++)
        {
            if (players.GetChild(i).GetComponent<PlayerManager>().enabled == true)
            {
                PlayerManager playerManager = players.GetChild(i).GetComponent<PlayerManager>();
                if (AddORLess == "+")
                {
                    playerManager.abilityTimes++;
                }
                else
                {
                    playerManager.abilityTimes--;
                }
            }
        }
    }
}
