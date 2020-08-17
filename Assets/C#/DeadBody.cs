using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadBody : MonoBehaviour
{
    public int startled,bullet, expansion, match;
    public bool used = false;
    public List<PlayerManager> canSee = new List<PlayerManager>();
    public List<PlayerManager> canSeeChild = new List<PlayerManager>();
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
                MeshRenderer meshRenderer = transform.GetChild(0).GetComponent<MeshRenderer>();
                Collider collider = transform.GetComponent<Collider>();
                SpriteRenderer crossSpriteRenderer = transform.GetChild(1).GetComponent<SpriteRenderer>();
                if (canSee.Contains(playerManager))
                {
                    meshRenderer.enabled = true;
                    collider.enabled = true;
                    if (canSeeChild.Contains(playerManager))
                    {
                        crossSpriteRenderer.enabled = true;
                        collider.enabled = false;
                    }
                    else
                    {
                        crossSpriteRenderer.enabled = false;
                        collider.enabled = true;
                    }
                }
                else
                {
                    meshRenderer.enabled = false;
                    crossSpriteRenderer.enabled = false;
                    collider.enabled = false;
                }
                break;
            }
        }
    }

    private void OnMouseDown()
    {
        for (int i = 0; i < playerManagers.childCount; i++)
        {
            PlayerManager playerManager = playerManagers.GetChild(i).GetComponent<PlayerManager>();
            if (playerManager.enabled == true)
            {
                if (playerManager.action > 0 && playerManager.equipment.Count < playerManager.heavyBurden)
                {
                    playerManager.action--;
                    used = true;
                    canSeeChild.Add(playerManager);
                    if (match != 0)
                    {
                        if (!playerManager.equipment.Contains("火柴"))
                        {
                            playerManager.equipment.Add("火柴");
                        }
                    }
                    playerManager.scared += startled;
                    playerManager.bullet += bullet;
                    playerManager.heavyBurden += expansion;
                    playerManager.match += match;
                    Debug.LogWarning("被嚇 : " + startled + " , 子彈 : " + bullet + " , 擴充 : " + expansion + " , 火柴 : " + match);
                }
                else
                {
                    if (playerManager.action <= 0)
                    {
                        Debug.LogError("沒行動了");
                    }
                    if (playerManager.equipment.Count >= playerManager.heavyBurden)
                    {
                        Debug.LogError("包包滿了");
                    }
                }
                break;
            }
        }
    }
}
