using System.Collections;
using System.Collections.Generic;
using Unity.Jobs;
using UnityEngine;

public class AbilityNavigation : MonoBehaviour
{
    [HideInInspector] public string abilityDir = "";

    Transform maze;
    List<int> exit = new List<int>();
    public int ExitNum = 2;
    PlayerManager playerManager;
    //int times = 3;
    void Start()
    {
        playerManager = gameObject.GetComponent<PlayerManager>();
        maze = GameObject.Find("MazeGen").transform;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z) || abilityDir != "")
        {
            if (playerManager.enabled == true && playerManager.abilityTimes > 0)
            {
                navigation(playerManager.pos);
            }
        }
        abilityDir = "";
    }

    void navigation(int[] pos)
    {
        int row = (MazeGen.row - 1) / 2, col = (MazeGen.col - 1) / 2;
        int[,] passwayLengths = new int[row * col,row * col];
        int cantWalk = 999999,monsterAttacked = 9999;
        string[] path = new string[row * col];
        List<string> passways = new List<string>();
        int FindExitNum = 0;

        int origin = pos[0] * col + pos[1];
        for (int i = 0; i < maze.GetChild(0).childCount; i++)
        {
            if (maze.GetChild(0).GetChild(i).childCount > 1)
            {
                if (maze.GetChild(0).GetChild(i).GetChild(1).name == "Exit(Clone)")
                {
                    exit.Add(i);
                    if (exit.Count >= ExitNum)
                    {
                        break;
                    }
                }
            }
        }

        foreach (Transform passway in maze.GetChild(1))
        {
            string[] sArray = passway.name.Split(new char[] { '_', ',' });
            int roomNumA = int.Parse(sArray[0]) * col + int.Parse(sArray[1]), roomNumB = int.Parse(sArray[2]) * col + int.Parse(sArray[3]);
            passways.Add(roomNumA + "," + roomNumB);
        }
        
        //設置通道長度
        for (int i = 0; i < row * col; i++)
        {
            for(int j = 0; j< row * col; j++)
            {
                if(i == j)
                {
                    passwayLengths[i,j] = 0;
                }
                else if(passways.Contains(i + "," + j) || passways.Contains(j + "," + i))
                {
                    int monsterNum = 0;
                    if (maze.GetChild(0).GetChild(j).GetComponent<Room>().collapse == 2)
                    {
                        monsterNum++;
                    }
                    if(maze.GetChild(0).GetChild(j).childCount > 1)
                    {
                        if(maze.GetChild(0).GetChild(j).GetChild(1).name == "Monster(Clone)")
                        {
                            monsterNum++;
                        }
                    }
                    if (monsterNum > 0)
                    {
                        passwayLengths[i, j] = monsterAttacked*monsterNum;
                    }
                    else
                    {
                        passwayLengths[i, j] = 1;
                    }
                }
                else
                {
                    passwayLengths[i, j] = cantWalk;
                }
                print(i + " , " + j + " : " + passwayLengths[i, j]);
            }
        }

        //存原點到各點的計算中最短距離
        int[] dis = new int[row * col];
        for(int i = 0; i < row * col; i++)
        {
            dis[i] = passwayLengths[origin, i];
        }
        //存原點到各點的地最短距離是否已確認
        int[] book = new int[row * col];
        book[origin] = 1;

        //找最短路徑(Dijkstra 演算法)
        for (int i = 0; i < row * col-1; i++)
        {
            //打亂for迴圈順序用的List
            List<int> randomList = new List<int>();
            for(int j = 0; j < row * col; j++)
            {
                randomList.Add(j);
            }
            //找到離原點最近的未確認點
            int min = cantWalk,u = origin;
            for (int j = 0; j < row * col; j++)
            {
                int r = Random.Range(0, randomList.Count);

                if (book[randomList[r]] == 0 && dis[randomList[r]] < min)
                {

                    min = dis[randomList[r]];
                    u = randomList[r];
                }
                randomList.RemoveAt(r);
            }
            //將該未確認點變成已確認點
            book[u] = 1;
            if (exit.Contains(u))
            {
                if (++FindExitNum >= ExitNum)
                {
                    showPath(origin,path,dis);
                    return;
                }
            }
            //從最新的已確認點延伸出去更新其他未確認點到原點的距離
            for(int j = 0; j < row * col; j++)
            {
                if (passwayLengths[u, j] < cantWalk)
                {
                    if (dis[j] > dis[u] + passwayLengths[u, j])
                    {
                        //更新j點到原點距離
                        dis[j] = dis[u] + passwayLengths[u, j];
                        //記錄過程點
                        path[j] = path[u] + "_" + u;
                    }
                }
            }
        }
        /*
        for(int i = 0; i < path.Length; i++)
        {
            print(i + " : " + path[i]);
        }
        */
    }

    void showPath(int origin, string[] path, int[] dis)
    {
        int lessStepExit = exit[Random.Range(0, exit.Count)];
        List<int> randomList = new List<int>();
        for (int i = 0; i < exit.Count; i++)
        {
            randomList.Add(i);
        }
        for (int i = 0; i < exit.Count; i++)
        {
            int r = Random.Range(0, randomList.Count);
            if (dis[lessStepExit] > dis[exit[r]])
            {
                lessStepExit = exit[r];
            }
            randomList.RemoveAt(r);
        }

        string exitPath = origin + path[lessStepExit] + "_" + lessStepExit;
        playerManager.abilityTimes--;
        
        FlyCard flyCard = new GameObject("FlyCard").AddComponent<FlyCard>();
        flyCard.exitPath = exitPath;
        flyCard.player = playerManager;
    }
}
