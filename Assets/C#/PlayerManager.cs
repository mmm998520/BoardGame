using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    //按鈕控制
    [HideInInspector] public string moveDir = "", matchDir = "", torchDir = "";
    
    GameManager gameManager;
    [HideInInspector] public int[] pos = new int[2];
    Transform maze;
    public List<string> equipment;
    public int scared, action, bullet, match,torch,abilityTimes;
    [SpaceAttribute]
    [HeaderAttribute("Player Setting")]
    public int heavyBurden;
    public int restScared, forciblyRestScared, maxScared, alarmedScared, actionMax;
    public bool died = false;

    private void Awake()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        maze = GameObject.Find("MazeGen").transform;
    }

    void Update()
    {
        if (action>0 && (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow)))
        {
            move(pos);
        }
        if (match>0 && (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D)))
        {
            useMatch(pos);

        }
        if(action>=actionMax/2 && Input.GetKeyDown(KeyCode.R))
        {
            action-= actionMax / 2;
            rest();
        }

        if (torch > 0 && (Input.GetKeyDown(KeyCode.Keypad8) || Input.GetKeyDown(KeyCode.Keypad2) || Input.GetKeyDown(KeyCode.Keypad4) || Input.GetKeyDown(KeyCode.Keypad6) || Input.GetKeyDown(KeyCode.Keypad5)))
        {
            useTorch(pos);
        }
    }

    public void move(int[] room)
    {
        int[] AfterRoom = new int[] { room[0], room[1] };
        List<int[]> passwayNum;
        if (Input.GetKeyDown(KeyCode.UpArrow) || moveDir == "up")
        {
            AfterRoom[0]--;
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow) || moveDir == "left")
        {
            AfterRoom[1]--;
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow) || moveDir == "down")
        {
            AfterRoom[0]++;
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow) || moveDir == "right")
        {
            AfterRoom[1]++;
        }
        //確認有沒有通道、有沒有門
        for(int i = 0; i < maze.GetChild(1).childCount; i++)
        {
            string[] sArray = maze.GetChild(1).GetChild(i).name.Split(new char[2] { '_', ',' });
            passwayNum = new List<int[]>();
            passwayNum.Add(new int[] { int.Parse(sArray[0]), int.Parse(sArray[1]) });
            passwayNum.Add(new int[] { int.Parse(sArray[2]), int.Parse(sArray[3]) });
            //print(passwayNum[0][0] + "," + passwayNum[0][1] + "_" + passwayNum[1][0] + "," + passwayNum[1][1]);
            if ((AfterRoom[0] == passwayNum[0][0] && AfterRoom[1] == passwayNum[0][1] && room[0] == passwayNum[1][0] && room[1] == passwayNum[1][1])
                                        || (room[0] == passwayNum[0][0] && room[1] == passwayNum[0][1] && AfterRoom[0] == passwayNum[1][0] && AfterRoom[1] == passwayNum[1][1]))
            {
                if (maze.GetChild(1).GetChild(i).childCount == 0)
                {
                    pos = new int[] { AfterRoom[0], AfterRoom[1] };
                    transform.position = new Vector3((pos[0] * 2) + 1, 0, pos[1] * 2 + 1);
                    action--;
                    addCanSee(pos);
                    monsterRoar(pos);
                    if (maze.GetChild(0).GetChild(AfterRoom[0] * ((MazeGen.col - 1) / 2) + AfterRoom[1]).GetComponent<Room>().collapse == 2)
                    {
                        if (bullet == 0)
                        {
                            Debug.LogError("died");
                            return;
                        }
                        else
                        {
                            bullet--;
                        }
                        maze.GetChild(0).GetChild(AfterRoom[0] * ((MazeGen.col - 1) / 2) + AfterRoom[1]).GetComponent<Room>().collapse = 3;
                        maze.GetChild(0).GetChild(AfterRoom[0] * ((MazeGen.col - 1) / 2) + AfterRoom[1]).GetComponent<MeshRenderer>().material.color = Color.gray;
                    }
                }
                else if(maze.GetChild(1).GetChild(i).GetChild(0).name == "opened")
                {
                    pos = new int[] { AfterRoom[0], AfterRoom[1] };
                    transform.position = new Vector3((pos[0] * 2) + 1, 0, pos[1] * 2 + 1);
                    action--;
                    addCanSee(pos);
                    monsterRoar(pos);
                    if (maze.GetChild(0).GetChild(AfterRoom[0] * ((MazeGen.col - 1) / 2) + AfterRoom[1]).GetComponent<Room>().collapse == 2)
                    {
                        if (bullet == 0)
                        {
                            Debug.LogError("died");
                            return;
                        }
                        else
                        {
                            bullet--;
                        }
                        maze.GetChild(0).GetChild(AfterRoom[0] * ((MazeGen.col - 1) / 2) + AfterRoom[1]).GetComponent<Room>().collapse = 3;
                        maze.GetChild(0).GetChild(AfterRoom[0] * ((MazeGen.col - 1) / 2) + AfterRoom[1]).GetComponent<MeshRenderer>().material.color = Color.gray;
                    }
                }
                else if (equipment.Contains(maze.GetChild(1).GetChild(i).GetChild(0).name.Split('_')[1]))
                {
                    equipment.Remove(maze.GetChild(1).GetChild(i).GetChild(0).name.Split('_')[1]);
                    maze.GetChild(1).GetChild(i).GetChild(0).name = "opened";
                    maze.GetChild(1).GetChild(i).GetComponent<Passway>().used = true;
                    maze.GetChild(1).GetChild(i).GetComponent<Passway>().canSeeChild.Remove(this);
                    pos = new int[] { AfterRoom[0], AfterRoom[1] };
                    transform.position = new Vector3((pos[0] * 2) + 1, 0, pos[1] * 2 + 1);
                    gameManager.addCollapse(10);
                    action--;
                    addCanSee(pos);
                    monsterRoar(pos);
                    if (maze.GetChild(0).GetChild(AfterRoom[0] * ((MazeGen.col - 1) / 2) + AfterRoom[1]).GetComponent<Room>().collapse == 2)
                    {
                        bullet--;
                        if (bullet < 0)
                        {
                            Debug.LogError("died");
                        }
                        maze.GetChild(0).GetChild(AfterRoom[0] * ((MazeGen.col - 1) / 2) + AfterRoom[1]).GetComponent<Room>().collapse = 3;
                        maze.GetChild(0).GetChild(AfterRoom[0] * ((MazeGen.col - 1) / 2) + AfterRoom[1]).GetComponent<MeshRenderer>().material.color = Color.gray;
                    }
                }
                return;
            }
        }
    }

    public void addCanSee(int[] room)
    {
        Transform temp = maze.GetChild(0).GetChild(room[0] * ((MazeGen.col - 1) / 2) + room[1]);
        //加房間
        if (!temp.GetComponent<Room>().canSee.Contains(this))
        {
            if (temp.GetComponent<Room>().canSee.Count == 0)
            {
                gameManager.addCollapse(2);
            }
            temp.GetComponent<Room>().canSee.Add(this);
        }
        //加通道
        string[] roomArray = temp.name.Split(',');
        for(int j = 0; j < maze.GetChild(1).childCount; j++)
        {
            string[] passwayArray = maze.GetChild(1).GetChild(j).name.Split(new char[] { '_', ',' });
            if ((roomArray[0] == passwayArray[0] && roomArray[1] == passwayArray[1]) || (roomArray[0] == passwayArray[2] && roomArray[1] == passwayArray[3]))
            {
                Passway passway = maze.GetChild(1).GetChild(j).GetComponent<Passway>();
                if (!passway.canSee.Contains(this))
                {
                    passway.canSee.Add(this);
                }
                if (!passway.used)
                {
                    if (!passway.canSeeChild.Contains(this))
                    {
                        passway.canSeeChild.Add(this);
                    }
                }
                else if (passway.canSeeChild.Contains(this))
                {
                    passway.canSeeChild.Remove(this);
                }
            }
        }
        //加四邊
        if (temp.name.Split(',')[0] == "0")
        {
            if (!maze.GetChild(2).GetChild(2).GetComponent<SideWall>().canSee.Contains(this))
            {
                maze.GetChild(2).GetChild(2).GetComponent<SideWall>().canSee.Add(this);
            }
        }
        if (temp.name.Split(',')[0] == "5")
        {
            if (!maze.GetChild(2).GetChild(3).GetComponent<SideWall>().canSee.Contains(this))
            {
                maze.GetChild(2).GetChild(3).GetComponent<SideWall>().canSee.Add(this);
            }
        }
        if (temp.name.Split(',')[1] == "0")
        {
            if (!maze.GetChild(2).GetChild(0).GetComponent<SideWall>().canSee.Contains(this))
            {
                maze.GetChild(2).GetChild(0).GetComponent<SideWall>().canSee.Add(this);
            }
        }
        if (temp.name.Split(',')[1] == "5")
        {
            if (!maze.GetChild(2).GetChild(1).GetComponent<SideWall>().canSee.Contains(this))
            {
                maze.GetChild(2).GetChild(1).GetComponent<SideWall>().canSee.Add(this);
            }
        }

        //加事件
        for (int i = 0; i < temp.childCount; i++)
        {
            temp.GetChild(i).rotation = Quaternion.Euler(new Vector3(0, Random.Range(0, 360), 0));
            if (temp.GetChild(i).name.Split('(')[0] == "Box")
            {
                if (!temp.GetChild(i).GetComponent<Box>().used)
                {
                    if (!temp.GetChild(i).GetComponent<Box>().canSee.Contains(this))
                    {
                        temp.GetChild(i).GetComponent<Box>().canSee.Add(this);
                    }
                }
                else
                {
                    if (temp.GetChild(i).GetComponent<Box>().canSee.Contains(this))
                    {
                        temp.GetChild(i).GetComponent<Box>().canSee.Remove(this);
                    }
                }
            }
            else if (temp.GetChild(i).name.Split('(')[0] == "DeadBody")
            {
                if (!temp.GetChild(i).GetComponent<DeadBody>().used)
                {
                    if (!temp.GetChild(i).GetComponent<DeadBody>().canSee.Contains(this))
                    {
                        temp.GetChild(i).GetComponent<DeadBody>().canSee.Add(this);
                    }
                }
                else
                {
                    if (temp.GetChild(i).GetComponent<DeadBody>().canSee.Contains(this))
                    {
                        temp.GetChild(i).GetComponent<DeadBody>().canSee.Remove(this);
                    }
                }
            }
            else if(temp.GetChild(i).name.Split('(')[0] == "BlueKey" || temp.GetChild(i).name.Split('(')[0] == "RedKey")
            {
                if (!temp.GetChild(i).GetComponent<Key>().used)
                {
                    if (!temp.GetChild(i).GetComponent<Key>().canSee.Contains(this))
                    {
                        temp.GetChild(i).GetComponent<Key>().canSee.Add(this);
                    }
                }
                else
                {
                    if (temp.GetChild(i).GetComponent<Key>().canSee.Contains(this))
                    {
                        temp.GetChild(i).GetComponent<Key>().canSee.Remove(this);
                    }
                }
            }
            else if (temp.GetChild(i).name.Split('(')[0] == "Treasure")
            {
                if (!temp.GetChild(i).GetComponent<Treasure>().used)
                {
                    if (!temp.GetChild(i).GetComponent<Treasure>().canSee.Contains(this))
                    {
                        temp.GetChild(i).GetComponent<Treasure>().canSee.Add(this);
                    }
                }
                else
                {
                    if (temp.GetChild(i).GetComponent<Treasure>().canSee.Contains(this))
                    {
                        temp.GetChild(i).GetComponent<Treasure>().canSee.Remove(this);
                    }
                }
            }
            else if (temp.GetChild(i).name.Split('(')[0] == "Exit")
            {
                if (!temp.GetChild(i).GetComponent<Exit>().canSee.Contains(this))
                {
                    temp.GetChild(i).GetComponent<Exit>().canSee.Add(this);
                }
            }
            else if (temp.GetChild(i).name.Split('(')[0] == "Monster")
            {
                if (!temp.GetChild(i).GetComponent<Monster>().canSee.Contains(this))
                {
                    temp.GetChild(i).GetComponent<Monster>().canSee.Add(this);
                }
            }
        }
    }

    public bool monsterRoar(int[] room)
    {
        bool hurt = false;
        for(int i=0;i< maze.GetChild(0).GetChild(room[0] * ((MazeGen.row - 1) / 2) + room[1]).childCount; i++)
        {
            if (maze.GetChild(0).GetChild(room[0] * ((MazeGen.row - 1) / 2) + room[1]).GetChild(i).name.Split('(')[0] == "Monster")
            {
                if (bullet == 0)
                {
                    if (!died)
                    {
                        Debug.LogError("died");
                        died = true;
                    }
                    hurt = true;
                    break;
                }
                else
                {
                    bullet--;
                    Debug.LogWarning("Hurt");
                    hurt = true;
                }
                Destroy(maze.GetChild(0).GetChild(room[0] * ((MazeGen.row - 1) / 2) + room[1]).GetChild(1).gameObject);
                gameManager.addCollapse(10);
            }
        }

        List<int[]> around = new List<int[]>();
        for(int i = -1; i < 2; i++)
        {
            for(int j = -1; j < 2; j++)
            {
                if (i != 0 || j != 0)
                {
                    around.Add(new int[] { room[0] + i, room[1] + j });
                }
            }
        }
        int monsterNum = 0;
        foreach(int[] roomTemp in around)
        {
            if(!(roomTemp[0]<0 || roomTemp[0] >= (MazeGen.row - 1) / 2 || roomTemp[1] < 0 || roomTemp[1] >= (MazeGen.col - 1) / 2))
            {
                foreach(Transform child in maze.GetChild(0).GetChild(roomTemp[0] * ((MazeGen.row - 1) / 2) + roomTemp[1]))
                {
                    if (child.name.Split('(')[0] == "Monster")
                    {
                        monsterNum++;
                    }
                }
            }
        }
        if(monsterNum != 0)
        {
            Debug.LogWarning("RRRRR * <color=blue>"+ monsterNum + "</color>");
            StartCoroutine("roar", monsterNum);
        }
        return hurt;
    }

    WaitForSeconds oneSecond = new WaitForSeconds(1);
    IEnumerator roar(int monsterNum)
    {
        for(int i = 0; i < monsterNum; i++)
        {
            Camera.main.transform.GetChild(1).GetComponent<AudioSource>().clip = (AudioClip)Resources.Load("Sound/Roar");
            Camera.main.transform.GetChild(1).GetComponent<AudioSource>().Play();
            yield return oneSecond;
        }
    }

    public void useMatch(int[] room)
    {
        int[] AfterRoom = new int[] { room[0], room[1] };
        List<int[]> passwayNum;
        if (Input.GetKeyDown(KeyCode.W) || matchDir == "up")
        {
            AfterRoom[0]--;
        }
        else if (Input.GetKeyDown(KeyCode.A) || matchDir == "left")
        {
            AfterRoom[1]--;
        }
        else if (Input.GetKeyDown(KeyCode.S) || matchDir == "down")
        {
            AfterRoom[0]++;
        }
        else if (Input.GetKeyDown(KeyCode.D) || matchDir == "right")
        {
            AfterRoom[1]++;
        }
        //確認有沒有通道、有沒有門
        //Debug.LogWarning(tempRoom[0] + "," + tempRoom[1] + "_" + tempTempRoom[0] + "," + tempTempRoom[1]);
        for (int i = 0; i < maze.GetChild(1).childCount; i++)
        {
            string[] sArray = maze.GetChild(1).GetChild(i).name.Split(new char[2] { '_', ',' });
            passwayNum = new List<int[]>();
            passwayNum.Add(new int[] { int.Parse(sArray[0]), int.Parse(sArray[1]) });
            passwayNum.Add(new int[] { int.Parse(sArray[2]), int.Parse(sArray[3]) });
            //print(passwayNum[0][0] + "," + passwayNum[0][1] + "_" + passwayNum[1][0] + "," + passwayNum[1][1]);
            if ((AfterRoom[0] == passwayNum[0][0] && AfterRoom[1] == passwayNum[0][1] && room[0] == passwayNum[1][0] && room[1] == passwayNum[1][1])
                                        || (room[0] == passwayNum[0][0] && room[1] == passwayNum[0][1] && AfterRoom[0] == passwayNum[1][0] && AfterRoom[1] == passwayNum[1][1]))
            {
                if (maze.GetChild(1).GetChild(i).childCount == 0)
                {
                    addCanSee(AfterRoom);
                    match--;
                    if (match <= 0 && equipment.Contains("火柴"))
                    {
                        equipment.Remove("火柴");
                    }
                }
                else if (maze.GetChild(1).GetChild(i).GetChild(0).name == "opened")
                {
                    addCanSee(AfterRoom);
                    match--;
                    if (match <= 0 && equipment.Contains("火柴"))
                    {
                        equipment.Remove("火柴");
                    }
                }
                else if(equipment.Contains(maze.GetChild(1).GetChild(i).GetChild(0).name.Split('_')[1]))
                {
                    equipment.Remove(maze.GetChild(1).GetChild(i).GetChild(0).name.Split('_')[1]);
                    maze.GetChild(1).GetChild(i).GetChild(0).name = "opened";
                    maze.GetChild(1).GetChild(i).GetComponent<Passway>().used = true;
                    maze.GetChild(1).GetChild(i).GetComponent<Passway>().canSeeChild.Remove(this);
                    gameManager.addCollapse(10);
                    addCanSee(AfterRoom);
                    match--;
                    if (match <= 0 && equipment.Contains("火柴"))
                    {
                        equipment.Remove("火柴");
                    }
                }
                return;
            }
        }
    }

    public void useTorch(int[] room)
    {
        //移動火把中心至需要的房間(可開門)
        int[] torchCenter = new int[2];
        torchCenter[0] = room[0];
        torchCenter[1] = room[1];
        if ((Input.GetKeyDown(KeyCode.Keypad8) || Input.GetKeyDown(KeyCode.Keypad2) || Input.GetKeyDown(KeyCode.Keypad4) || Input.GetKeyDown(KeyCode.Keypad6)) || (torchDir == "up" || torchDir == "down" || torchDir == "left" || torchDir == "right"))
        {
            if (Input.GetKeyDown(KeyCode.Keypad8) || torchDir == "up")
            {
                torchCenter[0]--;
            }
            else if (Input.GetKeyDown(KeyCode.Keypad4) || torchDir == "left")
            {
                torchCenter[1]--;
            }
            else if (Input.GetKeyDown(KeyCode.Keypad2) || torchDir == "down")
            {
                torchCenter[0]++;
            }
            else if (Input.GetKeyDown(KeyCode.Keypad6) || torchDir == "right")
            {
                torchCenter[1]++;
            }
            //確認有沒有通道、有沒有門
            List<int[]> passwayNum;
            for (int i = 0; i < maze.GetChild(1).childCount; i++)
            {
                string[] sArray = maze.GetChild(1).GetChild(i).name.Split(new char[2] { '_', ',' });
                passwayNum = new List<int[]>();
                passwayNum.Add(new int[] { int.Parse(sArray[0]), int.Parse(sArray[1]) });
                passwayNum.Add(new int[] { int.Parse(sArray[2]), int.Parse(sArray[3]) });
                if ((torchCenter[0] == passwayNum[0][0] && torchCenter[1] == passwayNum[0][1] && room[0] == passwayNum[1][0] && room[1] == passwayNum[1][1])
                                            || (room[0] == passwayNum[0][0] && room[1] == passwayNum[0][1] && torchCenter[0] == passwayNum[1][0] && torchCenter[1] == passwayNum[1][1]))
                {
                    if (maze.GetChild(1).GetChild(i).childCount == 0)
                    {

                    }
                    else if (maze.GetChild(1).GetChild(i).GetChild(0).name == "opened")
                    {

                    }
                    else if (equipment.Contains(maze.GetChild(1).GetChild(i).GetChild(0).name.Split('_')[1]))
                    {
                        equipment.Remove(maze.GetChild(1).GetChild(i).GetChild(0).name.Split('_')[1]);
                        maze.GetChild(1).GetChild(i).GetChild(0).name = "opened";
                        maze.GetChild(1).GetChild(i).GetComponent<Passway>().used = true;
                        maze.GetChild(1).GetChild(i).GetComponent<Passway>().canSeeChild.Remove(this);
                        gameManager.addCollapse(10);
                    }
                    else
                    {
                        return;
                    }
                    addCanSee(torchCenter);
                    break;
                }
            }
        }

        //若火把中心無誤則扣1火把
        if (--torch <= 0)
        {
            equipment.Remove("火把");
        }

        //從火把中心找相鄰的區域照亮(不可開門)
        for (int l = 0; l < 4; l++)
        {
            int[] lightRange = new int[] { torchCenter[0], torchCenter[1] };
            List<int[]> passwayNum;
            if (l == 0)
            {
                lightRange[0]--;
            }
            else if (l == 1)
            {
                lightRange[1]--;
            }
            else if (l == 2)
            {
                lightRange[0]++;
            }
            else if (l == 3)
            {
                lightRange[1]++;
            }
            //確認有沒有通道、有沒有門
            //Debug.LogWarning(tempRoom[0] + "," + tempRoom[1] + "_" + tempTempRoom[0] + "," + tempTempRoom[1]);
            for (int i = 0; i < maze.GetChild(1).childCount; i++)
            {
                string[] sArray = maze.GetChild(1).GetChild(i).name.Split(new char[2] { '_', ',' });
                passwayNum = new List<int[]>();
                passwayNum.Add(new int[] { int.Parse(sArray[0]), int.Parse(sArray[1]) });
                passwayNum.Add(new int[] { int.Parse(sArray[2]), int.Parse(sArray[3]) });
                //print(passwayNum[0][0] + "," + passwayNum[0][1] + "_" + passwayNum[1][0] + "," + passwayNum[1][1]);
                if ((lightRange[0] == passwayNum[0][0] && lightRange[1] == passwayNum[0][1] && torchCenter[0] == passwayNum[1][0] && torchCenter[1] == passwayNum[1][1])
                                            || (torchCenter[0] == passwayNum[0][0] && torchCenter[1] == passwayNum[0][1] && lightRange[0] == passwayNum[1][0] && lightRange[1] == passwayNum[1][1]))
                {
                    if (maze.GetChild(1).GetChild(i).childCount == 0)
                    {
                        addCanSee(lightRange);
                    }
                    break;
                }
            }
        }
    }

    public void rest()
    {
        scared -= restScared;
        if (scared < 0)
        {
            scared = 0;
        }
    }
}
