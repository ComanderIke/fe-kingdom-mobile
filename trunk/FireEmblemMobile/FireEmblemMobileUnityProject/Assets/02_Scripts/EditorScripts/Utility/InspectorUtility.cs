using System;
using UnityEditor;

namespace _02_Scripts.EditorScripts.Utility
{
    public static class InspectorUtility
    {
        public static void DrawHeader(string label)
        {
            EditorGUILayout.LabelField(label, EditorStyles.boldLabel);
        }
        public static void DrawPropertyField(this SerializedProperty serializedProperty)
        {
            EditorGUILayout.PropertyField(serializedProperty);
        }

        public static int DrawPopup(string label, SerializedProperty selectedIndexProperty, string[] options)
        {
            return EditorGUILayout.Popup(label, selectedIndexProperty.intValue, options);
        }
        public static int DrawPopup(string label, int selectedIndex, string[] options)
        {
            return EditorGUILayout.Popup(label, selectedIndex, options);
        }

        public static void DrawSpace(int amount=4)
        {
            EditorGUILayout.Space(amount);
        }

        public static void DrawHelpBox(string message, MessageType messageType = MessageType.Info, bool wide = true)
        {
            EditorGUILayout.HelpBox(message, messageType, wide);
        }

        public static void DrawDisabledField(Action action)
        {
            EditorGUI.BeginDisabledGroup(true);
            action?.Invoke();
            EditorGUI.EndDisabledGroup();
        }
    }
}