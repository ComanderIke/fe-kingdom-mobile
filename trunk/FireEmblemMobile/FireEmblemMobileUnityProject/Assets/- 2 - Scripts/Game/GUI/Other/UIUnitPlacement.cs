using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Game.GameActors.Units;
using Game.GameActors.Units.OnGameObject;
using Game.GameInput;
using Game.Grid;
using Game.GUI;
using Game.Manager;
using Game.WorldMapStuff.Controller;
using GameCamera;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[ExecuteInEditMode]
public class UIUnitPlacement : IUnitPlacementUI
{
    [SerializeField]
    private GameObject unitPrefab;
    // [SerializeField]
    // private Camera camera;
    private RaycastManager RaycastManager { get; set; }
    [SerializeField]
    private List<Unit> units;

    public GameObject unitFieldAnimationPrefab;

    private bool dragInitiated;
    private bool dragStarted;
    [SerializeField] private GameObject PrepUI;
    [SerializeField] private Button ShowPrepUIButton;
    [SerializeField] private Button StartButton;

    [SerializeField] private IUnitSelectionUI unitSelectionUI;
    [SerializeField] private UIObjectiveController conditionUI;

    private List<Unit> selectedUnits;
    // Update is called once per frame
 

    private Canvas canvas;
    void Start()
    {
        canvas = GetComponent<Canvas>();
    }

    private void OnEnable()
    {
        RaycastManager = new RaycastManager();
        unitSelectionUI.unitSelectionChanged += InvokeSelectionChanged;
        //  UpdateValues();
    }

    private void OnDisable()
    {
        unitSelectionUI.unitSelectionChanged -= InvokeSelectionChanged;
    }

    private void InvokeSelectionChanged(List<Unit> units)
    {
        unitSelectionChanged?.Invoke(units);
        selectedUnits = units;
        StartButton.interactable = units.Count!=0;
    }

    // private void UpdateValues()
    // {
    //     for (int i=layoutGroup.childCount-1; i>=0; i--){
    //         DestroyImmediate(layoutGroup.transform.GetChild(i).gameObject);
    //     }
    //
    //     if(units!=null)
    //         foreach (Unit u in units)
    //         {
    //            
    //             var go = Instantiate(unitPrefab, layoutGroup, false);
    //             go.GetComponent<UIUnitDragObject>().UnitPlacement = this;
    //             go.GetComponent<UIUnitDragObject>().SetUnitSprite(u.visuals.CharacterSpriteSet.MapSprite);
    //         }
    // }

    public void StartClicked()
    {
        OnFinished?.Invoke();
        Hide();
    }

    public override void Show(List<Unit> units, Chapter chapter)
    {
        this.units = units;
        this.chapter = chapter;
        if (selectedUnits == null|| selectedUnits.Count==0)
            selectedUnits = units;
        //UpdateValues();
        GetComponent<Canvas>().enabled = true;
        
    }
    
    public override void Hide()
    {
        GetComponent<Canvas>().enabled = false;
    }



    private void HideGrid()
    {
        PrepUI.SetActive(false);
        ShowPrepUIButton.gameObject.SetActive(true);
    }
    public void ShowGrid()
    {
        PrepUI.SetActive(true);
        ShowPrepUIButton.gameObject.SetActive(false);
    }
    public void MapButtonCLicked()
    {
        HideGrid();
    }
    public void ConditionButtonClicked()
    {
        conditionUI.Show(chapter);
        Hide();
    }
    public void Show()
    {
        GetComponent<Canvas>().enabled = true;

    }
    public void ExitButtonClicked()
    {
        GameSceneController.Instance.LoadWorldMapBeforeBattle();
    }
    public void UnitButtonClicked()
    {
        unitSelectionUI.Show(units,selectedUnits);
        Hide();
    }
    public void PlaceholderButtonCLicked()
    {
        Debug.Log("Placeholder Button Clicked!");
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
