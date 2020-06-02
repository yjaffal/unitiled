using UnityEngine;
using System.Collections;
using System.IO;
using System;
/// <summary>
/// Keeps checkig the tmx file of the map specified in attached TileModelMapper
/// and eventually updates the map in the scene based on the changes in the file
/// </summary>
[ExecuteInEditMode]
//[RequireComponent(typeof(TileModelMapper))]
//[RequireComponent(typeof(TMXImporter))]
public class RealTimeMapReader : MonoBehaviour
{

    /// <summary>
    /// Reference to the attached TMXImporter
    /// </summary>
   // TMXImporter importer;

    /// <summary>
    /// Reference to the attached TileModelMapper
    /// </summary>
    //TileModelMapper mapper;

    /// <summary>
    /// Tracks last modification time of the tmx file
    /// </summary>
    DateTime lastModified;

    /// <summary>
    /// Starts by getteing references to the attached TMXImporter
    /// and TileModelMapper and recording last modification time
    /// of the tmx file
    /// </summary>
    void Start()
    {
        // importer = GetComponent<TMXImporter>();
        // mapper = GetComponent<TileModelMapper>();

        // lastModified = File.GetLastWriteTime(importer.mapFile);
    }

    /// <summary>
    /// Updates by comparing current file modification time to the
    /// recorded last modification time and updates the scene using
    /// the new version of the file if it was modified
    /// </summary>
    void Update()
    {
        // DateTime fileTime = File.GetLastWriteTime(importer.mapFile);
        // if (!lastModified.Equals(fileTime))
        // {
        //     mapper.run = true;
        //     lastModified = fileTime;
        //     mapper.RerunBuilder();
        // }
    }
}
