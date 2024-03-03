using System.Collections.Generic;
using UnityEngine;

namespace Game.Dialog.DialogSystem
{
    public class LGDialogSO :ScriptableObject
    {
        [field:SerializeField]public string  DialogName { get; set; }
        [field:SerializeField][field:TextArea()]public string Text { get; set; }
        [field:SerializeField]public List<LGDialogChoiceData> Choices { get; set; }
        [field:SerializeField]public DialogType DialogueType { get; set; }
        [field:SerializeField]public bool IsStartingDialogue { get; set; }
        [field:SerializeField]public bool IsPortraitLeft { get; set; }
        [field:SerializeField]public DialogActor DialogActor { get; set; }

        public virtual void Initialize(string dialogName, DialogActor dialogActor, string text, List<LGDialogChoiceData> choices, DialogType dialogType,
            bool portraitLeft, bool isStartingDialog)
        {
            DialogName = dialogName;
            Text = text;
            Choices = choices;
            DialogueType = dialogType;
            IsStartingDialogue = isStartingDialog;
            IsPortraitLeft = portraitLeft;
            DialogActor = dialogActor;
        }
        // public static List<LGDialogChoiceData> CloneNodeChoices(List<LGDialogChoiceData> nodeChoices)
        // {
        //     List<LGDialogChoiceData> choices =new List<LGDialogChoiceData>();
        //     foreach (LGDialogChoiceData choice in nodeChoices)
        //     {
        //         //Debug.Log("Clone Node Choices: "+choice.RandomRate);
        //         LGDialogChoiceData choiceSaveData = new LGDialogChoiceData()
        //         {
        //             Text = choice.Text,
        //             NextDialogue = choice.NextDialogue,
        //             NextDialogueFail = choice.NextDialogueFail,
        //             NextDialogueRandom = new List<LGDialogSO>(choice.NextDialogueRandom),
        //             ItemRequirements = new List<ItemBP>(choice.ItemRequirements),
        //             ResourceRequirements = new List<ResourceEntry>(choice.ResourceRequirements),
        //             CharacterRequirements= new List<UnitBP>(choice.CharacterRequirements),
        //             AttributeRequirements = new List<ResponseStatRequirement>(choice.AttributeRequirements),
        //             RandomRate = choice.RandomRate
        //
        //         };
        //         choices.Add(choiceSaveData);
        //     }
        //
        //     return choices;
        // }
    }
}