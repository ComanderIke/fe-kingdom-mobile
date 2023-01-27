using System.Collections.Generic;
using _02_Scripts.EditorScripts.DialogueSystem.Elements;
using UnityEngine;

namespace _02_Scripts.EditorScripts.DialogueSystem.Data.Error
{
    public class LGNodeErrorData
    {
        public LGErrorData ErrorData { get; set; }
        public List<DialogNode> Nodes { get; set; }

        public LGNodeErrorData()
        {
            ErrorData = new LGErrorData();
            Nodes = new List<DialogNode>();
        }
        
    }
}