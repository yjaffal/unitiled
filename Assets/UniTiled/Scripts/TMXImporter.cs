#if (UNITY_EDITOR)
using UnityEngine;
using UnityEditor.Experimental.AssetImporters;
using UnityEditor;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.Xml;
using System.IO;
using System;

/// <summary>
/// Imports map data from a TMX file, deserializes its XML content, and generates
/// the matching 2D map on the XY plane
/// Runs in edit mode and can be used in play mode as well
/// </summary>

[ScriptedImporter(1, "tmx")]
public class TMXImporter : ScriptedImporter
{

    private static char[] FILEPATH_SEPARATORS = new char[] { '/', '\\' };
    private static char[] ROW_SEPARATOR = "\n".ToCharArray();
    private static char[] COMMA_SEPARATOR = ",".ToCharArray();

    private const uint FLIPPED_HORIZONTALLY_FLAG = 0x80000000;
    private const uint FLIPPED_VERTICALLY_FLAG = 0x40000000;
    private const uint FLIPPED_DIAGONALLY_FLAG = 0x20000000;

    private List<string> tsxOriginalLocations = new List<string>();

    public bool build3DModel;

    [SerializeField]
    public TileModelMapper modelMapper;

    public FilterMode spriteFilterMode = FilterMode.Point;

    /// <summary>
    /// Internal cache of loaded sprites
    /// </summary>
    private List<Sprite> spriteCache;

    /// <summary>
    /// Stores the IDs of the tiles that have been already loaded
    /// </summary>
    private List<int> usedTiles;

    private Transform rootTransform;

    private static int exceptionsToTolerate = 10;

    static int deadlockPreventionCount = 100000;
    public override void OnImportAsset(AssetImportContext ctx)
    {
        string preferredName = "Tiled Map";
        string[] pathParts = ctx.assetPath.Split('/');
        preferredName = pathParts[pathParts.Length - 1];
        GameObject rootObject = new GameObject(preferredName);
        rootTransform = rootObject.transform;

        try
        {
            ReloadMap(ctx.assetPath);

            if (build3DModel)
            {
                modelMapper.RerunBuilder(rootTransform, ctx.assetPath);
            }
            ctx.AddObjectToAsset(rootObject.name, rootObject);
            ctx.SetMainObject(rootObject);
            exceptionsToTolerate = 10;
            EditorScriptsController.enableSpriteImporter2D = false;
        }
        catch (System.Exception ex)
        {
            deadlockPreventionCount--;
            if (deadlockPreventionCount == 0)
            {
                deadlockPreventionCount = 100000;
                return;
            }

            DestroyImmediate(rootObject);
            exceptionsToTolerate--;
            if (exceptionsToTolerate > 0)
            {
                EditorScriptsController.enableSpriteImporter2D = true;
                AssetDatabase.ImportAsset(ctx.assetPath, ImportAssetOptions.ForceUpdate);
            }
            else
            {
                EditorScriptsController.enableSpriteImporter2D = false;
                exceptionsToTolerate = 10;
                Debug.LogWarning(ex.StackTrace);
                Debug.LogWarning("Error while importing TMX file. See previous message for details...");
            }
        }
    }

    public Transform RootTransform
    {
        get
        {
            return rootTransform;
        }
    }

    private void RegisterObjectsinContext(GameObject root, AssetImportContext ctx)
    {
        ctx.AddObjectToAsset(root.name, root);
        for (int i = 0; i < root.transform.childCount; i++)
        {
            RegisterObjectsinContext(root.transform.GetChild(i).gameObject, ctx);
        }
    }

