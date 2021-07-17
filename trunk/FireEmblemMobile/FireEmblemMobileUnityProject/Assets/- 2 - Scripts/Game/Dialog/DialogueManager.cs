using System;
using System.Collections.Generic;
using Game.Mechanics;
using UnityEngine;
using Unused.Dialogs;

namespace Game.Dialog
{
    public class DialogueManager : MonoBehaviour
    {
        public Conversation Conversation;

        public Dialogue leftDialog;
        public Dialogue rightDialog;
        private int index=-1;
        private void Start()
        {
            // foreach (var line in Conversation.lines)
            // {
            //     Debug.Log(line.sentence);
            //     if (line.LineType==LineType.MultipleChoice)
            //     {
            //         foreach(var option in line.options)
            //             Debug.Log("Option: " +option);
            //     }
            // }
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
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
                if (line.left)
                {
                    leftDialog.gameObject.SetActive(true);
                    rightDialog.gameObject.SetActive(false);
                    leftDialog.NextLine(line.sentence, line.unit.name, line.unit.visuals.CharacterSpriteSet.FaceSprite);
                }
                else
                {
                    leftDialog.gameObject.SetActive(false);
                    rightDialog.gameObject.SetActive(true);
                    rightDialog.NextLine(line.sentence,line.unit.name, line.unit.visuals.CharacterSpriteSet.FaceSprite );
                }
            }
            else
            {
                Debug.Log("Dialog end!");
            }
        }
    }
}