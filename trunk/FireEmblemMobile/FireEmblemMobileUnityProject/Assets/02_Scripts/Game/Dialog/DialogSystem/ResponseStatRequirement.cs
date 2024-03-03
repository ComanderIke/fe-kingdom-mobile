using System;
using Game.GameActors.Units.Numbers;

namespace Game.Dialog.DialogSystem
{
    [Serializable]
    public class ResponseStatRequirement
    {
        public AttributeType AttributeType;
        public int Amount;
    }
}