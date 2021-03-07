

using Game.States;

namespace Game.GUI
{
    public interface ILevelUpRenderer : IAnimation
    {
        void UpdateValues(string name, int levelBefore, int levelAfter, int[] stats, int[] statsIncreases);
    }
}