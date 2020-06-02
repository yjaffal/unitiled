using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditorScriptsController : MonoBehaviour {
	public static bool enableSpriteImporter2D = false;
	public static bool importSpriteSheet = false;
	public static TiledMap targetMap = null;
	public static int targetTileset = 0;
	public static FilterMode textureFilterMode = (FilterMode)(-1);
}
