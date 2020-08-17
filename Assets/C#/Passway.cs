using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Passway : MonoBehaviour
{
    public List<PlayerManager> canSee = new List<PlayerManager>();
    public List<PlayerManager> canSeeChild = new List<PlayerManager>();
    public bool used = false;
    Transform playerManagers;

    void Start()
    {
        playerManagers = GameObject.Find("Players").transform;
    }

    void Update()
    {
        updateView();
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
                if (CanSee)
                {
                    bool CanSeeChild = false;
                    for (int j = 0; j < canSeeChild.Count; j++)
                    {
                        if (playerManagers.GetChild(i).GetComponent<PlayerManager>() == canSeeChild[j])
                        {
                            if (transform.childCount > 0)
                            {
                                transform.GetChild(0).GetComponent<MeshRenderer>().enabled = true;
                                CanSeeChild = true;
                            }
                            break;
                        }
                    }
                    if (!CanSeeChild && transform.childCount > 0)
                    {
                        transform.GetChild(0).GetComponent<MeshRenderer>().enabled = false;
                    }
                }
                else
                {
                    transform.GetComponent<MeshRenderer>().enabled = false;
                    if (transform.childCount > 0)
                    {
                        transform.GetChild(0).GetComponent<MeshRenderer>().enabled = false;
                    }
                }
                break;
            }
        }
    }
}
