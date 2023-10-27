using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomPropertyDrawer(typeof(Cost))]
public class CostPropertyDrawer : PropertyDrawer {
	public override void OnGUI (Rect position, SerializedProperty property, GUIContent label) {
		// Using BeginProperty / EndProperty on the parent property means that
		// prefab override logic works on the entire property.
		EditorGUI.BeginProperty (position, label, property);
		
		// Draw label
		position = EditorGUI.PrefixLabel (position, GUIUtility.GetControlID (FocusType.Passive), label);
		
		// Don't make child fields be indented
		var indent = EditorGUI.indentLevel;
		EditorGUI.indentLevel = 0;
		
		// Calculate rects
		var goldRect = new Rect (position.x, position.y, position.width/2, position.height);
		var silverRect = new Rect (position.x+position.width/2, position.y, position.width/2, position.height);

		// Draw fields
		EditorGUIUtility.labelWidth = 20;
		EditorGUI.PropertyField (goldRect, property.FindPropertyRelative ("gold"), new GUIContent("G"));
		EditorGUI.PropertyField (silverRect, property.FindPropertyRelative ("silver"), new GUIContent("S"));

		// Set indent back to what it was
		EditorGUI.indentLevel = indent;
		
		EditorGUI.EndProperty ();
	}
}
