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
            Debug.Log("OnInspectorGUI: "+mytarget.width+" "+mytarget.height);
            // mytarget.width = EditorGUILayout.IntSlider("Width:", mytarget.width, 6, 24);
            // mytarget.height = EditorGUILayout.IntSlider("Height:", mytarget.height, 6, 24);
            if (GUILayout.Button("Build Grid"))
            {
                mytarget.Build(mytarget.width, mytarget.height);
            }
            
        }


    }


