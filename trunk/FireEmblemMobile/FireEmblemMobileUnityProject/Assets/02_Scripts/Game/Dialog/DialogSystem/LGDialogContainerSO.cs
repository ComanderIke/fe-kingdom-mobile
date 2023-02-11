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

        public List<string> GetDialogGroupNames()
        {
            List<string> dialogGroupNames = new List<string>();
            foreach (LGDialogGroupSO group in DialogueGroupes.Keys)
            {
                dialogGroupNames.Add(group.GroupName);
            }

            return dialogGroupNames;
        }

        public List<string> GetGroupedDialogNames(LGDialogGroupSO group, bool startingDialogsOnly)
        {
            List<LGDialogSO> groupedDialogs = DialogueGroupes[group];
            List<string> groupedDialogsNames = new List<string>();

            foreach (LGDialogSO groupedDialog in groupedDialogs)
            {
                if(startingDialogsOnly && !groupedDialog.IsStartingDialogue)
                    continue;
                groupedDialogsNames.Add(groupedDialog.DialogName);
            }

            return groupedDialogsNames;
        }
        public List<string> GetUngroupedDialogNames(bool startingDialogsOnly)
        {
            List<string> unGoupedDialogsNames = new List<string>();

            foreach (LGDialogSO dialog in UngroupedDialogs)
            {
                if(startingDialogsOnly && !dialog.IsStartingDialogue)
                    continue;
                unGoupedDialogsNames.Add(dialog.DialogName);
            }

            return unGoupedDialogsNames;
        }
    }
}