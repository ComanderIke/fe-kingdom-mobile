using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Game.GameActors.Units;
using Game.GUI;
using UnityEngine;

[ExecuteInEditMode]
public class UIUnitPlacement : IUnitPlacementUI
{
    [SerializeField]
    private GameObject unitPrefab;
    [SerializeField]
    private Transform layoutGroup;
    [SerializeField]
    private List<Unit> units;

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnEnable()
    {
        UpdateValues();
    }

    private void UpdateValues()
    {
        if(units!=null)
            foreach (Unit u in units)
            {
                Instantiate(unitPrefab, layoutGroup, false);
            }
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
}
