using System;
using System.Collections;
using System.Collections.Generic;
using __2___Scripts.External.Editor.Data.Save;
using __2___Scripts.External.Editor.Elements;
using UnityEditor.UIElements;
using UnityEngine;


[Serializable]
public class LGNodeSaveData 
{
    [field:SerializeField]public string ID { get; set; }
    [field:SerializeField] public string Name { get; set; }
    [field:SerializeField] public string Text { get; set; }
    [field:SerializeField] public bool IsPortraitLeft { get; set; }
    [field:SerializeField] public DialogActor DialogActor { get; set; }
    [field:SerializeField] public List<LGChoiceSaveData> Choices { get; set; }
    [field:SerializeField] public string GroupID { get; set; }
    [field:SerializeField] public DialogType DialgueType { get; set; }
    [field:SerializeField] public Vector2 Position { get; set; }
}
