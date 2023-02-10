using System;
using System.Collections.Generic;
using UnityEngine;


namespace __2___Scripts.External.Editor.Data.Save
{
    [Serializable]
    public class LGGroupSaveData
    {
        [field:SerializeField] public string ID { get; set; }
        [field:SerializeField] public string Name { get; set; }
        [field:SerializeField] public Vector2 Position { get; set; }
    }
}