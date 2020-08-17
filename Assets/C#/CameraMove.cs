using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class CameraMove : MonoBehaviour, IPointerDownHandler
{
    ButtonManager buttonManager;
    public string dir = "";
    void Start()
    {
        buttonManager = GameObject.Find("ButtonManager").GetComponent<ButtonManager>();
    }

    void Update()
    {
        
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        buttonManager.cameraDir = dir;
    }
}
