using Game.GameActors.Items.Weapons;
using Game.GameActors.Players;
using Game.GameActors.Units;
using Game.GameActors.Units.Numbers;
using Game.GameActors.Units.OnGameObject;
using Game.Grid;
using Game.Mechanics;
using Game.Mechanics.Battle;
using UnityEngine;

namespace Game.GameInput
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
        Weapon GetEquippedWeapon();
        Tile GetTile();

        bool IsPlayerControlled();
    }
}