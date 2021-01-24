using Game.GUI;
using Game.Manager;
using Game.Map;
using UnityEngine;

namespace Game.GameActors.Units.OnGameObject
{
    public class UnitInstantiator : MonoBehaviour
    {

        public GameObject UnitNormal;

        public void PlaceCharacter(Unit unit, int x, int y)
        {
            var unitGameObject = Instantiate(UnitNormal);
            unitGameObject.GetComponentInChildren<SpriteRenderer>().sprite = unit.CharacterSpriteSet.MapSprite;
            
            unitGameObject.GetComponentInChildren<BuffUi>()?.Initialize(unit);
            unitGameObject.name = unit.name;
            unitGameObject.transform.parent = gameObject.transform;
            unitGameObject.layer = LayerMask.NameToLayer("Characters");
            
            var unitController = unitGameObject.GetComponentInChildren<UnitInputController>();
            unitController.unit = unit;
            var unitRenderer = unitGameObject.GetComponentInChildren<UnitRenderer>();
            unitRenderer.unit = unit;
            unitRenderer.Init();
            var unitAnimator = unitGameObject.GetComponentInChildren<UnitAnimator>();
            unitAnimator.unit = unit;
          
            
            unit.GameTransform.GameObject = unitGameObject;
            GridGameManager.Instance.GetSystem<GridSystem>().SetUnitPosition(unit, x, y);
            

            //For Performance
            if(unitGameObject.GetComponentInChildren<Canvas>()!=null)
                unitGameObject.GetComponentInChildren<Canvas>().worldCamera = Camera.main;
        }
    }
}