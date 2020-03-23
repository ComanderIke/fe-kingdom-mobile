using Assets.GameActors.Players;
using Assets.GameActors.Units;
using UnityEngine;

namespace Assets.GUI.Base
{
    public class UnitListController : MonoBehaviour
    {
        public GameObject UnitListPrefab;
        public Transform UnitListTransform;

        private void Start()
        {
            foreach (var unit in Player.Instance.Units)
            {
                var unitListObject=Instantiate(UnitListPrefab, UnitListTransform, false).GetComponent<UnitListEntry>();
                unitListObject.SetUnit(unit);
            }
        }

    }
}
