using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Dereliction : MonoBehaviour
{
    GameManager gameManager;
    public string dereliction = "";
    public bool used = false;
    public List<PlayerManager> canSee = new List<PlayerManager>();
    public List<PlayerManager> canSeeChild = new List<PlayerManager>();
    Transform playerManagers;
    public int match;
    public int torch;

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
            PlayerManager playerManager = playerManagers.GetChild(i).GetComponent<PlayerManager>();
            if (playerManager.enabled == true)
            {
                TextMeshPro textMeshPro = transform.GetChild(1).GetComponent<TextMeshPro>();
                Collider collider = transform.GetComponent<Collider>();
                SpriteRenderer crossSpriteRenderer = transform.GetChild(2).GetComponent<SpriteRenderer>();
                if (canSee.Contains(playerManager))
                {
                    textMeshPro.enabled = true;
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
                    textMeshPro.enabled = false;
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
                    playerManager.equipment.Add(dereliction);
                    if(dereliction == "火柴")
                    {
                        playerManager.match += match;
                    }
                    else if (dereliction == "火把")
                    {
                        playerManager.torch += torch;
                    }
                    Debug.LogWarning(dereliction);
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
