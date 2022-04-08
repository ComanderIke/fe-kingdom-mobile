using Game.GameInput;
using Game.GUI;
using Game.Manager;
using Game.Map;
using UnityEngine;

namespace Game.GameActors.Units.OnGameObject
{
    public class UnitInstantiator : MonoBehaviour
    {

        public GameObject UnitNormal;
        public GameObject UnitEnemy;

        public void PlaceCharacter(Unit unit, int x, int y)
        {
            Debug.Log("Place Unit SkillCount: "+unit.SkillManager.Skills.Count);
            GameObject unitGameObject;
            if(GridGameManager.Instance.FactionManager.GetPlayerControlledFaction().Id==unit.Faction.Id)
                unitGameObject = Instantiate(UnitNormal);
            else
                unitGameObject = Instantiate(UnitEnemy);

            unitGameObject.GetComponentInChildren<BuffUi>()?.Initialize(unit);
            unitGameObject.name = unit.name;
            unitGameObject.transform.parent = gameObject.transform;
            unitGameObject.layer = LayerMask.NameToLayer("Characters");
            GameObject animatedSprite=Instantiate(unit.visuals.CharacterSpriteSet.animatedSprite, unitGameObject.transform, false);
            animatedSprite.transform.localPosition= new Vector3(0.5f, 0, 0);
         
            var unitController = unitGameObject.GetComponentInChildren<UnitInputController>();
            unitController.unit = unit;
          //  Debug.Log("Set Unit Controller: " + unit.name + " " + unitController);
            var unitRenderer = unitGameObject.GetComponentInChildren<UnitRenderer>();
            unitRenderer.unit = unit;
            unitRenderer.sprite = animatedSprite.GetComponent<SpriteRenderer>();
           // Debug.Log("Set Unit Renderer: " + unit.name + " " + unitRenderer);
            unitRenderer.Init();
            unit.visuals.unitRenderer = unitRenderer;
            animatedSprite.GetComponent<UnitAnimator>().SetUnit(unit);
            
            //Debug.Log("Set Unit Animator: " + unit.name + " " + unitAnimator);
           // Debug.Log("Set Unit GameObject: " + unit.name + " " + unitGameObject);
            unit.GameTransformManager.GameObject = unitGameObject;
            GridGameManager.Instance.GetSystem<GridSystem>().SetUnitPosition(unit, x, y);
          

            //For Performance
            if(unitGameObject.GetComponentInChildren<Canvas>()!=null)
                unitGameObject.GetComponentInChildren<Canvas>().worldCamera = Camera.main;
        }

        public void UnPlaceCharacter(Unit unit)
        {
            GameObject.Destroy(unit.GameTransformManager.GameObject);
            GridGameManager.Instance.GetSystem<GridSystem>()
                .GetTile(unit.GridComponent.GridPosition.X, unit.GridComponent.GridPosition.Y).Actor = null;
        }
    }
}