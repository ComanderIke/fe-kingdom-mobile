using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class ETMultiObjectTable : ScriptableObject
{
	[System.Serializable]
	public class Row
	{
		[SerializeField] private string _displayName;
		[SerializeField] private Object _target;
		[SerializeField] private float _height;

		public Row(Object target)
		{
			_target = target;
		}

		public string DisplayName {
			get {
				if (!string.IsNullOrEmpty(_displayName))
					return _displayName;
				if (_target != null)
					return _target.name;
				return "";
			}
			set { _displayName = value; }
		}
		
		public Object Target { get { return _target; } set { _target = value; } }
		public float Height { get { return _height; } set { _height = value; } }
	}
	
	[System.Serializable]
	public class Column
	{
		[SerializeField] private string _displayName;
		[SerializeField] private string _propertyPath;
		[SerializeField] private float _width;

		static string NicifyPropertyPath(string propertyPath)
		{
			string[] parts = propertyPath.Split(new char[1]{'.'}, 100);
			if (parts.Length == 0)
				return "";
			string result = ObjectNames.NicifyVariableName(parts[0]);
			for(int i = 1; i < parts.Length; i++)
			{
				result += '.';
				result += ObjectNames.NicifyVariableName(parts[i]);
			}
			return result;
		}

		public Column(string propertyPath)
		{
			_propertyPath = propertyPath;
			_displayName = NicifyPropertyPath(_propertyPath);
		}
		
		public string DisplayName {
			get {
				if (!string.IsNullOrEmpty(_displayName))
					return _displayName;
				if (!string.IsNullOrEmpty(_propertyPath))
					return _propertyPath;
				return "";
			}
			set { _displayName = value; }
		}
		
		public string PropertyPath { get { return _propertyPath; } set { _propertyPath = value; } }
		public float Width { get { return _width; } set { _width = value; } }
	}
	
	[SerializeField] private List<Row> _rows = new List<Row>();
	[SerializeField] private List<Column> _columns = new List<Column>();
	[SerializeField] private float _defaultRowHeight = 16.0f;
	[SerializeField] private float _defaultColumnWidth = 150.0f;
	private float _cachedTableHeight = -1;
	private float _cachedTableWidth = -1;

	public List<Row> Rows { get { return _rows; } }
	public List<Column> Columns { get { return _columns; } }

	public float DefaultRowHeight
	{
		get { return _defaultRowHeight; }
		set {
			_defaultRowHeight = value;
			_cachedTableHeight = -1;
		}
	}
	
	public float DefaultColumnWidth
	{
		get { return _defaultColumnWidth; }
		set {
			_defaultColumnWidth = value;
			_cachedTableWidth = -1;
		}
	}
	
	public void Clear()
	{
		ClearRows();
		ClearColumns();
	}
	
	public void ClearRows()
	{
		_rows = new List<Row>();
		_cachedTableHeight = -1;
	}
	
	public void ClearColumns()
	{
		_columns = new List<Column>();
		_cachedTableWidth = -1;
	}
	
	public void AddRow(Object target)
	{
		_rows.Add(new Row(target));
		_cachedTableHeight = -1;
	}
	
	public void InsertRow(int index, Object target)
	{
		_rows.Insert(index, new Row(target));
		_cachedTableHeight = -1;
	}
	
	public void RemoveRow(int index)
	{
		if (index < 0 || index >= _rows.Count)
			return;
		_rows.RemoveAt(index);
		_cachedTableHeight = -1;
	}
	
	public void SwapRows(int index1, int index2)
	{
		if (index1 < 0 || index1 >= _rows.Count || index2 < 0 || index2 >= _rows.Count || index1 == index2)
			return;
		Row temp = _rows[index1];
		_rows[index1] = _rows[index2];
		_rows[index2] = temp;
	}

	public void AddColumn(string propertyPath)
	{
		_columns.Add(new Column(propertyPath));
		_cachedTableWidth = -1;
	}
	
	public void InsertColumn(int index, string propertyPath)
	{
		_columns.Insert(index, new Column(propertyPath));
		_cachedTableWidth = -1;
	}

	public void RemoveColumn(int index)
	{
		if (index < 0 || index >= _columns.Count)
			return;
		_columns.RemoveAt(index);
		_cachedTableWidth = -1;
	}
	
	public void SwapColumns(int index1, int index2)
	{
		if (index1 < 0 || index1 >= _columns.Count || index2 < 0 || index2 >= _columns.Count || index1 == index2)
			return;
		Column temp = _columns[index1];
		_columns[index1] = _columns[index2];
		_columns[index2] = temp;
	}
	
	public float GetRowHeight(int index)
	{
		if (index < 0 || index >= _rows.Count || _rows[index] == null || _rows[index].Height <= 0)
			return _defaultRowHeight;
		return _rows[index].Height;
	}

	public float GetColumnWidth(int index)
	{
		if (index < 0 || index >= _columns.Count || _columns[index] == null || _columns[index].Width <= 0)
			return _defaultColumnWidth;
		return _columns[index].Width;
	}
	
	public void SetRowHeight(int index, float height)
	{
		if (index < 0 || index >= _rows.Count || _rows[index] == null)
			return;
		_rows[index].Height = Mathf.Max(0, height);
		_cachedTableHeight = -1;
	}
	
	public void SetColumnWidth(int index, float width)
	{
		if (index < 0 || index >= _columns.Count || _columns[index] == null)
			return;
		_columns[index].Width = Mathf.Max(0, width);
		_cachedTableWidth = -1;
	}

	public string GetRowName(int index)
	{
		if (index < 0 || index >= _rows.Count || _rows[index] == null)
			return "";
		return _rows[index].DisplayName;
	}

	public string GetColumnName(int index)
	{
		if (index < 0 || index >= _columns.Count || _columns[index] == null)
			return "";
		return _columns[index].DisplayName;
	}
	
	public float GetTableHeight()
	{
		if (_cachedTableHeight < 0)
		{
			_cachedTableHeight = 0;
			for (int i = 0; i < _rows.Count; i++)
			{
				_cachedTableHeight += GetRowHeight(i);
			}
		}
		return _cachedTableHeight;
	}
	
	public float GetTableWidth()
	{
		if (_cachedTableWidth < 0)
		{
			_cachedTableWidth = 0;
			for (int i = 0; i < _columns.Count; i++)
			{
				_cachedTableWidth += GetColumnWidth(i);
			}
		}
		return _cachedTableWidth;
	}

	public List<Object> GetAllTargets()
	{
		return _rows.ConvertAll<Object>(row => row.Target).Distinct().ToList();
	}
	
	public List<string> GetAllProperties()
	{
		return _columns.ConvertAll<string>(column => column.PropertyPath).Distinct().ToList();
	}

	void OnEnable()
	{
		_cachedTableHeight = -1;
		_cachedTableWidth = -1;
	}
}
