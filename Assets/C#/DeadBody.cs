using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadBody : MonoBehaviour
{
    public int startled,bullet, expansion, match;
    public bool used = false;
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
                for(int j = 0; j < canSee.Count; j++)
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
                    if (match != 0)
                    {
                        if (!playerManagers.GetChild(i).GetComponent<PlayerManager>().equipment.Contains("火柴"))
                        {
                            playerManagers.GetChild(i).GetComponent<PlayerManager>().equipment.Add("火柴");
                        }
                    }
                    playerManagers.GetChild(i).GetComponent<PlayerManager>().scared += startled;
                    playerManagers.GetChild(i).GetComponent<PlayerManager>().bullet += bullet;
                    playerManagers.GetChild(i).GetComponent<PlayerManager>().heavyBurden += expansion;
                    playerManagers.GetChild(i).GetComponent<PlayerManager>().match += match;
                    Debug.LogWarning("被嚇 : " + startled + " , 子彈 : " + bullet + " , 擴充 : " + expansion + " , 火柴 : " + match);
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
