using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exit : MonoBehaviour
{
    public List<PlayerManager> canSee = new List<PlayerManager>();
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
}
