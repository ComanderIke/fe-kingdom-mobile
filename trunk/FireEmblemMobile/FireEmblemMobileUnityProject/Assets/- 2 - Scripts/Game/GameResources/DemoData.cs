using System.Collections;
using System.Collections.Generic;
using Assets.GameActors.Players;
using Assets.GameActors.Units;
using UnityEngine;

public class DemoData : MonoBehaviour
{
    public Unit[] demoUnits;
    // Start is called before the first frame update
    void Start()
    {
        foreach (var unit in demoUnits)
        {
            var unitInst = Instantiate(unit);
            Player.Instance.Units.Add(unitInst);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
