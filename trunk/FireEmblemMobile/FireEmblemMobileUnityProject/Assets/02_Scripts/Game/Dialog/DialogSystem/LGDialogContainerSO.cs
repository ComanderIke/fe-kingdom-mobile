using System.Collections.Generic;
using _02_Scripts.Game.GUI.Utility;
using UnityEngine;

namespace _02_Scripts.Game.Dialog.DialogSystem
{
    public class LGDialogContainerSO:ScriptableObject
    {
        [field:SerializeField]public string FileName { get; set; }
        [field:SerializeField]public SerializableDictionary<LGDialogGroupSO, List<LGDialogSO>> DialogueGroupes { get; set; }
        [field:SerializeField]public List<LGDialogSO> UngroupedDialogs { get; set; }

        public void Initialize(string fileName)
        {
            FileName = fileName;
            DialogueGroupes = new SerializableDictionary<LGDialogGroupSO, List<LGDialogSO>>();
            UngroupedDialogs = new List<LGDialogSO>();
        }
    }
}