using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeGen : MonoBehaviour
{
    public GameObject gameManager;
    public Transform players;
    public  const int row = 6*2+1, col = 6*2+1, fill = 13;
    MazeCreate mazeCreate;
    void Awake()
    {
        mazeCreate = MazeCreate.GetMaze(row, col);

        //在基礎迷宮上額外做通道
        int[] filling = new int [fill];
        for(int i = 0; i < fill; i++)
        {
            filling[i] = Random.Range(0, row * col);
            for(int j = 0; j < i; j++)
            {
                if (filling[i] == filling[j])
                {
                    i--;
                    break;
                }
            }
            if(mazeCreate.mapList[filling[i] / col][filling[i] % col] == (int)MazeCreate.PointType.way || mazeCreate.mapList[filling[i] / col][filling[i] % col] == (int)MazeCreate.PointType.startpoint || filling[i] / col >= row - 1 || filling[i] / col <= 0 || filling[i] % col >= col - 1 || filling[i] % col <= 0 || (filling[i] / col % 2 == 0 && filling[i] % col % 2 == 0))
            {
                i--;
            }
        }

        for (int i = 0; i < fill; i++)
        {
            mazeCreate.mapList[filling[i] / col][filling[i] % col] = (int)MazeCreate.PointType.way;
        }
        //建立房間清單
        int _i = 0, _j = 0;
        List<List<GameObject>> rooms = new List<List<GameObject>>();
        rooms.Add(new List<GameObject>());
        //建立通道清單
        List<List<int[]>> passways = new List<List<int[]>>();
        //建構方塊
        for (int i = 0; i < row; i++)
        {
            for (int j = 0; j < col; j++)
            {
                //在能行走區域做方塊
                if ((mazeCreate.mapList[i][j] == (int)MazeCreate.PointType.startpoint || mazeCreate.mapList[i][j] == (int)MazeCreate.PointType.way) && !(i % 2 == 0 && j % 2 == 0))
                {
                    GameObject column = (GameObject)Resources.Load("Prefabs/maze");
                    column = MonoBehaviour.Instantiate(column);
                    column.transform.position = new Vector3(i, 0, j);

                    //起始點標記
                    if (mazeCreate.mapList[i][j] == (int)MazeCreate.PointType.startpoint)
                    {
                        column.transform.localScale *= 1.5f;
                        column.transform.parent = transform.GetChild(0);
                        //加入房間清單
                        rooms[_i].Add(column);
                        column.name = _i + "," + _j;
                        column.AddComponent<Room>();
                        _j++;
                    }
                    //通道標記
                    else if (i % 2 == 0 || j % 2 == 0)
                    {
                        column.GetComponent<MeshRenderer>().material.color = Color.green;
                        column.transform.parent = transform.GetChild(1);
                        //將通道兩端房間代號加入通道清單
                        int[] numberOne;
                        int[] numberTwo;
                        if (i % 2 == 1)
                        {
                            numberOne = new int[] { (i - 1) / 2, (j / 2) - 1 };
                            numberTwo = new int[] { (i - 1) / 2, j / 2 };
                        }
                        else
                        {
                            numberOne = new int[] { (i / 2) - 1, (j - 1) / 2 };
                            numberTwo = new int[] { i / 2, (j - 1) / 2 };
                        }
                        List<int[]> passway = new List<int[]>();
                        passway.Add(numberOne);
                        passway.Add(numberTwo);
                        passways.Add(passway);
                        column.name = numberOne[0] + "," + numberOne[1] + "_" + numberTwo[0] + "," + numberTwo[1];
                        column.AddComponent<Passway>();
                        //column.name = "_" + i + "_" + j;
                    }
                    //房間標記
                    else
                    {
                        column.transform.localScale *= 1.5f;
                        column.transform.parent = transform.GetChild(0);
                        //加入房間清單
                        rooms[_i].Add(column);
                        column.name = _i + "," + _j;
                        column.AddComponent<Room>();
                        _j++;
                    }

                    //房間清單換行
                    if (_j >= (col - 1) / 2)
                    {
                        rooms.Add(new List<GameObject>());
                        _j = 0;
                        _i++;
                    }
                }
                //處理特例
                else if(i % 2 == 0 && j % 2 == 0)
                {
                    mazeCreate.mapList[i][j] = (int)MazeCreate.PointType.wall;
                }
            }
        }
        //print("size"+passways.Count);

        gameManager.GetComponent<GameManager>().rooms = rooms;
    }
}
