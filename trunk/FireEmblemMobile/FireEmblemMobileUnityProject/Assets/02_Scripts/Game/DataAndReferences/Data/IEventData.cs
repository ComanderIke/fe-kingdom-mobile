using Game.Dialog.DialogSystem;

namespace Game.DataAndReferences.Data
{
    public interface IEventData
    {
        LGEventDialogSO GetRandomEvent();
   
        LGEventDialogSO GetEventById(string prefabId);
    }
}