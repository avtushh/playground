using UnityEngine;
using System.Collections;
using UnityEditor;

public class CreateScriptableObject {

	[MenuItem("Assets/Create/ShapesObject")]
	public static void CreateMyAsset()
	{
		ShapesList asset = ScriptableObject.CreateInstance<ShapesList>();

		AssetDatabase.CreateAsset(asset, "Assets/GrindMe/ShapesList.asset");
		AssetDatabase.SaveAssets();

		EditorUtility.FocusProjectWindow();

		Selection.activeObject = asset;
	}
}