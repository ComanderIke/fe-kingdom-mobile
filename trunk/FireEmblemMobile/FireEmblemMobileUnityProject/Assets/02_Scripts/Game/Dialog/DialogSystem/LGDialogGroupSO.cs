using UnityEngine;

namespace Game.Dialog.DialogSystem
{
    public class LGDialogGroupSO:ScriptableObject
    {
        [field:SerializeField]public string GroupName { get; set; }

        public void Initialize(string groupName)
        {
            GroupName = groupName;
        }
    }
}