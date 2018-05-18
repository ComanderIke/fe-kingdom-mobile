using Assets.Scripts.Characters;
using System.Collections.Generic;
using UnityEngine;

public class BuffUI : MonoBehaviour {

    [SerializeField]
    GameObject buffPrefab;
    Unit unit;
    // Use this for initialization
    Dictionary<string,GameObject> buffs;
	void Start () {
        Unit.onUnitCanMove += UnitMoveState;
        buffs = new Dictionary<string, GameObject>();
        unit = transform.parent.parent.GetComponent<UnitController>().Unit;
    }
	
	// Update is called once per frame
	void Update () {
        
	}
    void UnitMoveState(Unit unit, bool canMove)
    {
        if (this.unit == unit)
        {
            if (canMove)
            {
                if (buffs.ContainsKey("Move"))
                {
                    Destroy(buffs["Move"]);
                    buffs.Remove("Move");
                }
            }
            else
            {
                if (!buffs.ContainsKey("Move"))
                    buffs.Add("Move", GameObject.Instantiate(buffPrefab, this.transform));
            }
        }
    }
}
