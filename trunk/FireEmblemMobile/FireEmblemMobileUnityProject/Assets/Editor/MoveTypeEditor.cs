using System;
using System.Collections.Generic;
using Game.GameActors.Units;
using Game.Grid;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    [CustomEditor(typeof(MoveType))]
    public class MoveTypeEditor : UnityEditor.Editor
    {
        private MoveType myTarget;

        private void OnEnable()
        {
            myTarget = (MoveType)target;
            myTarget.movementCosts = new Dictionary<TerrainType, int>();
            foreach (var terrainType in (TerrainType[]) Enum.GetValues(typeof(TerrainType)))
            {
                myTarget.movementCosts.Add(terrainType, 1);
            }
        }
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            
            int column = 0;
            int maxColumn = 4;
            EditorGUILayout.LabelField("Movement Cost: ");
            EditorGUILayout.BeginHorizontal();
            foreach (var terrainType in (TerrainType[]) Enum.GetValues(typeof(TerrainType)))
            {
                var icon = Resources.Load<Texture2D>("TerrainIcons/"+terrainType.ToString());
                GUILayout.Box (icon, GUILayout.Width(40), GUILayout.Height(40));
                myTarget.movementCosts[terrainType] = EditorGUILayout.IntField(myTarget.movementCosts[terrainType],
                        GUILayout.Width(40), GUILayout.Height(40));


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