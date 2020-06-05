#if (UNITY_EDITOR)
using UnityEngine;
using System.Collections.Generic;
using UnityEditor;
using System.IO;
using System.Text;
using System.Xml;

/// <summary>
/// Creates 2D tiles from 3D models added as children to the
/// object holding this script
/// </summary>
[RequireComponent(typeof(Grid3DMaker))]
public class TileCreator : MonoBehaviour
{
    /// <summary>
    /// Default folder for 2D tiles generated from 3D tiles
    /// </summary>
    public const string TILES_FOLDER = "Genrated2DTiles";

    /// <summary>
    /// Default folder for 3D tile atlases
    /// </summary>
    public const string ATLASES_FOLDER = "3DTilesAtlases";

    /// <summary>
    /// Default folder for tilED maps (inside assets)
    /// </summary>
    public const string MAPS_FOLDER = "TiledMaps";

    /// <summary>
    /// Name of the generated set of tiles
    /// </summary>
    public string collectionName;

    /// <summary>
    /// Color to use for transparency. This will be replaced
    /// by alpha in the final tile image
    /// </summary>
    public Color transparentColor = new Color(1.0f, 0.0f, 1.0f, 1.0f);

    /// <summary>
    /// Size of tiles to be generated
    /// </summary>
    public int tileSize = 64;

    /// <summary>
    /// Internal index of the current model being handled
    /// </summary>
    int index = 0;

    /// <summary>
    /// Internal track of current rotation of the models
    /// </summary>
    int rot = -90;

    /// <summary>
    /// Stores game object of the model currently being handled
    /// </summary>
    GameObject current;

    /// <summary>
    /// Reference to the orthogonal camera used to capture
    /// screenshots for 2D tiles. This camera must have the name "TileGenerationCam"
    /// </summary>
    Transform captureCamTrans;

    /// <summary>
    /// References the Tile3D script attached to the
    /// model being currently handled
    /// </summary>
    Tile3D currentTile;

    /// <summary>
    /// Reference to the camera component of mainCam camera
    /// </summary>
    Camera captureCam;

    Camera prevCam;

    /// <summary>
    /// Stores file names of all generated tiles to iterate over them
    /// one by one at the end and add the alpha channel to the files
    /// </summary>
    List<string> generatedPaths;

    /// <summary>
    /// Starts by (1) finding "TileGenerationCam" and set its properties
    /// to match tile generation setting specified in the variables
    /// of this script, (2) refreshing the attached Grid3DMaker script to
    /// make sure all models are ready for tile generation, and (3) set orthogonal
    /// size of tile capturing camera to match the size of the 3D tiles as
    /// specified in the attached Grid3DMaker
    /// </summary>
    void Start()
    {
        GameObject captureCamObj = new GameObject("Tile Generation Cam");

        captureCamTrans = captureCamObj.transform;
        captureCamTrans.eulerAngles = new Vector3(90.0f, 0, 0);

        captureCam = captureCamObj.AddComponent<Camera>();
        captureCam.clearFlags = CameraClearFlags.Color;
        captureCam.backgroundColor = transparentColor;
        Grid3DMaker gMaker = GetComponent<Grid3DMaker>();

        captureCam.orthographicSize = Mathf.Min(gMaker.tileSize.x, gMaker.tileSize.z) * gMaker.maxDiff * 0.5f;
        captureCam.orthographic = true;

        GameObject prevCamObj = GameObject.Find("Tile Preview Cam");
        if (prevCamObj == null)
        {
            Debug.LogWarning("Warning: tile preview cam not found. Refresh grid 3D maker to re-instantiate cam");
            UnityEditor.EditorApplication.isPlaying = false;
            return;
        }

        prevCam = prevCamObj.GetComponent<Camera>();
        if(prevCam == null)
        {
            prevCam = prevCamObj.AddComponent<Camera>();
        }
        prevCam.depth = 1;
        //prevCam.rect = new Rect(0, -0.65f, 0.35f, 1);

        captureCamTrans.position = new Vector3(0, 50, 0);

        Transform prevCamTrans = prevCamObj.transform;
        prevCamTrans.parent = captureCamTrans;

        gMaker.Refresh();
        generatedPaths = new List<string>();
        string tilesPath = Application.dataPath + "/" + TILES_FOLDER + "/" + collectionName;
        if(Directory.Exists(tilesPath)){
            Directory.Delete(tilesPath, true);
        }
    }

