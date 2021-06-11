using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Game.GameActors.Units;
using Game.GUI;
using Game.WorldMapStuff.Model;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UnitSelectionUI : IUnitSelectionUI
{
    private List<Unit> units;
    public Transform parentForUnits;
    public GameObject unitUIPrefab;
    private List<UIUnitDragObject> selectedUnitUIs;
    private List<UIUnitDragObject> allUnitUis;
    public TextMeshProUGUI unitText;
    [HideInInspector]
    public bool init = false;
    public override void Show(List<Unit> units)
    {
        this.units = units;
        selectedUnitUIs = new List<UIUnitDragObject>();
        GetComponent<Canvas>().enabled = true;
        InstantiateUnits();
        for (int i = 0; i < Party.MaxSize && i < allUnitUis.Count; i++)
        {
            selectedUnitUIs.Add(allUnitUis[i]);
          
            allUnitUis[i].ShowSelected();
        }
        unitSelectionChanged?.Invoke(selectedUnitUIs.Select(s=>s.unit).ToList());
        selectedUnitUIs = new List<UIUnitDragObject>();
        unitText.SetText("Units: "+selectedUnitUIs.Count+"/"+Party.MaxSize);
    }

    private void InstantiateUnits()
    {
        for (int i=parentForUnits.childCount-1; i>=0; i--){
            DestroyImmediate(parentForUnits.transform.GetChild(i).gameObject);
        }

        allUnitUis = new List<UIUnitDragObject>();
        if (units != null)
        {
            foreach (Unit u in units)
            {
                var go = Instantiate(unitUIPrefab, parentForUnits, false);
                allUnitUis.Add(go.GetComponent<UIUnitDragObject>());
                go.GetComponent<UIUnitDragObject>().SetUnit(u);
                go.GetComponent<UIUnitDragObject>().UnitSelectionUI = this;
            }
        }
    }
    public override void Hide()
    {
        GetComponent<Canvas>().enabled = false;
    }

    public void UnitClicked(UIUnitDragObject unitUI)
    {
        if (!selectedUnitUIs.Contains(unitUI))
        {
            if (selectedUnitUIs.Count < Party.MaxSize)
            {
                selectedUnitUIs.Add(unitUI);
                unitSelectionChanged?.Invoke(selectedUnitUIs.Select(s=>s.unit).ToList());
                unitUI.ShowSelected();
            }
            else
            {
                Debug.Log("Player Error Sound to many Units selected");
            }
        }
        else
        {
            selectedUnitUIs.Remove(unitUI);
            unitSelectionChanged?.Invoke(selectedUnitUIs.Select(s=>s.unit).ToList());
            unitUI.HideSelected();
        }
        unitText.SetText("Units: "+selectedUnitUIs.Count+"/"+Party.MaxSize);
        
    }
}