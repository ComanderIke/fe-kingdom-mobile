using Assets.Scripts.Engine;
using Assets.Scripts.Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitsController : MonoBehaviour, Controller {

    private UnitController[] units;

    private void Init()
    {
        units = GetComponentsInChildren<UnitController>();
        
    }


    public void ShowUnits()
    {
        if (units == null)
            Init();
        foreach(UnitController unit in units)
        {
            unit.gameObject.SetActive(true);
        }
    }

    public void HideUnits()
    {
        if (units == null)
            Init();
        foreach (UnitController unit in units)
        {
            unit.gameObject.SetActive(false);
        }
    }
}
