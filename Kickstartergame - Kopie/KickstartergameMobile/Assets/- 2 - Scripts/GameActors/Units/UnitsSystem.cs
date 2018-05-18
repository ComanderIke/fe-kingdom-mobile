using Assets.Scripts.Engine;
using UnityEngine;

public class UnitsSystem : MonoBehaviour, EngineSystem {

    private UnitController[] units;

    void Start()
    {
        units = FindObjectsOfType<UnitController>();
        
    }


    public void ShowUnits()
    {

        foreach(UnitController unit in units)
        {
            unit.gameObject.SetActive(true);
        }
    }

    public void HideUnits()
    {
        foreach (UnitController unit in units)
        {
            unit.gameObject.SetActive(false);
        }
    }
}
