using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class ETMultiObjectTableWindow : EditorWindow
{
	public const float MIN_WIDTH = 150.0f;
	
	public static void OpenWindow(ETMultiObjectTable target)
	{
		ETMultiObjectTableWindow editorWindow = null;
		if (!TryGetWindowWithTarget(target, out editorWindow))
		{
			editorWindow = CreateWindowWithTarget(target);
		}
		editorWindow.Show();
		editorWindow.Focus();
	}

	public static bool TryGetWindowWithTarget(ETMultiObjectTable target, out ETMultiObjectTableWindow windowWithTarget)
	{
		foreach (ETMultiObjectTableWindow window in Resources.FindObjectsOfTypeAll<ETMultiObjectTableWindow>()) {
			if (window.target == target)
			{
				windowWithTarget = window;
				return true;
			}
		}
		windowWithTarget = null;
		return false;
	}

	private static ETMultiObjectTableWindow CreateWindowWithTarget(ETMultiObjectTable target)
	{
		ETMultiObjectTableWindow editorWindow = CreateInstance<ETMultiObjectTableWindow>();
		editorWindow.target = target;
		editorWindow.title = target.name;
		return editorWindow;
	}

	[SerializeField]
	private ETMultiObjectTable target;

	private List<SerializedObject> targetSerializedObjects;
	private Vector2 scroll;

	void ClearRows()
	{
		Undo.RecordObject(target, "Clear Rows");
		target.ClearRows();
		EditorUtility.SetDirty(target);
	}
	
	void ClearColumns()
	{
		Undo.RecordObject(target, "Clear Columns");
		target.ClearColumns();
		EditorUtility.SetDirty(target);
	}
	
	void Clear()
	{
		Undo.RecordObject(target, "Clear All");
		target.Clear();
		EditorUtility.SetDirty(target);
	}
	
	void AddColumn(string propertyPath)
	{
		Undo.RecordObject(target, "Add Column");
		target.AddColumn(propertyPath as string);
		EditorUtility.SetDirty(target);
	}
	
	void RemoveColumnAt(object index)
	{
		if (index is int)
		{
			Undo.RecordObject(target, "Remove Column");
			target.RemoveColumn((int)index);
			EditorUtility.SetDirty(target);
		}
	}
	
	void MoveColumnLeft(object index)
	{
		if (index is int)
		{
			Undo.RecordObject(target, "Move Column");
			target.SwapColumns((int)index-1, (int)index);
			EditorUtility.SetDirty(target);
		}
	}
	
	void MoveColumnRight(object index)
	{
		if (index is int)
		{
			Undo.RecordObject(target, "Move Column");
			target.SwapColumns((int)index, (int)index+1);
			EditorUtility.SetDirty(target);
		}
	}

	void AddRow(Object targetObject)
	{
		Undo.RecordObject(target, "Add Row");
		target.AddRow(targetObject);
		EditorUtility.SetDirty(target);
	}
	
	void AddEmptyRow(object index)
	{
		if (index is int)
		{
			Undo.RecordObject(target, "Add Row");
			target.InsertRow((int)index + 1, null);
			EditorUtility.SetDirty(target);
		}
	}
	
	void RemoveRowAt(object index)
	{
		if (index is int)
		{
			Undo.RecordObject(target, "Remove Row");
			target.RemoveRow((int)index);
			EditorUtility.SetDirty(target);
		}
	}
	
	void MoveRowUp(object index)
	{
		if (index is int)
		{
			Undo.RecordObject(target, "Move Row");
			target.SwapRows((int)index-1, (int)index);
			EditorUtility.SetDirty(target);
		}
	}
	
	void MoveRowDown(object index)
	{
		if (index is int)
		{
			Undo.RecordObject(target, "Move Row");
			target.SwapRows((int)index, (int)index+1);
			EditorUtility.SetDirty(target);
		}
	}

	void AddColumn(object propertyPath)
	{
		if (propertyPath is string)
		{
			AddColumn(propertyPath as string);
			EditorUtility.SetDirty(target);
		}
	}

	void AddEmptyColumn(object index)
	{
		if (index is int)
		{
			Undo.RecordObject(target, "Add Column");
			target.InsertColumn((int)index + 1, "");
			EditorUtility.SetDirty(target);
		}
	}
	
	void PingObject(object target)
	{
		if (target is Object)
		{
			EditorGUIUtility.PingObject(target as Object);
		}
	}

	List<string> GetAllPropertyPathsFromSerializedObject(SerializedObject serializedObject)
	{
		List<string> remainingPropertyPaths = new List<string>();
		if (serializedObject != null) {
			SerializedProperty prop = serializedObject.GetIterator();
			while (true) {
				remainingPropertyPaths.Add(prop.propertyPath);
				if (!prop.NextVisible(true))
					break;
			}
		}
		return remainingPropertyPaths;
	}
	
	void RebuildTargetSerializedObjects()
	{
		targetSerializedObjects = new List<SerializedObject>();
		foreach (ETMultiObjectTable.Row row in target.Rows) {
			if (row.Target != null) {
				targetSerializedObjects.Add(new SerializedObject(row.Target));
			}
			else
			{
				targetSerializedObjects.Add(null);
			}
		}
	}
	
	bool CheckTargetSerializedObjects()
	{
		if (targetSerializedObjects == null || targetSerializedObjects.Count != target.Rows.Count) {
			RebuildTargetSerializedObjects();
			return true;
		}
		for (int i = 0; i < target.Rows.Count; i++) {
			Object targetObject = (targetSerializedObjects[i] == null) ? null : targetSerializedObjects[i].targetObject;
			if (targetObject != target.Rows[i].Target) {
				RebuildTargetSerializedObjects();
				return true;
			}
		}
		return false;
	}
	
	void OnInspectorUpdate()
	{
		Repaint();
	}

	void Update()
	{
		if (target == null) {
			Close();
			return;
		}

		if (title != target.name)
		{
			title = target.name;
		}
	}

	void OnGUI()
	{
		GUILayout.BeginHorizontal();
		Rect cornerRect = GUILayoutUtility.GetRect(MIN_WIDTH, EditorGUIUtility.singleLineHeight);
		Rect columnsNames = GUILayoutUtility.GetRect(0, float.MaxValue, EditorGUIUtility.singleLineHeight, EditorGUIUtility.singleLineHeight);
		GUILayout.EndHorizontal();
		
		CheckTargetSerializedObjects();
		GUILayout.BeginHorizontal();
		Rect rowsNames = GUILayoutUtility.GetRect(MIN_WIDTH, MIN_WIDTH, 0, MIN_WIDTH, GUILayout.MaxWidth(MIN_WIDTH));
		Rect tableRect = GUILayoutUtility.GetRect(0, 0, float.MaxValue, float.MaxValue);
		GUILayout.EndHorizontal();
		
		// Clamp size to window size
		rowsNames.yMax = position.height;
		tableRect.yMax = position.height;

		// Table size
		float tableSizeY = target.GetTableHeight();
		float tableSizeX = target.GetTableWidth();

		// Table cells
		scroll = GUI.BeginScrollView(tableRect, scroll, new Rect(0, 0, tableSizeX, tableSizeY));
		float tablePositionY = 0;
		for (int i = 0; i < target.Rows.Count && i < targetSerializedObjects.Count; i++)
		{
			float rowHeight = target.GetRowHeight(i);
			SerializedObject obj = targetSerializedObjects[i];
			if (obj != null)
			{
				obj.Update();
				float tablePositionX = 0;
				for (int j = 0; j < target.Columns.Count; j++) {
					ETMultiObjectTable.Column column = target.Columns[j];
					float columnWidth = target.GetColumnWidth(j);
					SerializedProperty property = obj.FindProperty(column.PropertyPath);
					if (property != null) {
						EditorGUI.PropertyField(new Rect(tablePositionX, tablePositionY, columnWidth, rowHeight),
							property, GUIContent.none, true);
					}
					tablePositionX += columnWidth;
				}
				obj.ApplyModifiedProperties();
			}
			tablePositionY += rowHeight;
		}
		GUI.EndScrollView();

		// Corner Button
		if (GUI.Button(cornerRect, "", EditorStyles.miniButtonMid)) {
			GenericMenu menu = new GenericMenu();
			menu.AddItem(new GUIContent("Clear Rows"), false, ClearRows);
			menu.AddItem(new GUIContent("Clear Columns"), false, ClearColumns);
			menu.AddItem(new GUIContent("Clear All"), false, Clear);
			menu.ShowAsContext();
		}

		// Columns names
		GUI.changed = false;
		GUI.BeginGroup(columnsNames);
		float currentX = -scroll.x;
		for (int i = 0; i < target.Columns.Count; i++) {
			float columnWidth = target.GetColumnWidth(i);
			int controlId = EditorGUIUtility.GetControlID(FocusType.Passive);
			Rect resizeRect = new Rect(currentX + columnWidth - 5, 0, 10, EditorGUIUtility.singleLineHeight);
			EditorGUIUtility.AddCursorRect(resizeRect, MouseCursor.ResizeHorizontal, controlId);
			switch(Event.current.type)
			{
				case EventType.MouseDown:
				{
					if (resizeRect.Contains(Event.current.mousePosition))
					{
						GUIUtility.hotControl = controlId;
						Event.current.Use();
					}
					break;
				}
				case EventType.MouseDrag:
				{
					if (GUIUtility.hotControl == controlId)
					{
						Undo.RecordObject(target, "Resize Column");
						target.SetColumnWidth(i, Event.current.mousePosition.x - currentX);
						Event.current.Use();
					}
					break;
				}
				case EventType.MouseUp:
				{
					if (GUIUtility.hotControl == controlId)
					{
						Undo.CollapseUndoOperations(Undo.GetCurrentGroup());
						GUIUtility.hotControl = 0;
						EditorUtility.SetDirty(target);
						Event.current.Use();
					}
					break;
				}
			}
			if (GUI.Button(new Rect(currentX, 0, columnWidth, EditorGUIUtility.singleLineHeight), target.GetColumnName(i), EditorStyles.miniButtonMid))
			{
				GenericMenu menu = new GenericMenu();
				menu.AddItem(new GUIContent("Remove Column"), false, RemoveColumnAt, i);
				menu.AddItem(new GUIContent("Add Empty Column"), false, AddEmptyColumn, i);
				menu.AddItem(new GUIContent("Move Left"), false, MoveColumnLeft, i);
				menu.AddItem(new GUIContent("Move Right"), false, MoveColumnRight, i);
				menu.ShowAsContext();
			}
			currentX += columnWidth;
		}
		GUI.EndGroup();
		
		// Rows names
		GUI.BeginGroup(rowsNames);
		float currentY = -scroll.y;
		for (int i = 0; i < target.Rows.Count; i++)
		{
			float rowHeight = target.GetRowHeight(i);
			int controlId = EditorGUIUtility.GetControlID(FocusType.Passive);
			Rect resizeRect = new Rect(0, currentY + rowHeight - 5, MIN_WIDTH, 10);
			EditorGUIUtility.AddCursorRect(resizeRect, MouseCursor.ResizeVertical, controlId);
			switch(Event.current.type)
			{
				case EventType.MouseDown:
				{
					if (resizeRect.Contains(Event.current.mousePosition))
					{
						GUIUtility.hotControl = controlId;
						Event.current.Use();
					}
					break;
				}
				case EventType.MouseDrag:
				{
					if (GUIUtility.hotControl == controlId)
					{
						Undo.RecordObject(target, "Resize Row");
						target.SetRowHeight(i, Event.current.mousePosition.y - currentY);
						Event.current.Use();
					}
					break;
				}
				case EventType.MouseUp:
				{
					if (GUIUtility.hotControl == controlId)
					{
						Undo.CollapseUndoOperations(Undo.GetCurrentGroup());
						GUIUtility.hotControl = 0;
						EditorUtility.SetDirty(target);
						Event.current.Use();
					}
					break;
				}
			}
			ETMultiObjectTable.Row row = target.Rows[i];
			Rect rowButtonRect = new Rect(0, currentY, MIN_WIDTH, rowHeight);
			if (Event.current.type == EventType.MouseDrag && rowButtonRect.Contains(Event.current.mousePosition) && row.Target != null)
			{
				DragAndDrop.PrepareStartDrag();
				DragAndDrop.objectReferences = new Object[]{ row.Target };
				DragAndDrop.StartDrag("Dragging " + row.Target.ToString());
				Event.current.Use();
			}
			if (GUI.Button(new Rect(0, currentY, MIN_WIDTH, rowHeight), target.GetRowName(i), EditorStyles.miniButtonMid))
			{
				GenericMenu menu = new GenericMenu();
				if (row.Target != null)
				{
					menu.AddItem(new GUIContent("Show in Project"), false, PingObject, row.Target);
				}
				menu.AddItem(new GUIContent("Remove Row"), false, RemoveRowAt, i);
				menu.AddItem(new GUIContent("Add Empty Row"), false, AddEmptyRow, i);
				if (row.Target != null)
				{
					List<string> objectProperties = GetAllPropertyPathsFromSerializedObject(targetSerializedObjects[i]);
					List<string> usedProperties = target.GetAllProperties();
					objectProperties.RemoveAll(s => usedProperties.Contains(s));
					foreach(string propertyPath in objectProperties)
					{
						menu.AddItem(new GUIContent("Add Column/" + propertyPath), false, AddColumn, propertyPath);
					}
				}
				menu.AddItem(new GUIContent("Move Up"), false, MoveRowUp, i);
				menu.AddItem(new GUIContent("Move Down"), false, MoveRowDown, i);
				menu.ShowAsContext();
			}
			currentY += rowHeight;
		}
		GUI.EndGroup();

		// Hack to unfocus selected control
		if (GUI.changed)
			GUI.FocusControl("");

		if (Event.current.type == EventType.DragUpdated || Event.current.type == EventType.DragPerform) {
			// show a drag-add icon on the mouse cursor
			DragAndDrop.visualMode = DragAndDropVisualMode.Copy;

			if (Event.current.type == EventType.DragPerform) {
				bool accepted = false;
				
				List<Object> targets = target.GetAllTargets();
				foreach (UnityEngine.Object obj in DragAndDrop.objectReferences) {
					if (AssetDatabase.Contains(obj)) {
						if (!targets.Contains(obj)) {
							// First Accepted -> Register Undo
							if (accepted == false) {
								Undo.RecordObject(target, "Add Rows");
							}
							target.AddRow(obj);
							accepted = true;
						}
					}
				}
				
				if (accepted) {
					DragAndDrop.AcceptDrag();
					EditorUtility.SetDirty(target);
				}
			}
			
			Event.current.Use();
		}

		// Hack to unfocus selected control
		GUI.SetNextControlName("");
		GUI.Button(new Rect(-10, -10, 0, 0), "");
	}
}
