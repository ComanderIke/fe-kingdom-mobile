using UnityEngine;
using UnityEditor;
using System.IO;
 
public static class ETUtility
{
	public static T CreateAsset<T>() where T : ScriptableObject
	{
		T asset = ScriptableObject.CreateInstance<T>();
 
		string path = "Assets";
		Object[] selectedAssets = Selection.GetFiltered(typeof(UnityEngine.Object), SelectionMode.Assets | SelectionMode.TopLevel);
		if (selectedAssets.Length > 0)
		{
			UnityEngine.Object obj = selectedAssets[0];
			path = AssetDatabase.GetAssetPath(obj);
			if (File.Exists(path))
			{
				path = Path.GetDirectoryName(path);
			}
		}
 
		string assetPathAndName = AssetDatabase.GenerateUniqueAssetPath(path + "/New " + typeof(T).ToString() + ".asset");
 
		AssetDatabase.CreateAsset (asset, assetPathAndName);
 
		AssetDatabase.SaveAssets();
		EditorUtility.FocusProjectWindow();
		Selection.activeObject = asset;

		return asset;
	}
}