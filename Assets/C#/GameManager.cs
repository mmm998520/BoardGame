using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using UnityEditor;
//using UnityEditor.ShortcutManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public List<List<GameObject>> rooms = new List<List<GameObject>>();
    public Transform maze, players, scoreboard;
    int n = 0;
    public int collapse = 0;

    void Start()
    {
        addRoomData();
        doorGen();
        players.GetChild(0).GetComponent<PlayerManager>().monsterRoar(players.GetChild(0).GetComponent<PlayerManager>().pos);
        Camera.main.transform.position = new Vector3(players.GetChild(0).position.x, 10, players.GetChild(0).position.z);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            ClearConsole();
            switchPlayer();
        }
        showScoreboard();
    }

    // Alt + C
    //[Shortcut("Clear Console", KeyCode.C, ShortcutModifiers.Alt)]
    public static void ClearConsole()
    {
        /*
        var assembly = Assembly.GetAssembly(typeof(SceneView));
        var type = assembly.GetType("UnityEditor.LogEntries");
        var method = type.GetMethod("Clear");
        method.Invoke(new object(), null);
        */
    }

    List<int> boxValue()
    {
        List<int> boxValue = new List<int>();
        for (int i = 0; i < 4; i++)
        {
            boxValue.Add(5);
        }
        for (int i = 0; i < 5; i++)
        {
            boxValue.Add(10);
        }
        for (int i = 0; i < 4; i++)
        {
            boxValue.Add(15);
        }
        for (int i = 0; i < 1; i++)
        {
            boxValue.Add(20);
        }
        return boxValue;
    }

    List<int> startled()
    {
        List<int> startled = new List<int>();
        for (int i = 0; i < 4; i++)
        {
            startled.Add(3);
        }
        for (int i = 0; i < 6; i++)
        {
            startled.Add(5);
        }
        for (int i = 0; i < 6; i++)
        {
            startled.Add(10);
        }
        for (int i = 0; i < 4; i++)
        {
            startled.Add(20);
        }
        return startled;
    }

    void doorGen()
    {
        List<int[]> tempOne, tempTheOther;
        int r;
        for(int i = 0; i < 6; i++)
        {
            do
            {
                tempOne = new List<int[]>();
                tempTheOther = new List<int[]>();
                r = Random.Range(0, maze.GetChild(1).childCount);
                string[] sArray = maze.GetChild(1).GetChild(r).name.Split(new char[2] { '_', ',' });
                tempOne.Add(new int[] { int.Parse(sArray[0]), int.Parse(sArray[1]) });
                tempOne.Add(new int[] { int.Parse(sArray[2]), int.Parse(sArray[3]) });
                tempTheOther.Add(new int[] { int.Parse(sArray[2]), int.Parse(sArray[3]) });
                tempTheOther.Add(new int[] { int.Parse(sArray[0]), int.Parse(sArray[1]) });
            } while (maze.GetChild(1).GetChild(r).childCount != 0);

            Transform door = Instantiate((GameObject)Resources.Load("Prefabs/Door")).transform;
            door.parent = maze.GetChild(1).GetChild(r);
            door.localPosition = Vector3.zero;
            door.localScale = new Vector3(1, 1.3f, 1);
            if (i < 3)
            {
                door.GetComponent<MeshRenderer>().material.color = Color.blue;
                door.name = "door_Blue";
            }
            else
            {
                door.GetComponent<MeshRenderer>().material.color = Color.red;
                door.name = "door_Red";
            }
            
        }
        
    }

    List<List<GameObject>> TotalList()
    {
        GameObject Event;
        int boxR, startledR;
        List<int> boxValue = this.boxValue();
        List<int> startled = this.startled();

        List<List<GameObject>> totalList = new List<List<GameObject>>();
        for (int i = 0; i < (MazeGen.row - 1) / 2 * (MazeGen.col - 1) / 2; i++)
        {
            List<GameObject> temp = new List<GameObject>();
            Event = Instantiate((GameObject)Resources.Load("Prefabs/Null"));
            temp.Add(Event);
            totalList.Add(temp);
        }

        Event = Instantiate((GameObject)Resources.Load("Prefabs/Start"));
        totalList[0].Add(Event);

        Event = Instantiate((GameObject)Resources.Load("Prefabs/Exit"));
        totalList[1].Add(Event);
        Event = Instantiate((GameObject)Resources.Load("Prefabs/Exit"));
        totalList[2].Add(Event);

        Event = Instantiate((GameObject)Resources.Load("Prefabs/Monster"));
        totalList[3].Add(Event);
        Event = Instantiate((GameObject)Resources.Load("Prefabs/Treasure"));
        Event.GetComponent<Treasure>().value = 30;
        totalList[3].Add(Event);
        Event = Instantiate((GameObject)Resources.Load("Prefabs/Monster"));
        totalList[4].Add(Event);
        Event = Instantiate((GameObject)Resources.Load("Prefabs/Treasure"));
        Event.GetComponent<Treasure>().value = 30;
        totalList[4].Add(Event);
        Event = Instantiate((GameObject)Resources.Load("Prefabs/Monster"));
        totalList[5].Add(Event);
        Event = Instantiate((GameObject)Resources.Load("Prefabs/Treasure"));
        Event.GetComponent<Treasure>().value = 40;
        totalList[5].Add(Event);

        for(int i = 6; i <= 8; i++)
        {
            Event = Instantiate((GameObject)Resources.Load("Prefabs/Box"));
            boxR = Random.Range(0, boxValue.Count);
            Event.GetComponent<Box>().value = boxValue[boxR];
            Event.GetComponent<Box>().startled = 0;
            boxValue.RemoveAt(boxR);
            totalList[i].Add(Event);
        }

        for (int i = 9; i <= 11; i++)
        {
            Event = Instantiate((GameObject)Resources.Load("Prefabs/Box"));
            startledR = Random.Range(0, startled.Count);
            Event.GetComponent<Box>().value = 0;
            Event.GetComponent<Box>().startled = startled[startledR];
            startled.RemoveAt(startledR);
            totalList[i].Add(Event);
        }

        for (int i = 12; i <= 14; i++)
        {
            Event = Instantiate((GameObject)Resources.Load("Prefabs/Box"));
            boxR = Random.Range(0, boxValue.Count);
            startledR = Random.Range(0, startled.Count);
            Event.GetComponent<Box>().value = boxValue[boxR];
            Event.GetComponent<Box>().startled = startled[startledR];
            boxValue.RemoveAt(boxR);
            startled.RemoveAt(startledR);
            totalList[i].Add(Event);
        }

        int k = 15;
        for(int i = 0; i < 3; i++)
        {
            for(int j = 0; j < i + 1; j++)
            {
                Event = Instantiate((GameObject)Resources.Load("Prefabs/Box"));
                if (i == 0)
                {
                    boxR = Random.Range(0, boxValue.Count);
                    Event.GetComponent<Box>().value = boxValue[boxR];
                    Event.GetComponent<Box>().startled = 0;
                    boxValue.RemoveAt(boxR);
                    totalList[k].Add(Event);
                }
                else if (i == 1)
                {
                    startledR = Random.Range(0, startled.Count);
                    Event.GetComponent<Box>().value = 0;
                    Event.GetComponent<Box>().startled = startled[startledR];
                    startled.RemoveAt(startledR);
                    totalList[k].Add(Event);
                }
                else
                {
                    boxR = Random.Range(0, boxValue.Count);
                    startledR = Random.Range(0, startled.Count);
                    Event.GetComponent<Box>().value = boxValue[boxR];
                    Event.GetComponent<Box>().startled = startled[startledR];
                    boxValue.RemoveAt(boxR);
                    startled.RemoveAt(startledR);
                    totalList[k].Add(Event);
                }

                Event = Instantiate((GameObject)Resources.Load("Prefabs/Box"));
                if (j == 0)
                {
                    boxR = Random.Range(0, boxValue.Count);
                    Event.GetComponent<Box>().value = boxValue[boxR];
                    Event.GetComponent<Box>().startled = 0;
                    boxValue.RemoveAt(boxR);
                    totalList[k++].Add(Event);
                }
                else if (j == 1)
                {
                    startledR = Random.Range(0, startled.Count);
                    Event.GetComponent<Box>().value = 0;
                    Event.GetComponent<Box>().startled = startled[startledR];
                    startled.RemoveAt(startledR);
                    totalList[k++].Add(Event);
                }
                else
                {
                    boxR = Random.Range(0, boxValue.Count);
                    startledR = Random.Range(0, startled.Count);
                    Event.GetComponent<Box>().value = boxValue[boxR];
                    Event.GetComponent<Box>().startled = startled[startledR];
                    boxValue.RemoveAt(boxR);
                    startled.RemoveAt(startledR);
                    totalList[k++].Add(Event);
                }
            }
        }

        for(int i = 21; i <= 26; i++)
        {
            Event = Instantiate((GameObject)Resources.Load("Prefabs/DeadBody"));
            startledR = Random.Range(0, startled.Count);
            Event.GetComponent<DeadBody>().startled = startled[startledR];
            startled.RemoveAt(startledR);
            if (i == 21)
            {
                Event.GetComponent<DeadBody>().bullet = 1;
                Event.GetComponent<DeadBody>().expansion = 1;
                Event.GetComponent<DeadBody>().match = 0;
            }
            else if (i == 22)
            {
                Event.GetComponent<DeadBody>().bullet = 1;
                Event.GetComponent<DeadBody>().expansion = 0;
                Event.GetComponent<DeadBody>().match = 0;
            }
            else if (i == 23)
            {
                Event.GetComponent<DeadBody>().bullet = 0;
                Event.GetComponent<DeadBody>().expansion = 2;
                Event.GetComponent<DeadBody>().match = 0;
            }
            else if (i == 24)
            {
                Event.GetComponent<DeadBody>().bullet = 0;
                Event.GetComponent<DeadBody>().expansion = 0;
                Event.GetComponent<DeadBody>().match = 2;
                //三人
                Event.GetComponent<DeadBody>().bullet++;
                //三人
            }
            else if (i == 25)
            {
                Event.GetComponent<DeadBody>().bullet = 0;
                Event.GetComponent<DeadBody>().expansion = 0;
                Event.GetComponent<DeadBody>().match = 1;
                //三人
                Event.GetComponent<DeadBody>().bullet++;
                //三人
            }
            else if (i == 26)
            {
                Event.GetComponent<DeadBody>().bullet = 0;
                Event.GetComponent<DeadBody>().expansion = 1;
                Event.GetComponent<DeadBody>().match = 1;
            }
            totalList[i].Add(Event);
        }

        for (int i = 27; i <= 29; i++)
        {
            Event = Instantiate((GameObject)Resources.Load("Prefabs/BlueKey"));
            totalList[i].Add(Event);
        }
        for (int i = 30; i <= 32; i++)
        {
            Event = Instantiate((GameObject)Resources.Load("Prefabs/RedKey"));
            totalList[i].Add(Event);
        }

        //三人
        Event = Instantiate((GameObject)Resources.Load("Prefabs/Monster"));
        totalList[33].Add(Event);
        Event = Instantiate((GameObject)Resources.Load("Prefabs/Treasure"));
        Event.GetComponent<Treasure>().value = 35;
        totalList[33].Add(Event);
        //三人

        return totalList;
    }

    List<int[]> InsidePos()
    {
        List<int[]> insidePos = new List<int[]>();
        for(int i = 1; i < (MazeGen.col - 1) / 2 - 1; i++)
        {
            for (int j = 1; j < (MazeGen.row - 1) / 2 - 1; j++)
            {
                insidePos.Add(new int[] { i, j});
            }
        }
        return insidePos;
    }

    List<int[]> SidePos()
    {
        List<int[]> sidePos = new List<int[]>();
        for (int i = 0; i < (MazeGen.col - 1) / 2; i++)
        {
            sidePos.Add(new int[] { 0, i });
        }
        for (int i = 1; i < (MazeGen.row - 1) / 2 - 1; i++)
        {
            sidePos.Add(new int[] { i, (MazeGen.col - 1) / 2 - 1 });
        }
        for (int i = (MazeGen.col - 1) / 2 - 1; i >= 0; i--)
        {
            sidePos.Add(new int[] { (MazeGen.row - 1) / 2 - 1, i });
        }
        for (int i = (MazeGen.row - 1) / 2 - 2; i > 0; i--)
        {
            sidePos.Add(new int[] { i, 0 });
        }
        return sidePos;
    }

    List<int[]> exitPos()
    {
        List<int[]> sidePos = SidePos();

        List<int[]> exitPos = new List<int[]>();
        int r = Random.Range(0, sidePos.Count);
        exitPos.Add(sidePos[r]);

        //刪掉附近(空兩格)的可能避免出口太近
        int less = 2,add = 0;
        if (r < 2)
        {
            less = r;
        }
        for(int i = 0; i < 3 + less; i++)
        {
            if(r - less >= sidePos.Count)
            {
                add++;
            }
            else
            {
                sidePos.RemoveAt(r - less);
            }
        }
        for(int i = 0; i < 2 - less; i++)
        {
            sidePos.RemoveAt(sidePos.Count - 1);
        }
        for(int i = 0; i < add; i++)
        {
            sidePos.RemoveAt(0);
        }
        
        r = Random.Range(0, sidePos.Count);
        exitPos.Add(sidePos[r]);
        return exitPos;
    }

    void addRoomData()
    {
        List<List<GameObject>> totalList = TotalList();
        List<int[]> exitPos = this.exitPos();
        totalList[1][0].transform.parent = rooms[exitPos[0][0]][exitPos[0][1]].transform;
        totalList[1][0].transform.localPosition = Vector3.zero;
        totalList[1][1].transform.parent = rooms[exitPos[0][0]][exitPos[0][1]].transform;
        totalList[1][1].transform.localPosition = Vector3.zero;
        totalList.RemoveAt(1);
        totalList[1][0].transform.parent = rooms[exitPos[1][0]][exitPos[1][1]].transform;
        totalList[1][0].transform.localPosition = Vector3.zero;
        totalList[1][1].transform.parent = rooms[exitPos[1][0]][exitPos[1][1]].transform;
        totalList[1][1].transform.localPosition = Vector3.zero;
        totalList.RemoveAt(1);

        List<int[]> insidePos = this.InsidePos();
        int random = Random.Range(0, insidePos.Count);
        totalList[0][0].transform.parent = rooms[insidePos[random][0]][insidePos[random][1]].transform;
        totalList[0][0].transform.localPosition = Vector3.zero;
        totalList[0][1].transform.parent = rooms[insidePos[random][0]][insidePos[random][1]].transform;
        totalList[0][1].transform.localPosition = Vector3.zero;
        totalList.RemoveAt(0);
        for (int k = 0; k < players.childCount; k++)
        {
            players.GetChild(k).position = maze.GetChild(0).GetChild(insidePos[random][0] * ((MazeGen.col - 1) / 2) +insidePos[random][1]).position;
            players.GetChild(k).GetComponent<PlayerManager>().pos = new int[] { insidePos[random][0], insidePos[random][1] };
            players.GetChild(k).GetComponent<PlayerManager>().addCanSee(players.GetChild(k).GetComponent<PlayerManager>().pos);
        }
        //bool firstStart = true;
        for (int i = 0; i < (MazeGen.row - 1) / 2; i++)
        {
            for (int j = 0; j < (MazeGen.col - 1) / 2; j++)
            {
                if (!((i == exitPos[0][0] && j == exitPos[0][1]) || (i == exitPos[1][0] && j == exitPos[1][1]) || (i == insidePos[random][0] && j == insidePos[random][1])))
                {
                    int r = Random.Range(0, totalList.Count);
                    /*if(r == 0 && firstStart)
                    {
                        firstStart = false;
                        for (int k = 0; k < players.childCount; k++)
                        {
                            players.GetChild(k).position = maze.GetChild(0).GetChild(i * ((MazeGen.col - 1) / 2) + j).position;
                            players.GetChild(k).GetComponent<PlayerManager>().pos = new int[] { i, j };
                            players.GetChild(k).GetComponent<PlayerManager>().addCanSee(players.GetChild(k).GetComponent<PlayerManager>().pos);
                        }
                    }*/
                    for (int k = 0; k < totalList[r].Count; k++)
                    {
                        totalList[r][k].transform.parent = rooms[i][j].transform;
                        totalList[r][k].transform.localPosition = Vector3.zero;
                    }
                    totalList.RemoveAt(r);
                }
            }
        }
    }

    public void switchPlayer()
    {
        players.GetChild(n).GetComponent<PlayerManager>().enabled = false;
        if(++n >= players.childCount)
        {
            n = 0;
            scoreboard.GetComponent<Scoreboard>().updateScoreboard();
        }
        players.GetChild(n).GetComponent<PlayerManager>().enabled = true;
        players.GetChild(n).GetComponent<PlayerManager>().addCanSee(players.GetChild(n).GetComponent<PlayerManager>().pos);
        //判定玩家驚嚇值
        if (players.GetChild(n).GetComponent<PlayerManager>().scared >= players.GetChild(n).GetComponent<PlayerManager>().maxScared)
        {
            players.GetChild(n).GetComponent<PlayerManager>().action = 0;
            players.GetChild(n).GetComponent<PlayerManager>().scared -= players.GetChild(n).GetComponent<PlayerManager>().forciblyRestScared;
            if (players.GetChild(n).GetComponent<PlayerManager>().scared < 0)
            {
                players.GetChild(n).GetComponent<PlayerManager>().scared = 0;
            }
        }
        else if (players.GetChild(n).GetComponent<PlayerManager>().scared >= players.GetChild(n).GetComponent<PlayerManager>().alarmedScared)
        {
            players.GetChild(n).GetComponent<PlayerManager>().action = players.GetChild(n).GetComponent<PlayerManager>().actionMax / 2;
        }
        else
        {
            players.GetChild(n).GetComponent<PlayerManager>().action = players.GetChild(n).GetComponent<PlayerManager>().actionMax;
        }
    }

    public void addCollapse(int num)
    {
        int tempCollapse = collapse;
        collapse += num;
        if (tempCollapse < 50 && collapse >= 50)
        {
            Debug.LogError("50");
            Camera.main.transform.GetChild(2).GetComponent<AudioSource>().clip = (AudioClip)Resources.Load("Sound/Alarm");
            Camera.main.transform.GetChild(2).GetComponent<AudioSource>().Play();
        }
        if (tempCollapse < 80 && collapse >= 80)
        {
            Debug.LogError("80");
        }
        if (tempCollapse < 100 && collapse >= 100)
        {
            GameObject.Find("Start(Clone)").transform.parent.GetComponent<Room>().collapse = 2;
            GameObject.Find("Start(Clone)").transform.parent.GetComponent<MeshRenderer>().material.color = Color.black;
            Debug.LogError("100");
        }
    }

    public void throwObj(int num)
    {
        for (int i = 0; i < players.childCount; i++)
        {
            PlayerManager playerManager = players.GetChild(i).GetComponent<PlayerManager>();
            if (playerManager.enabled == true)
            {
                if (playerManager.equipment.Count > num && players.GetChild(i).GetComponent<PlayerManager>().action > 0)
                {
                    playerManager.action--;

                    //將丟棄物件生成於房間中
                    Transform room = maze.GetChild(0).GetChild(playerManager.pos[0] * ((MazeGen.col - 1) / 2) + playerManager.pos[1]);
                    Transform temp = Instantiate((GameObject)Resources.Load("Prefabs/Dereliction"),room.position,Quaternion.Euler(0,Random.Range(0,360),0)).transform;
                    temp.parent = room;
                    temp.GetComponent<Dereliction>().dereliction = playerManager.equipment[num];
                    temp.GetChild(1).GetComponent<TextMeshPro>().text = playerManager.equipment[num];
                    if (playerManager.equipment[num] == "火柴")
                    {
                        temp.GetComponent<Dereliction>().match = playerManager.match;
                        temp.GetChild(1).GetComponent<TextMeshPro>().text = temp.GetChild(1).GetComponent<TextMeshPro>().text + " : " + playerManager.match;
                        playerManager.match = 0;
                    }
                    else if(playerManager.equipment[num] == "火把")
                    {
                        temp.GetComponent<Dereliction>().torch = playerManager.torch;
                        temp.GetChild(1).GetComponent<TextMeshPro>().text = temp.GetChild(1).GetComponent<TextMeshPro>().text + " : " + playerManager.torch;
                        playerManager.torch = 0;
                    }
                    temp.GetComponent<Dereliction>().canSee.Add(playerManager);
                    playerManager.equipment.RemoveAt(num);
                }
                else
                {
                    if (players.GetChild(i).GetComponent<PlayerManager>().action <= 0)
                    {
                        Debug.LogError("沒行動了");
                    }

                }
                return;
            }
        }
    }

    public void showScoreboard()
    {
        if (Input.GetKey(KeyCode.Tab))
        {
            scoreboard.gameObject.SetActive(true);
        }
        else
        {
            scoreboard.gameObject.SetActive(false);
        }
        for(int i = 0; i < players.childCount; i++)
        {
            if (Input.GetKey((KeyCode)49+i))
            {
                scoreboard.GetChild(i).gameObject.SetActive(true);
            }
            else
            {
                scoreboard.GetChild(i).gameObject.SetActive(false);
            }
        }
        if (Input.GetKey(KeyCode.Alpha0))
        {
            scoreboard.GetChild(players.childCount).gameObject.SetActive(true);
        }
        else
        {
            scoreboard.GetChild(players.childCount).gameObject.SetActive(false);
        }
    }
}
