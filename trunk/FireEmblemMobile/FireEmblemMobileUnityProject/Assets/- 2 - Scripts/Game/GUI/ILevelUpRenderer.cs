

namespace Game.GUI
{
    public interface ILevelUpRenderer
    {
        void UpdateValues(string name, int levelBefore, int levelAfter, int[] stats, int[] statsIncreases);
    }
}