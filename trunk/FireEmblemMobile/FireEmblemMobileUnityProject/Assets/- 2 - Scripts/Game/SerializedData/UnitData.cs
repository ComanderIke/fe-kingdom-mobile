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
        private StatsData statsData;
        [SerializeField]
        private GrowthsData growthsData;
        [SerializeField]
        private int moveTypeId;
        [SerializeField]
        public ExperienceManager ExperienceManager;
        [SerializeField]
        public TurnStateManager TurnStateManager;
        [SerializeField]
        public string visualsID;
        public UnitData(Unit unit)
        {
            this.name = unit.name;
            ExperienceManager = unit.ExperienceManager;
            TurnStateManager = unit.TurnStateManager;
            this.moveTypeId = unit.MoveType.moveTypeId;
            this.growthsData = unit.Growths.GetSaveData();
            this.statsData = unit.Stats.GetSaveData();
            visualsID = unit.visuals.CharacterSpriteSet.name;
        }

        public virtual void Load(Unit unit)
        {
            unit.name = name;
            unit.Stats.LoadData(statsData);
            unit.Growths.LoadData(growthsData);
            unit.ExperienceManager = ExperienceManager;
            unit.TurnStateManager = TurnStateManager;
            unit.visuals = new UnitVisual
            {
                CharacterSpriteSet = GameAssets.Instance.visuals.LoadCharacterSpriteSet(visualsID)
            };
            unit.MoveType = GameData.Instance.UnitData.GetMoveType(moveTypeId);
           
        }
    }
}