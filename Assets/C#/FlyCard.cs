using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyCard : MonoBehaviour
{
    public List<List<PlayerManager>> canSee = new List<List<PlayerManager>>();
    public string exitPath;
    Transform maze,playerManagers;
    List<int[]> Positions = new List<int[]>();
    List<int> used = new List<int>();//0沒到頭沒到;1有到頭沒到尾;2有道頭有到尾
    public PlayerManager player;
    bool myTurn = true;
    void Start()
    {
        maze = GameObject.Find("MazeGen").transform;
        playerManagers = GameObject.Find("Players").transform;
        print(exitPath);

        string[] sArray = exitPath.Split('_');

        for (int i = 0; i < sArray.Length; i++)
        {
            Positions.Add(new int[2]);
            Positions[i][0] = int.Parse(sArray[i]) / 6;
            Positions[i][1] = int.Parse(sArray[i]) % 6;
            print(Positions[i][0] + " , " + Positions[i][1]);
            if (i > 2)
            {
                break;
            }
        }
        for (int i = 0; i < sArray.Length - 1; i++)
        {
            LineRenderer cardRoad = new GameObject("cardRoad").AddComponent<LineRenderer>();
            cardRoad.transform.parent = transform;
            cardRoad.positionCount = 2;
            cardRoad.SetPosition(0, maze.GetChild(0).GetChild(int.Parse(sArray[i])).position + Vector3.up);
            cardRoad.SetPosition(1, maze.GetChild(0).GetChild(int.Parse(sArray[i + 1])).position + Vector3.up);
            cardRoad.endWidth = 0.01f;
            cardRoad.startWidth = 0.2f;
            cardRoad.enabled = false;

            canSee.Add(new List<PlayerManager>());

            if (i != 0)
            {
                used.Add(0);
            }
            else
            {
                used.Add(1);
                canSee[0].Add(player);
            }
            if (i >= 2)
            {
                break;
            }
        }
    }

    void Update()
    {
        for (int i = 0; i < playerManagers.childCount; i++)
        {
            PlayerManager playerManager = playerManagers.GetChild(i).GetComponent<PlayerManager>();
            if (playerManager.enabled == true && playerManager != player && myTurn)
            {
                myTurn = false;
            }
            else if(playerManager.enabled == true && playerManager == player && !myTurn)
            {
                Destroy(gameObject);
            }
            else if(myTurn)
            {
                addCanSee();
            }
        }
        for (int i = 0; i < playerManagers.childCount; i++)
        {
            if (playerManagers.GetChild(i).GetComponent<PlayerManager>().enabled == true)
            {
                for (int j = 0; j < canSee.Count; j++)
                {
                    bool CanSee = false;
                    for (int k = 0; k < canSee[j].Count; k++)
                    {
                        if (playerManagers.GetChild(i).GetComponent<PlayerManager>() == canSee[j][k])
                        {
                            transform.GetChild(j).GetComponent<LineRenderer>().enabled = true;
                            CanSee = true;
                            break;
                        }
                    }
                    if (!CanSee)
                    {
                        transform.GetChild(j).GetComponent<LineRenderer>().enabled = false;
                    }
                }
                break;
            }
        }
    }

    void addCanSee()
    {
        for (int i = 0; i < playerManagers.childCount; i++)
        {
            for (int j = 0; j < transform.childCount; j++)
            {
                PlayerManager playerManager = playerManagers.GetChild(i).GetComponent<PlayerManager>();
                if (playerManager.pos[0] == Positions[j][0] && playerManager.pos[1] == Positions[j][1])
                {
                    if (playerManager == player)
                    {
                        if (j > 0)
                        {
                            if (used[j - 1] == 1 && used[j] == 0)
                            {
                                used[j] = 1;
                                used[j - 1] = 2;
                                if (!canSee[j].Contains(playerManager))
                                {
                                    canSee[j].Add(playerManager);
                                }
                                if (!canSee[j - 1].Contains(playerManager))
                                {
                                    canSee[j - 1].Add(playerManager);
                                }
                            }
                        }
                    }
                    else if (used[j] == 1 || used[j] == 2)
                    {
                        if (!canSee[j].Contains(playerManager))
                        {
                            canSee[j].Add(playerManager);
                        }
                    }
                }
                else if (playerManager.pos[0] == Positions[j + 1][0] && playerManager.pos[1] == Positions[j + 1][1])
                {
                    if (playerManager != player)
                    {
                        if (used[j] == 1 || used[j] == 2)
                        {
                            if (!canSee[j].Contains(playerManager))
                            {
                                canSee[j].Add(playerManager);
                            }
                        }
                    }
                }
            }
        }
    }
}