    /// <summary>
    /// Updates by positioning the camera over the current model
    /// and capture a screenshot to be used as a 2D tile for that model.
    /// Another perspective camera is used to capture the screenshot for the
    /// 3D preview tile. This function also handles rotating all models once
    /// the current rotaion is completed for all of them. All tiles are captured
    /// as 3D preview in addition to top-view with 0, 90, 180 and 270 degrees rotations.
    /// File name format is height-tileObjectName_Rotation.png.
    /// When tile generation is done, the current object is saved as a prefab with collection
    /// name in "Assets/Prefabs" folder
    /// </summary>
    void Update()
    {

        if (transform.childCount == 0)
        {
            Debug.LogWarning("Warning: '" + gameObject.name + "' has no children to build tiles from. Exiting...");
            UnityEditor.EditorApplication.isPlaying = false;
            return;
        }

        if (prevCam == null)
        {
            return;
        }

        if (!GameViewUtils.SizeExists(GameViewSizeGroupType.Standalone, tileSize, tileSize))
        {
            GameViewUtils.AddCustomSize(GameViewUtils.GameViewSizeType.FixedResolution,
                GameViewSizeGroupType.Standalone, tileSize, tileSize,
            tileSize + "*" + tileSize + " tile generation");
        }

        GameViewUtils.SetSize(GameViewUtils.FindSize(GameViewSizeGroupType.Standalone, tileSize, tileSize));

        if (string.IsNullOrEmpty(collectionName.Trim()))
        {
            Debug.LogWarning("Warning: collection name cannot be empty. Exiting...");
            UnityEditor.EditorApplication.isPlaying = false;
            return;
        }

        string tilesPath = Application.dataPath + "/" + TILES_FOLDER + "/" + collectionName;

        if (rot > 270)
        {

            SetTextureAlpha();
            GenerateTilesetFile();
            Debug.Log("Done!! your sprites are in " + tilesPath);
            try
            {
                string atlasesFullPath = Application.dataPath + "/Resources/" + ATLASES_FOLDER;
                if (!Directory.Exists(atlasesFullPath))
                {
                    Directory.CreateDirectory(atlasesFullPath);
                }
                //Object prefab = PrefabUtility.CreateEmptyPrefab("Assets/Resources/" + ATLASES_FOLDER + "/" + collectionName + ".prefab");
                //PrefabUtility.ReplacePrefab(gameObject, prefab, ReplacePrefabOptions.ConnectToPrefab);
                PrefabUtility.SaveAsPrefabAssetAndConnect(gameObject, "Assets/Resources/" + ATLASES_FOLDER + "/" + collectionName + ".prefab", InteractionMode.AutomatedAction);

                Debug.Log("Your tile collection was saved as " + atlasesFullPath + "/" + collectionName + ".prefab");
            }
            catch
            {
                Debug.LogError("Error: unable to save atlas prefab");
            }

            UnityEditor.EditorApplication.isPlaying = false;
            EditorScriptsController.enableSpriteImporter2D = true;
            AssetDatabase.Refresh();
            EditorScriptsController.enableSpriteImporter2D = false;
            return;
        }

        MoveToNext();

        if (Time.frameCount < 5)
        {
            return;
        }

        string rotString = "_000";

        if (index == transform.childCount - 1)
        {
            //Special case of last tile
            if (rot == 0)
            {
                rotString = "_090";
            }
            else if (rot == 90)
            {
                rotString = "_180";
            }
            else if (rot == 180)
            {
                rotString = "_270";
            }
        }
        else
        {
            if (rot == 90)
            {
                rotString = "_090";
            }
            else if (rot == 180)
            {
                rotString = "_180";
            }
            else if (rot == 270)
            {
                rotString = "_270";
            }
        }

        currentTile = current.GetComponent<Tile3D>();

        string fullPath;

        if (rot != -90)
        {
            fullPath = tilesPath + "/" + currentTile.height.ToString("F2") + "-" + current.name + rotString + ".png";
        }
        else
        {
            fullPath = tilesPath + "/" + currentTile.height.ToString("F2") + "-" + current.name + "__PRV" + ".png";
        }
        generatedPaths.Add(fullPath);

        if (!Directory.Exists(tilesPath))
        {
            Directory.CreateDirectory(tilesPath);
        }

        try
        {
            ScreenCapture.CaptureScreenshot(fullPath);
        }
        catch
        {
            Debug.LogError("Error: unable to save 2D tile sprite '" + fullPath + "'. Exiting...");
            UnityEditor.EditorApplication.isPlaying = false;
        }

        index++;
        if (index == transform.childCount)
        {
            index = 0;
            rot += 90;
            if (rot != 0)
            {
                for (int i = 0; i < transform.childCount; i++)
                {
                    Transform t = transform.GetChild(i);

                    t.Rotate(0, 90, 0, Space.World);

                }
            }
        }
    }

