using Assets.GameActors.Units;
using Assets.GameActors.Units.OnGameObject;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.GUI
{
    public class BuffUi : MonoBehaviour
    {
        [SerializeField] private GameObject buffPrefab = default;
        private Dictionary<string, GameObject> buffs;

        private void OnEnable()
        {
            buffs = new Dictionary<string, GameObject>();
        }

        public void Initialize(Unit unit)
        {
            
        }

        private void UnitMoveState(Unit unit, bool canMove)
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
                    buffs.Add("Move", Instantiate(buffPrefab, transform));
            }
        }
    }
}