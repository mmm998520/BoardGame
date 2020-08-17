using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scoreboard : MonoBehaviour
{
    Transform players,gameManager;
    public List<List<List<int>>> scoreboard = new List<List<List<int>>>();
    public List<int> collapses = new List<int>();

    void Start()
    {
        players = GameObject.Find("Players").transform;
        gameManager = GameObject.Find("GameManager").transform;
        for (int i = 0; i < players.childCount; i++)
        {
            List < List<int>> playerScore = new List<List<int>>();
            List<int> scared = new List<int>();
            List<int> money = new List<int>();
            playerScore.Add(scared);
            playerScore.Add(money);
            scoreboard.Add(playerScore);

            Transform player = Instantiate(new GameObject("player"+i)).transform;
            Transform ScaredLine = Instantiate((GameObject)Resources.Load("Prefabs/ScaredLine")).transform;
            Transform MoneyLine = Instantiate((GameObject)Resources.Load("Prefabs/MoneyLine")).transform;
            player.parent = transform;
            ScaredLine.parent = player;
            MoneyLine.parent = player;
            player.localPosition = Vector3.zero;
        }
        Transform CollapseLine = Instantiate((GameObject)Resources.Load("Prefabs/CollapseLine")).transform;
        CollapseLine.parent = transform;
        CollapseLine.localPosition = Vector3.zero;

        Transform Coordinate = Instantiate((GameObject)Resources.Load("Prefabs/Coordinate")).transform;
        Coordinate.parent = transform;
        Coordinate.localPosition = Vector3.zero;
    }

    public void updateScoreboard()
    {
        for (int i = 0; i < players.childCount; i++)
        {
            scoreboard[i][0].Add(players.GetChild(i).GetComponent<PlayerManager>().scared);
            int money = 0;
            for(int j=0; j < players.GetChild(i).GetComponent<PlayerManager>().equipment.Count; j++)
            {
                if (players.GetChild(i).GetComponent<PlayerManager>().equipment[j].Split(' ')[0] == "錢")
                {
                    money += int.Parse(players.GetChild(i).GetComponent<PlayerManager>().equipment[j].Split(' ')[2]);
                }
            }
            scoreboard[i][1].Add(money);

            LineRenderer ScaredLine = transform.GetChild(i).GetChild(0).GetComponent<LineRenderer>();
            ScaredLine.SetPosition(++ScaredLine.positionCount - 1, new Vector3(-scoreboard[i][0][scoreboard[i][0].Count - 1] * 0.1f, 0, ScaredLine.positionCount));
            LineRenderer MoneyLine = transform.GetChild(i).GetChild(1).GetComponent<LineRenderer>();
            MoneyLine.SetPosition(++MoneyLine.positionCount - 1, new Vector3(-scoreboard[i][1][scoreboard[i][1].Count - 1] * 0.1f, 0, MoneyLine.positionCount));
        }

        collapses.Add(gameManager.GetComponent<GameManager>().collapse);
        LineRenderer CollapseLine = transform.GetChild(players.childCount).GetComponent<LineRenderer>();
        CollapseLine.SetPosition(++CollapseLine.positionCount - 1, new Vector3(-collapses[collapses.Count-1] * 0.1f, 0, CollapseLine.positionCount));/**/
    }
}
