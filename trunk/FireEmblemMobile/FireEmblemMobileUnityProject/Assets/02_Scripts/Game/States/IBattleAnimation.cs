using System;
using Game.GameActors.InteractableGridObjects;
using Game.GameActors.Units.Interfaces;
using Game.States.Mechanics;
using Game.States.Mechanics.Battle;

namespace Game.States
{
    public interface IBattleAnimation
    {
        void Show(BattleSimulation battleSimulation,  BattlePreview battlePreview, IBattleActor attacker, IAttackableTarget defender);
        void Hide();
        event Action<int> OnFinished;
        void Cleanup();
    }
}