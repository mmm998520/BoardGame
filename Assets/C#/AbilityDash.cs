using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityDash : MonoBehaviour
{
    [HideInInspector] public string abilityDir = "";

    Transform maze;
    PlayerManager playerManager;
    GameManager gameManager;
    //int times = 3;

    void Start()
    {
        maze = GameObject.Find("MazeGen").transform;
        playerManager = gameObject.GetComponent<PlayerManager>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I) || Input.GetKeyDown(KeyCode.K) || Input.GetKeyDown(KeyCode.J) || Input.GetKeyDown(KeyCode.L) || (abilityDir != "" && abilityDir != "mid"))
        {
            if (gameObject.GetComponent<PlayerManager>().enabled == true && playerManager.abilityTimes > 0)
            {
                dash(gameObject.GetComponent<PlayerManager>().pos);
                gameObject.GetComponent<PlayerManager>().scared = 0;
                playerManager.abilityTimes--;
            }
        }
        abilityDir = "";
    }

    void dash(int[] pos)
    {
        do
        {
            int[] AfterRoom = new int[] { pos[0], pos[1] };
            List<int[]> tempOne;
            if (Input.GetKeyDown(KeyCode.I) || abilityDir == "up")
            {
                AfterRoom[0]--;
            }
            else if (Input.GetKeyDown(KeyCode.J) || abilityDir == "left")
            {
                AfterRoom[1]--;
            }
            else if (Input.GetKeyDown(KeyCode.K) || abilityDir == "down")
            {
                AfterRoom[0]++;
            }
            else if (Input.GetKeyDown(KeyCode.L) || abilityDir == "right")
            {
                AfterRoom[1]++;
            }
            //確認有沒有通道、有沒有門
            for (int i = 0; i < maze.GetChild(1).childCount; i++)
            {
                string[] sArray = maze.GetChild(1).GetChild(i).name.Split(new char[2] { '_', ',' });
                tempOne = new List<int[]>();
                tempOne.Add(new int[] { int.Parse(sArray[0]), int.Parse(sArray[1]) });
                tempOne.Add(new int[] { int.Parse(sArray[2]), int.Parse(sArray[3]) });
                //print(tempOne[0][0] + "," + tempOne[0][1] + "_" + tempOne[1][0] + "," + tempOne[1][1]);
                if ((AfterRoom[0] == tempOne[0][0] && AfterRoom[1] == tempOne[0][1] && pos[0] == tempOne[1][0] && pos[1] == tempOne[1][1])
                                            || (pos[0] == tempOne[0][0] && pos[1] == tempOne[0][1] && AfterRoom[0] == tempOne[1][0] && AfterRoom[1] == tempOne[1][1]))
                {
                    if (maze.GetChild(1).GetChild(i).childCount == 0)
                    {
                        pos = new int[] { AfterRoom[0], AfterRoom[1] };
                        playerManager.pos = pos;
                        transform.position = new Vector3((pos[0] * 2) + 1, 0, pos[1] * 2 + 1);
                        playerManager.addCanSee(pos);
                        bool Return = false;
                        if (playerManager.monsterRoar(pos))
                        {
                            Return = true;
                        }
                        if (maze.GetChild(0).GetChild(AfterRoom[0] * ((MazeGen.col - 1) / 2) + AfterRoom[1]).GetComponent<Room>().collapse == 2)
                        {
                            if (gameObject.GetComponent<PlayerManager>().bullet == 0)
                            {
                                Debug.LogError("died");
                                return;
                            }
                            else
                            {
                                gameObject.GetComponent<PlayerManager>().bullet--;
                                maze.GetChild(0).GetChild(AfterRoom[0] * ((MazeGen.col - 1) / 2) + AfterRoom[1]).GetComponent<Room>().collapse++;
                                maze.GetChild(0).GetChild(AfterRoom[0] * ((MazeGen.col - 1) / 2) + AfterRoom[1]).GetComponent<MeshRenderer>().material.color = Color.gray;
                                return;
                            }
                        }
                        if (Return)
                        {
                            return;
                        }
                        break;
                    }
                    else if (maze.GetChild(1).GetChild(i).GetChild(0).name == "opened")
                    {
                        pos = new int[] { AfterRoom[0], AfterRoom[1] };
                        playerManager.pos = pos;
                        transform.position = new Vector3((pos[0] * 2) + 1, 0, pos[1] * 2 + 1);
                        playerManager.addCanSee(pos);
                        bool Return = false;
                        if (playerManager.monsterRoar(pos))
                        {
                            Return = true;
                        }
                        if (maze.GetChild(0).GetChild(AfterRoom[0] * ((MazeGen.col - 1) / 2) + AfterRoom[1]).GetComponent<Room>().collapse == 2)
                        {
                            if (gameObject.GetComponent<PlayerManager>().bullet == 0)
                            {
                                Debug.LogError("died");
                                return;
                            }
                            else
                            {
                                gameObject.GetComponent<PlayerManager>().bullet--;
                                maze.GetChild(0).GetChild(AfterRoom[0] * ((MazeGen.col - 1) / 2) + AfterRoom[1]).GetComponent<Room>().collapse++;
                                maze.GetChild(0).GetChild(AfterRoom[0] * ((MazeGen.col - 1) / 2) + AfterRoom[1]).GetComponent<MeshRenderer>().material.color = Color.gray;
                                return;
                            }
                        }
                        if (Return)
                        {
                            return;
                        }
                        break;
                    }
                    else if (playerManager.equipment.Contains(maze.GetChild(1).GetChild(i).GetChild(0).name.Split('_')[1]))
                    {
                        playerManager.equipment.Remove(maze.GetChild(1).GetChild(i).GetChild(0).name.Split('_')[1]);
                        maze.GetChild(1).GetChild(i).GetChild(0).name = "opened";
                        maze.GetChild(1).GetChild(i).GetComponent<Passway>().used = true;
                        maze.GetChild(1).GetChild(i).GetComponent<Passway>().canSeeChild.Remove(playerManager);
                        pos = new int[] { AfterRoom[0], AfterRoom[1] };
                        playerManager.pos = pos;
                        transform.position = new Vector3((pos[0] * 2) + 1, 0, pos[1] * 2 + 1);
                        gameManager.addCollapse(10);
                        playerManager.addCanSee(pos);
                        bool Return = false;
                        if (playerManager.monsterRoar(pos))
                        {
                            Return = true;
                        }
                        if (maze.GetChild(0).GetChild(AfterRoom[0] * ((MazeGen.col - 1) / 2) + AfterRoom[1]).GetComponent<Room>().collapse == 2)
                        {
                            if (gameObject.GetComponent<PlayerManager>().bullet == 0)
                            {
                                Debug.LogError("died");
                                return;
                            }
                            else
                            {
                                gameObject.GetComponent<PlayerManager>().bullet--;
                                maze.GetChild(0).GetChild(AfterRoom[0] * ((MazeGen.col - 1) / 2) + AfterRoom[1]).GetComponent<Room>().collapse++;
                                maze.GetChild(0).GetChild(AfterRoom[0] * ((MazeGen.col - 1) / 2) + AfterRoom[1]).GetComponent<MeshRenderer>().material.color = Color.gray;
                                return;
                            }
                        }
                        if (Return)
                        {
                            return;
                        }
                        /*
                        if (maze.GetChild(0).GetChild(pos[0] * ((MazeGen.col - 1) / 2) + pos[1]).childCount > 1)
                        {
                            if(maze.GetChild(0).GetChild(pos[0] * ((MazeGen.col - 1) / 2) + pos[1]).GetChild(1).name == "Monster(Clone)")
                            {
                                gameObject.GetComponent<PlayerManager>().pos = pos;
                                if (gameObject.GetComponent<PlayerManager>().bullet == 0)
                                {
                                    Debug.LogError("died");
                                    return;
                                }
                                else
                                {
                                    gameObject.GetComponent<PlayerManager>().bullet--;
                                }
                                return;
                            }
                        }
                        */
                        break;
                    }
                    else
                    {
                        gameObject.GetComponent<PlayerManager>().pos = pos;
                        return;
                    }
                }
                if(i>= maze.GetChild(1).childCount - 1)
                {
                    gameObject.GetComponent<PlayerManager>().pos = pos;
                    return;
                }
            }
        } while (true);
    }
}
