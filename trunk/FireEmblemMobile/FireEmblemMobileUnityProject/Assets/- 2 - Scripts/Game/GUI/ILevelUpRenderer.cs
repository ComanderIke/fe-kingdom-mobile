

namespace Game.GUI
{
    public interface ILevelUpRenderer
    {
        void Show(string name, int levelBefore, int levelAfter, int[] stats, int[] statsIncreases);
    }
}