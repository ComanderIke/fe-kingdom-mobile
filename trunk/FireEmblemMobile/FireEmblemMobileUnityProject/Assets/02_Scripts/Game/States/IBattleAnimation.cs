using System;
using Game.GameActors.Players;
using Game.GameInput;

namespace Game.Mechanics
{
    public interface IBattleAnimation
    {
        void Show(BattleSimulation battleSimulation, IBattleActor attacker, IAttackableTarget defender);
        void Hide();
        event Action<int> OnFinished;
        void Cleanup();
    }
}