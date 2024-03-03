using System;
using Game.GameActors.Items;

namespace Game.Dialog.DialogSystem
{
    [Serializable]
    public class ResourceEntry
    {
        public int Amount;
        public ResourceType ResourceType;
        

        public ResourceEntry(int i, ResourceType gold)
        {
            this.Amount = i;
            this.ResourceType = gold;
        }
    }
}