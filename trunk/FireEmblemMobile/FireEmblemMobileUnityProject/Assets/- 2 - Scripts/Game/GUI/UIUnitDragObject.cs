using System.Collections;
using System.Collections.Generic;
using Game.GUI;
using GameEngine.Tools;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIUnitDragObject : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    
    public IUnitPlacementUI UnitPlacement { get; set; }

    public RectTransform rectTransform;
    private CanvasGroup canvasGroup;

    
    // Start is called before the first frame update
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
    }
    

    public void OnDrag(PointerEventData eventData)
    {
        
        //UnitPlacement.OnDrag(this, eventData);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        //canvasGroup.blocksRaycasts = false;
        //UnitPlacement.OnBeginDrag(this, eventData);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
       // UnitPlacement.OnEndDrag(this, eventData);
        //canvasGroup.blocksRaycasts = true;
    }
}
