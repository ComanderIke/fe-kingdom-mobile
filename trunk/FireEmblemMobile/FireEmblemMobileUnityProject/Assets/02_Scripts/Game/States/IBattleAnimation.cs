using System;
using Game.GameActors.Players;
using Game.GameInput;
using Game.Mechanics.Battle;

namespace Game.Mechanics
{
    public interface IBattleAnimation
    {
        void Show(BattleSimulation battleSimulation,  BattlePreview battlePreview, IBattleActor attacker, IAttackableTarget defender);
        void Hide();
        event Action<int> OnFinished;
        void Cleanup();
    }
}