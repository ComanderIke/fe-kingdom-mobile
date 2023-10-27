using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(ETMultiObjectTable))]
public class ETMultiObjectTableEditor : Editor
{
	[MenuItem("Assets/Create/Multi Object Table")]
	static void CreateAtCurrentFolder()
	{
		ETMultiObjectTable table = ETUtility.CreateAsset<ETMultiObjectTable>();
		table.DefaultRowHeight = EditorGUIUtility.singleLineHeight;
		EditorUtility.SetDirty(table);
	}
	
	public override void OnInspectorGUI()
	{
		if (GUILayout.Button("Open Table Editor"))
		{
			ETMultiObjectTableWindow.OpenWindow(target as ETMultiObjectTable);
		}
	}
}
