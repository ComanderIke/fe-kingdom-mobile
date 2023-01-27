using UnityEngine;

namespace _02_Scripts.EditorScripts.DialogueSystem.Data.Error
{
    public class LGErrorData
    {
        public Color Color { get; set; }

        public LGErrorData()
        {
            GenerateRandomColor();
        }

        private void GenerateRandomColor()
        {
            Color = new Color32((byte)Random.Range(65, 256),(byte)Random.Range(50, 176),(byte)Random.Range(50, 176),255);
        }
    }
}