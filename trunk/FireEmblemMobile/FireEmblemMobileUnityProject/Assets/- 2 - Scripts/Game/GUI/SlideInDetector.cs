using System;
using System.Collections;
using System.Collections.Generic;
using GameCamera;
using GameEngine.Input;
using GameEngine.Tools;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.PlayerLoop;

public class SlideInDetector : MonoBehaviour, IPointerDownHandler 
{
    private IDragPerformer DragPerformer { get; set; }
    private IRayProvider RayProvider { get; set; }
    private ICameraInputProvider CameraInputProvider { get; set; }

    private RectTransform rectT;
    public bool isOpen;
    private Vector3 originPos;
    private Vector3 openPos;
    public GameObject SideBarButton;
    public void Start()
    {
        DragPerformer = new UIDragPerformer(0.01f);
        CameraInputProvider = new MouseCameraInputProvider();
        RayProvider = new ScreenPointToRayProvider(Camera.main);
        rectT = GetComponent<RectTransform>();
        originPos=rectT.anchoredPosition;
        openPos = new Vector3(originPos.x - rectT.rect.width, originPos.y, originPos.z);
    }

    public void ButtonClicked()
    {
        isOpen = !isOpen;
        if (isOpen)
        {
            Open();
        }
        else
        {
            Close();
        }
    }
    void Update()
    {
        if (CameraInputProvider.InputPressed())
        {
            DragPerformer.Drag(transform, CameraInputProvider.InputPosition());
            if (rectT.anchoredPosition.x - originPos.x >= 0)
            {
                rectT.anchoredPosition = originPos;
            }
            else if (originPos.x - rectT.anchoredPosition.x > rectT.rect.width)
            {
                rectT.anchoredPosition = openPos;
            }
        }


        if (CameraInputProvider.InputPressedUp())
        {
            DragPerformer.EndDrag(transform);
            if (rectT.anchoredPosition.x - originPos.x > -rectT.rect.width/3f*1f)
            {
                Close();
            }
            else if (originPos.x - rectT.anchoredPosition.x > rectT.rect.width/3f*2f)
            {
                Open();
           
            }
            rectT.anchoredPosition = isOpen ? openPos : originPos;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        DragPerformer.StartDrag(transform, CameraInputProvider.InputPosition());
    }
    public void Open()
    {
        rectT.anchoredPosition = openPos;
        isOpen = true;
        SideBarButton.transform.localScale = new Vector3(1, 1, 1);
    }

    public void Close()
    {
        rectT.anchoredPosition = originPos;
        isOpen = false;
        SideBarButton.transform.localScale = new Vector3(-1, 1, 1);
    }
}

public class UIDragPerformer : IDragPerformer
{
    private float dragSpeed;
    private Vector3 lastPosition;
    private bool startedDrag;
    private Vector3 startPos;
    private const float DISTANCE_TILL_DRAG = 0.15f;

    
    public UIDragPerformer(float dragSpeed)
    {
        this.dragSpeed = dragSpeed;
    }
    public void Drag(Transform transform, Vector3 dragDestination)
    {
       
        if (!startedDrag)
            return;
        // Debug.Log("Drag");
        if (!IsDragging && Vector3.Distance((startPos),  dragDestination) >= DISTANCE_TILL_DRAG)
        {
            IsDragging = true;
        }
      
        var delta = dragDestination - lastPosition; 
        //Debug.Log(startPos+" "+dragDestination+" "+delta);
        lastPosition = dragDestination;
        transform.Translate(delta.x * dragSpeed, 0, 0, Space.Self);
        
    }

    public void StartDrag(Transform transform, Vector3 dragDestination)
    {
        lastPosition = startPos = dragDestination;
        startedDrag = true;
        IsDragging = false;
    }

    
    public void EndDrag(Transform transform)
    
    {
        if (!startedDrag)
            return;
        //Debug.Log("EndDrag");
      
        startedDrag = false;
        IsDragging = false;
        startPos = Vector3.zero;

    }

    public bool IsDragging { get; set; }
}
