using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitController : MonoBehaviour {

    private MovableObject[] units;

    private void Init()
    {
        units = GetComponentsInChildren<MovableObject>();
    }

    public void ShowUnits()
    {
        if (units == null)
            Init();
        foreach(MovableObject unit in units)
        {
            unit.gameObject.SetActive(true);
        }
    }

    public void HideUnits()
    {
        if (units == null)
            Init();
        foreach (MovableObject unit in units)
        {
            unit.gameObject.SetActive(false);
        }
    }
}
