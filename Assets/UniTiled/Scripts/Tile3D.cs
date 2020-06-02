using UnityEngine;
using System.Collections;

/// <summary>
/// Attached to 3D models that represent tiles to set its properties
/// </summary>
public class Tile3D : MonoBehaviour
{
    /// <summary>
    /// Alignment position of the model relative to tile center
    /// </summary>
    public Alignment alignTo = Alignment.center;

    /// <summary>
    /// Whether a mesh collider should be attached to the model
    /// </summary>
    public bool generateCollider = true;

    /// <summary>
    /// Minimum distance to randomly move the model from its 
    /// original position when building the map
    /// </summary>
    public Vector3 minRandomDisplacement = Vector3.zero;

    /// <summary>
    /// Maximum distance to randomly move the model from its 
    /// original position when building the map
    /// </summary>
    public Vector3 maxRandomDisplacement = Vector3.zero;

    /// <summary>
    /// Angle to use its multiples as possible random
    /// rotations for the model when generating the map
    /// </summary>
    public float randomRotationSnapAngle = 0.0f;


    /// <summary>
    /// Minimum factor of random scale
    /// </summary>
    public float minRandomScale = 0.0f;

    /// <summary>
    /// Maximum factor of randoom scale
    /// </summary>
    public float maxRandomScale = 0.0f;

    /// <summary>
    /// Minimum number of additional random copies
    /// of the tile model to create when generating
    /// the map.
    /// </summary>
    public int minRandomCopies = 0;
	
	/// <summary>
    /// Maximum number of additional random copies
    /// of the tile model to create when generating
    /// the map.
    /// </summary>
    public int maxRandomCopies = 0;

    /// <summary>
    /// The encoded height of the tile model, rad from tile file name
    /// </summary>
    [HideInInspector]
    public float height;


}

/// <summary>
/// The (x,z) position of tile's model relative to tile's center position
/// </summary>
public enum Alignment
{
    center, left, right, bottom, top, upperLeft, upperRight, lowerLeft, lowerRight
}