using UnityEngine;

namespace Game.Dialog
{
    public interface IDialogActor
    {
        public string Name { get; }
        public Sprite FaceSprite { get; }
    }
}