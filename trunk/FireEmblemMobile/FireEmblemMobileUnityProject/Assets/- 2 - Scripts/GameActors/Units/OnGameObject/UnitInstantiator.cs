using Assets.Grid;
using Assets.GUI;
using UnityEngine;

namespace Assets.GameActors.Units.OnGameObject
{
    public class UnitInstantiator : MonoBehaviour
    {
        public GameObject UnitBig;

        public GameObject UnitNormal;

        public void PlaceCharacter(Unit unit, int x, int y)
        {
            var unitGameObject = Instantiate(unit.GridPosition is BigTilePosition ? UnitBig : UnitNormal);
            unitGameObject.GetComponentInChildren<SpriteRenderer>().sprite = unit.CharacterSpriteSet.MapSprite;
            unitGameObject.GetComponentInChildren<SpriteRenderer>().sortingOrder = 3;
            unitGameObject.GetComponentInChildren<BuffUi>().Initialize(unit);
            unitGameObject.name = unit.Name;
            unitGameObject.transform.parent = gameObject.transform;
            unitGameObject.layer = LayerMask.NameToLayer("Characters");
            var unitController = unitGameObject.GetComponentInChildren<UnitController>();
            unitController.Unit = unit;
            unit.GameTransform.GameObject = unitGameObject;
            unit.SetPosition(x, y);
            //For Performance
            unitGameObject.GetComponentInChildren<Canvas>().worldCamera =
                GameObject.FindGameObjectWithTag("UICamera").GetComponent<Camera>();
        }
    }
}