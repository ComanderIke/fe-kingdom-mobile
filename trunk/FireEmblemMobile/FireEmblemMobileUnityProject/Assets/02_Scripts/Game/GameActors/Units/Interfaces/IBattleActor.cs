using Game.GameActors.Factions;
using Game.GameActors.InteractableGridObjects;
using Game.GameActors.Items.Weapons;
using Game.GameActors.Units.Components;
using Game.GameActors.Units.Numbers;
using Game.GameActors.Units.OnGameObject;
using Game.GameActors.Units.Progression;
using Game.GameActors.Units.UnitState;
using Game.GameActors.Units.Visuals;
using Game.Graphics.BattleAnimations;
using Game.Grid.Tiles;

namespace Game.GameActors.Units.Interfaces
{
    public interface IBattleActor:IAttackableTarget
    {
        BattleComponent BattleComponent { get; set; }
        Stats Stats { get; set; }
  
      //  int Sp { get; set; }
       // int SpBars{ get; set; }
       // int MaxSpBars { get; }
        Faction Faction { get; set; }
        ExperienceManager ExperienceManager { get; }
        TurnStateManager TurnStateManager { get; set; }
        GameTransformManager GameTransformManager { get; set; }
        AnimatedCombatCharacter BattleGO { get; set; }
        UnitVisual Visuals { get; }
        GridComponent GridComponent { get; set; }
        StatusEffectManager StatusEffectManager { get; set; }
        public PowerTriangleType PowerTriangleType { get; set; }
        public bool IsPowerTypeEffective(IBattleActor defender);
        public bool IsPowerTypeInEffective(IBattleActor defender);
        int RevivalStones { get; set; }
        Weapon GetEquippedWeapon();
        Tile GetTile();

        bool IsPlayerControlled(bool includeTempted=true); 
        float GetAttackDelay();
    }
}