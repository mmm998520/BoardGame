using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    [HideInInspector] public bool collapseButton = false;

    public int collapse = 0;
    public List<PlayerManager> canSee = new List<PlayerManager>();
    Transform playerManagers, maze;

    void Start()
    {
        playerManagers = GameObject.Find("Players").transform;
        maze = GameObject.Find("MazeGen").transform;
    }

    void Update()
    {
        updateView();
        Collapse();
        collapseButton = false;
    }

    private void LateUpdate()
    {
        lateCollapse();
    }

    void Collapse()
    {
        if ((Input.GetKeyDown(KeyCode.C) || collapseButton) && collapse >= 2)
        {
            string[] sArray = gameObject.name.Split(',');
            int[] room = new int[] { int.Parse(sArray[0]), int.Parse(sArray[1]) };
            if ((room[0] - 1) >= 0 && (room[0] - 1) < (MazeGen.row - 1) / 2)
            {
                if (maze.GetChild(0).GetChild((room[0] - 1) * ((MazeGen.col - 1) / 2) + room[1]).GetComponent<Room>().collapse == 0)
                {
                    maze.GetChild(0).GetChild((room[0] - 1) * ((MazeGen.col - 1) / 2) + room[1]).GetComponent<Room>().collapse = 1;
                }
            }
            if ((room[0] + 1) >= 0 && (room[0] + 1) < (MazeGen.row - 1) / 2)
            {
                if (maze.GetChild(0).GetChild((room[0] + 1) * ((MazeGen.col - 1) / 2) + room[1]).GetComponent<Room>().collapse == 0)
                {
                    maze.GetChild(0).GetChild((room[0] + 1) * ((MazeGen.col - 1) / 2) + room[1]).GetComponent<Room>().collapse = 1;
                }
            }
            if ((room[1] - 1) >= 0 && (room[1] - 1) < (MazeGen.col - 1) / 2)
            {
                if (maze.GetChild(0).GetChild(room[0] * ((MazeGen.col - 1) / 2) + (room[1] - 1)).GetComponent<Room>().collapse == 0)
                {
                    maze.GetChild(0).GetChild(room[0] * ((MazeGen.col - 1) / 2) + (room[1] - 1)).GetComponent<Room>().collapse = 1;
                }
            }
            if ((room[1] + 1) >= 0 && (room[1] + 1) < (MazeGen.col - 1) / 2)
            {
                if (maze.GetChild(0).GetChild(room[0] * ((MazeGen.col - 1) / 2) + (room[1] + 1)).GetComponent<Room>().collapse == 0)
                {
                    maze.GetChild(0).GetChild(room[0] * ((MazeGen.col - 1) / 2) + (room[1] + 1)).GetComponent<Room>().collapse = 1;
                }
            }
        }
    }

    void lateCollapse()
    {
        if (collapse == 1)
        {
            collapse = 2;
            gameObject.GetComponent<MeshRenderer>().material.color = Color.black;
        }
        if (collapse == 2)
        {
            for (int i = 0; i < playerManagers.childCount; i++)
            {
                string[] sArray = gameObject.name.Split(',');
                int[] room = new int[] { int.Parse(sArray[0]), int.Parse(sArray[1]) };
                PlayerManager playerManager = playerManagers.GetChild(i).GetComponent<PlayerManager>();
                if (room[0] == playerManager.pos[0] && room[1] == playerManager.pos[1])
                {
                    
                    if (playerManager.bullet == 0)
                    {
                        if (!playerManager.died)
                        {
                            Debug.LogError(playerManagers.GetChild(i).name + " died");
                            playerManager.died = true;
                            Camera.main.transform.GetChild(2).GetComponent<AudioSource>().clip = (AudioClip)Resources.Load("Sound/Hurt");
                            Camera.main.transform.GetChild(2).GetComponent<AudioSource>().Play();
                        }
                    }
                    else
                    {
                        playerManager.bullet--;
                        collapse = 3;
                        gameObject.GetComponent<MeshRenderer>().material.color = new Color(0.55f,0.55f,0.55f,1);
                        Camera.main.transform.GetChild(2).GetComponent<AudioSource>().clip = (AudioClip)Resources.Load("Sound/Hurt");
                        Camera.main.transform.GetChild(2).GetComponent<AudioSource>().Play();
                        print("a");
                    }
                }
            }
        }
    }

    void updateView()
    {
        for (int i = 0; i < playerManagers.childCount; i++)
        {
            if (playerManagers.GetChild(i).GetComponent<PlayerManager>().enabled == true)
            {
                bool CanSee = false;
                for (int j = 0; j < canSee.Count; j++)
                {
                    if (playerManagers.GetChild(i).GetComponent<PlayerManager>() == canSee[j])
                    {
                        transform.GetComponent<MeshRenderer>().enabled = true;
                        CanSee = true;
                        break;
                    }
                }
                if (!CanSee)
                {
                    transform.GetComponent<MeshRenderer>().enabled = false;
                }
                break;
            }
        }
    }
    
}