    /// <summary>
    /// Reloads the tmx file and regenerates the 2D map
    /// </summary>
    private void ReloadMap(string mapFile)
    {
        while (rootTransform.childCount > 0)
        {
            for (int i = 0; i < rootTransform.childCount; i++)
            {
                GameObject child = rootTransform.GetChild(i).gameObject;
                DestroyImmediate(child);
            }
        }

        XmlSerializer mapFileReader = new XmlSerializer(typeof(TiledMap));
        string mapFolder = mapFile.Substring(0, mapFile.LastIndexOfAny(FILEPATH_SEPARATORS) + 1);
        TiledMap map = null;
        using (XmlTextReader reader = new XmlTextReader(mapFile))
        {
            map = (TiledMap)mapFileReader.Deserialize(reader);
        }

        if (map == null || map.layers == null || map.layers.Length == 0)
        {
            return;
        }

        if (map.tileSetEntries != null && map.tileSetEntries.Length > 0)
        {
            map.tileSets = new TiledTileSetFile[map.tileSetEntries.Length];
            XmlSerializer tileSetFileReader = new XmlSerializer(typeof(TiledTileSetFile));
            for (int i = 0; i < map.tileSetEntries.Length; i++)
            {
                string tileSetFile = map.tileSetEntries[i].source;

                List<string> mapFolderParts = new List<string>(mapFolder.Split("/".ToCharArray(), StringSplitOptions.RemoveEmptyEntries));
                List<string> tileSetFileParts = new List<string>(tileSetFile.Split("/".ToCharArray(), StringSplitOptions.RemoveEmptyEntries));
                string pathStart = tileSetFileParts[0];
                while(pathStart == "..")
                {
                    tileSetFileParts.RemoveAt(0);
                    mapFolderParts.RemoveAt(mapFolderParts.Count - 1);
                    pathStart = tileSetFileParts[0];
                }

                string tileSetPath = string.Join("/", new string[]{
                    string.Join("/", mapFolderParts.ToArray()),
                    string.Join("/", tileSetFileParts.ToArray())
                        });

                using (XmlTextReader reader = new XmlTextReader(tileSetPath))
                {
                    map.tileSets[i] = (TiledTileSetFile)tileSetFileReader.Deserialize(reader);
                }
                string loc = Application.dataPath.Replace("Assets", "") + tileSetPath;
                tsxOriginalLocations.Add(loc);
            }
        }

        int z = 0;
        for (int l = map.layers.Length - 1; l >= 0; l--)
        {

            TiledLayer layer = map.layers[l];

            spriteCache = new List<Sprite>();

            usedTiles = new List<int>();

            int w, h;
            w = layer.width;
            h = layer.height;

            GameObject layerObject = new GameObject(layer.name);
            layerObject.transform.parent = rootTransform;
            layerObject.isStatic = true;

            TiledProperty[] layerProperties = layer.customProperties;

            if (layerProperties != null && layerProperties.Length > 0)
            {
                LayerCustomProperties properties = layerObject.AddComponent<LayerCustomProperties>();
                for (int i = 0; i < layerProperties.Length; i++)
                {
                    if (layerProperties[i].name.ToLower().Equals("height"))
                    {
                        float constantHeight;
                        if (float.TryParse(layerProperties[i].value, out constantHeight))
                        {
                            properties.height = constantHeight;
                        }
                    }
                }
            }

            string[] layerData = layer.data.Value.Trim().Split(ROW_SEPARATOR,
                                    System.StringSplitOptions.RemoveEmptyEntries);

            for (int i = 0; i < h; i++)
            {
                string[] row = layerData[i].Split(COMMA_SEPARATOR);
                for (int j = 0; j < w; j++)
                {
                    uint tid = uint.Parse(row[j]);

                    bool flipX = (tid & FLIPPED_HORIZONTALLY_FLAG) >> 31 == 1;
                    tid &= ~FLIPPED_HORIZONTALLY_FLAG;

                    bool flipY = (tid & FLIPPED_VERTICALLY_FLAG) >> 30 == 1;
                    tid &= ~FLIPPED_VERTICALLY_FLAG;

                    bool flipDiag = (tid & FLIPPED_DIAGONALLY_FLAG) >> 29 == 1;
                    tid &= ~FLIPPED_DIAGONALLY_FLAG;

                    int tileID = (int)tid;

                    if (tileID != 0)
                    {
                        Vector3 tilePosition = new Vector3((w * -0.5f) + j, (h * 0.5f) - i, 0);
                        GameObject tileObject = new GameObject("TILE[" + i + "," + j + "]");
                        tileObject.isStatic = true;
                        tileObject.transform.position = tilePosition;
                        tileObject.transform.parent = layerObject.transform;
                        //tileObject.transform.localScale = new Vector3(1.01f, 1.01f, 1.0f);
                        SpriteRenderer sr = tileObject.AddComponent<SpriteRenderer>();
                        sr.sortingOrder = z;

                        if (flipDiag)
                        {
                            tileObject.transform.Rotate(0, 0, 90);
                            tileObject.transform.localScale = new Vector3(1, -1, 1);
                        }
                        sr.flipX = flipX;
                        sr.flipY = flipY;
                        int spriteIndex = usedTiles.IndexOf(tileID);
                        if (spriteIndex < 0)
                        {
                            //new tile
                            Sprite sp = GetTile(tileID, map);
                            spriteCache.Add(sp);
                            usedTiles.Add(tileID);
                            sr.sprite = sp;
                        }
                        else
                        {
                            sr.sprite = spriteCache[spriteIndex];
                        }

                        AnimationFrame[] frames = GetAnimations(tileID, map);
                        if (frames != null)
                        {
                            AnimatedSprite anim = tileObject.AddComponent<AnimatedSprite>();
                            anim.frames = frames;
                        }
                        //Add colliders
                        TiledObjectGroup group = GetObjectsGroup(map, tileID);
                        if (group != null && group.objects != null)
                        {
                            float ppu = sr.sprite.pixelsPerUnit;
                            Collider2D col;
                            foreach (TiledTObject obj in group.objects)
                            {
                                if (obj.polygon != null)
                                {
                                    Vector2 startPoint = new Vector2(obj.x / ppu - 0.5f, -obj.y / ppu + 0.5f);
                                    Vector2[] points = obj.polygon.GetPoints();
                                    for (int k = 0; k < points.Length; k++)
                                    {
                                        points[k].x /= ppu;
                                        points[k].y /= -ppu;
                                        points[k].x += startPoint.x;
                                        points[k].y += startPoint.y;
                                    }
                                    col = tileObject.AddComponent<PolygonCollider2D>();
                                    RotatePoints(points, -obj.rotation, points[0]);
                                    if (flipY)
                                    {
                                        RotatePoints(points, 180.0f, Vector3.zero);
                                    }
                                    ((PolygonCollider2D)col).points = points;
                                }
                                else if (obj.polyline != null)
                                {
                                    Vector2 startPoint = new Vector2(obj.x / ppu - 0.5f, -obj.y / ppu + 0.5f);
                                    Vector2[] points = obj.polyline.GetPoints();
                                    for (int k = 0; k < points.Length; k++)
                                    {
                                        points[k].x /= ppu;
                                        points[k].y /= -ppu;
                                        points[k].x += startPoint.x;
                                        points[k].y += startPoint.y;
                                    }
                                    col = tileObject.AddComponent<EdgeCollider2D>();
                                    RotatePoints(points, -obj.rotation, points[0]);
                                    ((EdgeCollider2D)col).points = points;
                                }
                                else if (obj.ellipse != null)
                                {
                                    Vector2 center = new Vector2(obj.x / ppu - 0.5f, -obj.y / ppu + 0.5f);
                                    float width = obj.width / ppu;
                                    float height = obj.height / ppu;
                                    center.x += width / 2.0f;
                                    center.y -= height / 2.0f;

                                    if (Mathf.Abs(width - height) < 0.1f)
                                    {
                                        col = tileObject.AddComponent<CircleCollider2D>();
                                        float radius = Mathf.Max(height, width) / 2.0f;
                                        ((CircleCollider2D)col).radius = radius;
                                    }
                                    else
                                    {
                                        int vertices = 24;
                                        col = tileObject.AddComponent<PolygonCollider2D>();
                                        Vector2[] points = new Vector2[vertices];
                                        float angStep = 360.0f / vertices;
                                        Vector2 rotationCenter = new Vector2(float.MaxValue, float.MinValue);
                                        for (int p = 0; p < points.Length; p++)
                                        {
                                            float ang = angStep * p * Mathf.Deg2Rad;
                                            float x = width * Mathf.Cos(ang) * 0.5f;
                                            float y = height * Mathf.Sin(ang) * 0.5f;
                                            points[p] = new Vector2(x, y);
                                            if (x < rotationCenter.x)
                                            {
                                                rotationCenter.x = x;
                                            }
                                            if (y > rotationCenter.y)
                                            {
                                                rotationCenter.y = y;
                                            }
                                        }
                                        RotatePoints(points, -obj.rotation, rotationCenter);
                                        ((PolygonCollider2D)col).points = points;
                                    }
                                    col.offset = center;
                                }
                                else
                                {
                                    Vector2 offset = new Vector2(obj.x / ppu, -obj.y / ppu);
                                    float colWidth = obj.width / ppu;
                                    float colHeight = obj.height / ppu;

                                    Vector2[] points = new Vector2[4];
                                    float x = obj.x / ppu - 0.5f;
                                    float y = -obj.y / ppu + 0.5f;
                                    points[0] = new Vector2(x, y);
                                    points[1] = new Vector2(x + colWidth, y);
                                    points[2] = new Vector2(x + colWidth, y - colHeight);
                                    points[3] = new Vector2(x, y - colHeight);

                                    RotatePoints(points, -obj.rotation, points[0]);
                                    col = tileObject.AddComponent<PolygonCollider2D>();
                                    ((PolygonCollider2D)col).points = points;

                                    if (col != null)
                                    {
                                        col.offset += new Vector2(group.offsetX / ppu, -group.offsetY / ppu);
                                    }
                                }
                            }
                        }
                    }
                }
            }

            z--;
        }

        Resources.UnloadUnusedAssets();
    }

