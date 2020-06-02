using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Grid3DMaker))]
public class GridGeneratorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        if (GUILayout.Button("Refresh"))
        {
            ((Grid3DMaker)target).Refresh();
        }

        EditorStyles.label.wordWrap = true;
        EditorGUILayout.LabelField("Adjust 'Tile Preview Cam' position and location so that it renders top-left tile clearly");
    }

    [MenuItem("UniTiled/Create 3D Grid")]
    public static void AddScriptToObject()
    {
        if (Selection.activeTransform != null)
        {
            Vector3 pos = Selection.activeTransform.position;
            GameObject gridRoot = new GameObject("UniTiled 3D Grid");
            gridRoot.transform.position = pos;
            Grid3DMaker gridMaker = gridRoot.AddComponent<Grid3DMaker>();
            gridRoot.AddComponent<TileCreator>();
            foreach(Transform child in Selection.GetTransforms(SelectionMode.TopLevel)){
                child.parent = gridRoot.transform;
            }
            gridMaker.Refresh();
        }

    }
}