    /// <summary>
    /// Advences the camera to the next model when the capturing of the
    /// current model is done
    /// </summary>
    void MoveToNext()
    {
        current = transform.GetChild(index).gameObject;

        captureCamTrans.position = new Vector3(current.transform.position.x,
                                                    captureCamTrans.position.y,
                                                     current.transform.position.z);

        captureCam.enabled = rot != -90;
        prevCam.enabled = rot == -90;
    }

    /// <summary>
    /// Used to draw a GUI text representing tile hight when the 3D preview
    /// is generated
    /// </summary>
    void OnGUI()
    {
        if (rot == -90 || (rot == 0 && index == 0))
        {//The second condition is a special case for the last tile.
         //Otherwise OnGUI won't work and height is not displayed
            if (current != null)
            {
                currentTile = current.GetComponent<Tile3D>();
                GUI.TextField(new Rect(2, 2, 35, 20), currentTile.height.ToString("F2"));
            }
        }

    }

    /// <summary>
    /// Iterates over all files in generatedPaths list and replaces
    /// transparency color with alpha. Also puts a 1-pixel black outline
    /// around pixels to separate them from alpha
    /// </summary>
    void SetTextureAlpha()
    {
        Color alpha = new Color(1.0f, 1.0f, 1.0f, 0.0f);
        foreach (string fullPath in generatedPaths)
        {

            Texture2D tex = new Texture2D(captureCam.pixelWidth, captureCam.pixelHeight);
            tex.LoadImage(System.IO.File.ReadAllBytes(fullPath));
            System.IO.File.Delete(fullPath);
            List<Vector2> outlinePixels = new List<Vector2>();
            List<Vector2> alphaPixels = new List<Vector2>();

            for (int i = 0; i < tex.width; i++)
            {
                for (int j = 0; j < tex.height; j++)
                {
                    if (tex.GetPixel(i, j) == transparentColor)
                    {
                        //Check if all surrounding pixels are transparent
                        //in this case set to alpha
                        bool setAlpha = true;

                        int center = i;
                        int left = i - 1;
                        int right = i + 1;
                        int up = j + 1;
                        int down = j - 1;

                        if (tex.GetPixel(center, j) != transparentColor)
                        {
                            setAlpha = false;
                        }
                        else
                        {
                            if (up < tex.height - 1)
                            {
                                if (tex.GetPixel(center, up) != transparentColor)
                                {
                                    setAlpha = false;
                                }
                                else
                                {
                                    if (down >= 0)
                                    {
                                        if (tex.GetPixel(center, down) != transparentColor)
                                        {
                                            setAlpha = false;
                                        }
                                    }
                                }
                            }
                        }

                        if (setAlpha)
                        {

                            if (left >= 0)
                            {

                                if (tex.GetPixel(left, j) != transparentColor)
                                {
                                    setAlpha = false;
                                }
                                else
                                {
                                    if (up < tex.height - 1)
                                    {
                                        if (tex.GetPixel(left, up) != transparentColor)
                                        {
                                            setAlpha = false;
                                        }
                                        else
                                        {
                                            if (down >= 0)
                                            {
                                                if (tex.GetPixel(left, down) != transparentColor)
                                                {
                                                    setAlpha = false;
                                                }
                                            }
                                        }
                                    }
                                }
                            }

                            if (setAlpha)
                            {
                                if (right < tex.width - 1)
                                {

                                    if (tex.GetPixel(right, j) != transparentColor)
                                    {
                                        setAlpha = false;
                                    }
                                    else
                                    {
                                        if (up < tex.height - 1)
                                        {
                                            if (tex.GetPixel(right, up) != transparentColor)
                                            {
                                                setAlpha = false;
                                            }
                                            else
                                            {
                                                if (down >= 0)
                                                {
                                                    if (tex.GetPixel(right, down) != transparentColor)
                                                    {
                                                        setAlpha = false;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }

                        if (setAlpha)
                        {
                            alphaPixels.Add(new Vector2(i, j));
                        }
                        else
                        {
                            outlinePixels.Add(new Vector2(i, j));
                        }
                    }
                }
            }

            if (alphaPixels.Count > 0)
            {
                for (int i = 0; i < tex.width; i++)
                {
                    for (int j = 0; j < tex.height; j++)
                    {
                        bool border = (i == 0 || i == tex.width - 1) || (j == 0 || j == tex.height - 1);
                        if (border)
                        {
                            outlinePixels.Add(new Vector2(i, j));
                        }
                    }
                }
            }

            foreach (Vector2 v in outlinePixels)
            {
                tex.SetPixel((int)v.x, (int)v.y, Color.black);
            }

            foreach (Vector2 v in alphaPixels)
            {
                tex.SetPixel((int)v.x, (int)v.y, alpha);
            }

            tex.Apply();

            byte[] bytes = tex.EncodeToPNG();
            System.IO.File.WriteAllBytes(fullPath, bytes);
            Object.Destroy(tex);

        }
    }

    /// <summary>
    /// Generates TSX file including all captured 2D sprites
    /// </summary>
    void GenerateTilesetFile()
    {
        string tilesetsPath = Application.dataPath + "/" + MAPS_FOLDER + "/tilesets";
        if (!Directory.Exists(tilesetsPath))
        {
            Directory.CreateDirectory(tilesetsPath);
        }
        string filePath = tilesetsPath + "/" + collectionName + ".tsx";

        generatedPaths.Sort();

        try
        {
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
            XmlWriter writer = XmlWriter.Create(filePath);
            writer.WriteStartDocument();
            writer.WriteStartElement("tileset");
            writer.WriteAttributeString("name", collectionName);

            Vector2 tileSize = Handles.GetMainGameViewSize();

            writer.WriteAttributeString("tilewidth", tileSize.x.ToString());
            writer.WriteAttributeString("tileheight", tileSize.y.ToString());
            writer.WriteAttributeString("tilecount", generatedPaths.Count.ToString());
            writer.WriteAttributeString("columns", "0");

            writer.WriteStartElement("grid");
            writer.WriteAttributeString("orientation", "orthogonal");
            writer.WriteAttributeString("width", "1");
            writer.WriteAttributeString("height", "1");
            writer.WriteEndElement();

            for (int i = 0; i < generatedPaths.Count; i++)
            {
                writer.WriteStartElement("tile");
                writer.WriteAttributeString("id", i.ToString());

                writer.WriteStartElement("image");
                writer.WriteAttributeString("width", tileSize.x.ToString());
                writer.WriteAttributeString("height", tileSize.y.ToString());

                string source = generatedPaths[i].Substring(generatedPaths[i].LastIndexOf("/") + 1);

                writer.WriteAttributeString("source", "../../" + TILES_FOLDER + "/" + collectionName + "/" + source);
                writer.WriteEndElement();
                writer.WriteEndElement();
            }

            writer.WriteEndDocument();
            writer.Close();
            Debug.Log("Tileset file created successfully in " + filePath);
        }
        catch
        {
            Debug.LogError("Error while generatin tmx file");
        }

    }
}
#endif