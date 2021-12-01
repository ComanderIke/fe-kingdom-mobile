using Game.GameActors.Players;
using Game.GameActors.Units;
using Game.GameActors.Units.Attributes;
using Game.GameActors.Units.OnGameObject;
using Game.Grid;
using Game.Mechanics;
using Game.Mechanics.Battle;

namespace Game.GameInput
{
    public interface IBattleActor
    {
        BattleComponent BattleComponent { get; set; }
        Stats Stats { get; set; }
        int Hp { get; set; }
        int Sp { get; set; }
        int SpBars{ get; set; }
        int MaxSpBars { get; }
        Faction Faction { get; set; }
        ExperienceManager ExperienceManager { get; }
        TurnStateManager TurnStateManager { get; set; }
        GameTransformManager GameTransformManager { get; set; }

        object Clone();
        bool IsAlive();
        void Die();
        Tile GetTile();

    }
}