using System.Linq;
using Assets.Grid;
using Assets.GUI;
using UnityEngine.UI;
using UnityEngine;

namespace Assets.GameActors.Units.OnGameObject
{
    public class UnitInstantiator : MonoBehaviour
    {

        public GameObject UnitNormal;

        public void PlaceCharacter(Unit unit, int x, int y)
        {
            var unitGameObject = Instantiate(UnitNormal);
            unitGameObject.GetComponentInChildren<SpriteRenderer>().sprite = unit.CharacterSpriteSet.MapSprite;
            unitGameObject.GetComponentInChildren<BuffUi>()?.Initialize(unit);
            unitGameObject.name = unit.Name;
            unitGameObject.transform.parent = gameObject.transform;
            unitGameObject.layer = LayerMask.NameToLayer("Characters");
            
            var unitController = unitGameObject.GetComponentInChildren<UnitInputController>();
            unitController.Unit = unit;
            var unitRenderer = unitGameObject.GetComponentInChildren<UnitRenderer>();
            unitRenderer.Unit = unit;
            unitRenderer.Init();
            unit.GameTransform.GameObject = unitGameObject;
            unit.SetPosition(x, y);

            //For Performance
            if(unitGameObject.GetComponentInChildren<Canvas>()!=null)
                unitGameObject.GetComponentInChildren<Canvas>().worldCamera = Camera.main;
        }
    }
}