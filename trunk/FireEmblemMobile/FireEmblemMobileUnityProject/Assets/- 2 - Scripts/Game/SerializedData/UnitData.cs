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
        private int moveTypeId;
        [SerializeField]
        public ExperienceManager ExperienceManager;
        [SerializeField]
        public TurnStateManager TurnStateManager;
        [SerializeField]
        public int VisualsID;
        public UnitData(Unit unit)
        {
            this.name = unit.name;
            ExperienceManager = unit.ExperienceManager;
            TurnStateManager = unit.TurnStateManager;
            this.moveTypeId = unit.MoveType.moveTypeId;
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
            unit.MoveType = GameData.Instance.UnitData.GetMoveType(moveTypeId);
           
        }
    }
}