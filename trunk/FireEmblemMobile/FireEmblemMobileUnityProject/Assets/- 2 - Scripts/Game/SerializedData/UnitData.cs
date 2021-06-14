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
        public string spriteID;
        [SerializeField]
        public string vfxID;
        
        public UnitData(Unit unit)
        {
            this.name = unit.name;
            ExperienceManager = unit.ExperienceManager;
            TurnStateManager = unit.TurnStateManager;
            this.moveTypeId = unit.MoveType.moveTypeId;
            this.growthsData = unit.Growths.GetSaveData();
            this.statsData = unit.Stats.GetSaveData();
            spriteID = unit.visuals.CharacterSpriteSet.ID;
            vfxID = unit.visuals.UnitEffectVisual.ID;
        }

        public virtual void Load(Unit unit)
        {
            unit.name = name;
            unit.Stats = ScriptableObject.CreateInstance<Stats>();
            unit.Stats.LoadData(statsData);
            unit.Growths = ScriptableObject.CreateInstance<Growths>();
            unit.Growths.LoadData(growthsData);
            unit.ExperienceManager = ExperienceManager;
            unit.TurnStateManager = TurnStateManager;
            unit.visuals = new UnitVisual();
            unit.visuals.CharacterSpriteSet = GameAssets.Instance.visuals.LoadCharacterSpriteSet(spriteID);
            unit.visuals.UnitEffectVisual = GameAssets.Instance.visuals.LoadUnitEffectVisual(vfxID);
            unit.MoveType = GameData.Instance.UnitData.GetMoveType(moveTypeId);
           
        }
    }
}