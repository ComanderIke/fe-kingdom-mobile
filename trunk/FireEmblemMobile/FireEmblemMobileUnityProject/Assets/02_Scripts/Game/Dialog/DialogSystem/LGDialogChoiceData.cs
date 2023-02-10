using System;
using UnityEngine;

namespace _02_Scripts.Game.Dialog.DialogSystem
{
    [Serializable]
    public class LGDialogChoiceData
    {
        [field:SerializeField]public string Text { get; set; }
        [field:SerializeField]public LGDialogSO NextDialogue { get; set; }
        
    }
}