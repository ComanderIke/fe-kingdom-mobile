using System.Collections.Generic;
using _02_Scripts.EditorScripts.DialogueSystem.Elements;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace _02_Scripts.EditorScripts.DialogueSystem.Data.Error
{
    public class LGGroupErrorData
    {
        public LGErrorData ErrorData { get; set; }
        public List<DialogGroup> Groups { get; set; }

        public LGGroupErrorData()
        {
            ErrorData = new LGErrorData();
            Groups = new List<DialogGroup>();
        }
        
    }
}