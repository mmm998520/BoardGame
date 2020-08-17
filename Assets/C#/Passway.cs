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
            PlayerManager playerManager = playerManagers.GetChild(i).GetComponent<PlayerManager>();
            if (playerManager.enabled == true)
            {
                MeshRenderer meshRenderer = transform.GetComponent<MeshRenderer>();
                if (canSee.Contains(playerManager))
                {
                    meshRenderer.enabled = true;
                    if (canSeeChild.Contains(playerManager))
                    {
                        if (transform.childCount > 0)
                        {
                            transform.GetChild(0).GetComponent<MeshRenderer>().enabled = true;
                        }
                    }
                    else
                    {
                        if (transform.childCount > 0)
                        {
                            transform.GetChild(0).GetComponent<MeshRenderer>().enabled = false;
                        }
                    }
                }
                else
                {
                    meshRenderer.enabled = false;
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
