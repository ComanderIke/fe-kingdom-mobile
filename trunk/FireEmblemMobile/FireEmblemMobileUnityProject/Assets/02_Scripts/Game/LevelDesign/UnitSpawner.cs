using System;
using Game.AI.DecisionMaking;
using Game.GameActors.Factions;
using Game.GameActors.Items;
using Game.GameActors.Units;
using Game.Manager;
using UnityEngine;
using UnityEngine.Serialization;
using IServiceProvider = Game.Manager.IServiceProvider;

namespace Game.LevelDesign
{
    [ExecuteInEditMode]
    public class UnitSpawner : MonoBehaviour
    {

        [FormerlySerializedAs("unit")] [SerializeField]private UnitBP unitBp;
        public FactionId FactionId;
        public WeightSet AIWeightSet;
        public AIBehaviour AIBehaviour;
        [SerializeField]private int aiGroupId;//0 is no group
        public ItemBP DropableItem;
        public int X => (int) transform.localPosition.x;
        public int Y => (int)transform.localPosition.y;

        public SpriteRenderer spriteRenderer;
        void Update()
        {
            transform.localPosition = new Vector3((int) transform.localPosition.x, (int) transform.localPosition.y,
                (int) transform.localPosition.z);
            spriteRenderer.sprite = unitBp.visuals.CharacterSpriteSet.MapSprite;
            gameObject.name = "Enemy" + unitBp.name;
        }

        public Unit GetUnit()
        {
            var unit = unitBp.Create(Guid.NewGuid(), AIBehaviour);
            MyDebug.LogTODO("AI GROUPS HERE");
            if (aiGroupId != 0)
            {
                var faction = GridGameManager.Instance.FactionManager.FactionFromId(FactionId);
                faction.AddtoAIGroup(aiGroupId, this.AIBehaviour.GetState(), unit);
            }

            //check if ai group with id exists
            //if not create a new one and add this unit
            //if yes add this unit
            //in aicomponent check if augroup then use group state instead of own/override it.
            if (DropableItem != null)
                unit.DropableItem = DropableItem.Create();
            return unit;
        }
    }
}
