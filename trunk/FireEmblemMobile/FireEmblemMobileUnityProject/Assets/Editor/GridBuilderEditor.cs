
using Assets.GameResources;
using Assets.Grid;
using Assets.Map;
using UnityEditor;
using UnityEngine;

namespace Assets.Editor
{
    [CustomEditor(typeof(MapSystem))]
    public class GridBuilderEditor : UnityEditor.Editor
    {
        private MapSystem mytarget;
        private MapData mapData;
        private void OnEnable()
        {
            mytarget = (MapSystem)target;
            mapData = FindObjectOfType<DataScript>().MapData;
        }

        public override void OnInspectorGUI()
        {
            mapData.Width = EditorGUILayout.IntSlider("Width:", mapData.Width, 6, 24);
            mapData.Height = EditorGUILayout.IntSlider("Height:", mapData.Height, 8, 24);
            if (GUILayout.Button("Build Grid"))
            {
                int width = mapData.Width;
                int height = mapData.Height;
                mytarget.GridBuilder = new GridBuilder(mytarget.GridResources.GridSprite, mytarget.GridResources.CellMaterialValid, mytarget.GridResources.CellMaterialInvalid);
                mytarget.GridBuilder.Build(width, height, mytarget.GridTransform);
            }
            base.OnInspectorGUI();
        }


    }
}

