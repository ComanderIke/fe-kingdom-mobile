using Game.GameActors.Units;
using Game.GameActors.Units.Numbers;
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
        public int hp;
        [SerializeField]
        private StatsData statsData;
        [SerializeField]
        private Attributes growthsData;
        [SerializeField]
        public ExperienceManager ExperienceManager;
        [SerializeField]
        public TurnStateManager TurnStateManager;
        [SerializeField]
        public string unitBlueprintID;
 
        
        public UnitData(Unit unit)
        {
            this.name = unit.name;
            ExperienceManager = unit.ExperienceManager;
            TurnStateManager = unit.TurnStateManager;
            this.growthsData = unit.Growths;
            this.statsData = unit.Stats.GetSaveData();
            this.unitBlueprintID = unit.bluePrintID;
            this.hp = unit.Hp;
        }

        public virtual Unit Load()
        {
            Unit unit = Object.Instantiate(GameData.Instance.GetHumanBlueprint(unitBlueprintID));
            unit.name = name;
            unit.Stats = new Stats();//ScriptableObject.CreateInstance<Stats>();
            unit.Stats.LoadData(statsData);
            unit.Growths = new Attributes(growthsData);//ScriptableObject.CreateInstance<Growths>();
            // unit.Growths.LoadData(growthsData);
            unit.ExperienceManager = new ExperienceManager();
            unit.ExperienceManager.Exp = ExperienceManager.Exp;
            unit.ExperienceManager.Level = ExperienceManager.Level;
            unit.TurnStateManager = TurnStateManager;
            unit.Hp = hp;
            return unit;
            // unit.visuals = new UnitVisual();
            //
            // unit.visuals.CharacterSpriteSet = GameAssets.Instance.visuals.LoadCharacterSpriteSet(spriteID);
            //  unit.visuals.UnitEffectVisual = GameAssets.Instance.visuals.LoadUnitEffectVisual(vfxID);
            // unit.MoveType = GameData.Instance.UnitData.GetMoveType(moveTypeId);

        }
    }
}