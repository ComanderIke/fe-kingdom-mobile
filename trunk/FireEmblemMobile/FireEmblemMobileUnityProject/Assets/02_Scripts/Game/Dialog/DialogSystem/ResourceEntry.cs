using System;
using Game.GameActors.Items;

namespace _02_Scripts.Game.Dialog.DialogSystem
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