using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Dialog
{
    public class DialogueManager : MonoBehaviour
    {
        public Conversation Conversation;

        public Dialogue dialog;
        private int index=-1;
        public Action dialogEnd;
        private bool active = false;

        public void ShowDialog(Conversation conversation)
        {
            this.Conversation = conversation;
            active = true;
            index = -1;
            dialog.Show();
            NextLine();
        }

        

        private void Update()
        {
            if (active && Input.GetMouseButtonDown(0))
            {
                NextLine();
            }
        }

        void NextLine()
        {
            if (Conversation == null)
                return;
            if (index < Conversation.lines.Count - 1)
            {
                index++;
                var line = Conversation.lines[index];
                
                dialog.NextLine(line.sentence, line.Actor.Name, line.Actor.FaceSprite, line.left);
                
               
            }
            else
            {
                Debug.Log("Dialog end!");
                dialogEnd?.Invoke();
                dialog.gameObject.SetActive(false);
                active = false;

            }
        }
    }
}