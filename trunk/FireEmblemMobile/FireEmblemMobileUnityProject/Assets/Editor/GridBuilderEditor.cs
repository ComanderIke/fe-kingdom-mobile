using Assets.Scripts.Grid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MapSystem))]
public class GridBuilderEditor : Editor
{
    private MapSystem mytarget;
    private MapData mapData;
    private void OnEnable()
    {
        this.mytarget = (MapSystem)target;
        mapData = GameObject.FindObjectOfType<DataScript>().mapData;
    }

    public override void OnInspectorGUI()
    {
        mapData.width = EditorGUILayout.IntSlider("Width:", mapData.width, 6, 24);
        mapData.height = EditorGUILayout.IntSlider("Height:", mapData.height, 8, 24);
        if (GUILayout.Button("Build Grid"))
        {
            int width = mapData.width;
            int height = mapData.height;
            mytarget.GridBuilder = new GridBuilder();
            mytarget.GridBuilder.Build(width, height, mytarget.gridTransform);
        }
        base.OnInspectorGUI();
    }


}

