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
    private void OnEnable()
    {
        this.mytarget = (MapSystem)target;
    }

    public override void OnInspectorGUI()
    {
        if(GUILayout.Button("Build Grid"))
        {
            int width = GameObject.FindObjectOfType<DataScript>().mapData.width;
            int height = GameObject.FindObjectOfType<DataScript>().mapData.height;
            mytarget.GridBuilder = new GridBuilder();
            mytarget.GridBuilder.Build(width, height, mytarget.gridTransform);
        }
        base.OnInspectorGUI();
    }


}

