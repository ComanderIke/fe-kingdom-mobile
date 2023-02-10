using System;
using UnityEngine;

namespace __2___Scripts.External.Editor.Data.Save
{
    [Serializable]
    public class LGChoiceSaveData
    {
        
        [field: SerializeField] public string Text { get; set; }
       [field: SerializeField] public string NodeID { get; set; }
    }
}