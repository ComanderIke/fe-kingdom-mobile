using UnityEngine;

namespace Game.Dialog
{
    [System.Serializable]
    public class Line
    {
        public string sentence;
        [SerializeField] DialogActor actor;
        public bool left = true;
        public IDialogActor Actor => actor;
    }
}