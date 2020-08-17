﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour
{
    GameManager gameManager;
    public bool used = false;
    public List<PlayerManager> canSee = new List<PlayerManager>();
    Transform playerManagers;

    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
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
                        transform.GetChild(0).GetComponent<MeshRenderer>().enabled = true;
                        transform.GetComponent<Collider>().enabled = true;
                        CanSee = true;
                        break;
                    }
                }
                if (!CanSee)
                {
                    transform.GetChild(0).GetComponent<MeshRenderer>().enabled = false;
                    transform.GetComponent<Collider>().enabled = false;
                }
                break;
            }
        }
    }
    private void OnMouseDown()
    {
        for (int i = 0; i < playerManagers.childCount; i++)
        {
            if (playerManagers.GetChild(i).GetComponent<PlayerManager>().enabled == true)
            {
                if (playerManagers.GetChild(i).GetComponent<PlayerManager>().action > 0 && playerManagers.GetChild(i).GetComponent<PlayerManager>().equipment.Count < playerManagers.GetChild(i).GetComponent<PlayerManager>().heavyBurden)
                {
                    playerManagers.GetChild(i).GetComponent<PlayerManager>().action--;
                    used = true;
                    canSee.Remove(playerManagers.GetChild(i).GetComponent<PlayerManager>());
                    transform.GetChild(0).GetComponent<MeshRenderer>().enabled = false;
                    transform.GetComponent<Collider>().enabled = false;
                    playerManagers.GetChild(i).GetComponent<PlayerManager>().equipment.Add(gameObject.name.Split('K')[0]);
                    Debug.LogWarning("鑰匙 : " + gameObject.name.Split('K')[0]);
                    gameManager.addCollapse(5);
                }
                else 
                {
                    if (playerManagers.GetChild(i).GetComponent<PlayerManager>().action <= 0)
                    {
                        Debug.LogError("沒行動了");
                    }
                    if (playerManagers.GetChild(i).GetComponent<PlayerManager>().equipment.Count >= playerManagers.GetChild(i).GetComponent<PlayerManager>().heavyBurden)
                    {
                        Debug.LogError("包包滿了");
                    }
                }
                break;
            }
        }
    }
}
