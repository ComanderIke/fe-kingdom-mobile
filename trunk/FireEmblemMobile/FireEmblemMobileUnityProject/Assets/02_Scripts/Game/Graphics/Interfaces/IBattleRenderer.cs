using Game.GameActors.Units.Interfaces;

namespace Game.Graphics.Interfaces
{
    public interface IBattleRenderer
    {
        void Hide();
        void Show(IBattleActor attacker, IBattleActor defender, bool[] attackSequence);
    }
}