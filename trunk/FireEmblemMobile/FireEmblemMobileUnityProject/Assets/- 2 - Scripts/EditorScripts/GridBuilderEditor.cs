using Game.GameResources;
using Game.Grid;
using Game.Map;
using UnityEditor;
using UnityEngine;


    [CustomEditor(typeof(GridBuilder))]
    public class GridBuilderEditor : UnityEditor.Editor
    {
        
        private GridBuilder mytarget;
       
        private void OnEnable()
        {
            mytarget = (GridBuilder)target;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            // mytarget.gridData.width = EditorGUILayout.IntSlider("Width:", mytarget.gridData.width, 6, 24);
            // mytarget.gridData.height = EditorGUILayout.IntSlider("Height:", mytarget.gridData.height, 6, 24);
            // if (GUILayout.Button("Build Grid"))
            // {
            //     mytarget.Build(mytarget.gridData.width, mytarget.gridData.height);
            // }
            
        }


    }


