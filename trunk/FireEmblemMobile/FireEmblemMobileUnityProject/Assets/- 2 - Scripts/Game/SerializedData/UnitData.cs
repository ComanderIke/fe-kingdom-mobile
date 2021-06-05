using Game.GameActors.Units;
using Game.GameActors.Units.Attributes;
using Game.GameResources;
using UnityEngine;

namespace Game.GameActors.Players
{
    [System.Serializable]
    public class UnitData
    {
        [SerializeField]
        public string name;
        [SerializeField]
        private Stats stats;
        [SerializeField]
        private Growths growths;
        [SerializeField]
        private MoveType moveType;
        public ExperienceManager ExperienceManager;
        public TurnStateManager TurnStateManager;
        [SerializeField]
        public int VisualsID;
        public UnitData(Unit unit)
        {
            this.name = unit.name;
            ExperienceManager = unit.ExperienceManager;
            TurnStateManager = unit.TurnStateManager;
            this.moveType = unit.MoveType;
            this.growths = unit.Growths;
            this.stats = unit.Stats;
            VisualsID = unit.visuals.ID;
        }

        public virtual void Load(Unit unit)
        {
            unit.name = name;
            unit.Stats = stats;
            unit.Growths = growths;
            unit.ExperienceManager = ExperienceManager;
            unit.TurnStateManager = TurnStateManager;
            unit.visuals = new UnitVisual
            {
                CharacterSpriteSet = GameAssets.Instance.visuals.LoadCharacterSpriteSet(VisualsID)
            };
            unit.MoveType = moveType;
        }
    }
}