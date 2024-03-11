using System;
using Game.AI.DecisionMaking;
using Game.GameActors.Factions;
using Game.GameActors.Units;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game.LevelDesign
{
    [ExecuteInEditMode]
    public class UnitSpawner : MonoBehaviour
    {

        [FormerlySerializedAs("unit")] [SerializeField]private UnitBP unitBp;
        public FactionId FactionId;
        public WeightSet AIWeightSet;
        public AIBehaviour AIBehaviour;
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
          
            return unit;
        }
    }
}
