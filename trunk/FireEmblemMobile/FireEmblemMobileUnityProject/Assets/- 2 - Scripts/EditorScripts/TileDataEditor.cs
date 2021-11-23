using System;
using System.Collections.Generic;
using System.Linq;
using Game.GameActors.Units;
using Game.Grid;
using Game.Map;
using UnityEditor;
using UnityEngine;


    [CustomEditor(typeof(TileData))]
    public class TileDataEditor : UnityEditor.Editor
    {
        private TileData myTarget;
        
        
        private void OnEnable()
        {
            myTarget = (TileData) target;

            // if (myTarget.movementCostSerialized == null)
            //     myTarget.movementCostSerialized = new List<int>();
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            // int column = 0;
            // int maxColumn = 4;
            // EditorGUILayout.LabelField("Movement Cost: ");
            // EditorGUILayout.BeginHorizontal();
            // int cnt = 0;
            // foreach (var moveType in (MoveType[]) Enum.GetValues(typeof(MoveType)))
            // {
            // var icon = Resources.Load<Texture2D>("MoveIcons/" + moveType.ToString());
            // GUILayout.Box(icon, GUILayout.Width(40), GUILayout.Height(40));
            // while (myTarget.movementCostSerialized.Count <= cnt)
            // {
            //     myTarget.movementCostSerialized.Add(0);
            // }
            // myTarget.movementCostSerialized[cnt] = EditorGUILayout.IntField(myTarget.movementCostSerialized[cnt],
            //     GUILayout.Width(40), GUILayout.Height(40));
            // //
            // cnt++;
            //
            // if (column >= maxColumn)
            // {
            //     column = 0;
            //     EditorGUILayout.EndHorizontal();
            //     GUILayout.Space(1);
            //     EditorGUILayout.BeginHorizontal();
            // }
            // else
            // {
            //     column++;
            // }
            // }
            //
            // EditorGUILayout.EndHorizontal();
        }
    
}