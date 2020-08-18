using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonManager : MonoBehaviour
{
    Transform players,maze;
    GameManager gameManager;
    public string cameraDir = "";

    void Start()
    {
        players = GameObject.Find("Players").transform;
        maze = GameObject.Find("MazeGen").transform;
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            cameraDir = "";
        }
        else if(cameraDir == "up")
        {
            Camera.main.transform.Translate(Vector3.up * Time.deltaTime * 10);
        }
        else if (cameraDir == "down")
        {
            Camera.main.transform.Translate(Vector3.down * Time.deltaTime * 10);
        }
        else if (cameraDir == "left")
        {
            Camera.main.transform.Translate(Vector3.left * Time.deltaTime * 10);
        }
        else if (cameraDir == "right")
        {
            Camera.main.transform.Translate(Vector3.right * Time.deltaTime * 10);
        }
    }

    public void moveButton(string dir)
    {
        for(int i = 0; i < players.childCount; i++)
        {
            PlayerManager playerManager = players.GetChild(i).GetComponent<PlayerManager>();
            if (playerManager.enabled == true)
            {
                playerManager.moveDir = dir;
                if (playerManager.action > 0)
                {
                    playerManager.move(playerManager.pos);
                }
                playerManager.moveDir = "";
                return;
            }
        }
    }

    public void restButton()
    {
        for (int i = 0; i < players.childCount; i++)
        {
            PlayerManager playerManager = players.GetChild(i).GetComponent<PlayerManager>();
            if (playerManager.enabled == true)
            {
                if (playerManager.action >= playerManager.actionMax / 2)
                {
                    playerManager.action -= playerManager.actionMax / 2;
                    playerManager.rest();
                }
                return;
            }
        }
    }

    public void matchButton(string dir)
    {
        for (int i = 0; i < players.childCount; i++)
        {
            PlayerManager playerManager = players.GetChild(i).GetComponent<PlayerManager>();
            if (playerManager.enabled == true)
            {
                playerManager.matchDir = dir;
                if (playerManager.match > 0)
                {
                    playerManager.useMatch(playerManager.pos);
                }
                playerManager.matchDir = "";
                return;
            }
        }
    }

    public void torchButton(string dir)
    {
        for (int i = 0; i < players.childCount; i++)
        {
            PlayerManager playerManager = players.GetChild(i).GetComponent<PlayerManager>();
            if (playerManager.enabled == true)
            {
                playerManager.torchDir = dir;
                if (playerManager.torch > 0 && playerManager.torchDir != "")
                {
                    playerManager.useTorch(playerManager.pos);
                }
                playerManager.torchDir = "";
                return;
            }
        }
    }

    public void abilityButton(string dir)
    {
        for (int i = 0; i < players.childCount; i++)
        {
            PlayerManager playerManager = players.GetChild(i).GetComponent<PlayerManager>();
            if (playerManager.enabled == true)
            {
                if (players.GetChild(i).GetComponent<AbilityBreakWall>())
                {
                    players.GetChild(i).GetComponent<AbilityBreakWall>().abilityDir = dir;
                }
                else if (players.GetChild(i).GetComponent<AbilityDash>())
                {
                    players.GetChild(i).GetComponent<AbilityDash>().abilityDir = dir;
                }
                else if (players.GetChild(i).GetComponent<AbilityNavigation>())
                {
                    players.GetChild(i).GetComponent<AbilityNavigation>().abilityDir = dir;
                }
                return;
            }
        }
    }

    public void switchPlayerButton()
    {
        gameManager.switchPlayer();
    }

    public void collapseButton()
    {
        if (gameManager.collapse < 100)
        {
            gameManager.addCollapse(15);
        }
        for(int i = 0; i < maze.GetChild(0).childCount; i++)
        {
            maze.GetChild(0).GetChild(i).GetComponent<Room>().collapseButton = true;
        }
    }
}
