using System.Collections.Generic;
using UnityEngine;

namespace Game.Dialog
{
    public class DialogueManager : MonoBehaviour
    {
        public Queue<string> sentences;
        public Conversation Conversation;

        private void Start()
        {
            foreach (var line in Conversation.lines)
            {
                Debug.Log(line.sentence);
                if (line.LineType==LineType.MultipleChoice)
                {
                    foreach(var option in line.options)
                        Debug.Log("Option: " +option);
                }
            }
        }
    }
}