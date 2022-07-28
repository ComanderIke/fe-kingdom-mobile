using Game.GameInput;

namespace Game.Mechanics
{
    public interface IBattleRenderer
    {
        void Hide();
        void Show(IBattleActor attacker, IBattleActor defender, bool[] attackSequence);
    }
}