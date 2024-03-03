using Game.GUI.EncounterUI.Merchant;

namespace Game.GameInput.Interfaces
{
    public interface IItemClickedReceiver
    {
        void ItemClicked(StockedItem item);
    }
}