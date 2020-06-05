using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TileCreator))]
public class TileCreatorEditor : Editor
{
    private bool isToggleOn;

    public override void OnInspectorGUI()
    {

		DrawDefaultInspector();
		if(GUILayout.Button("Start Tile Generation")){
			UnityEditor.EditorApplication.isPlaying = true;
		}
    }
}