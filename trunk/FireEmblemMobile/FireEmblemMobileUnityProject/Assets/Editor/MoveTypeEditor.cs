using System;
using Game.GameActors.Units;
using Game.Grid;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    [CustomEditor(typeof(MoveType))]
    public class MoveTypeEditor : UnityEditor.Editor
    {
        private MoveType mytarget;

        private void OnEnable()
        {
            mytarget = (MoveType)target;
        }
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
           
            int x = 0;
            int column = 0;
            int maxColumn = 4;
            EditorGUILayout.LabelField("Movement Cost: ");
            EditorGUILayout.BeginHorizontal();
            foreach (TerrainType terrainType in (TerrainType[]) Enum.GetValues(typeof(TerrainType)))
            {
                var icon = Resources.Load<Texture2D>("TerrainIcons/"+terrainType.ToString());
                GUILayout.Box (icon, GUILayout.Width(40), GUILayout.Height(40));
                EditorGUILayout.IntField(x,GUILayout.Width(40), GUILayout.Height(40));
                if (column >= maxColumn)
                {
                    column = 0;
                    EditorGUILayout.EndHorizontal();
                    GUILayout.Space(1);
                    EditorGUILayout.BeginHorizontal();
                }
                else
                {
                    column++;
                }
                
            }
            EditorGUILayout.EndHorizontal();

            
            
        }
    }
}