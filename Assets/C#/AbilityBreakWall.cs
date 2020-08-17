using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityBreakWall : MonoBehaviour
{
    [HideInInspector] public string abilityDir = "";

    Transform maze;
    PlayerManager playerManager;
    //int times = 3;
    void Start()
    {
        maze = GameObject.Find("MazeGen").transform;
        playerManager = gameObject.GetComponent<PlayerManager>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I) || Input.GetKeyDown(KeyCode.K) || Input.GetKeyDown(KeyCode.J) || Input.GetKeyDown(KeyCode.L) || (abilityDir != "" && abilityDir != "mid"))
        {
            if (playerManager.enabled == true && playerManager.abilityTimes > 0)
            {
                breakWall(playerManager.pos);
            }
        }
        abilityDir = "";
    }

    void breakWall(int[] pos)
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

        if(AfterRoom[0]>=0 && AfterRoom[0]< (MazeGen.row - 1) / 2 && AfterRoom[1] >= 0 && AfterRoom[1] < (MazeGen.col - 1) / 2)
        {
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
                    if (maze.GetChild(1).GetChild(i).childCount != 0)
                    {
                        Destroy(maze.GetChild(1).GetChild(i).GetChild(0).gameObject);
                        maze.GetChild(1).GetChild(i).GetComponent<MeshRenderer>().material.color = Color.green;
                        playerManager.abilityTimes--;
                    }
                    return;
                }
            }

            GameObject column = (GameObject)Resources.Load("Prefabs/maze");
            column = MonoBehaviour.Instantiate(column);
            column.transform.position = new Vector3((transform.position.x + maze.GetChild(0).GetChild(AfterRoom[0] * ((MazeGen.col - 1) / 2) + AfterRoom[1]).position.x) / 2, 0, (transform.position.z + maze.GetChild(0).GetChild(AfterRoom[0] * ((MazeGen.col - 1) / 2) + AfterRoom[1]).position.z) / 2);
            column.GetComponent<MeshRenderer>().material.color = Color.green;
            column.transform.parent = maze.GetChild(1);
            //將通道兩端房間代號加入通道清單
            column.name = pos[0] + "," + pos[1] + "_" + AfterRoom[0] + "," + AfterRoom[1];
            Passway passway = column.AddComponent<Passway>();
            passway.canSee.Add(playerManager);
            playerManager.abilityTimes--;
        }
    }
}
