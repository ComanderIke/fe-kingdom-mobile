using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Game.GameActors.Units;
using Game.GameActors.Units.OnGameObject;
using Game.GameInput;
using Game.GUI;
using Game.Manager;
using GameCamera;
using UnityEngine;
using UnityEngine.EventSystems;

[ExecuteInEditMode]
public class UIUnitPlacement : IUnitPlacementUI
{
    [SerializeField]
    private GameObject unitPrefab;
    [SerializeField]
    private Camera camera;
    [SerializeField]
    private Transform layoutGroup;
    private RaycastManager RaycastManager { get; set; }
    [SerializeField]
    private List<Unit> units;

    public GameObject unitFieldAnimationPrefab;

    private bool dragInitiated;
    private bool dragStarted;
    // Update is called once per frame
 

    private Canvas canvas;
    void Start()
    {
        canvas = GetComponent<Canvas>();
    }

    private void OnEnable()
    {
        RaycastManager = new RaycastManager();
        UpdateValues();
    }

    private void UpdateValues()
    {
        if(units!=null)
            foreach (Unit u in units)
            {
                var go = Instantiate(unitPrefab, layoutGroup, false);
                go.GetComponent<UIUnitDragObject>().UnitPlacement = this;
            }
    }

    public void StartClicked()
    {
        OnFinished?.Invoke();
        Hide();
    }

    public override void Show(List<Unit> units)
    {
        this.units = units;
        UpdateValues();
        GetComponent<Canvas>().enabled = true;
        
    }
    
    public override void Hide()
    {
        GetComponent<Canvas>().enabled = false;
    }
    // public override void OnDrag(UIUnitDragObject uiUnitDragObject, PointerEventData eventData)
    // {
    //     uiUnitDragObject.rectTransform.anchoredPosition += eventData.delta/canvas.scaleFactor;
    // }

    // public override void OnEndDrag(UIUnitDragObject uiUnitDragObject, PointerEventData eventData)
    // {
    //
    //     Debug.Log(eventData.position);
    //
    //     var screenRay = Camera.main.ScreenPointToRay(eventData.position);
    //     // Perform Physics2D.GetRayIntersection from transform and see if any 2D object was under transform.position on drop.
    //     RaycastHit2D hit2D = Physics2D.GetRayIntersection(screenRay);
    //     if (hit2D)
    //     {
    //         Debug.Log(hit2D.transform.gameObject.name);
    //         var dropComponent = hit2D.transform.gameObject.GetComponent<IDropHandler>();
    //         if (dropComponent != null)
    //             dropComponent.OnDrop(eventData);
    //     }
    // }

  
}
