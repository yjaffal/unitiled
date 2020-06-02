using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEditor;

public class SpriteImporter2D : AssetPostprocessor
{

    private void OnPreprocessTexture()
    {
        if (!EditorScriptsController.enableSpriteImporter2D)
        {
            return;
        }

        TiledMap map = EditorScriptsController.targetMap;

        TextureImporter importer = (TextureImporter)assetImporter;
        //object[] args = new object[2] { 0, 0 };
        //MethodInfo mi = typeof(TextureImporter).GetMethod("GetWidthAndHeight", BindingFlags.NonPublic | BindingFlags.Instance);
        //mi.Invoke(importer, args);

        //int width = (int)args[0];
        //int height = (int)args[1];

        TextureImporterSettings texSettings = new TextureImporterSettings();
        importer.ReadTextureSettings(texSettings);
        texSettings.filterMode = EditorScriptsController.textureFilterMode;
        if (EditorScriptsController.importSpriteSheet)
        {
            texSettings.spriteMode = (int)SpriteImportMode.Multiple;
            TiledTileSetFile tSet = map.tileSets[EditorScriptsController.targetTileset];
            SpriteMetaData[] tiles = new SpriteMetaData[tSet.tileCount];
            
            int row = (tSet.tileCount / tSet.columns) - 1;
            int rows = row;
            int col = 0;
            for(int i = 0; i < tiles.Length; i++){
                SpriteMetaData dat = new SpriteMetaData();
                dat.pivot = Vector2.zero;
                Vector2 pos = Vector2.zero;
                pos.y += tSet.margin;
                pos.x += tSet.margin;

                pos.x += col * (map.tileWidth + tSet.spacing);
                pos.y += row * (map.tileHeight + tSet.spacing);

                Rect tileRect = new Rect(pos.x, pos.y, map.tileWidth, map.tileHeight);
                dat.rect = tileRect;
                dat.name = "Tile_"+i;
                col++;
                if(col == tSet.columns){
                    col = 0;
                    row--;
                }
                tiles[i] = dat;
            }
            importer.spritesheet = tiles;
        }
        else
        {
            texSettings.spriteMode = (int)SpriteImportMode.Single;
        }

        importer.filterMode = EditorScriptsController.textureFilterMode;
        texSettings.spriteAlignment = (int)SpriteAlignment.Custom;
        importer.SetTextureSettings(texSettings);

        int ppu = map != null? Mathf.Min(map.tileWidth, map.tileHeight) : 100;
        importer.spritePixelsPerUnit = ppu;
        importer.textureType = TextureImporterType.Sprite;
        importer.spritePivot = Vector2.zero;
    }


}
