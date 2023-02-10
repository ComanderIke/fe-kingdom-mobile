using System.Collections.Generic;
using __2___Scripts.External.Editor.Elements;
using UnityEditor;
using UnityEngine;

namespace _02_Scripts.Game.Dialog.DialogSystem
{
    public class LGDialogSO :ScriptableObject
    {
        [field:SerializeField]public string  DialogName { get; set; }
        [field:SerializeField][field:TextArea()]public string Text { get; set; }
        [field:SerializeField]public List<LGDialogChoiceData> Choices { get; set; }
        [field:SerializeField]public DialogType DialogueType { get; set; }
        [field:SerializeField]public bool IsStartingDialogue { get; set; }

        public void Initialize(string dialogName, string text, List<LGDialogChoiceData> choices, DialogType dialogType,
            bool isStartingDialog)
        {
            DialogName = dialogName;
            Text = text;
            Choices = choices;
            DialogueType = dialogType;
            IsStartingDialogue = isStartingDialog;
        }
    }
}