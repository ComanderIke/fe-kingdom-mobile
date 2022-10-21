using System;
using System.Collections.Generic;
using System.Linq;
using Game.Dialog;
using Game.GameActors.Units;
using Game.Grid;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;


    [CustomEditor(typeof(Conversation))]
    public class ConversationLine : UnityEditor.Editor
    {
        private Conversation myTarget;
        
        
        private void OnEnable()
        {
            myTarget = (Conversation)target;
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            SerializedProperty arrayProp = serializedObject.FindProperty("lines");
          //  base.OnInspectorGUI();
            arrayProp.isExpanded = EditorGUILayout.Foldout(arrayProp.isExpanded, "Lines");
            if (arrayProp.isExpanded) {
                arrayProp.arraySize = EditorGUILayout.IntField("Count", arrayProp.arraySize);
            
                for (int i = 0; i < arrayProp.arraySize; ++i) {
                    GUILayout.BeginVertical("Line "+i, GUI.skin.box);
                    GUILayout.Space(20); // spacer to account for the title
// ... your box content ...
                  
                    serializedObject.ApplyModifiedProperties();
            
                    arrayProp = serializedObject.FindProperty("lines");
                    var transformProp = arrayProp.GetArrayElementAtIndex(i);
                    
                   // EditorGUILayout.PropertyField(transformProp);
                    var sentenceProp = transformProp.FindPropertyRelative("sentence");
                    EditorGUILayout.PropertyField(sentenceProp);
                    var charNameProp = transformProp.FindPropertyRelative("actor");
                    EditorGUILayout.PropertyField(charNameProp);
                    // var lineTypeProp = transformProp.FindPropertyRelative("LineType");
                    // EditorGUILayout.PropertyField(lineTypeProp);
                    var leftProp = transformProp.FindPropertyRelative("left");
                    EditorGUILayout.PropertyField(leftProp);
                 GUILayout.EndVertical();
                    EditorGUILayout.Separator();
                    
                }
                serializedObject.ApplyModifiedProperties();
            }
        }
    }