    private void RotatePoints(Vector2[] points, float angle, Vector2 center)
    {
        float sin = Mathf.Sin(angle * Mathf.Deg2Rad);
        float cos = Mathf.Cos(angle * Mathf.Deg2Rad);
        for (int i = 0; i < points.Length; i++)
        {
            points[i].x -= center.x;
            points[i].y -= center.y;
            float x1 = points[i].x * cos - points[i].y * sin;
            float y1 = points[i].y * cos + points[i].x * sin;
            points[i].x = x1 + center.x;
            points[i].y = y1 + center.y;
        }
    }

    private TiledObjectGroup GetObjectsGroup(TiledMap map, int tileId)
    {
        for (int i = 0; i < map.tileSets.Length; i++)
        {
            TiledTileSetFile tsFile = map.tileSets[i];
            if(tsFile.tiles != null)
            {
                foreach (TiledTile tile in tsFile.tiles)
                {
                    if (tile.id + map.tileSetEntries[i].firstGID == tileId)
                    {
                        return tile.objectsGroup;
                    }
                }
            }
        }
        return null;
    }

    /// <summary>
    /// TODO: Function under construction
    /// </summary>
    /// <param name="tileID">Tile I.</param>
    /// <param name="map">Map.</param>
    /// <param name="tileObject">Tile object.</param>
    private void AttachTileProperties(int tileID, TiledMap map, GameObject tileObject)
    {
        for (int i = 0; i < map.tileSets.Length; i++)
        {
            TiledTileSetFile tSet = map.tileSets[i];
            if (tSet.image == null)
            {
                //for single images
                foreach (TiledTile tile in tSet.tiles)
                {
                    if (tile.id + map.tileSetEntries[i].firstGID == tileID)
                    {
                        if (tile.customProperties != null && tile.customProperties.Length > 0)
                        {
                            Tile3D tileScript = tileObject.AddComponent<Tile3D>();
                            for (int j = 0; j < tile.customProperties.Length; j++)
                            {
                                string pName = tile.customProperties[j].name.ToLower();
                                string pValue = tile.customProperties[j].value;

                                if (pName.Equals("generatecollider"))
                                {
                                    bool gen;
                                    if (bool.TryParse(pValue, out gen))
                                    {
                                        tileScript.generateCollider = gen;
                                    }
                                }
                                else if (pName.Equals("minrandomdisplacement") || pName.Equals("maxrandomdisplacement"))
                                {
                                    /*
                                    float x, y, z;
                                    Vector3 displacement;
                                    string[] vals = pValue.Split(',');
                                    if (vals.Length == 3)
                                    {
                                        //TODO: continue from here
                                    }
                                    */
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    /// <summary>    /// Loads the sprite of the provided tile ID from Resources folder
    /// </summary>

    /// <returns>Sprite of the tile if found, null otherwise</returns>
    /// <param name="tileID">Id of the tile in the tmx map file</param>
    /// <param name="map">The tmx map deserialization</param>
    private Sprite GetTile(int tileID, TiledMap map)
    {

        for (int i = 0; i < map.tileSets.Length; i++)
        {
            string tsxFolder =
            tsxOriginalLocations[i].Substring(0, tsxOriginalLocations[i].LastIndexOfAny(FILEPATH_SEPARATORS));

            tsxFolder = tsxFolder.Substring(tsxFolder.IndexOf("Assets"));
            TiledTileSetFile tSet = map.tileSets[i];
            if (tSet.image == null)
            {
                //for single images
                foreach (TiledTile tile in tSet.tiles)
                {
                    if (tile.id + map.tileSetEntries[i].firstGID == tileID)
                    {
                        string path = FormatPath(tsxFolder + "/" + tile.image.source);
                        Sprite result = null;
                        int resIndex = path.ToLower().IndexOf("resources");

                        string fullPath = Path.Combine(Application.dataPath, path);
                        fullPath = Path.GetFullPath((new Uri(fullPath)).LocalPath);
                        path = fullPath.Substring(fullPath.IndexOf("Assets") + 7);

                        if (resIndex > 0)
                        {
                            string resPath = path.Substring(resIndex + 10, path.Length - (resIndex + 10));
                            resPath = resPath.Substring(0, resPath.LastIndexOf("."));
                            result = Resources.Load<Sprite>(resPath);
                        }
                        else
                        {
                            result = AssetDatabase.LoadAssetAtPath<Sprite>(path);
                        }

                        if (result == null)
                        {
                            if (File.Exists(Application.dataPath.Replace("Assets", "") + path))
                            {
                                EditorScriptsController.enableSpriteImporter2D = true;
                                EditorScriptsController.importSpriteSheet = false;
                                EditorScriptsController.targetMap = map;
                                EditorScriptsController.textureFilterMode = spriteFilterMode;
                                //File is there, but
                                //import settings were off, give another chance to find the sprite

                                AssetDatabase.ImportAsset(path);
                                exceptionsToTolerate++;
                            }

                            throw new FileNotFoundException("'" + path + "': Tile not found");
                        }

                        return result;
                    }
                }
            }
            else
            {
                //for spritesheet
                int id = tileID - map.tileSetEntries[i].firstGID;
                if (id >= 0 && id < tSet.tileCount)
                {
                    string path = FormatPath(tsxFolder + "/" + tSet.image.source);
                    UnityEngine.Object[] objs = AssetDatabase.LoadAllAssetsAtPath(path);
                    bool settingsChanged = false;
                    if (EditorScriptsController.textureFilterMode != spriteFilterMode)
                    {
                        settingsChanged = true;
                    }
                    if (objs == null || objs.Length - 1 < tSet.tileCount || settingsChanged)
                    {
                        if (File.Exists(Application.dataPath.Replace("Assets", "") + path))
                        {
                            EditorScriptsController.enableSpriteImporter2D = true;
                            EditorScriptsController.importSpriteSheet = true;
                            EditorScriptsController.textureFilterMode = spriteFilterMode;
                            EditorScriptsController.targetMap = map;
                            EditorScriptsController.targetTileset = i;
                            AssetDatabase.ImportAsset(path);
                            exceptionsToTolerate++;
                            if (settingsChanged)
                            {
                                throw new System.Exception("Nothing serious, just to fire reimport");
                            }
                        }
                    }

                    Sprite[] sprites = new Sprite[objs.Length - 1];
                    for (int j = 1; j < objs.Length; j++)
                    {
                        sprites[j - 1] = (Sprite)objs[j];
                    }

                    return (sprites[id]);
                }
            }
        }

        return null;
    }

    /// <summary>
    /// Gets sprite animation of the provided tile ID
    /// </summary>
    /// <returns>Array containing animation frames and their times if any, null otherwize</returns>
    /// <param name="tileID">Id of the tile in the tmx map file</param>
    /// <param name="map">The tmx map deserializtion</param>
    private AnimationFrame[] GetAnimations(int tileID, TiledMap map)
    {
        for (int i = 0; i < map.tileSets.Length; i++)
        {
            TiledTileSetFile tSet = map.tileSets[i];
            if (tSet.tiles != null)
            {
                foreach (TiledTile tile in tSet.tiles)
                {
                    int id = tileID - map.tileSetEntries[i].firstGID;
                    if (id == tile.id &&
                        tile.animation != null)
                    {
                        AnimationFrame[] result = new AnimationFrame[tile.animation.frames.Length];

                        for (int j = 0; j < result.Length; j++)
                        {
                            result[j].sprite = GetTile(tile.animation.frames[j].tileID + map.tileSetEntries[i].firstGID, map);
                            result[j].duration = tile.animation.frames[j].duration;
                        }

                        return result;
                    }
                }
            }
        }
        return null;
    }

    private string FormatPath(string path)
    {
        return path.Replace(FILEPATH_SEPARATORS[0], FILEPATH_SEPARATORS[1]).TrimEnd(FILEPATH_SEPARATORS);
    }

    private void PostProcessor()
    {

    }
}
#endif