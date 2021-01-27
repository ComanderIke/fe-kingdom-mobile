using Game.GameActors.Units.Attributes;
using Game.Mechanics;
using Game.Mechanics.Battle;

namespace Game.GameInput
{
    public interface IBattleActor
    {
        BattleStats BattleStats { get; set; }
        Stats Stats { get; set; }
        int Hp { get; set; }
        int Sp { get; set; }
        IBattleActor Clone();
    }
}