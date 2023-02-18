using _02_Scripts.Game.Dialog.DialogSystem;

namespace Game.GameResources
{
    public interface IEventData
    {
        LGEventDialogSO GetRandomEvent(int tier);
        LGEventDialogSO GetSpecialEvent(int index);
    }
}