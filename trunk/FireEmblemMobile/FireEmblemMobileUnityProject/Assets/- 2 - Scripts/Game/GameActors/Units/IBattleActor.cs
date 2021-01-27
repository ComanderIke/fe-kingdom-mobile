using Game.GameActors.Units;
using Game.GameActors.Units.Attributes;
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
        
        object Clone();
    }
}